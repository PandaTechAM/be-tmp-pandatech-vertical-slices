using BaseConverter.Attributes;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.Delete;

public class DeleteUsersCommand : ICommand
{
   [PropertyBaseConverter] public List<long> Ids { get; set; } = null!;
}
