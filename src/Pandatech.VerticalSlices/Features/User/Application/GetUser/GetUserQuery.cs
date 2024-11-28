using System.Text.Json.Serialization;
using Pandatech.VerticalSlices.Features.User.Contracts.GetUser;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.User.Application.GetUser;

public class GetUserQuery(long id) : IQuery<GetUserQueryResponse>
{
   [JsonIgnore]
   public long Id { get; set; } = id;
}