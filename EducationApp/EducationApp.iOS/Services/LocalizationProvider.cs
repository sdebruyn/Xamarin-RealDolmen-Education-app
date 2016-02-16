using EducationApp.Services;
using Foundation;

namespace EducationApp.iOS.Services
{
    public class LocalizationProvider : ILocalizedStringProvider
    {
        public string GetLocalizedString(Localized identifier) => GetLocalizedString(identifier.ToString());
        public string GetLocalizedString(LanguageCode identifier) => GetLocalizedString(identifier.ToString());

        private static string GetLocalizedString(string strIdent)
            => NSBundle.MainBundle.LocalizedString(strIdent, strIdent);
    }
}