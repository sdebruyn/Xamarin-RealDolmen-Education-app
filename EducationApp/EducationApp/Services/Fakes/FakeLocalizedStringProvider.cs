namespace EducationApp.Services.Fakes
{
    public class FakeLocalizedStringProvider : ILocalizedStringProvider
    {
        public string GetLocalizedString(Localized identifier) => identifier.ToString();
        public string GetLocalizedString(LanguageCode identifier) => identifier.ToString();
    }
}