using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.tbk.item.recommend.get
    /// </summary>
    public class TbkItemRecommendGetRequest : BaseTopRequest<Top.Api.Response.TbkItemRecommendGetResponse>
    {
        /// <summary>
        /// 返回数量，默认20，最大值40
        /// </summary>
        public Nullable<long> Count { get; set; }

        /// <summary>
        /// 需返回的字段列表
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public Nullable<long> NumIid { get; set; }

        /// <summary>
        /// 链接形式：1：PC，2：无线，默认：１
        /// </summary>
        public Nullable<long> Platform { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.tbk.item.recommend.get";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("count", this.Count);
            parameters.Add("fields", this.Fields);
            parameters.Add("num_iid", this.NumIid);
            parameters.Add("platform", this.Platform);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("fields", this.Fields);
            RequestValidator.ValidateRequired("num_iid", this.NumIid);
        }

        #endregion
    }
}
