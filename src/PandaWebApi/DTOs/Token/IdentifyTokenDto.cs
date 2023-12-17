using System.Diagnostics.CodeAnalysis;

namespace PandaWebApi.DTOs.Token;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class IdentifyTokenDto
{
    public long TokenId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string TokenSignature { get; set; } = null!;
    public IdentifyUserDto User { get; set; } = null!;
}