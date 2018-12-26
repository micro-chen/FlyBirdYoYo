using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.openuid.get.bymixnick
    /// </summary>
    public class OpenuidGetBymixnickRequest : BaseTopRequest<Top.Api.Response.OpenuidGetBymixnickResponse>
    {
        /// <summary>
        /// 无线类应用获取到的混淆的nick
        /// </summary>
        public string MixNick { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.openuid.get.bymixnick";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("mix_nick", this.MixNick);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("mix_nick", this.MixNick);
        }

        #endregion
    }
}
