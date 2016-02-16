using System.Collections.Generic;
using System.Threading.Tasks;
using EducationApp.Models;

namespace EducationApp.Services
{
    public interface ICategoryService
    {
        Task<ICollection<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryDetailsAsync(int categoryId);
        Task FetchSpeculativelyAsync();
    }
}