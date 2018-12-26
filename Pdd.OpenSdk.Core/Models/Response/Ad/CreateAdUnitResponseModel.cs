using Newtonsoft.Json;
namespace Pdd.OpenSdk.Core.Models.Response.Ad
{
    public class AdUnitCreateResponse
    {

        /// <summary>
        /// Examples: true
        /// </summary>
        [JsonProperty("is_success")]
        public bool IsSuccess { get; set; }
    }

    public class CreateAdUnitResponseModel
    {

        /// <summary>
        /// Examples: {"is_success":true}
        /// </summary>
        [JsonProperty("ad_unit_create_response")]
        public AdUnitCreateResponse AdUnitCreateResponse { get; set; }
    }


}
