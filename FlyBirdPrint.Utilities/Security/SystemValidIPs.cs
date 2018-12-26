using System;
using System.Collections.Generic;


namespace FlyBirdYoYo.Utilities.Security
{
    /// <summary>
    /// 系统开始的时候 --有效的IP集合
    /// </summary>
    public class SystemValidIPs
    {
        private static readonly List<string> _vaildIPCollention = new List<string>();


        /// <summary>
        /// 静态构造函数
        /// </summary>
        static SystemValidIPs()
        {

            //----IP 白名单功能暂时先未实现从xml文件读取 配置的ip 集合

        }

        public static List<string> VaildIPCollention
        {
            get
            {
                throw new NotImplementedException("IP 白名单功能暂时先未实现");
                //return   _vaildIPCollention;
            }
        }

    }
}
