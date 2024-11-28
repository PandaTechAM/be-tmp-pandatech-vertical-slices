using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.UserConfig.Application.CreateOrUpdate;

public record CreateOrUpdateUserConfigCommand(Dictionary<string, string> Configs) : ICommand;