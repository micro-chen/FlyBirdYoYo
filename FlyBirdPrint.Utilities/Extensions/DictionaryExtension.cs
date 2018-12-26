using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace System
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
        /// </summary>
        public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key) == false) dict.Add(key, value);
            return dict;
        }
        /// <summary>
        /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static Dictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }

        /// <summary>
        /// 获取与指定的键相关联的值，如果没有则返回输入的默认值
        /// </summary>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        /// <summary>
        /// 向字典中批量添加键值对
        /// </summary>
        /// <param name="replaceExisted">如果已存在，是否替换</param>
        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted)
        {
            foreach (var item in values)
            {
                if (dict.ContainsKey(item.Key) == false || replaceExisted)
                    dict[item.Key] = item.Value;
            }
            return dict;
        }

        /// <summary>
        /// 将名值对转换为url 参数
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public static string ToQueryString(this NameValueCollection nvc)
        {
            if (nvc == null) return string.Empty;


            StringBuilder sb = new StringBuilder();

            bool isFirstParaEnd = false;
            for (int i = 0; i < nvc.Keys.Count; i++)
            {
                string key = nvc.Keys[i];
                if (string.IsNullOrWhiteSpace(key)) continue;

                string splitChar = string.Empty;
                //分隔符
                if (isFirstParaEnd == false)
                {
                    splitChar = "?";
                    isFirstParaEnd = true;
                }
                else
                {
                    splitChar = "&";
                }


                string[] values = nvc.GetValues(key);
                if (values == null)
                {
                    foreach (string value in values)
                    {
                        sb.AppendFormat("{0}{1}=", splitChar, Uri.EscapeDataString(key));
                    }
                }
                else
                {
                    foreach (string value in values)
                    {
                        sb.AppendFormat("{0}{1}={2}", splitChar, Uri.EscapeDataString(key), Uri.EscapeDataString(value));
                    }
                }
            }


            return sb.ToString();
        }

    }
}
