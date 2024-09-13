using System.Net;
using Polly;
using Polly.CircuitBreaker;
using Polly.Registry;
using Polly.Retry;

namespace Pandatech.VerticalSlices.SharedKernel.Helpers;

public static class ResilienceDefaultPipelineProvider
{
   public static ResiliencePipeline GetDefaultPipeline(
      this ResiliencePipelineProvider<string> resiliencePipelineProvider)
   {
      return resiliencePipelineProvider.GetPipeline(DefaultPipelineName);
   }

   internal const string DefaultPipelineName = "DefaultPipeline";

   internal static RetryStrategyOptions TooManyRequestsRetryOptions =>
      new()
      {
         MaxRetryAttempts = 5,
         BackoffType = DelayBackoffType.Exponential,
         UseJitter = true,
         Delay = TimeSpan.FromMilliseconds(3000),
         ShouldHandle = new PredicateBuilder()
            .Handle<HttpRequestException>(exception => exception.StatusCode == HttpStatusCode.TooManyRequests)
      };

   internal static RetryStrategyOptions DefaultNetworkRetryOptions =>
      new()
      {
         MaxRetryAttempts = 7,
         BackoffType = DelayBackoffType.Exponential,
         UseJitter = true,
         Delay = TimeSpan.FromMilliseconds(800),
         ShouldHandle = new PredicateBuilder()
            .Handle<HttpRequestException>(exception => exception.StatusCode == HttpStatusCode.RequestTimeout ||
                                                       (int)exception.StatusCode! >= 500)
      };

   internal static CircuitBreakerStrategyOptions DefaultCircuitBreakerOptions =>
      new()
      {
         FailureRatio = 0.5,
         SamplingDuration = TimeSpan.FromSeconds(30),
         MinimumThroughput = 200,
         BreakDuration = TimeSpan.FromSeconds(45),
         ShouldHandle = new PredicateBuilder().Handle<Exception>()
      };
}