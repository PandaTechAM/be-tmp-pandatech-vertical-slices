using System.Net;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Pandatech.VerticalSlices.SharedKernel.Helpers;

internal static class ResilienceHttpOptions
{
   public static HttpRetryStrategyOptions DefaultTooManyRequestsRetryOptions =>
      new()
      {
         MaxRetryAttempts = 5,
         BackoffType = DelayBackoffType.Exponential,
         UseJitter = true,
         Delay = TimeSpan.FromMilliseconds(3000),
         ShouldHandle = args =>
         {
            if (args.Outcome.Exception is HttpRequestException httpException)
            {
               return ValueTask.FromResult((int)httpException.StatusCode! == 429);
            }

            if (args.Outcome.Result is not null && args.Outcome.Result.StatusCode == HttpStatusCode.TooManyRequests)
            {
               return ValueTask.FromResult(true);
            }

            return ValueTask.FromResult(false);
         },
         DelayGenerator = args =>
         {
            if (args.Outcome.Result is null)
            {
               return ValueTask.FromResult<TimeSpan?>(null);
            }

            if (!args.Outcome.Result.Headers.TryGetValues("Retry-After", out var values))
            {
               return ValueTask.FromResult<TimeSpan?>(null);
            }

            var retryAfterValue = values.FirstOrDefault();

            if (int.TryParse(retryAfterValue, out var retryAfterSeconds))
            {
               return ValueTask.FromResult<TimeSpan?>(TimeSpan.FromSeconds(retryAfterSeconds));
            }

            if (!DateTimeOffset.TryParseExact(retryAfterValue,
                   "R", // RFC1123 pattern
                   System.Globalization.CultureInfo.InvariantCulture,
                   System.Globalization.DateTimeStyles.None,
                   out var retryAfterDate))
            {
               return ValueTask.FromResult<TimeSpan?>(null);
            }

            var retryDelay = retryAfterDate - DateTimeOffset.UtcNow;
            return ValueTask.FromResult<TimeSpan?>(retryDelay > TimeSpan.Zero ? retryDelay : TimeSpan.MinValue);
         }
      };

   public static HttpRetryStrategyOptions DefaultNetworkRetryOptions =>
      new()
      {
         MaxRetryAttempts = 7,
         BackoffType = DelayBackoffType.Exponential,
         UseJitter = true,
         Delay = TimeSpan.FromMilliseconds(800),
         ShouldHandle = args =>
         {
            if (args.Outcome.Exception is HttpRequestException httpException)
            {
               return ValueTask.FromResult((int)httpException.StatusCode! >= 500 ||
                                           (int)httpException.StatusCode! == 408);
            }

            return ValueTask.FromResult(args.Outcome.Result is not null &&
                                        (args.Outcome.Result.StatusCode == HttpStatusCode.RequestTimeout ||
                                         (int)args.Outcome.Result.StatusCode >= 500));
         }
      };

   public static HttpCircuitBreakerStrategyOptions DefaultCircuitBreakerOptions =>
      new()
      {
         FailureRatio = 0.5,
         SamplingDuration = TimeSpan.FromSeconds(30),
         MinimumThroughput = 200,
         BreakDuration = TimeSpan.FromSeconds(45),
         ShouldHandle = args =>
         {
            if (args.Outcome.Exception is not null)
            {
               return ValueTask.FromResult(true);
            }

            return args.Outcome.Result is null
               ? ValueTask.FromResult(false)
               : ValueTask.FromResult(!args.Outcome.Result.IsSuccessStatusCode);
         }
      };
}