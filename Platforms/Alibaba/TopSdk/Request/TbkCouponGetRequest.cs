using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.tbk.coupon.get
    /// </summary>
    public class TbkCouponGetRequest : BaseTopRequest<Top.Api.Response.TbkCouponGetResponse>
    {
        /// <summary>
        /// 券ID
        /// </summary>
        public string ActivityId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public Nullable<long> ItemId { get; set; }

        /// <summary>
        /// 带券ID与商品ID的加密串
        /// </summary>
        public string Me { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.tbk.coupon.get";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("activity_id", this.ActivityId);
            parameters.Add("item_id", this.ItemId);
            parameters.Add("me", this.Me);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
        }

        #endregion
    }
}
