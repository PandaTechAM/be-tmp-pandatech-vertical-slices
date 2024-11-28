using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.User.Application.Delete;

public record DeleteUsersCommand(string Filter) : ICommand;