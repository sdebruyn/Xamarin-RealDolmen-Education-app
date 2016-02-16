using System.Collections.Generic;
using System.Threading.Tasks;
using EducationApp.Models;

namespace EducationApp.Services
{
    public interface ICourseService
    {
        /// <summary>
        ///     Search for courses.
        /// </summary>
        /// <param name="query">Part of the title of the courses that should be found.</param>
        /// <returns>A list of courses.</returns>
        Task<ICollection<Course>> SearchCoursesAsync(string query);

        Task FetchSpeculativelyAsync(int courseId);
        Task FetchCoursesSpeculativelyAsync(params Category[] subcategories);

        Task<Course> GetCourseDetailsAsync(int courseId);

        Task<ICollection<Course>> GetCategoryCoursesAsync(int categoryId, bool speculatively = false);

        Task UpdateCourseSessionsAsync(Course course, bool speculatively = false);
    }
}