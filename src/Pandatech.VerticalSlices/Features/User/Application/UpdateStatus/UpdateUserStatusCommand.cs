using System.Text.Json.Serialization;
using Pandatech.VerticalSlices.Domain.Enums;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.User.Application.UpdateStatus;

public class UpdateUserStatusCommand : ICommand
{
    [JsonIgnore]
    public long Id { get; set; }

    public UserStatus Status { get; set; }
}
