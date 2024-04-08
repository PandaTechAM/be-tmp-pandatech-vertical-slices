using System.Text.Json.Serialization;
using Pandatech.VerticalSlices.Features.User.Contracts.GetById;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.GetById;

public class GetUserByIdV1Query(long id) : IQuery<GetUserByIdV1QueryResponse>
{
   [JsonIgnore] public long Id { get; set; } = id;
}
