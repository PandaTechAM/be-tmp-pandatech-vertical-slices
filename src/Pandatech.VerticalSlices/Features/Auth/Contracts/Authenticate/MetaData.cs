using Pandatech.VerticalSlices.SharedKernel.Enums;

namespace Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;

public class MetaData
{
   public string RequestId { get; set; } = null!;
   public DateTime RequestTime { get; set; }
   public SupportedLanguageType LanguageId { get; set; }
   public ClientType ClientType { get; set; }
}