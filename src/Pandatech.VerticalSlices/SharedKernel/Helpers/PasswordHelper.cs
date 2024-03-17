using Pandatech.Crypto;

namespace Pandatech.VerticalSlices.SharedKernel.Helpers;

public static class PasswordHelper
{
  public static bool ValidatePassword(this string password)
  {
    return Password.Validate(password, 8, true, true, true, false);
  }

  public const string WrongPasswordMessage = "password_must_include_at_least_8_characters_1_uppercase_1_lowercase_and_1_digit";

  public static string SecureGenerate() => Password.GenerateRandom(24, true, true, true, true);
}
