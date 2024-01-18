namespace PandaWebApi.DTOs.UserToken;

public class IdentifyTokenDto
{
    public long TokenId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string AccessTokenSignature { get; set; } = null!;
    public IdentifyUserDto User { get; set; } = null!;
}