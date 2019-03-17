using Newtonsoft.Json;

namespace Alipay.AopSdk.Core.Response
{
	/// <summary>
	///     AlipayMobilePublicShortlinkCreateResponse.
	/// </summary>
	public class AlipayMobilePublicShortlinkCreateResponse : AopResponse
	{
		/// <summary>
		///     结果码
		/// </summary>
		[JsonProperty("code")]
		public override string Code { get; set; }

		/// <summary>
		///     成功
		/// </summary>
		[JsonProperty("msg")]
		public override string Msg { get; set; }

		/// <summary>
		///     短链接url
		/// </summary>
		[JsonProperty("shortlink")]
		public string Shortlink { get; set; }
	}
}