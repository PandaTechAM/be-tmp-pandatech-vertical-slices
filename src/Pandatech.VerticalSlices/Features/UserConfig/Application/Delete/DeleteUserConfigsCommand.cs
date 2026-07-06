using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.UserConfig.Application.Delete;

public record DeleteUserConfigsCommand(List<string> Keys) : ICommand;
