using System.Net;
using System.Text;
using Pandatech.VerticalSlices.Features.Auth.Helpers;
using Pandatech.VerticalSlices.SharedKernel.Enums;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class HttpContextExtensions
{
    private const string DefaultIpAddress = "0.0.0.0";

    private static string? ExtractIpAddressFromHeader(HttpContext httpContext, string headerName)
    {
        if (!httpContext.Request.Headers.TryGetValue(headerName, out var value))
        {
            return null;
        }

        if (headerName != "Forwarded")
        {
            return value.ToString()
                .Split(',')
                .FirstOrDefault()
                ?.Trim();
        }

        var forwardedValues = value.ToString()
            .Split(';')
            .Select(p => p.Trim());
        var forValue = forwardedValues.FirstOrDefault(p => p.StartsWith("for="));
        if (!string.IsNullOrWhiteSpace(forValue) && forValue.Length > 4)
        {
            return forValue.Substring(4)
                .Trim();
        }

        return null;
    }

    private static bool IsValidIpAddress(string? ipAddress)
    {
        return !string.IsNullOrWhiteSpace(ipAddress) && IPAddress.TryParse(ipAddress, out var parsed)
                                                     && !parsed.IsIPv6UniqueLocal && ipAddress != "::1";
    }

    extension(HttpContext httpContext)
    {
        public string TryParseAccessTokenSignature(IHostEnvironment environment)
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

        public string TryParseRefreshTokenSignature(IHostEnvironment environment)
        {
            var refreshTokenName = CookieHelper.FormatCookieName("refresh_token", environment);
            var refreshTokenSignature = httpContext.Request.Cookies[refreshTokenName];

            if (!string.IsNullOrEmpty(refreshTokenSignature))
            {
                return refreshTokenSignature;
            }

            refreshTokenSignature = httpContext.Request
                .Headers["refresh-token"]
                .ToString();

            return refreshTokenSignature;
        }

        public string TryParseClientType()
        {
            var clientType = httpContext.Request
                .Headers["client-type"]
                .ToString();
            return clientType;
        }

        public string TryParseRequestId()
        {
            var requestId = httpContext.Request.Headers.RequestId.ToString();
            return requestId;
        }

        public string TryParseUniqueIdPerDevice(IHostEnvironment environment)
        {
            var deviceCookie = CookieHelper.FormatCookieName("device", environment);
            var uniqueIdPerDevice = httpContext.Request.Cookies[deviceCookie];

            if (string.IsNullOrEmpty(uniqueIdPerDevice))
            {
                uniqueIdPerDevice = httpContext.Request
                    .Headers["device"]
                    .ToString();
            }

            return uniqueIdPerDevice;
        }

        public string TryParseDeviceName()
        {
            return httpContext.Request
                .Headers["device-name"]
                .ToString();
        }

        public SupportedLanguageType TryParseLanguageId()
        {
            return httpContext.Request
                .Headers
                .AcceptLanguage
                .ToString()
                .GetLanguage();
        }

        public string TryParseUserAgent()
        {
            return httpContext.Request.Headers.UserAgent.ToString();
        }

        public string? TryParseForwardedHeaders()
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
                "X-Forwarded-Port",
                "CF-Connecting-IP"
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

        public string TryParseLatitude()
        {
            var latitude = "0";

            httpContext.Request.Headers.TryGetValue("Latitude", out var latValue);

            if (!string.IsNullOrEmpty(latValue.ToString()))
            {
                latitude = latValue.ToString();
            }

            return latitude;
        }

        public string TryParseLongitude()
        {
            var longitude = "0";

            httpContext.Request.Headers.TryGetValue("Longitude", out var longValue);

            if (!string.IsNullOrEmpty(longValue.ToString()))
            {
                longitude = longValue.ToString();
            }

            return longitude;
        }

        public decimal TryParseAccuracy()
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

        public string TryParseUserNetworkAddress()
        {
            // Check at first Cloudflare "CF-Connecting-IP" header, which contains client real IP address
            string[] headersToCheck = ["CF-Connecting-IP", "X-Forwarded-For", "Forwarded", "X-Real-IP"];

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
    }
}
