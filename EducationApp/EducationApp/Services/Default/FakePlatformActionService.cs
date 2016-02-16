using System.Diagnostics;
using System.Threading.Tasks;

namespace EducationApp.Services.Default
{
    public class FakePlatformActionService : IPlatformActionService
    {
        public void SendEmail(string receiver, string subject)
            => Debug.WriteLine($"Email sent about {subject} to {receiver}!");

        public bool CanSendMail() => true;
        public Task OpenBrowserAsync(string url) => Task.Run(() => Debug.WriteLine("Debug browser open on " + url));

        public Task SetLanguageAsync(LanguageCode language)
            => Task.Run(() => Debug.WriteLine($"Language set to {language}."));
    }
}