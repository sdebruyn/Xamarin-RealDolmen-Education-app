using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EducationApp.Extensions;
using EducationApp.Models;

namespace EducationApp.Services.Fakes
{
    public class FakeCategoryService : ICategoryService
    {
        private const int NumberOfCategories = 10;

        public Task<ICollection<Category>> GetAllCategoriesAsync()
        {
            var random = new Random();
            ICollection<Category> categories = new List<Category>();

            for (byte i = 0; i < NumberOfCategories; i++)
            {
                var category = GetFakeCategory(i, random);
                categories.Add(category);
            }

            return Task.FromResult(categories);
        }

        public async Task<Category> GetCategoryDetailsAsync(int categoryId)
        {
            var root = GetFakeCategory(1);
            root.Id = categoryId;
            var retrieved = await GetAllCategoriesAsync().ConfigureAwait(false);
            retrieved.ForEach(category =>
            {
                category.ParentCategoryId = root.Id;
                root.Subcategories.Add(category);
            });
            return root;
        }

        public Task FetchSpeculativelyAsync() => Task.Run(() => { });

        public static Category GetFakeCategory(byte presentationOrder, Random random = null)
        {
            if (random == null)
            {
                random = new Random();
            }

            return new Category
            {
                Id = random.Next(),
                ParentCategoryId = null,
                PresentationOrder = presentationOrder,
                Name = $"Fake category {random.Next(presentationOrder)}",
                Subcategories = new ObservableCollection<Category>(new List<Category>()),
                Courses = new ObservableCollection<Course>(new List<Course>())
            };
        }
    }
}