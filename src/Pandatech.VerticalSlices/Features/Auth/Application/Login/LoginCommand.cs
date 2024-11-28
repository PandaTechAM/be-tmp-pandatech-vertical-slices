using System.ComponentModel;
using Pandatech.VerticalSlices.Features.Auth.Contracts.Login;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.VerticalSlices.Features.Auth.Application.Login;

public class LoginCommand : ICommand<LoginCommandResponse>
{
   [DefaultValue("admin@admin.com")]
   public required string Username { get; set; }

   [DefaultValue("Qwertyui123@")]
   public required string Password { get; set; }
}