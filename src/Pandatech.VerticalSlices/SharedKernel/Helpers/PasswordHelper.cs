using Pandatech.Crypto;

namespace Pandatech.VerticalSlices.SharedKernel.Helpers;

public static class PasswordHelper
{
   public const string WrongPasswordMessage =
      "password_must_include_at_least_8_characters_1_uppercase_1_lowercase_and_1_digit";

   public static bool ValidatePassword(this string password)
   {
      return Password.Validate(password, 8, true, true, true, false);
   }

   public static string SecureGenerate()
   {
      return Password.GenerateRandom(24, true, true, true, true);
   }
}