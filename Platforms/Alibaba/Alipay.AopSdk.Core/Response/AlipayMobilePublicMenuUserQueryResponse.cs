using Newtonsoft.Json;

namespace Alipay.AopSdk.Core.Response
{
	/// <summary>
	///     AlipayMobilePublicMenuUserQueryResponse.
	/// </summary>
	public class AlipayMobilePublicMenuUserQueryResponse : AopResponse
	{
		/// <summary>
		///     结果码
		/// </summary>
		[JsonProperty("code")]
		public override string Code { get; set; }

		/// <summary>
		///     菜单唯一标识
		/// </summary>
		[JsonProperty("menu_key")]
		public string MenuKey { get; set; }

		/// <summary>
		///     结果码描述
		/// </summary>
		[JsonProperty("msg")]
		public override string Msg { get; set; }
	}
}