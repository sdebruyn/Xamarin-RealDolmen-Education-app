using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using EducationApp.Models;

namespace EducationApp.Services.Web.Utilities
{
    public abstract class BaseCachedWebService
    {
        private readonly ILocalizedStringProvider _localizedStringProvider;
        protected readonly object LockObject;

        protected BaseCachedWebService(ILocalizedStringProvider localizedStringProvider)
        {
            _localizedStringProvider = localizedStringProvider;
            LockObject = new object();
        }

        protected static IObjectBlobCache Cache => (IObjectBlobCache) BlobCache.LocalMachine;

        protected async Task<LanguageCode> GetLanguageCodeAsync()
        {
            LanguageCode code;
            string fromCache;
            try
            {
                fromCache = await Cache.GetObject<string>(nameof(LanguageCode));
            }
            catch (Exception)
            {
                fromCache = null;
            }
            if (fromCache != null)
            {
                if (Enum.TryParse(fromCache, true, out code))
                {
                    return code;
                }
            }
            var fromResources = _localizedStringProvider.GetLocalizedString(Localized.LanguageCode);
            if (Enum.TryParse(fromResources, true, out code))
            {
                return code;
            }
            return default(LanguageCode);
        }

        protected static async Task<Category> GetCategoryFromCacheAsync(int categoryId)
        {
            try
            {
                return await Cache.GetObject<Category>(GetCategoryCacheKey(categoryId));
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        protected static async Task<Category> GetSubcategoryFromCacheAsync(int categoryId)
        {
            try
            {
                return await Cache.GetObject<Category>(GetSubcategoryCacheKey(categoryId));
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        protected static string GetCategoryCacheKey(int categoryId) => "category" + categoryId;
        protected static string GetCategoryCacheKey(Category category) => GetCategoryCacheKey(category.Id);
        protected static string GetSubcategoryCacheKey(Category category) => GetSubcategoryCacheKey(category.Id);
        protected static string GetSubcategoryCacheKey(int id) => "subcategory" + id;
    }
}