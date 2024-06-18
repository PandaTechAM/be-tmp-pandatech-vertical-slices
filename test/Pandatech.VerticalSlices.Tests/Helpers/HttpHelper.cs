namespace Pandatech.VerticalSlices.Tests.Helpers;

public class HttpHelper
{
   internal static class Urls
   {
      private const string BaseUrl = "https://localhost:5001/";
      private const string Version1 = "api/v1";

      public const string V1PostSomething = $"{BaseUrl + Version1}/somethings";
   }
}
