using BaseConverter;
using GridifyExtensions.Models;
using Pandatech.VerticalSlices.Domain.Entities;
using Pandatech.VerticalSlices.Domain.Enums;

namespace Pandatech.VerticalSlices.Context.EntityFilters;

public class UserEntityFilters : FilterMapper<User>
{
   public UserEntityFilters()
   {
      AddMap("Id", x => x.Id, x => PandaBaseConverter.Base36ToBase10NotNull(x));
      AddMap("Username", x => x.Username);
      AddMap("FullName", x => x.FullName);
      AddMap("Role", x => x.Role != UserRole.SuperAdmin);
      AddMap("Status", x => x.Status);
      AddMap("Comment", x => x.Comment);

      AddDefaultOrderBy("FullName");
   }
}
