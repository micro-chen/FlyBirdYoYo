using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.tbk.tpwd.create
    /// </summary>
    public class TbkTpwdCreateRequest : BaseTopRequest<Top.Api.Response.TbkTpwdCreateResponse>
    {
        /// <summary>
        /// 扩展字段JSON格式
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// 口令弹框logoURL
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 口令弹框内容
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 口令跳转目标页
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 生成口令的淘宝用户ID
        /// </summary>
        public string UserId { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.tbk.tpwd.create";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("ext", this.Ext);
            parameters.Add("logo", this.Logo);
            parameters.Add("text", this.Text);
            parameters.Add("url", this.Url);
            parameters.Add("user_id", this.UserId);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("text", this.Text);
            RequestValidator.ValidateRequired("url", this.Url);
        }

        #endregion
    }
}
