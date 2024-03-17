using BaseConverter.Attributes;

namespace Pandatech.VerticalSlices.DTOs.User
{
    public class UpdatePasswordDto
    {
      [PandaPropertyBaseConverter]

        public long Id { get; set; }
        public string NewPassword { get; set; } = null!;
    }
}
