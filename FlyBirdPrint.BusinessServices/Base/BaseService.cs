using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using FlyBirdYoYo.Utilities.Interface;
using FlyBirdYoYo.Utilities.Ioc;
using FlyBirdYoYo.Utilities.Caching;

namespace FlyBirdYoYo.BusinessServices
{
    public class BaseService : IBusinessBaseService
    {

        public BaseService()
        {

        }
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public ILoginAuthedUserDTO LoginUser
        {
            get
            {
                return ApplicationContext.Current.User;

            }
        }





        /// <summary>
        /// 异步清理业务缓存
        /// </summary>
        /// <param name="cacheKeys"></param>
        /// <returns></returns>
        public Task ClearCacheAsync(UserCacheKeyHashSet cacheKeys)
        {
            if (null == cacheKeys || cacheKeys.Count <= 0)
            {
                return Task.FromResult<object>(null);
            }

            IEnumerable<string> currentUserCacheKeys = null;

            if (ApplicationContext.Current.UserId > 0)
            {


                //仅清理当前用户级的缓存！
                var userIdPrefixString = string.Concat(ApplicationContext.Current.UserId,":");
                currentUserCacheKeys = cacheKeys.AsParallel().Where(x =>
               {
                   if (x.IndexOf(userIdPrefixString)==0)
                   {
                       return true;
                   }

                   return false;
               });

            }
            else
            {
                //系统级用户--全部清理
                currentUserCacheKeys = cacheKeys;
            }


            //过滤掉前缀
            if (currentUserCacheKeys.IsNotEmpty())
            {

                currentUserCacheKeys = currentUserCacheKeys.Select(key =>
                {
                    var pos = key.IndexOf(':');
                    return key.Substring(pos+1);
                });
            }
            var tsk = this.ClearCacheAsync(currentUserCacheKeys);

            //只要调用了清理，那么在核心层进行拷贝，这个地方直接清理掉集合
            cacheKeys.Clear();
            return tsk;
        }

        /// <summary>
        /// 异步清理掉缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="callBackHandler"></param>
        /// <returns></returns>
        public Task ClearCacheAsync(string cacheKey, Action callBackHandler = null)
        {
            if (string.IsNullOrEmpty(cacheKey))
            {
                return Task.FromResult<object>(null);
            }

            return ClearCacheAsync(new string[] { cacheKey }, callBackHandler);
        }
        /// <summary>
        /// 异步清理业务缓存
        /// </summary>
        /// <param name="cacheKeys"></param>
        /// <param name="callBackHandler"></param>
        /// <returns></returns>
        public Task ClearCacheAsync(IEnumerable<string> cacheKeys, Action callBackHandler = null)
        {
            if (cacheKeys.IsEmpty())
            {
                return Task.FromResult<object>(null);
            }

            Task clearTask = null;



            string[] orgCacheKeys = cacheKeys.ToArray();
            int totalKeys = orgCacheKeys.Length;
            if (totalKeys <= 1)
            {
                //-------单个key模式----
                clearTask = Task.Factory.StartNew(() =>
                {
                    CacheConfigFactory.GetCacheManager().Remove(orgCacheKeys[0]);
                });


            }
            else
            {
                //-------批量模式-------
                string[] inner_cacheKeysCopy = new string[orgCacheKeys.Length];
                orgCacheKeys.CopyTo(inner_cacheKeysCopy, 0);

                clearTask = Task.Factory.StartNew((state) =>
                 {
                     string[] keys = (string[])state;

                     keys.AsParallel().ForAll(key =>
                     {
                         CacheConfigFactory.GetCacheManager().Remove(key);

                     });

                     if (null != callBackHandler)
                     {
                         callBackHandler();
                     }

                 }, inner_cacheKeysCopy);
            }


            return clearTask;
        }

    }
}
