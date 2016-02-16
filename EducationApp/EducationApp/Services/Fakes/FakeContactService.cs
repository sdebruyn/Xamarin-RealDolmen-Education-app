using System.Diagnostics;
using System.Threading.Tasks;
using EducationApp.Models;
using Newtonsoft.Json;

namespace EducationApp.Services.Fakes
{
    public class FakeContactService : IContactService
    {
        public Task SendFeedbackAsync(int sessionId, ScoredFeedback feedback) => ToDebugAsync(sessionId, feedback);

        public Task SendSubscriptionAsync(int sessionId, Subscription subscription)
            => ToDebugAsync(sessionId, subscription);

        public Task SendContactFormAsync(int sessionId, ContactForm contactForm) => ToDebugAsync(sessionId, contactForm);

        private static Task ToDebugAsync(int sessionId, object content)
            =>
                Task.Run(
                    () =>
                        Debug.WriteLine(
                            $"Received contact info for session {sessionId}: {JsonConvert.SerializeObject(content)}"));
    }
}