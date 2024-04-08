using BaseConverter;
using Pandatech.Crypto;
using PandaTech.IEnumerableFilters.Extensions;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.Infrastructure.Extensions;

public static class CryptoExtensions
{
   public static WebApplicationBuilder AddPandaCryptoAndFilters(this WebApplicationBuilder builder)
   {
      builder.ConfigureBaseConverter(builder.Configuration[ConfigurationPaths.Base36Chars]!);
      builder.ConfigureEncryptedConverter(builder.Configuration[ConfigurationPaths.AesKey]!);
      builder.Services.AddPandatechCryptoAes256(o => o.Key = builder.Configuration[ConfigurationPaths.AesKey]!);
      builder.Services.AddPandatechCryptoArgon2Id();

      return builder;
   }
}
