using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;

namespace System.Collections.Generic
{
    public static class CollectionExtension
    {
        /// <summary>
        /// 获取可迭代列表中的随机一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T GetRandomItem<T>(this IEnumerable<T> source)
        {
            if (source.IsEmpty())
            {
                throw new Exception("source can not be empty!");
            }
         
            T item = default(T);


            var rand = new Random(((int)DateTime.Now.Ticks));
            int pos = rand.Next(0, source.Count() - 1);//获取随机位置
            item = source.ElementAt(pos);

            return item;
        }
    }
}
