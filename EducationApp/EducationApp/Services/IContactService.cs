using System.Threading.Tasks;
using EducationApp.Models;

namespace EducationApp.Services
{
    public interface IContactService
    {
        Task SendFeedbackAsync(int sessionId, ScoredFeedback feedback);

        Task SendSubscriptionAsync(int sessionId, Subscription subscription);

        Task SendContactFormAsync(int sessionId, ContactForm contactForm);
    }
}