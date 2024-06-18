using BaseConverter;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.Extensions;

public static class CryptoExtensions
{
   public static WebApplicationBuilder AddPandaCrypto(this WebApplicationBuilder builder)
   {
      builder.ConfigureBaseConverter(builder.Configuration[ConfigurationPaths.Base36Chars]!);
      builder.Services.AddPandatechCryptoAes256(o => o.Key = builder.Configuration[ConfigurationPaths.AesKey]!);
      builder.Services.AddPandatechCryptoArgon2Id();

      return builder;
   }
}