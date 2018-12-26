using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.tbk.uatm.favorites.get
    /// </summary>
    public class TbkUatmFavoritesGetRequest : BaseTopRequest<Top.Api.Response.TbkUatmFavoritesGetResponse>
    {
        /// <summary>
        /// 需要返回的字段列表，不能为空，字段名之间使用逗号分隔
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// 第几页，从1开始计数
        /// </summary>
        public Nullable<long> PageNo { get; set; }

        /// <summary>
        /// 默认20，页大小，即每一页的活动个数
        /// </summary>
        public Nullable<long> PageSize { get; set; }

        /// <summary>
        /// 默认值-1；选品库类型，1：普通选品组，2：高佣选品组，-1，同时输出所有类型的选品组
        /// </summary>
        public Nullable<long> Type { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.tbk.uatm.favorites.get";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("fields", this.Fields);
            parameters.Add("page_no", this.PageNo);
            parameters.Add("page_size", this.PageSize);
            parameters.Add("type", this.Type);
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
