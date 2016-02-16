using System.Collections.Generic;
using System.Threading.Tasks;
using EducationApp.Models;
using Refit;

namespace EducationApp.Services
{
    [Headers("Accept: application/json")]
    public interface IEducationApi
    {
        [Get("/categories")]
        Task<ICollection<Category>> GetCategoriesAsync();

        [Get("/categories/{categoryId}?expand=subcategories")]
        Task<Category> GetCategoryDetailsAsync(int categoryId);

        [Get("/courses")]
        Task<ICollection<Course>> SearchCoursesAsync([AliasAs("q")] string query, string language,
            string fields = null, string status = "isactive,incatalog", string expand = null, int page = 1,
            int pageSize = Constants.Internet.DefaultResultsPerPage,
            string sort = "Title");

        [Post("/sessions/{sessionId}/feedbacks")]
        [Headers("Authorization: Bearer")]
        Task SendFeedbackAsync(int sessionId, [Body] ScoredFeedback feedback);

        [Post("/sessions/{sessionId}/subscribtions")]
        [Headers("Authorization: Bearer")]
        Task SendSubscriptionAsync(int sessionId, [Body] Subscription subscription);

        [Post("/sessions/{sessionId}/contact")]
        [Headers("Authorization: Bearer")]
        Task SendContactFormAsync(int sessionId, [Body] ContactForm contactForm);

        [Get("/subcategories/{subcategoryId}/courses")]
        Task<ICollection<Course>> GetCategoryCoursesAsync(int subcategoryId, string language,
            int page = 1, int pageSize = Constants.Internet.DefaultResultsPerPage, string fields = null,
            string expand = null);

        [Get("/courses/{courseId}")]
        Task<Course> GetCourseDetailsAsync(int courseId, string language, int page = 1,
            int pageSize = Constants.Internet.DefaultResultsPerPage, string expand = "instructor,descriptions");

        [Get("/courses/{courseId}/sessions")]
        Task<ICollection<Session>> GetCourseSessionsAsync(int courseId, int page = 1,
            int pageSize = Constants.Internet.DefaultResultsPerPage, string fields = null,
            string expand = "Sessionschedules,Location,Classroom");
    }
}