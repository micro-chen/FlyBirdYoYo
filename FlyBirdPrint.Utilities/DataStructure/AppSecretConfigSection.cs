using System;
using System.Collections.Generic;
using System.Text;
using FlyBirdYoYo.Utilities.Interface;

namespace FlyBirdYoYo.Utilities.DataStructure
{
    /// <summary>
    /// 对接平台的的app kye配置节点
    /// </summary>
    public class AppSecretConfigSection: IConfigSection
    {
        /// <summary>
        /// 配置节点名称
        /// </summary>
        public const string SectionName = "App_Token_Conf";

        /// <summary>
        /// 拼多多的 appkey
        /// </summary>
        public PinduoduoAppSecret Pdd { get; set; }

        /// <summary>
        /// 菜鸟的 appkey
        /// </summary>
        public CaiNiaoAppSecret CaiNiao { get; set; }



        //public TaobaoAppSecret Taobao { get; set; }
    }

    /// <summary>
    /// 拼多多对接oauth
    /// </summary>
    public class PinduoduoAppSecret
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string code_address { get; set; }
        public string access_token_address { get; set; }
        public string redirect_uri { get; set; }

        /// <summary>
        /// 正式环境拼多多API接口基础地址。（官方声明只能post）
        /// http://open.pinduoduo.com/#/document
        /// </summary>
        public string api_base_address { get; set; }
    }

    /// <summary>
    /// 菜鸟打印ISV接入凭据
    /// </summary>
    public class CaiNiaoAppSecret
    {
        /// <summary>
        /// 商家授权地址
        /// </summary>
        public string ShopOAuthAddress { get; set; }
        /// <summary>
        /// 商家交换Token地址
        /// </summary>
        public string ShopExtenTokenAddress { get; set; }

        /// <summary>
        /// 菜鸟LINK 通信地址
        /// </summary>
        public string PacUrl { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }

        /// <summary>
        /// 回调地址
        /// </summary>
        public string redirect_uri { get; set; }
    }

    //public class TaobaoAppSecret
    //{
    //    public string app_key { get; set; }
    //    public string app_secret { get; set; }

    //    public long adzoneId { get; set; }

    //    public string tkTemplate { get; set; }
    //}


}
