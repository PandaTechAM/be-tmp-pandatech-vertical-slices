using GridifyExtensions.Models;
using Pandatech.VerticalSlices.Features.User.Contracts.GetUser;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.GetUsers;

public class GetUsersQuery : GridifyQueryModel, IQuery<PagedResponse<GetUserQueryResponse>>;
