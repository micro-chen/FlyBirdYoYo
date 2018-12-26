using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace System
{
    public static class ObjectExtension
    {



        /// <summary>
        /// 对象转为字典 Dictionary<string,object>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            
            if (null == obj)
            {
                return null;
            }
            string jsonStr = obj.ToJson();
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonStr);

            //IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            //timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            //return Newtonsoft.Json.JsonConvert.SerializeObject(obj, timeFormat);
        }



        /// <summary>
        /// 转换为json  字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            if (null==obj)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(obj);

            //IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            //timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            //return Newtonsoft.Json.JsonConvert.SerializeObject(obj, timeFormat);
        }

        /// <summary>
        /// 字符串反序列化到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T FromJsonToObject<T>(this string jsonStr)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonStr);
        }


        /// <summary>
        /// object  是否为空字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        public static bool IsNullOrEmpty(this object obj)
        {
          
            if (obj == null)
            {
                return true;
            }

            if (obj.ToString().Length<=0)
            {
                return true;
            }

            return false;
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
        public static string IsNullDefault(this object obj,string def)
        {
            if (obj == null)
            {
                return def;
            }
            return obj.ToString();
        }

        /// <summary>
        /// 是否是整形的默认值or null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int IsNullDefault(this object obj)
        {
            if (obj == null)
            {
                return default(int);
            }
            return obj.ToString().ToInt();
        }


    }
}
