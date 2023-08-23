using System.ComponentModel.DataAnnotations;

namespace PandaWebApi.Models;

public class SomeModel
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}