using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PandaWebApi.Models;

public class Token
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid TokenString { get; set; }

    public User User { get; set; } = null!;
    public long UserId { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
}