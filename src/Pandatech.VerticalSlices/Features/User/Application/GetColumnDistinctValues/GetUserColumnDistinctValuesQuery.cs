using GridifyExtensions.Models;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.GetColumnDistinctValues;

public class GetUserColumnDistinctValuesQuery : ColumnDistinctValueQueryModel, IQuery<PagedResponse<object>>;
