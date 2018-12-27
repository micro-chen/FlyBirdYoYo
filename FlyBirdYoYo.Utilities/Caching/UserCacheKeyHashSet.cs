using System;
using System.Collections.Generic;
using System.Text;

namespace FlyBirdYoYo.Utilities.Caching
{
    /// <summary>
    /// 用户级缓存键对象
    /// </summary>
    public class UserCacheKeyHashSet : HashSet<string>
    {
        public new bool Add(string itemCacheKey)
        {
            long userId = ApplicationContext.Current.UserId;
            string wrapperKey = string.Concat(userId, ":", itemCacheKey);

            return base.Add(wrapperKey);
        }


    }
}
