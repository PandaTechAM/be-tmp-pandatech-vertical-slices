using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.UserConfig.Application.Delete;

public record DeleteUserConfigsCommand(List<string> Keys) : ICommand;
