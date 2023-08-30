using System.Text.RegularExpressions;

namespace PandaWebApi.Helpers
{
    public static class RegExHelper
    {
        private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(500);

        private static readonly Regex RegexValidEmail = 
            new Regex(@"^[\w-_]+(\.[\w!#$%'*+\/=?\^`{|}]+)*@((([\-\w]+\.)+[a-zA-Z]{2,20})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", 
                      RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, 
                      RegexTimeout);

        private static readonly Regex RegexValidUserName = 
            new Regex(@"^[a-zA-Z0-9_]{5,20}$", 
                      RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, 
                      RegexTimeout);

        private static readonly Regex RegexValidPhone = 
            new Regex(@"^\+?\d{1,4}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}$", 
                      RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, 
                      RegexTimeout);

        private static readonly Regex RegexValidPassword = 
            new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", 
                      RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, 
                      RegexTimeout);

        private static readonly Regex RegexValidSocialCard = 
            new Regex(@"^(?:-(?:[1-9](?:\d{0,2}(?:,\d{3})+|\d*))|(?:0|(?:[1-9](?:\d{0,2}(?:,\d{3})+|\d*))))(?:.\d+|)$", 
                      RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, 
                      RegexTimeout);

        private static readonly Regex RegexValidSecureUrl = 
            new Regex(@"^https:\/\/[a-zA-Z0-9-]+\.[a-zA-Z0-9-]+$", 
                      RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, 
                      RegexTimeout);

        private static readonly Regex RegexValidUrlWithSecureWildcard = 
            new Regex(@"^https:\/\/\*\.[a-zA-Z0-9-]+\.[a-zA-Z0-9-]+$", 
                      RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, 
                      RegexTimeout);

        public static bool IsValidSecureUri(string uri, bool allowWildcards)
        {
            return allowWildcards 
                ? RegexValidSecureUrl.IsMatch(uri) || RegexValidUrlWithSecureWildcard.IsMatch(uri) 
                : RegexValidSecureUrl.IsMatch(uri);
        }

        public static bool IsValidEmail(string email)
        {
            return RegexValidEmail.IsMatch(email);
        }

        public static bool IsValidPhone(string phone)
        {
            return RegexValidPhone.IsMatch(phone);
        }

        public static bool IsValidUserName(string userName)
        {
            return RegexValidUserName.IsMatch(userName);
        }

        public static bool IsValidPassword(string password)
        {
            return RegexValidPassword.IsMatch(password);
        }

        public static bool IsValidSocialCard(string socialCardNumber)
        {
            return RegexValidSocialCard.IsMatch(socialCardNumber);
        }
    }
}