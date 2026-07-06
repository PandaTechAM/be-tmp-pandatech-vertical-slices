using Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;
using Pandatech.VerticalSlices.Features.Auth.Contracts.Login;
using Pandatech.VerticalSlices.Features.Auth.Contracts.RefreshToken;
using SharedKernel.Extensions;

namespace Pandatech.VerticalSlices.Features.Auth.Helpers;

public static class CookieHelper
{
    private static void CreateSecureCookies(this List<Cookie> cookies,
        HttpContext httpContext,
        string domain,
        IHostEnvironment environment)
    {
        foreach (var cookie in cookies)
        {
            cookie.ExpirationDate ??= DateTime.Now.AddYears(10);

            var cookieOptions = new CookieOptions
            {
                Expires = cookie.ExpirationDate,
                SameSite = SameSiteMode.None,
                Secure = true,
                HttpOnly = true
            };

            if (!environment.IsLocal())
            {
                cookieOptions.Domain = domain;
            }


            if (!environment.IsLocalOrDevelopment())
            {
                cookieOptions.SameSite = SameSiteMode.Strict;
            }


            httpContext.Response.Cookies.Append(cookie.Key, cookie.Value, cookieOptions);
        }
    }

    public static string FormatCookieName(string attributeName, IHostEnvironment environment)
    {
        return !environment.IsProduction()
            ? $"ti_{environment.GetShortEnvironmentName()}_{attributeName}"
            : $"ti_{attributeName}";
    }


    private static void RefreshIdentityCookies(IdentityCookies cookies,
        HttpContext httpContext,
        IHostEnvironment environment,
        string domain)
    {
        List<Cookie> newCookies =
        [
            new(FormatCookieName("access_token", environment),
                cookies.AccessTokenSignature,
                cookies.AccessTokenExpiresAt),
            new(FormatCookieName("refresh_token", environment),
                cookies.RefreshTokenSignature,
                cookies.RefreshTokenExpiresAt)
        ];

        newCookies.CreateSecureCookies(httpContext, domain, environment);
    }

    extension(HttpContext httpContext)
    {
        public void DeleteAllCookies(IHostEnvironment environment, string domain)
        {
            foreach (var cookie in httpContext.Request.Cookies)
            {
                if (cookie.Key.Contains(FormatCookieName("device", environment)))
                {
                    continue; // Skip deletion for cookies containing the unique device ID
                }


                var cookieOptions = new CookieOptions
                {
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None
                };

                if (!environment.IsLocal())
                {
                    cookieOptions.Domain = domain;
                }

                if (!environment.IsLocalOrDevelopment())
                {
                    cookieOptions.SameSite = SameSiteMode.Strict;
                }

                httpContext.Response.Cookies.Delete(cookie.Key, cookieOptions);
            }
        }

        public void PrepareAndSetCookies(RefreshTokenV1CommandResponse mediatorResponse,
            IHostEnvironment environment,
            string domain)
        {
            var cookies = new IdentityCookies
            {
                AccessTokenSignature = mediatorResponse.AccessTokenSignature,
                RefreshTokenSignature = mediatorResponse.RefreshTokenSignature,
                RefreshTokenExpiresAt = mediatorResponse.RefreshTokenExpiration,
                AccessTokenExpiresAt = mediatorResponse.AccessTokenExpiration
            };
            RefreshIdentityCookies(cookies, httpContext, environment, domain);
        }

        public void PrepareAndSetCookies(LoginCommandResponse mediatorResponse,
            IHostEnvironment environment,
            string domain)
        {
            var cookies = new IdentityCookies
            {
                AccessTokenSignature = mediatorResponse.AccessTokenSignature,
                RefreshTokenSignature = mediatorResponse.RefreshTokenSignature,
                RefreshTokenExpiresAt = mediatorResponse.RefreshTokenExpiration,
                AccessTokenExpiresAt = mediatorResponse.AccessTokenExpiration
            };
            RefreshIdentityCookies(cookies, httpContext, environment, domain);
        }
    }
}
