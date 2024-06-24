using GridifyExtensions.Models;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.GetColumnDistinctValues;

public class GetUserColumnDistinctValuesQuery : ColumnDistinctValueCursoredQueryModel, IQuery<CursoredResponse<object>>;
