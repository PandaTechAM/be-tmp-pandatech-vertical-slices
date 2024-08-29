using System.Diagnostics;
using System.Text.Json;

namespace Pandatech.VerticalSlices.SharedKernel.Middlewares;

public class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
{
   private static readonly HashSet<string> SensitiveKeywords = new(StringComparer.OrdinalIgnoreCase)
   {
      "pwd",
      "pass",
      "secret",
      "token",
      "cookie",
      "auth"
   };

   public async Task InvokeAsync(HttpContext context)
   {
      if (context.Request.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
      {
         await next(context);
         return;
      }

      var requestLog = await CaptureRequestAsync(context.Request);

      // Swap the response stream to capture the response
      var originalBodyStream = context.Response.Body;
      await using var responseBody = new MemoryStream();
      context.Response.Body = responseBody;
      var stopwatch = Stopwatch.StartNew();
      try
      {
         await next(context);
      }
      finally
      {
         stopwatch.Stop();
         var responseLog = await CaptureResponseAsync(context.Response);

         logger.LogInformation(
            "Request {Method} {Query} responded {StatusCode} in {ElapsedMilliseconds}ms. RequestHeaders: {RequestHeaders}, RequestBody: {RequestBody}, ResponseHeaders: {ResponseHeaders}, ResponseBody: {ResponseBody}",
            context.Request.Method,
            context.Request.QueryString,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds,
            requestLog.Headers,
            requestLog.Body,
            responseLog.Headers,
            responseLog.Body);

         // Reset the response body to the original stream
         await responseBody.CopyToAsync(originalBodyStream);
      }
   }

   private static async Task<(string Headers, string Body)> CaptureRequestAsync(HttpRequest request)
   {
      request.EnableBuffering();
      var (headers, bodyContent) = await CaptureLogAsync(request.Body, request.Headers);
      request.Body.Position = 0;
      return (headers, bodyContent);
   }

   private static async Task<(string Headers, string Body)> CaptureResponseAsync(HttpResponse response)
   {
      response.Body.Seek(0, SeekOrigin.Begin);
      var (headers, bodyContent) = await CaptureLogAsync(response.Body, response.Headers);
      response.Body.Seek(0, SeekOrigin.Begin);
      return (headers, bodyContent);
   }

   private static async Task<(string Headers, string Body)> CaptureLogAsync(Stream bodyStream, IHeaderDictionary headers)
   {
      using var reader = new StreamReader(bodyStream, leaveOpen: true);
      var body = await reader.ReadToEndAsync();
      var sanitizedHeaders = JsonSerializer.Serialize(RedactSensitiveData(headers));
      var bodyContent = JsonSerializer.Serialize(ParseAndRedactJson(body));

      return (sanitizedHeaders, bodyContent);
   }

   private static object ParseAndRedactJson(string body)
   {
      if (string.IsNullOrWhiteSpace(body))
         return string.Empty;

      try
      {
         var jsonElement = JsonSerializer.Deserialize<JsonElement>(body);
         return RedactSensitiveData(jsonElement);
      }
      catch (JsonException)
      {
         // Return the body as a string if it’s not valid JSON, 
         // but still wrapped in an object to avoid conversion to a string
         return RedactSensitiveString(body);
      }
   }

   private static Dictionary<string, string> RedactSensitiveData(IHeaderDictionary headers)
   {
      var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

      foreach (var header in headers)
      {
         var key = header.Key;
         var value = SensitiveKeywords.Any(keyword => key.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            ? "[REDACTED]"
            : header.Value.ToString();

         result[key] = value;
      }

      return result;
   }

   private static object RedactSensitiveData(JsonElement element)
   {
      switch (element.ValueKind)
      {
         case JsonValueKind.Object:
            var maskedObject = new Dictionary<string, object>();
            foreach (var property in element.EnumerateObject())
            {
               var propertyName = property.Name;

               if (SensitiveKeywords.Any(keyword => propertyName.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
               {
                  maskedObject[propertyName] = "[REDACTED]";
               }
               else
               {
                  maskedObject[propertyName] = RedactSensitiveData(property.Value);
               }
            }
            return maskedObject;

         case JsonValueKind.Array:
            var maskedArray = element.EnumerateArray()
                                     .Select(RedactSensitiveData)
                                     .ToArray();
            return maskedArray;

         case JsonValueKind.String:
            var stringValue = element.GetString();
            return RedactSensitiveString(stringValue);

         case JsonValueKind.Undefined:
         case JsonValueKind.Number:
         case JsonValueKind.True:
         case JsonValueKind.False:
         case JsonValueKind.Null:
         default:
            return element.GetRawText();
      }
   }

   private static string RedactSensitiveString(string? value)
   {
      if (string.IsNullOrWhiteSpace(value))
         return string.Empty;

      return SensitiveKeywords.Any(keyword => value.Contains(keyword, StringComparison.OrdinalIgnoreCase))
         ? "[REDACTED]"
         : value;
   }
}
