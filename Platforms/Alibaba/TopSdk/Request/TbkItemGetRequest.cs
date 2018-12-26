using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.tbk.item.get
    /// </summary>
    public class TbkItemGetRequest : BaseTopRequest<Top.Api.Response.TbkItemGetResponse>
    {
        /// <summary>
        /// 后台类目ID，用,分割，最大10个，该ID可以通过taobao.itemcats.get接口获取到
        /// </summary>
        public string Cat { get; set; }

        /// <summary>
        /// 折扣价范围上限，单位：元
        /// </summary>
        public Nullable<long> EndPrice { get; set; }

        /// <summary>
        /// 淘客佣金比率下限，如：1234表示12.34%
        /// </summary>
        public Nullable<long> EndTkRate { get; set; }

        /// <summary>
        /// 需返回的字段列表
        /// </summary>
        public string Fields { get; set; }

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
        /// 排序_des（降序），排序_asc（升序），销量（total_sales），淘客佣金比率（tk_rate）， 累计推广量（tk_total_sales），总支出佣金（tk_total_commi）
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 折扣价范围下限，单位：元
        /// </summary>
        public Nullable<long> StartPrice { get; set; }

        /// <summary>
        /// 淘客佣金比率上限，如：1234表示12.34%
        /// </summary>
        public Nullable<long> StartTkRate { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.tbk.item.get";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("cat", this.Cat);
            parameters.Add("end_price", this.EndPrice);
            parameters.Add("end_tk_rate", this.EndTkRate);
            parameters.Add("fields", this.Fields);
            parameters.Add("is_overseas", this.IsOverseas);
            parameters.Add("is_tmall", this.IsTmall);
            parameters.Add("itemloc", this.Itemloc);
            parameters.Add("page_no", this.PageNo);
            parameters.Add("page_size", this.PageSize);
            parameters.Add("platform", this.Platform);
            parameters.Add("q", this.Q);
            parameters.Add("sort", this.Sort);
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
            RequestValidator.ValidateRequired("fields", this.Fields);
        }

        #endregion
    }
}
