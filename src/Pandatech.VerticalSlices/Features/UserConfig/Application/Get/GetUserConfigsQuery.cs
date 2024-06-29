using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.UserConfig.Application.Get;

public record GetUserConfigsQuery(string[] Keys) : IQuery<Dictionary<string, string>>;
