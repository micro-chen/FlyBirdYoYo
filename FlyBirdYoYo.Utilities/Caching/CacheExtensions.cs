using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;

namespace FlyBirdYoYo.Utilities.Caching
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// 获取缓存项，如果没有，那么从新添加到缓存并加载
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>Cached item</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 60, acquire);
        }

        /// <summary>
        /// 获取缓存项，如果没有，那么从新添加到缓存并加载
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="key">Cache key</param>
        /// <param name="cacheTime">缓存时间（秒）0 表示不缓存</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>Cached item</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsHasSet(key))
            {
                return cacheManager.Get<T>(key);
            }

            var result = acquire();
            if (cacheTime > 0)
                cacheManager.Set(key, result, cacheTime);
            return result;
        }

        /// <summary>
        /// 移除缓存项-by  正则匹配键
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="pattern">Pattern</param>
        /// <param name="keys">All keys in the cache</param>
        public static void RemoveByPattern(this ICacheManager cacheManager, string pattern, IEnumerable<string> keys)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (var key in keys.Where(p => regex.IsMatch(p.ToString())).ToList())
                cacheManager.Remove(key);
        }


        #region MyRegion IMemoryCache  Extension Method

        /// <summary>
        /// 向内存缓存对象中插入一条记录，并基于文件依赖
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependency"></param>
        /// <param name="callbackHandler"></param>
        public static void Set<T>(this IMemoryCache cache,
            string key,
            T value,
            FileCacheDependency dependency,
            PostEvictionDelegate callbackHandler = null
            )
        {
            var fileInfo = new FileInfo(dependency.FilePath);
            var fileProvider = new PhysicalFileProvider(fileInfo.DirectoryName);

            var options = new MemoryCacheEntryOptions()
                                .AddExpirationToken(fileProvider.Watch(fileInfo.Name));
            if (null != callbackHandler)
            {
                //注册回调事件
                options.RegisterPostEvictionCallback(callbackHandler);
            }
            cache.Set(key, value, options);

        }

        /// <summary>
        ///  向内存缓存对象中插入一条记录，并基于时间依赖
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependency"></param>
        /// <param name="callbackHandler"></param>
        public static void Set<T>(this IMemoryCache cache, 
            string key, 
            T value,
            TimeCacheDependency dependency,
             PostEvictionDelegate callbackHandler = null
            )
        {
            var options = new MemoryCacheEntryOptions();
            if (null != callbackHandler)
            {
                //注册回调事件
                options.RegisterPostEvictionCallback(callbackHandler);
            }


            if (dependency.Policy == CacheItemPolicy.AbsoluteExpiration)
            {
                options.SetAbsoluteExpiration(dependency.Time);
            }
            else
            {
                options.SetSlidingExpiration(dependency.Time);
            }
            cache.Set(key, value, options);
        }

        #endregion


    }


}
