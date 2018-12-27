using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.Linq;

namespace FlyBirdYoYo.Utilities.Http
{
    /// <summary>
    /// URL的操作类
    /// </summary>
    public class UrlUtil
    {
        static System.Text.Encoding encoding = System.Text.Encoding.UTF8;

        public static bool IsStaticFileURL(string URL)
        {
            var ignorePattern = @".*(\.axd|\.gif|\.jpg|\.jpeg|\.png|\.css|\.js)";
            if (System.Text.RegularExpressions.Regex.IsMatch(URL, ignorePattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #region URL的64位编码
        public static string Base64Encrypt(string sourthUrl)
        {
            string eurl = HttpUtility.UrlEncode(sourthUrl);
            eurl = Convert.ToBase64String(encoding.GetBytes(eurl));
            return eurl;
        }
        #endregion

        #region URL的64位解码
        public static string Base64Decrypt(string eStr)
        {        
            if (!IsBase64(eStr))
            {
                return eStr;
            }
            byte[] buffer = Convert.FromBase64String(eStr);
            string sourthUrl = encoding.GetString(buffer);
            sourthUrl = HttpUtility.UrlDecode(sourthUrl);
            return sourthUrl;
        }
        /// <summary>
        /// 是否是Base64字符串
        /// </summary>
        /// <param name="eStr"></param>
        /// <returns></returns>
        public static bool IsBase64(string eStr)
        {
            if ((eStr.Length % 4) != 0)
            {
                return false;
            }
            if (!Regex.IsMatch(eStr, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
            {
                return false;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 添加URL参数
        /// </summary>
        public static string AddParam(string url, string paramName, string value)
        {
            Uri uri = new Uri(url);
            if (string.IsNullOrEmpty(uri.Query))
            {
                string eval =HttpUtility.UrlEncode(value);
                return String.Concat(url, "?" + paramName + "=" + eval);
            }
            else
            {
                string eval = HttpUtility.UrlEncode(value);
                return String.Concat(url, "&" + paramName + "=" + eval);
            }
        }
        /// <summary>
        /// 更新URL参数
        /// </summary>
        public static string UpdateParam(string url, string paramName, string value)
        {
            string keyWord = paramName+"=";
            int index = url.IndexOf(keyWord)+keyWord.Length;
            int index1 = url.IndexOf("&", index);
            if (index1 == -1)
            {
                url = url.Remove(index, url.Length - index);
                url = string.Concat(url, value);
                return url;
            }
            url = url.Remove(index,index1 - index);
            url = url.Insert(index, value);
            return url;
        }


   
        /// <summary>
        /// 分析 url 字符串中的参数信息
        /// </summary>
        /// <param name="url">输入的 URL</param>
        /// <param name="baseUrl">输出 URL 的基础部分</param>
        /// <param name="nvc">输出分析后得到的 (参数名,参数值) 的集合</param>
        public static void ParseUrl(string url, out string baseUrl, out NameValueCollection nvc)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            nvc = new NameValueCollection();
            baseUrl = "";

            if (url == "")
                return;

            int questionMarkIndex = url.IndexOf('?');

            if (questionMarkIndex == -1)
            {
                baseUrl = url;
                return;
            }
            baseUrl = url.Substring(0, questionMarkIndex);
            if (questionMarkIndex == url.Length - 1)
                return;
            string ps = url.Substring(questionMarkIndex + 1);

            // 开始分析参数对    
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(ps);

            foreach (Match m in mc)
            {
                nvc.Add(m.Result("$2").ToLower(), m.Result("$3"));
            }
        }


        /// <summary>
        /// 将url地址 参数做格式化
        /// </summary>
        /// <param name="url"></param>
        /// <param name="urlParas"></param>
        /// <returns></returns>
        public static string GetFormatUrl(string url, Dictionary<string, object> urlParas)
        {
            var tagetUrl = "";
            //无参数 直接返回
            if (null == urlParas || urlParas.Keys.Count <= 0)
            {
                return url;
            }

            try
            {
                ///a=3&b=555&.........
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < urlParas.Keys.Count; i++)
                {
                    var item = urlParas.ElementAt(i);
                    var key = item.Key;
                    string value = string.Empty;
                    if (null != item.Value)
                    {
                        value = HttpUtility.UrlEncode(item.Value.ToString());
                    }
                    if (i == 0)
                    {
                        sb.Append(key).Append("=").Append(value);
                    }
                    else
                    {
                        sb.Append("&").Append(key).Append("=").Append(value);
                    }
                }

                //将键值对拼接
                if (url.Contains('?'))
                {
                    tagetUrl = string.Concat(url, "&", sb.ToString());
                }
                else
                {
                    tagetUrl = string.Concat(url, "?", sb.ToString());
                }
            }
            catch
            {
            }

            return tagetUrl;

        }

    }
}
