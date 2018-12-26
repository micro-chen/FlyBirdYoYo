using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.tbk.item.info.get
    /// </summary>
    public class TbkItemInfoGetRequest : BaseTopRequest<Top.Api.Response.TbkItemInfoGetResponse>
    {
        /// <summary>
        /// ip地址，影响邮费获取，如果不传或者传入不准确，邮费无法精准提供
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 商品ID串，用,分割，最大40个
        /// </summary>
        public string NumIids { get; set; }

        /// <summary>
        /// 链接形式：1：PC，2：无线，默认：１
        /// </summary>
        public Nullable<long> Platform { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.tbk.item.info.get";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("ip", this.Ip);
            parameters.Add("num_iids", this.NumIids);
            parameters.Add("platform", this.Platform);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("num_iids", this.NumIids);
        }

        #endregion
    }
}
