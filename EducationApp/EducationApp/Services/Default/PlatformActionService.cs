using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Plugin.Messaging;
using Plugin.Share;

namespace EducationApp.Services.Default
{
    public class PlatformActionService : IPlatformActionService
    {
        private readonly IBlobCache _cache;

        public PlatformActionService()
        {
            _cache = BlobCache.UserAccount;
        }

        public void SendEmail(string receiver, string subject)
        {
            if (CanSendMail())
            {
                MessagingPlugin.EmailMessenger.SendEmail(receiver, subject);
            }
        }

        public bool CanSendMail() => MessagingPlugin.EmailMessenger.CanSendEmail;

        public Task OpenBrowserAsync(string url) => CrossShare.Current.OpenBrowser(url);

        public async Task SetLanguageAsync(LanguageCode language)
        {
            await _cache.InvalidateAll();
            await _cache.InsertObject(nameof(LanguageCode), language.ToString(), null);
        }
    }
}