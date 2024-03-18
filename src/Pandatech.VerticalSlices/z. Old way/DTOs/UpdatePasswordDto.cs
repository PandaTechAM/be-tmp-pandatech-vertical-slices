using BaseConverter.Attributes;

namespace Pandatech.VerticalSlices.z._Old_way.DTOs
{
    public class UpdatePasswordDto
    {
      [PandaPropertyBaseConverter]

        public long Id { get; set; }
        public string NewPassword { get; set; } = null!;
    }
}
