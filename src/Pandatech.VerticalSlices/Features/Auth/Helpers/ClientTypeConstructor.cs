using Pandatech.VerticalSlices.SharedKernel.Enums;
using ResponseCrafter.StandardHttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Helpers;

public static class ClientTypeConstructor
{
  public static ClientType ConvertToEnum(this string clientType, bool isRequired = true)
  {
    if (clientType == "")
    {
      if (isRequired)
      {
        throw new BadRequestException("Client type is not found in request header");
      }

      return ClientType.Other;
    }

    int.TryParse(clientType, out var clientTypeInt);

    if (clientTypeInt is 0)
    {
      throw new BadRequestException("Client type is not valid");
    }

    var highestEnumNumber = Enum.GetValues(typeof(ClientType)).Length;

    if (clientTypeInt < 1 || clientTypeInt > highestEnumNumber)
    {
      throw new BadRequestException("Client type is not valid");
    }

    return (ClientType)clientTypeInt;
  }
}
