using System.Globalization;
using Pandatech.VerticalSlices.SharedKernel.Enums;

namespace Pandatech.VerticalSlices.SharedKernel.Helpers;

public static class LanguageHelper
{
   public static SupportedLanguageType GetLanguage(this string language)
   {
      return language switch
      {
         "en-US" => SupportedLanguageType.EnglishUs,
         "ru-RU" => SupportedLanguageType.Russian,
         "hy-AM" => SupportedLanguageType.Armenian,
         _ => SupportedLanguageType.EnglishUs
      };
   }

   //this one is the best practice way of handling
   public static void SetLanguage(SupportedLanguageType language)
   {
      var cultureInfo = language switch
      {
         SupportedLanguageType.EnglishUs => new CultureInfo("en-US"),
         SupportedLanguageType.Russian => new CultureInfo("ru-RU"),
         SupportedLanguageType.Armenian => new CultureInfo("hy-AM"),
         _ => CultureInfo.InvariantCulture
      };

      CultureInfo.CurrentCulture = cultureInfo;
      CultureInfo.CurrentUICulture = cultureInfo;
   }
}
