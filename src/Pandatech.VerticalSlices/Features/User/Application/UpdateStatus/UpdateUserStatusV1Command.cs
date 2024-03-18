using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BaseConverter.Attributes;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.UpdateStatus
{
   public class UpdateUserStatusV1Command : ICommand
   {
      [PandaPropertyBaseConverter]
      [JsonIgnore]
      public long Id { get; set; }

      public UserStatus Status { get; set; }
   }
}
