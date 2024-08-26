using Pandatech.Crypto;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class CryptoExtensions
{
   public static WebApplicationBuilder AddPandaCrypto(this WebApplicationBuilder builder)
   {
      builder.Services.AddPandatechCryptoAes256(o => o.Key = builder.Configuration.GetAesKey());
      builder.Services.AddPandatechCryptoArgon2Id();

      return builder;
   }
}