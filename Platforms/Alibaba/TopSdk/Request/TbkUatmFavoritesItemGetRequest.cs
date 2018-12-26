using System;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.tbk.uatm.favorites.item.get
    /// </summary>
    public class TbkUatmFavoritesItemGetRequest : BaseTopRequest<Top.Api.Response.TbkUatmFavoritesItemGetResponse>
    {
        /// <summary>
        /// 推广位id，需要在淘宝联盟后台创建；且属于appkey备案的媒体id（siteid），如何获取adzoneid，请参考，http://club.alimama.com/read-htm-tid-6333967.html?spm=0.0.0.0.msZnx5
        /// </summary>
        public Nullable<long> AdzoneId { get; set; }

        /// <summary>
        /// 选品库的id
        /// </summary>
        public Nullable<long> FavoritesId { get; set; }

        /// <summary>
        /// 需要输出则字段列表，逗号分隔
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// 第几页，默认：1，从1开始计数
        /// </summary>
        public Nullable<long> PageNo { get; set; }

        /// <summary>
        /// 页大小，默认20，1~100
        /// </summary>
        public Nullable<long> PageSize { get; set; }

        /// <summary>
        /// 链接形式：1：PC，2：无线，默认：１
        /// </summary>
        public Nullable<long> Platform { get; set; }

        /// <summary>
        /// 自定义输入串，英文和数字组成，长度不能大于12个字符，区分不同的推广渠道
        /// </summary>
        public string Unid { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.tbk.uatm.favorites.item.get";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("adzone_id", this.AdzoneId);
            parameters.Add("favorites_id", this.FavoritesId);
            parameters.Add("fields", this.Fields);
            parameters.Add("page_no", this.PageNo);
            parameters.Add("page_size", this.PageSize);
            parameters.Add("platform", this.Platform);
            parameters.Add("unid", this.Unid);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("adzone_id", this.AdzoneId);
            RequestValidator.ValidateRequired("favorites_id", this.FavoritesId);
            RequestValidator.ValidateRequired("fields", this.Fields);
        }

        #endregion
    }
}
