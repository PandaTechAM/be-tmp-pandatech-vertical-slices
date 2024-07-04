using Pandatech.VerticalSlices.SharedKernel.Enums;

namespace Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;

public class MetaData
{
   public required string RequestId { get; set; }
   public DateTime RequestTime { get; set; }
   public SupportedLanguageType LanguageId { get; set; }
   public ClientType ClientType { get; set; }
}
