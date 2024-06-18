using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.Delete;

public class DeleteUsersCommand : ICommand
{
   public List<string> Ids { get; set; } = null!;
}
