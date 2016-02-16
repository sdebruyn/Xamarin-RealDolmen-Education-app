using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using EducationApp.Exceptions;
using EducationApp.Extensions;
using EducationApp.Models;
using EducationApp.Services.Web.Utilities;
using static Plugin.Connectivity.CrossConnectivity;

namespace EducationApp.Services.Web
{
    public class WebCourseService : BaseCachedWebService, ICourseService
    {
        private readonly IDispatcherHelper _dispatcherHelper;
        private readonly IEducationApi _speculative;
        private readonly IEducationApi _userInitiated;

        public WebCourseService(IUserInitiatedClient userInitiated, ISpeculativeClient speculative,
            ILocalizedStringProvider loc, IDispatcherHelper dispatcherHelper)
            : base(loc)
        {
            _dispatcherHelper = dispatcherHelper;
            _speculative = speculative.Client;
            _userInitiated = userInitiated.Client;
        }

        /// <exception cref="DataSourceException">Error occured while fetching the search results.</exception>
        public async Task<ICollection<Course>> SearchCoursesAsync(string query)
        {
            ICollection<Course> resultCollection;
            var cacheKey = GetCourseSearchCacheKey(query);
            try
            {
                resultCollection = await Cache.GetObject<ICollection<Course>>(cacheKey);
            }
            catch (KeyNotFoundException)
            {
                resultCollection = null;
            }
            if (resultCollection != null)
            {
                return resultCollection;
            }

            if (!Current.IsConnected)
            {
                resultCollection = await GetCachedCoursesAsync(query).ConfigureAwait(false);
                return resultCollection;
            }

            try
            {
                resultCollection =
                    await
                        _userInitiated.SearchCoursesAsync(query,
                            (await GetLanguageCodeAsync().ConfigureAwait(false)).ToString()).ConfigureAwait(false);
                Cache.InsertObject(cacheKey, resultCollection);
                Cache.InsertIfMoreDetailsAsync(resultCollection.ToDictionary(GetCourseCacheKey, c => c),
                    TimeSpan.FromDays(Constants.Internet.DefaultCacheDays));

                if (resultCollection.Any())
                {
                    FetchSpeculativelyAsync(resultCollection.First().Id);
                }

                return resultCollection;
            }
            catch (Exception exc)
            {
                throw new DataSourceException(exc);
            }
        }

        public async Task FetchSpeculativelyAsync(int courseId)
        {
            if (!Current.IsConnected || Current.IsSlow())
            {
                return;
            }

            var course = await GetCourseFromCacheAsync(courseId, 2).ConfigureAwait(false);
            if (course == null || course.GetDetailLevel() == 0)
            {
                course = await FetchCourseDetailsAsync(courseId, true).ConfigureAwait(false);
                Cache.InsertObject(GetCourseCacheKey(course), course,
                    TimeSpan.FromDays(Constants.Internet.DefaultCacheDays));
            }
        }

        public async Task FetchCoursesSpeculativelyAsync(params Category[] subcategories)
        {
            if (!Current.IsConnected || Current.IsSlow() || subcategories.Length > 3)
            {
                return;
            }

            foreach (var category in subcategories)
            {
                try
                {
                    await GetCategoryCoursesAsync(category.Id, true).ConfigureAwait(false);
                }
                catch (Exception)
                {
                }
            }
        }

        public async Task<Course> GetCourseDetailsAsync(int courseId)
            => await GetCourseFromCacheAsync(courseId, 2).ConfigureAwait(false) ??
               await FetchCourseDetailsAsync(courseId).ConfigureAwait(false);

        /// <exception cref="ConnectionException">No connection to the web to fetch the courses.</exception>
        /// <exception cref="DataSourceException">Exception occured while fetching data from the web.</exception>
        public async Task<ICollection<Course>> GetCategoryCoursesAsync(int categoryId, bool speculatively = false)
        {
            var fromCache = await GetSubcategoryFromCacheAsync(categoryId).ConfigureAwait(false);
            if (fromCache != null && fromCache.GetDetailLevel() >= 2)
            {
                return fromCache.Courses;
            }
            var courses = await FetchCategoryCoursesAsync(categoryId, speculatively).ConfigureAwait(false);
            UpdateCategoryCoursesInCacheAsync(courses, categoryId);
            return courses;
        }

        public async Task UpdateCourseSessionsAsync(Course course, bool speculatively = false)
        {
            if ((course.GetDetailLevel() >= 3 && Current.IsFast()) || speculatively)
            {
                FetchCourseSessionsAsync(course, true);
                return;
            }
            await GetCourseSessionsFromCacheOrFetchAsync(course);
        }

        public async Task GetCourseSessionsFromCacheOrFetchAsync(Course course)
        {
            if (course.GetDetailLevel() >= 3)
            {
                return;
            }
            var fromCache = await GetCourseFromCacheAsync(course.Id, 3).ConfigureAwait(false);
            if (fromCache != null)
            {
                course.Sessions.Fill(fromCache.Sessions);
                return;
            }
            await FetchCourseSessionsAsync(course, false).ConfigureAwait(false);
        }

        private async Task UpdateCategoryCoursesInCacheAsync(ICollection<Course> courses, int categoryId)
        {
            await
                Cache.InsertIfMoreDetailsAsync(courses.ToDictionary(GetCourseCacheKey, c => c),
                    TimeSpan.FromDays(Constants.Internet.DefaultCacheDays))
                    .ConfigureAwait(false);
            var category = await GetSubcategoryFromCacheAsync(categoryId).ConfigureAwait(false);
            if (category == null)
            {
                return;
            }
            category.Courses.Fill(courses);
            await
                Cache.InsertObject(GetSubcategoryCacheKey(categoryId), category,
                    TimeSpan.FromDays(Constants.Internet.DefaultCacheDays));
        }

        /// <exception cref="ConnectionException">No connection to the web to fetch the courses.</exception>
        /// <exception cref="DataSourceException">Exception occured while fetching data from the web.</exception>
        public async Task<ICollection<Course>> FetchCategoryCoursesAsync(int categoryId, bool speculatively = false)
        {
            if (!Current.IsConnected)
            {
                throw new ConnectionException();
            }
            try
            {
                var client = speculatively ? _speculative : _userInitiated;
                var courses =
                    await
                        client.GetCategoryCoursesAsync(categoryId,
                            (await GetLanguageCodeAsync().ConfigureAwait(false)).ToString()).ConfigureAwait(false);
                return courses.Where(c => (c.IsActive ?? true) && (c.InCatalog ?? true)).ToList();
            }
            catch (Exception exc)
            {
                throw new DataSourceException(exc);
            }
        }

        private string GetCourseCacheKey(Course course) => GetCourseCacheKey(course.Id);

        private async Task<Course> GetCourseFromCacheAsync(int courseId, int requiredDetailLevel = 1)
        {
            Course course;
            try
            {
                course = await Cache.GetObject<Course>(GetCourseCacheKey(courseId));
            }
            catch (KeyNotFoundException)
            {
                course = null;
            }
            if (course == null || course.GetDetailLevel() < requiredDetailLevel)
            {
                return null;
            }
            return course;
        }

        private string GetCourseCacheKey(int courseId) => "course" + courseId + GetLanguageCodeAsync();

        private async Task FetchCourseSessionsAsync(Course course, bool speculative = false)
        {
            if (!Current.IsConnected)
            {
                throw new ConnectionException();
            }
            try
            {
                var apiService = speculative ? _speculative : _userInitiated;
                var shouldFetch = true;
                var page = 1;
                const int pageSize = 10;
                _dispatcherHelper.ExecuteOnUiThread(() => course.Sessions.Clear());
                var random = new Random();
                while (shouldFetch)
                {
                    var sessions = await apiService.GetCourseSessionsAsync(course.Id, page, pageSize);
#if DEBUG
                    sessions.ForEach(s => s.AddFakeData(random));
#endif
                    if (sessions.Count < pageSize)
                    {
                        shouldFetch = false;
                    }
                    _dispatcherHelper.ExecuteOnUiThread(() => sessions.ForEach(s =>
                    {
                        if (s.FirstStartTime != null && s.FirstStartTime.Value > DateTime.Today)
                        {
                            course.Sessions.Add(s);
                        }
                    }));
                    page++;
                }
                _dispatcherHelper.ExecuteOnUiThread(() => course.Sessions.Sort());
                Cache.InsertIfMoreDetailsAsync(GetCourseCacheKey(course), course, TimeSpan.FromDays(5));
            }
            catch (Exception exc)
            {
                throw new DataSourceException(exc);
            }
        }

        private async Task<Course> FetchCourseDetailsAsync(int courseId, bool speculative = false)
        {
            Course course;
            if (!Current.IsConnected)
            {
                throw new ConnectionException();
            }
            try
            {
                var apiService = speculative ? _speculative : _userInitiated;
                course =
                    await
                        apiService.GetCourseDetailsAsync(courseId,
                            (await GetLanguageCodeAsync().ConfigureAwait(false)).ToString()).ConfigureAwait(false);
                course.CleanNullProperties();
            }
            catch (Exception exc)
            {
                throw new DataSourceException(exc);
            }
            return course;
        }

        private string GetCourseSearchCacheKey(string query) => $"coursesearch-{query}-{GetLanguageCodeAsync()}";

        private async Task<ICollection<Course>> GetCachedCoursesAsync(string query = null)
        {
            var cachedCourses = await Cache.GetAllObjects<Course>().Distinct();
            if (query.IsNotNullOrEmpty())
            {
                return cachedCourses.Where(c => c.Title.Contains(query)).ToList();
            }
            return cachedCourses.ToList();
        }
    }
}