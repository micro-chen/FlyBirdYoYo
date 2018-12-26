using Newtonsoft.Json;
namespace Pdd.OpenSdk.Core.Models.Response.Goods
{
    public class ChangeGoodsCpsUnitResponseModel
    {

        /// <summary>
        /// Examples: true
        /// </summary>
        [JsonProperty("is_change_success")]
        public bool IsChangeSuccess { get; set; }
    }


}
