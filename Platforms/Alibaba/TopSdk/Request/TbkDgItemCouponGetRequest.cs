using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.tbk.dg.item.coupon.get
    /// </summary>
    public class TbkDgItemCouponGetRequest : BaseTopRequest<Top.Api.Response.TbkDgItemCouponGetResponse>
    {
        /// <summary>
        /// mm_xxx_xxx_xxx的第三位
        /// </summary>
        public Nullable<long> AdzoneId { get; set; }

        /// <summary>
        /// 后台类目ID，用,分割，最大10个，该ID可以通过taobao.itemcats.get接口获取到
        /// </summary>
        public string Cat { get; set; }

        /// <summary>
        /// 第几页，默认：1（当后台类目和查询词均不指定的时候，最多出10000个结果，即page_no*page_size不能超过10000；当指定类目或关键词的时候，则最多出100个结果）
        /// </summary>
        public Nullable<long> PageNo { get; set; }

        /// <summary>
        /// 页大小，默认20，1~100
        /// </summary>
        public Nullable<long> PageSize { get; set; }

        /// <summary>
        /// 1：PC，2：无线，默认：1
        /// </summary>
        public Nullable<long> Platform { get; set; }

        /// <summary>
        /// 查询词
        /// </summary>
        public string Q { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.tbk.dg.item.coupon.get";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("adzone_id", this.AdzoneId);
            parameters.Add("cat", this.Cat);
            parameters.Add("page_no", this.PageNo);
            parameters.Add("page_size", this.PageSize);
            parameters.Add("platform", this.Platform);
            parameters.Add("q", this.Q);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("adzone_id", this.AdzoneId);
            RequestValidator.ValidateMaxLength("cat", this.Cat, 10);
        }

        #endregion
    }
}
