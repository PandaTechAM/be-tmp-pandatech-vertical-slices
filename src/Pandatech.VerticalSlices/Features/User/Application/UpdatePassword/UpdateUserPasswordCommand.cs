using System.Text.Json.Serialization;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.User.Application.UpdatePassword;

public class UpdateUserPasswordCommand : ICommand
{
   [JsonIgnore]
   public long Id { get; set; }

   public required string NewPassword { get; set; }
}