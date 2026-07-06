using GridifyExtensions.Models;
using Pandatech.VerticalSlices.Features.User.Contracts.GetUser;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.User.Application.GetUsers;

public class GetUsersQuery : GridifyQueryModel, IQuery<PagedResponse<GetUserQueryResponse>>;
