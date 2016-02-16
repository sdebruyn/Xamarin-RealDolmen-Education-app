using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using EducationApp.Models;
using EducationApp.Services;
using Microsoft.Practices.ServiceLocation;

namespace EducationApp.Extensions
{
    public static class CacheExtensions
    {
        public static async Task InsertWithoutOverwriteAsync<T>(this IObjectBlobCache cache, string key, T value,
            DateTimeOffset expiration) where T : class
        {
            await cache.Flush();
            T fromCache;
            try
            {
                fromCache = await cache.GetObject<T>(key);
            }
            catch (KeyNotFoundException)
            {
                fromCache = null;
            }
            if (fromCache != null)
            {
                await cache.InsertObject(key, value, expiration);
            }
        }

        public static Task InsertWithoutOverwriteAsync<T>(this IObjectBlobCache cache, string key, T value,
            TimeSpan expiration) where T : class
            => InsertWithoutOverwriteAsync(cache, key, value, DateTimeOffset.UtcNow + expiration);

        public static async Task InsertWithoutOverwriteAsync<T>(this IObjectBlobCache cache,
            IDictionary<string, T> keyValuePairs, DateTimeOffset expiration) where T : class
        {
            foreach (var pair in keyValuePairs)
            {
                await InsertWithoutOverwriteAsync(cache, pair.Key, pair.Value, expiration).ConfigureAwait(false);
            }
        }

        public static Task InsertWithoutOverwriteAsync<T>(this IObjectBlobCache cache,
            IDictionary<string, T> keyValuePairs,
            TimeSpan expiration) where T : class
            => InsertWithoutOverwriteAsync(cache, keyValuePairs, DateTimeOffset.UtcNow + expiration);

        public static async Task InsertIfMoreDetailsAsync<T>(this IObjectBlobCache cache, string key, T value,
            DateTimeOffset expiration) where T : class, IDetailLeveled
        {
            await cache.Flush();
            IDetailLeveled fromCache;
            try
            {
                fromCache = await cache.GetObject<T>(key);
            }
            catch (KeyNotFoundException)
            {
                fromCache = null;
            }
            var newValue = (IDetailLeveled) value;
            if (fromCache == null || newValue.GetDetailLevel() >= fromCache.GetDetailLevel())
            {
                await cache.InsertObject(key, value, expiration);
            }
        }

        public static Task InsertIfMoreDetailsAsync<T>(this IObjectBlobCache cache, string key, T value,
            TimeSpan expiration)
            where T : class, IDetailLeveled
            => InsertIfMoreDetailsAsync(cache, key, value, DateTimeOffset.UtcNow + expiration);

        public static async Task InsertIfMoreDetailsAsync<T>(this IObjectBlobCache cache,
            IDictionary<string, T> keyValuePairs, DateTimeOffset expiration) where T : class, IDetailLeveled
        {
            foreach (var pair in keyValuePairs)
            {
                await InsertIfMoreDetailsAsync(cache, pair.Key, pair.Value, expiration).ConfigureAwait(false);
            }
        }

        public static Task InsertIfMoreDetailsAsync<T>(this IObjectBlobCache cache, IDictionary<string, T> keyValuePairs,
            TimeSpan expiration) where T : class, IDetailLeveled
            => InsertIfMoreDetailsAsync(cache, keyValuePairs, DateTimeOffset.UtcNow + expiration);

        public static async Task InsertAllIfMoreDetailsAsync<T>(this IObjectBlobCache cache,
            IDictionary<string, T> keyValuePairs, DateTimeOffset expiration) where T : class, IDetailLeveled
        {
            await cache.Flush();
            var toInsert = new Dictionary<string, T>();
            foreach (var pair in keyValuePairs)
            {
                IDetailLeveled fromCache;
                try
                {
                    fromCache = await cache.GetObject<T>(pair.Key);
                }
                catch (KeyNotFoundException)
                {
                    fromCache = null;
                }
                var newValue = (IDetailLeveled) pair.Value;
                if (fromCache == null || newValue.GetDetailLevel() >= fromCache.GetDetailLevel())
                {
                    toInsert.Add(pair.Key, pair.Value);
                }
            }
            if (toInsert.Any())
            {
                try
                {
                    await cache.InsertObjects(toInsert, expiration);
                }
                catch (Exception e)
                {
                    ServiceLocator.Current.GetInstance<ILoggingService>().Report(e, cache);
                }
            }
        }

        public static Task InsertAllIfMoreDetailsAsync<T>(this IObjectBlobCache cache,
            IDictionary<string, T> keyValuePairs,
            TimeSpan expiration) where T : class, IDetailLeveled
            => InsertAllIfMoreDetailsAsync(cache, keyValuePairs, DateTimeOffset.UtcNow + expiration);
    }
}