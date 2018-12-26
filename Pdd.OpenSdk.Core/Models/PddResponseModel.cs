namespace Pdd.OpenSdk.Core.Models.Response
{
    /// <summary>
    ///拼多多官方接口调用返回的结果基类
    /// </summary>
    public abstract class PddResponseModel
    {
        /// <summary>
        /// 响应内容
        /// </summary>
        public string ResponseContent { get; set; }

    }
}
