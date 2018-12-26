using System;
using System.Collections.Generic;

namespace System.Collections.Concurrent
{
    public static class BlockingCollectionExtenstion
    {
        /// <summary>
        /// 批量插入到锁定集合
        /// </summary>
        /// <param name="_blokList"></param>
        /// <param name="_dataList"></param>
        public static void AddRange<T>(this BlockingCollection<T> _blokList,IEnumerable<T> _dataList) {
            if (null==_dataList)
            {
                return;
            }
            var cursor = _dataList.GetEnumerator();
            while (cursor.MoveNext())
            {
                T item = cursor.Current;
                _blokList.Add(item);
            }
        }
    }
}
