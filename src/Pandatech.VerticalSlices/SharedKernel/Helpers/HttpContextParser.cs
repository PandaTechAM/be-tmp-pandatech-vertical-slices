using System.Net;
using System.Text;
using Pandatech.VerticalSlices.Features.Auth.Helpers;
using Pandatech.VerticalSlices.SharedKernel.Enums;

namespace Pandatech.VerticalSlices.SharedKernel.Helpers;

public static class HttpContextParser
{
   private const string DefaultIpAddress = "0.0.0.0";

   public static string TryParseAccessTokenSignature(this HttpContext httpContext, IHostEnvironment environment)
   {
      var accessTokenName = CookieHelper.FormatCookieName("access_token", environment);
      var accessTokenSignature = httpContext.Request.Cookies[accessTokenName];

      if (!string.IsNullOrEmpty(accessTokenSignature))
      {
         return accessTokenSignature;
      }

      accessTokenSignature = httpContext.Request.Headers.Authorization.ToString();

      return accessTokenSignature;
   }

   public static string TryParseRefreshTokenSignature(this HttpContext httpContext, IHostEnvironment environment)
   {
      var refreshTokenName = CookieHelper.FormatCookieName("refresh_token", environment);
      var refreshTokenSignature = httpContext.Request.Cookies[refreshTokenName];

      if (!string.IsNullOrEmpty(refreshTokenSignature))
      {
         return refreshTokenSignature;
      }

      refreshTokenSignature = httpContext.Request.Headers["refresh-token"].ToString();

      return refreshTokenSignature;
   }

   public static string TryParseClientType(this HttpContext httpContext)
   {
      var clientType = httpContext.Request.Headers["client-type"].ToString();
      return clientType;
   }

   public static string TryParseRequestId(this HttpContext httpContext)
   {
      var requestId = httpContext.Request.Headers.RequestId.ToString();
      return requestId;
   }

   public static string TryParseUniqueIdPerDevice(this HttpContext httpContext, IHostEnvironment environment)
   {
      var deviceCookie = CookieHelper.FormatCookieName("device", environment);
      var uniqueIdPerDevice = httpContext.Request.Cookies[deviceCookie];

      if (string.IsNullOrEmpty(uniqueIdPerDevice))
      {
         uniqueIdPerDevice = httpContext.Request.Headers["device"].ToString();
      }

      return uniqueIdPerDevice;
   }

   public static string TryParseDeviceName(this HttpContext httpContext)
   {
      return httpContext.Request.Headers["device-name"].ToString();
   }

   public static SupportedLanguageType TryParseLanguageId(this HttpContext httpContext)
   {
      return httpContext.Request.Headers.AcceptLanguage.ToString().GetLanguage();
   }

   public static string TryParseUserAgent(this HttpContext httpContext)
   {
      return httpContext.Request.Headers.UserAgent.ToString();
   }

   public static string? TryParseForwardedHeaders(this HttpContext httpContext)
   {
      var forwardedHeaders = httpContext.Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());

      var headerNames = new List<string>
      {
         "X-Forwarded-For",
         "Forwarded-For",
         "X-Forwarded",
         "Forwarded",
         "X-Real-IP",
         "X-ProxyUser-IP",
         "X-Original-URL",
         "X-Rewrite-URL",
         "Via",
         "X-Forwarded-Host",
         "X-Forwarded-Proto",
         "X-Forwarded-Server",
         "X-Forwarded-Port"
      };

      var stringBuilder = new StringBuilder();

      var foundHeader = false;

      foreach (var headerName in headerNames)
      {
         if (forwardedHeaders.TryGetValue(headerName, out var headerValue))
         {
            if (foundHeader)
            {
               stringBuilder.Append(", ");
            }

            if (string.IsNullOrEmpty(headerValue))
            {
               foundHeader = false;
               continue;
            }

            stringBuilder.Append($"{headerName}: {headerValue}");
            foundHeader = true;
         }
      }

      return foundHeader ? stringBuilder.ToString() : null;
   }

   public static string TryParseLatitude(this HttpContext httpContext)
   {
      var latitude = "0";

      httpContext.Request.Headers.TryGetValue("Latitude", out var latValue);

      if (!string.IsNullOrEmpty(latValue.ToString()))
      {
         latitude = latValue.ToString();
      }

      return latitude;
   }

   public static string TryParseLongitude(this HttpContext httpContext)
   {
      var longitude = "0";

      httpContext.Request.Headers.TryGetValue("Longitude", out var longValue);

      if (!string.IsNullOrEmpty(longValue.ToString()))
      {
         longitude = longValue.ToString();
      }

      return longitude;
   }

   public static decimal TryParseAccuracy(this HttpContext httpContext)
   {
      var accuracy = 0m;

      httpContext.Request.Headers.TryGetValue("Accuracy", out var accValue);


      if (!string.IsNullOrEmpty(accValue))
      {
         return accuracy;
      }

      decimal.TryParse(accValue, out accuracy);

      return accuracy;
   }

   public static string TryParseUserNetworkAddress(this HttpContext httpContext)
   {
      string[] headersToCheck = ["X-Forwarded-For", "Forwarded", "X-Real-IP"];

      foreach (var header in headersToCheck)
      {
         var ipAddress = ExtractIpAddressFromHeader(httpContext, header);
         if (IsValidIpAddress(ipAddress))
         {
            return ipAddress!;
         }
      }

      return IsValidIpAddress(httpContext.Connection.RemoteIpAddress?.ToString() ?? "")
         ? httpContext.Connection.RemoteIpAddress!.ToString()
         : DefaultIpAddress;
   }

   private static string? ExtractIpAddressFromHeader(HttpContext httpContext, string headerName)
   {
      if (!httpContext.Request.Headers.TryGetValue(headerName, out var value))
      {
         return null;
      }

      if (headerName != "Forwarded")
      {
         return value.ToString().Split(',').FirstOrDefault()?.Trim();
      }

      var forwardedValues = value.ToString().Split(';').Select(p => p.Trim());
      var forValue = forwardedValues.FirstOrDefault(p => p.StartsWith("for="));
      if (!string.IsNullOrWhiteSpace(forValue) && forValue.Length > 4)
      {
         return forValue.Substring(4).Trim();
      }

      return null;
   }

   private static bool IsValidIpAddress(string? ipAddress)
   {
      return !string.IsNullOrWhiteSpace(ipAddress) && IPAddress.TryParse(ipAddress, out var parsed)
                                                   && !parsed.IsIPv6UniqueLocal && ipAddress != "::1";
   }
}
