using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.openuid.change
    /// </summary>
    public class OpenuidChangeRequest : BaseTopRequest<Top.Api.Response.OpenuidChangeResponse>
    {
        /// <summary>
        /// openUid
        /// </summary>
        public string OpenUid { get; set; }

        /// <summary>
        /// 转换到的appkey
        /// </summary>
        public string TargetAppKey { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.openuid.change";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("open_uid", this.OpenUid);
            parameters.Add("target_app_key", this.TargetAppKey);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("open_uid", this.OpenUid);
            RequestValidator.ValidateRequired("target_app_key", this.TargetAppKey);
        }

        #endregion
    }
}
