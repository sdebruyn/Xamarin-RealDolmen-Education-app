using System.Threading.Tasks;

namespace EducationApp.Services
{
    public interface IPlatformActionService
    {
        void SendEmail(string receiver, string subject);
        bool CanSendMail();
        Task OpenBrowserAsync(string url);
        Task SetLanguageAsync(LanguageCode language);
    }
}