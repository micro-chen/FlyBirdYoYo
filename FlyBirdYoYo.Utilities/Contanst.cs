using System;

namespace FlyBirdYoYo.Utilities
{
    public class Contanst
    {

        /// <summary>
        /// 发起调用的基础类型
        /// </summary>
        public static Type CallingEntryPointBasetype { get; set; }


        /// <summary>
        /// 分页存储过程调用名称
        /// </summary>
        public const string PageSql_Call_Name = "DbManage_GetRecordsByPage";//调用入口

        /// <summary>
        /// 登录授权后 客户端颁发的cookie 键
        /// </summary>
        public const string Login_Cookie_Client_Key = ".auth.token";

         /// <summary>
        /// 登陆的基本用户信息的cookie键
        /// </summary>
        public const string Login_Cookie_UserInfo = ".login.user";

        /// <summary>
        /// 登陆的超级管理用户信息的cookie键
        /// </summary>
        public const string Login_Cookie_SystemAdminUserInfo = ".login.sys.user";


        /// <summary>
        /// Cookie全局根域名
        /// </summary>
        public static  string Global_Site_Domain_Cookie
        {
            get
            {
                return ConfigHelper.AppSettingsConfiguration.GetConfig("DomainNameForCookieCrossSite");
            }
        }

        /// <summary>
        /// 配置的webapi域名
        /// </summary>
        public static string DomainNameForServerApi
        {
            get
            {
                string domainName= ConfigHelper.AppSettingsConfiguration.GetConfig("DomainNameForServerApi");
                if (domainName.IsNullOrEmpty())
                {
                    domainName = "http://api.flybirdyoyo.com";
                }
                return domainName;
            }
        }




        /// <summary>
        /// 默认站点名称
        /// </summary>
        public const string Default_Site_Domain_Name = "快鸟YoYo";


        /// <summary>
        /// 站点名称的配置节
        /// </summary>
        public const string Config_Node_SiteName = "siteName";
        /// <summary>
        /// 站点登陆授权过期配置节
        /// </summary>
        public const string Config_Node_SignTimeOut = "signTimeOut";

        /// <summary>
        /// 默认登录过期时间
        /// </summary>
        public const int Default_SignTimeOut = 20;
        /// <summary>
        /// cookie -浏览器用户标识
        /// </summary>
        public const string Cookie_Key_BrowserSign = "_ckb_sn";

        /// <summary>
        /// 系统查询级的关键参数key
        /// access_token
        /// </summary>
        public const string System_Key_Access_Token = "access_token";

        /// <summary>
        /// 缓存TokenKey前缀  格式 cache_token_{userid}
        /// </summary>
        public const string UserToken_CacheKey = "cachetoken";


    }
}
