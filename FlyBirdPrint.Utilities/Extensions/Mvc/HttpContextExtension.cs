using System;
using System.Web;
using Microsoft.AspNetCore.Http;
using FlyBirdYoYo.Utilities;


namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// 扩展 从http 上下文获取cookie信息
    /// </summary>
    public static class HttpContextExtension
    {
        /// <summary>
        /// 默认cookie过期时间 7天
        /// </summary>
        const int DEFAULT_COOKIE_EXPIRE_TIME = 365;
        /// <summary>
        /// 设置cookie，设置默认过期时间：DEFAULT_COOKIE_EXPIRE_TIME
        /// </summary>
        /// <param name="context"></param>
        /// <param name="domain"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="isHttpOnly"></param>
        public static void SetCookie(this HttpContext context, string domain, string name, string value, bool isHttpOnly = false)
        {
            var expireTime = TimeSpan.FromDays(DEFAULT_COOKIE_EXPIRE_TIME);
            SetCookie(context, domain, name, value, expireTime, isHttpOnly);
        }
        /// <summary>
        /// 设置Cooke ，可以设定过期时间
        /// </summary>
        /// <param name="context"></param>
        /// <param name="domain"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="expirTime"></param>
        /// <param name="isHttpOnly"></param>
        public static void SetCookie(this HttpContext context, string domain, string name, string value, TimeSpan expirTime, bool isHttpOnly = false)
        {
            var ckOption = new CookieOptions();// context.Request.Cookies[name];

            ckOption.Expires = DateTime.Now.AddMilliseconds(expirTime.TotalMilliseconds);
            ckOption.Domain = domain;
            ckOption.HttpOnly = isHttpOnly;
            ckOption.SameSite = SameSiteMode.None;
            ckOption.Path = "/";
            string valueEncode = HttpUtility.UrlEncode(value);
            context.Response.Cookies.Append(name, valueEncode, ckOption);
        }
        public static string GetCookie(this HttpContext context, string name)
        {
            var cookie = context.Request.Cookies[name];
            if (!cookie.IsNull()) return HttpUtility.UrlDecode(cookie);
            return null;
        }

        public static T GetCookie<T>(this HttpContext context, string name) where T : class
        {
            var cookie = context.Request.Cookies[name];
            if (!cookie.IsNull()) return HttpUtility.UrlDecode(cookie).FromJson<T>();
            return null;
        }
        public static void RemoveCookie(this HttpContext context, string name, string domain=null)
        {
            var cookie = context.Request.Cookies[name];
            if (!cookie.IsNullOrEmpty())
            {
                if (string.IsNullOrEmpty(domain))
                {
                    domain = Contanst.Global_Site_Domain_Cookie;
                }
                var ckOption = new CookieOptions();
                ckOption.Domain = domain;
                ckOption.SameSite = SameSiteMode.None;
                ckOption.Path = "/";
                context.Response.Cookies.Delete(name, ckOption);
            }
        }




    }



}
