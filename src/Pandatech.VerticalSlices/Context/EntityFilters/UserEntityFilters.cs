using BaseConverter;
using GridifyExtensions.Models;
using Pandatech.VerticalSlices.Domain.Entities;
using Pandatech.VerticalSlices.Domain.Enums;

namespace Pandatech.VerticalSlices.Context.EntityFilters;

public class UserEntityFilters : FilterMapper<User>
{
   public UserEntityFilters()
   {
      GenerateMappings();
      AddMap("Role", x => x.Role != UserRole.SuperAdmin);
      AddMap("Id", x => x.Id, x => PandaBaseConverter.Base36ToBase10NotNull(x));
      AddMap("Username", x => x.Username.ToLower(), x => x.ToLower());
      AddMap("FullName", x => x.FullName.ToLower(), x => x.ToLower());
      AddMap("Comment", x => x.Comment.ToLower(), x => x.ToLower());
      AddDefaultOrderBy("FullName");
   }
}
