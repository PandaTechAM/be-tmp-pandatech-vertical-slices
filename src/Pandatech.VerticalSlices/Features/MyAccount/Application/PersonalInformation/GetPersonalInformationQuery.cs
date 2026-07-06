using Pandatech.VerticalSlices.Features.MyAccount.Contracts;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.MyAccount.Application.PersonalInformation;

public record GetPersonalInformationQuery : IQuery<GetPersonalInformationQueryResponse>;
