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
      AddDefaultOrderBy("FullName");
   }
}
