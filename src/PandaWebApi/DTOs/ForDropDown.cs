using System.ComponentModel.DataAnnotations;

namespace PandaVault.DTOs;

public class ForDropDown
{
   [Required] 
   public long Id { get; set; }
   [Required]
   public string Name { get; set; } = null!;
}