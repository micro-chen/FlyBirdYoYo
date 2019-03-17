using Newtonsoft.Json;

namespace Alipay.AopSdk.Core.Response
{
	/// <summary>
	///     AlipayMobilePublicMessagebatchPushResponse.
	/// </summary>
	public class AlipayMobilePublicMessagebatchPushResponse : AopResponse
	{
		/// <summary>
		///     成功
		/// </summary>
		[JsonProperty("code")]
		public override string Code { get; set; }

		/// <summary>
		///     消息ID
		/// </summary>
		[JsonProperty("data")]
		public string Data { get; set; }

		/// <summary>
		///     成功
		/// </summary>
		[JsonProperty("msg")]
		public override string Msg { get; set; }
	}
}