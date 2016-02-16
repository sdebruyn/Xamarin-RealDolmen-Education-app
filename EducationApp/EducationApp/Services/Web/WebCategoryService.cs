using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using EducationApp.Exceptions;
using EducationApp.Extensions;
using EducationApp.Models;
using EducationApp.Services.Web.Utilities;
using static Plugin.Connectivity.CrossConnectivity;

namespace EducationApp.Services.Web
{
    public class WebCategoryService : BaseCachedWebService, ICategoryService
    {
        private const string CategoryListKey = "clist";
        private readonly ILoggingService _loggingService;
        private readonly IEducationApi _speculativeClient;
        private readonly IEducationApi _userInitiatedClient;
        private bool _isBackgroundFetching;

        public WebCategoryService(IUserInitiatedClient userInitiatedClient, ISpeculativeClient speculativeClient,
            ILocalizedStringProvider loc, ILoggingService loggingService) : base(loc)
        {
            _loggingService = loggingService;
            _userInitiatedClient = userInitiatedClient.Client;
            _speculativeClient = speculativeClient.Client;
        }

        /// <exception cref="DataSourceException">Exception while fetching data.</exception>
        /// <exception cref="ConnectionException">No data available (disconnected from data source and not in cache).</exception>
        public async Task<ICollection<Category>> GetAllCategoriesAsync()
        {
            var cachedCategories = await GetCachedCategoriesAsync().ConfigureAwait(false);
            if (cachedCategories.Any())
            {
                UpdateCategoryListCacheAsync();
                return cachedCategories;
            }
            if (!Current.IsConnected)
            {
                throw new ConnectionException();
            }
            ICollection<Category> categories;
            try
            {
                categories = await _userInitiatedClient.GetCategoriesAsync().ConfigureAwait(false);
                categories.SetCountProperty(c => c.PresentationOrder);
            }
            catch (Exception exc)
            {
                throw new DataSourceException(exc);
            }
            UpdateCategoryListCacheAsync(categories);
            return categories;
        }

        /// <exception cref="ConnectionException">No data available (disconnected from data source and not in cache).</exception>
        /// <exception cref="DataSourceException">Exception while fetching data.</exception>
        public async Task<Category> GetCategoryDetailsAsync(int categoryId)
        {
            var category = await GetCategoryFromCacheAsync(categoryId).ConfigureAwait(false);
            if (category == null || category.GetDetailLevel() < 1)
            {
                category = await FetchCategoryDetailsAsync(categoryId).ConfigureAwait(false);
            }
            Cache.InsertIfMoreDetailsAsync(GetCategoryCacheKey(category), category,
                TimeSpan.FromDays(Constants.Internet.DefaultCacheDays));
            Cache.InsertAllIfMoreDetailsAsync(category.Subcategories.ToDictionary(GetSubcategoryCacheKey, c => c),
                TimeSpan.FromDays(Constants.Internet.DefaultCacheDays));
            return category;
        }

        public async Task FetchSpeculativelyAsync() => await UpdateCategoryListCacheAsync().ConfigureAwait(false);

        private static async Task<ICollection<Category>> GetCachedCategoriesAsync()
        {
            string list;
            try
            {
                list = await Cache.GetObject<string>(CategoryListKey);
            }
            catch (KeyNotFoundException)
            {
                list = null;
            }
            var result = new List<Category>();
            if (list.IsNotNullOrEmpty())
            {
                var ids = list.Split(',').Select(int.Parse).ToList();
                foreach (var id in ids)
                {
                    var category = await Cache.GetObject<Category>(GetCategoryCacheKey(id));
                    result.Add(category);
                }
            }
            return result;
        }

        private async Task UpdateCategoryListCacheAsync(ICollection<Category> categories = null)
        {
            if (!Current.IsConnected || _isBackgroundFetching)
            {
                return;
            }
            lock (LockObject)
            {
                _isBackgroundFetching = true;
            }
            if (categories == null)
            {
                categories = await _speculativeClient.GetCategoriesAsync().ConfigureAwait(false);
            }
            categories.SetCountProperty(c => c.PresentationOrder);
            if (Current.IsFast())
            {
                // ReSharper disable once PossibleMultipleEnumeration
                var detailed = new List<Category>();
                foreach (var category in categories)
                {
                    try
                    {
                        var cat = await _speculativeClient.GetCategoryDetailsAsync(category.Id).ConfigureAwait(false);
                        cat.Subcategories.SetCountProperty(sc => sc.PresentationOrder);
                        detailed.Add(cat);
                    }
                    catch (Exception e)
                    {
                        _loggingService.Report(e, this);
                    }
                }
                await
                    Cache.InsertAllIfMoreDetailsAsync(detailed.ToDictionary(GetCategoryCacheKey, c => c),
                        TimeSpan.FromDays(Constants.Internet.DefaultCacheDays)).ConfigureAwait(false);
            }
            else if (Current.IsSlow())
            {
                await
                    Cache.InsertAllIfMoreDetailsAsync(categories.ToDictionary(GetCategoryCacheKey, c => c),
                        TimeSpan.FromDays(Constants.Internet.DefaultCacheDays)).ConfigureAwait(false);
            }
            var catlist = string.Join(",", categories.Select(c => c.Id));
            await Cache.InsertObject(CategoryListKey, catlist);
            lock (LockObject)
            {
                _isBackgroundFetching = false;
            }
        }

        /// <exception cref="DataSourceException">Exception while fetching data.</exception>
        /// <exception cref="ConnectionException">No data available (disconnected from data source and not in cache).</exception>
        private async Task<Category> FetchCategoryDetailsAsync(int categoryId)
        {
            Category category;
            if (!Current.IsConnected)
            {
                throw new ConnectionException();
            }
            try
            {
                var client = _userInitiatedClient;
                category = await client.GetCategoryDetailsAsync(categoryId).ConfigureAwait(false);
                category.Subcategories.SetCountProperty(sc => sc.PresentationOrder);
            }
            catch (Exception exc)
            {
                throw new DataSourceException(exc);
            }
            return category;
        }
    }
}