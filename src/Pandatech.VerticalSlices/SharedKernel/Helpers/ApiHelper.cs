namespace Pandatech.VerticalSlices.SharedKernel.Helpers;

public static class ApiHelper
{
  private const string BaseApiPath = "/api/v";

  
  public static string GetRoutePrefix(int version, string baseRoute)
  {
    return $"{BaseApiPath}{version}{baseRoute}";
  }

  public const string GroupNameMain = "MainV1";
  
}
