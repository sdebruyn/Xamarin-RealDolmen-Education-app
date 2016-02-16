using Windows.ApplicationModel.Resources;
using EducationApp.Exceptions;
using EducationApp.Extensions;
using EducationApp.Services;

namespace EducationApp.WinPhone.Services
{
    public sealed class LocalizationProvider : ILocalizedStringProvider
    {
        private readonly ResourceLoader _loader;
        private readonly ILoggingService _loggingService;

        public LocalizationProvider(ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _loader = new ResourceLoader();
        }

        public string GetLocalizedString(Localized enumIdentifier)
        {
            var identifier = enumIdentifier.ToString().Replace('_', '.');
            var localizedString = GetLocalizedString(identifier);
            if (localizedString.IsNullOrEmpty())
            {
                _loggingService.Report(new LocalizationException(enumIdentifier), this);
            }
            return localizedString;
        }

        public string GetLocalizedString(LanguageCode identifier) => GetLocalizedString(identifier.ToString());

        private string GetLocalizedString(string identifier) => _loader.GetString(identifier);
    }
}