using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.tbk.content.get
    /// </summary>
    public class TbkContentGetRequest : BaseTopRequest<Top.Api.Response.TbkContentGetResponse>
    {
        /// <summary>
        /// 推广位
        /// </summary>
        public Nullable<long> AdzoneId { get; set; }

        /// <summary>
        /// 表示取这个时间点以前的数据，以毫秒为单位（出参中的last_timestamp是指本次返回的内容中最早一条的数据，下一次作为before_timestamp传过来，即可实现翻页）
        /// </summary>
        public Nullable<long> BeforeTimestamp { get; set; }

        /// <summary>
        /// 类目
        /// </summary>
        public Nullable<long> Cid { get; set; }

        /// <summary>
        /// 默认可不传,内容库类型:1 优质内容
        /// </summary>
        public Nullable<long> ContentSet { get; set; }

        /// <summary>
        /// 表示期望获取条数
        /// </summary>
        public Nullable<long> Count { get; set; }

        /// <summary>
        /// 图片高度
        /// </summary>
        public Nullable<long> ImageHeight { get; set; }

        /// <summary>
        /// 图片宽度
        /// </summary>
        public Nullable<long> ImageWidth { get; set; }

        /// <summary>
        /// 内容类型，1:图文、2: 图集、3: 短视频
        /// </summary>
        public Nullable<long> Type { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.tbk.content.get";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("adzone_id", this.AdzoneId);
            parameters.Add("before_timestamp", this.BeforeTimestamp);
            parameters.Add("cid", this.Cid);
            parameters.Add("content_set", this.ContentSet);
            parameters.Add("count", this.Count);
            parameters.Add("image_height", this.ImageHeight);
            parameters.Add("image_width", this.ImageWidth);
            parameters.Add("type", this.Type);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("adzone_id", this.AdzoneId);
            RequestValidator.ValidateMaxValue("image_height", this.ImageHeight, 1000);
            RequestValidator.ValidateMinValue("image_height", this.ImageHeight, 10);
            RequestValidator.ValidateMaxValue("image_width", this.ImageWidth, 1000);
            RequestValidator.ValidateMinValue("image_width", this.ImageWidth, 10);
        }

        #endregion
    }
}
