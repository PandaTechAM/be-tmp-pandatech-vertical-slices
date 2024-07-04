using BaseConverter.Attributes;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.Delete;

public class DeleteUsersCommand : ICommand
{
   [PropertyBaseConverter] public required List<long> Ids { get; set; }
}
