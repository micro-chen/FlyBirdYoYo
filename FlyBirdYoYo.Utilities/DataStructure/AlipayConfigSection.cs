using System;
using System.Collections.Generic;
using System.Text;
using FlyBirdYoYo.Utilities.Interface;

namespace FlyBirdYoYo.Utilities.DataStructure
{
    /// <summary>
    /// 支付宝配置
    /// </summary>
    public class AlipayConfigSection: IConfigSection
    {
        /// <summary>
        /// 配置节点名称
        /// </summary>
        public const string SectionName = "Alipay";

        /// <summary>
        /// 应用对应的支付宝公钥
        /// </summary>
        public string AlipayPublicKey { get; set; }

        /// <summary>
        /// 应用公钥 
        /// </summary>
        public string AppPublicKey { get; set; }
        /// <summary>
        /// 应用id
        /// </summary>
        public long? AppId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string CharSet { get; set; }

        /// <summary>
        /// 支付宝网关
        /// </summary>
        public string Gatewayurl { get; set; }

        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// 签名加密类型
        /// </summary>
        public string SignType { get; set; }

        /// <summary>
        /// 商户id
        /// </summary>
        public long? Uid { get; set; }

        /// <summary>
        /// 默认通知网关
        /// </summary>
        public string notify_url { get; set; }

        /// <summary>
        /// 回调地址
        /// </summary>
        public string return_url { get; set; }
    }
}
