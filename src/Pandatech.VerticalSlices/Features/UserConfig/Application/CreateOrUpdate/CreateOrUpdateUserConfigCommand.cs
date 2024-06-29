using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.UserConfig.Application.CreateOrUpdate;

public record CreateOrUpdateUserConfigCommand(Dictionary<string, string> Configs) : ICommand;
