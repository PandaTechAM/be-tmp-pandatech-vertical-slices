using Pandatech.Crypto;

namespace Pandatech.VerticalSlices.SharedKernel.Helpers;

public static class PasswordHelper
{
   public const string WrongPasswordMessage =
      "password_must_include_at_least_10_characters_1_uppercase_1_lowercase_1_special_char_and_1_digit";

   public static bool ValidatePassword(this string password)
   {
      return Password.Validate(password, 10, true, true, true, true);
   }

   public static string SecureGenerate()
   {
      return Password.GenerateRandom(24, true, true, true, true);
   }
}
