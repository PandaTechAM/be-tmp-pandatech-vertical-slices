using BaseConverter.Attributes;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.Delete;

public class DeleteUsersV1Command : ICommand
{
   [PandaPropertyBaseConverter] public List<long> Ids { get; set; } = null!;
}
