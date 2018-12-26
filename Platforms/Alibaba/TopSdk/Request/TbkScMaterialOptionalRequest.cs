using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.tbk.sc.material.optional
    /// </summary>
    public class TbkScMaterialOptionalRequest : BaseTopRequest<Top.Api.Response.TbkScMaterialOptionalResponse>
    {
        /// <summary>
        /// adzone_id
        /// </summary>
        public Nullable<long> AdzoneId { get; set; }

        /// <summary>
        /// 后台类目ID，用,分割，最大10个，该ID可以通过taobao.itemcats.get接口获取到
        /// </summary>
        public string Cat { get; set; }

        /// <summary>
        /// 折扣价范围上限，单位：元
        /// </summary>
        public Nullable<long> EndPrice { get; set; }

        /// <summary>
        /// 淘客佣金比率上限，如：1234表示12.34%
        /// </summary>
        public Nullable<long> EndTkRate { get; set; }

        /// <summary>
        /// 是否有优惠券，设置为true表示该商品有优惠券，设置为false或不设置表示不判断这个属性
        /// </summary>
        public Nullable<bool> HasCoupon { get; set; }

        /// <summary>
        /// 好评率是否高于行业均值
        /// </summary>
        public Nullable<bool> IncludeGoodRate { get; set; }

        /// <summary>
        /// 成交转化是否高于行业均值
        /// </summary>
        public Nullable<bool> IncludePayRate30 { get; set; }

        /// <summary>
        /// 退款率是否低于行业均值
        /// </summary>
        public Nullable<bool> IncludeRfdRate { get; set; }

        /// <summary>
        /// ip参数影响邮费获取，如果不传或者传入不准确，邮费无法精准提供
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 是否海外商品，设置为true表示该商品是属于海外商品，设置为false或不设置表示不判断这个属性
        /// </summary>
        public Nullable<bool> IsOverseas { get; set; }

        /// <summary>
        /// 是否商城商品，设置为true表示该商品是属于淘宝商城商品，设置为false或不设置表示不判断这个属性
        /// </summary>
        public Nullable<bool> IsTmall { get; set; }

        /// <summary>
        /// 所在地
        /// </summary>
        public string Itemloc { get; set; }

        /// <summary>
        /// 是否包邮，true表示包邮，空或false表示不限
        /// </summary>
        public Nullable<bool> NeedFreeShipment { get; set; }

        /// <summary>
        /// 是否加入消费者保障，true表示加入，空或false表示不限
        /// </summary>
        public Nullable<bool> NeedPrepay { get; set; }

        /// <summary>
        /// 牛皮癣程度，取值：1:不限，2:无，3:轻微
        /// </summary>
        public Nullable<long> NpxLevel { get; set; }

        /// <summary>
        /// 第几页，默认：１
        /// </summary>
        public Nullable<long> PageNo { get; set; }

        /// <summary>
        /// 页大小，默认20，1~100
        /// </summary>
        public Nullable<long> PageSize { get; set; }

        /// <summary>
        /// 链接形式：1：PC，2：无线，默认：１
        /// </summary>
        public Nullable<long> Platform { get; set; }

        /// <summary>
        /// 查询词
        /// </summary>
        public string Q { get; set; }

        /// <summary>
        /// site_id
        /// </summary>
        public Nullable<long> SiteId { get; set; }

        /// <summary>
        /// 排序_des（降序），排序_asc（升序），销量（total_sales），淘客佣金比率（tk_rate）， 累计推广量（tk_total_sales），总支出佣金（tk_total_commi），价格（price）
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 店铺dsr评分，筛选高于等于当前设置的店铺dsr评分的商品0-50000之间
        /// </summary>
        public Nullable<long> StartDsr { get; set; }

        /// <summary>
        /// 折扣价范围下限，单位：元
        /// </summary>
        public Nullable<long> StartPrice { get; set; }

        /// <summary>
        /// 淘客佣金比率下限，如：1234表示12.34%
        /// </summary>
        public Nullable<long> StartTkRate { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.tbk.sc.material.optional";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("adzone_id", this.AdzoneId);
            parameters.Add("cat", this.Cat);
            parameters.Add("end_price", this.EndPrice);
            parameters.Add("end_tk_rate", this.EndTkRate);
            parameters.Add("has_coupon", this.HasCoupon);
            parameters.Add("include_good_rate", this.IncludeGoodRate);
            parameters.Add("include_pay_rate_30", this.IncludePayRate30);
            parameters.Add("include_rfd_rate", this.IncludeRfdRate);
            parameters.Add("ip", this.Ip);
            parameters.Add("is_overseas", this.IsOverseas);
            parameters.Add("is_tmall", this.IsTmall);
            parameters.Add("itemloc", this.Itemloc);
            parameters.Add("need_free_shipment", this.NeedFreeShipment);
            parameters.Add("need_prepay", this.NeedPrepay);
            parameters.Add("npx_level", this.NpxLevel);
            parameters.Add("page_no", this.PageNo);
            parameters.Add("page_size", this.PageSize);
            parameters.Add("platform", this.Platform);
            parameters.Add("q", this.Q);
            parameters.Add("site_id", this.SiteId);
            parameters.Add("sort", this.Sort);
            parameters.Add("start_dsr", this.StartDsr);
            parameters.Add("start_price", this.StartPrice);
            parameters.Add("start_tk_rate", this.StartTkRate);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("adzone_id", this.AdzoneId);
            RequestValidator.ValidateRequired("site_id", this.SiteId);
            RequestValidator.ValidateMaxValue("start_dsr", this.StartDsr, 50000);
        }

        #endregion
    }
}
