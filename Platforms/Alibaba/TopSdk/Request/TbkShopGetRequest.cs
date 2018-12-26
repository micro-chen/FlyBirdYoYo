using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.tbk.shop.get
    /// </summary>
    public class TbkShopGetRequest : BaseTopRequest<Top.Api.Response.TbkShopGetResponse>
    {
        /// <summary>
        /// 累计推广商品上限
        /// </summary>
        public Nullable<long> EndAuctionCount { get; set; }

        /// <summary>
        /// 淘客佣金比率上限，1~10000
        /// </summary>
        public Nullable<long> EndCommissionRate { get; set; }

        /// <summary>
        /// 信用等级上限，1~20
        /// </summary>
        public Nullable<long> EndCredit { get; set; }

        /// <summary>
        /// 店铺商品总数上限
        /// </summary>
        public Nullable<long> EndTotalAction { get; set; }

        /// <summary>
        /// 需返回的字段列表
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// 是否商城的店铺，设置为true表示该是属于淘宝商城的店铺，设置为false或不设置表示不判断这个属性
        /// </summary>
        public Nullable<bool> IsTmall { get; set; }

        /// <summary>
        /// 第几页，默认1，1~100
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
        /// 排序_des（降序），排序_asc（升序），佣金比率（commission_rate）， 商品数量（auction_count），销售总数量（total_auction）
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 累计推广商品下限
        /// </summary>
        public Nullable<long> StartAuctionCount { get; set; }

        /// <summary>
        /// 淘客佣金比率下限，1~10000
        /// </summary>
        public Nullable<long> StartCommissionRate { get; set; }

        /// <summary>
        /// 信用等级下限，1~20
        /// </summary>
        public Nullable<long> StartCredit { get; set; }

        /// <summary>
        /// 店铺商品总数下限
        /// </summary>
        public Nullable<long> StartTotalAction { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.tbk.shop.get";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("end_auction_count", this.EndAuctionCount);
            parameters.Add("end_commission_rate", this.EndCommissionRate);
            parameters.Add("end_credit", this.EndCredit);
            parameters.Add("end_total_action", this.EndTotalAction);
            parameters.Add("fields", this.Fields);
            parameters.Add("is_tmall", this.IsTmall);
            parameters.Add("page_no", this.PageNo);
            parameters.Add("page_size", this.PageSize);
            parameters.Add("platform", this.Platform);
            parameters.Add("q", this.Q);
            parameters.Add("sort", this.Sort);
            parameters.Add("start_auction_count", this.StartAuctionCount);
            parameters.Add("start_commission_rate", this.StartCommissionRate);
            parameters.Add("start_credit", this.StartCredit);
            parameters.Add("start_total_action", this.StartTotalAction);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("fields", this.Fields);
            RequestValidator.ValidateRequired("q", this.Q);
        }

        #endregion
    }
}
