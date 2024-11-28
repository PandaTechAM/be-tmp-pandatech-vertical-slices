using GridifyExtensions.Models;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.User.Application.GetColumnDistinctValues;

public class GetUserColumnDistinctValuesQuery : ColumnDistinctValueCursoredQueryModel, IQuery<CursoredResponse<object>>;