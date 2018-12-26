using Newtonsoft.Json;
namespace Pdd.OpenSdk.Core.Models.Request.Sms
{
    public partial class QuerySmsShortStatisticRequestModel : PddRequestModel
    {
        /// <summary>
        /// 任务id
        /// </summary>
        [JsonProperty("setting_id")]
        public int SettingId { get; set; }

    }

}
