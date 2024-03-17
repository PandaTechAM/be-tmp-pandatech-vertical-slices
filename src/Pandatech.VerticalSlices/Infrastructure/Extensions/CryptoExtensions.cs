using BaseConverter;
using Pandatech.Crypto;
using PandaTech.IEnumerableFilters.Extensions;

namespace Pandatech.VerticalSlices.Infrastructure.Extensions;

public static class CryptoExtensions
{
  public static WebApplicationBuilder AddPandaCryptoAndFilters(this WebApplicationBuilder builder)
  {

    builder.ConfigureBaseConverter(builder.Configuration["Security:Base36Chars"]!);
    builder.ConfigureEncryptedConverter(builder.Configuration["Security:AesKey"]!);
    builder.Services.AddPandatechCryptoAes256(o => o.Key = builder.Configuration["Security:AesKey"]!);
    builder.Services.AddPandatechCryptoArgon2Id();

    return builder;
  }
}
