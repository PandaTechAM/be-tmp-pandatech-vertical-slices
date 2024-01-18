namespace PandaWebApi.DTOs.UserToken;

public class CookieDto
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
    public DateTime ExpirationDate { get; set; }
    
    public CookieDto(string key, string value, DateTime expirationDate)
    {
        Key = key;
        Value = value;
        ExpirationDate = expirationDate;
    }
}