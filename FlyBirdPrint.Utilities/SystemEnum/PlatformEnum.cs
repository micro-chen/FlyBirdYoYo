using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace FlyBirdYoYo.Utilities.SystemEnum
{
    /// <summary>
    /// 销售平台枚举
    /// </summary>
    public enum PlatformEnum
    {
        [Description("默认平台-拼多多")]
        Default = 0,

        [Description("拼多多")]
        Pdd = 1,
        [Description("京东")]
        Jingdong = 2,
        [Description("唯品会")]
        Vip = 3,
        [Description("国美")]
        Guomei = 4,
        [Description("苏宁")]
        Suning = 5,
        [Description("当当")]
        Dangdang = 6,
        [Description("微店")]
        WeiDian = 7,
        [Description("返利网")]
        FanLiWang = 8,
        [Description("有赞")]
        YouZan = 9,
        [Description("一淘")]
        ETao = 112,
        [Description("阿里妈妈")]
        Alimama = 113,
        [Description("天猫")]
        Tmall = 114,
        [Description("淘宝")]
        Taobao = 115,
    }
}
