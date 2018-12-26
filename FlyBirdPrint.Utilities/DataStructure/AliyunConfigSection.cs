using System;
using System.Collections.Generic;
using System.Text;
using FlyBirdYoYo.Utilities.Interface;

namespace FlyBirdYoYo.Utilities.DataStructure
{
    public class AliyunConfigSection : IConfigSection
    {
        /// <summary>
        /// 配置节点名称
        /// </summary>
        public const string SectionName = "AliyunConfig";

        public static AliyunConfigSection Instance { get;} =
            ConfigHelper.GetConfigSection<AliyunConfigSection>(SectionName);

        /// <summary>
        /// 加密KEY
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// AccessSecret
        /// </summary>
        public string AccessSecret { get; set; }

        /// <summary>
        /// OSS配置
        /// </summary>
        public OssConfig OssConfig { get; set; }


        /// <summary>
        /// 短信配置
        /// </summary>
        public SmsConfig SmsConfig { get; set; }
    }

    /// <summary>
    /// OSS配置
    /// </summary>
    public class OssConfig
    {
        /// <summary>
        /// Buckt名称
        /// </summary>
        public string BucktName { get; set; }

        /// <summary>
        /// OSS服务器Location
        /// </summary>
        public string Location{ get; set; }

    /// <summary>
    /// CenterPoint
    /// </summary>
    public string CenterPoint { get; set; }
    }


    /// <summary>
    /// 短信配置
    /// </summary>
    public class SmsConfig
    {
        /// <summary>
        /// 地区
        /// </summary>
        public string RegionIdForPop { get; set; }

        /// <summary>
        /// 短信API产品名称
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// API域名
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 短信模板集合
        /// </summary>
        public string Types { get; set; }
    }

}
