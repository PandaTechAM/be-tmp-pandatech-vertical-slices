namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class StringExtensions
{
   public static string RemovePhoneFormatParenthesesAndAdditionSign(this string phoneString)
   {
      return phoneString.Replace("(", "").Replace(")", "").Replace("+", "");
   }
}
