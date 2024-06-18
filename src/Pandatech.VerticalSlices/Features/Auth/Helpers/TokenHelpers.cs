namespace Pandatech.VerticalSlices.Features.Auth.Helpers;

public static class TokenHelpers
{
   public const int AccessTokenExpirationMinutes = 10;

   public static int SetRefreshTokenExpirationMinutes(IConfiguration configuration)
   {
      var refreshTokenExpirationMinutes = configuration.GetValue<int>("Security:RefreshTokenExpirationMinutes");

      return refreshTokenExpirationMinutes == 0
         ? 1440
         : Math.Max(refreshTokenExpirationMinutes, 60);
   }

   public static int SetRefreshTokenMaxExpirationMinutes(IConfiguration configuration)
   {
      var refreshTokenMaxExpirationMinutes = configuration.GetValue<int>("Security:RefreshTokenMaxExpirationMinutes");

      return refreshTokenMaxExpirationMinutes == 0
         ? int.MaxValue
         : Math.Max(refreshTokenMaxExpirationMinutes, SetRefreshTokenExpirationMinutes(configuration));
   }
}
