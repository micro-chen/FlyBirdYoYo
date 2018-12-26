using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.wireless.share.tpwd.query
    /// </summary>
    public class WirelessShareTpwdQueryRequest : BaseTopRequest<Top.Api.Response.WirelessShareTpwdQueryResponse>
    {
        /// <summary>
        /// 淘口令
        /// </summary>
        public string PasswordContent { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.wireless.share.tpwd.query";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("password_content", this.PasswordContent);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("password_content", this.PasswordContent);
        }

        #endregion
    }
}
