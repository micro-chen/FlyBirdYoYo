using Newtonsoft.Json;
namespace Pdd.OpenSdk.Core.Models.Request.Logisticscs
{
    public partial class GetLogisticsTicketProblemTypeRequestModel : PddRequestModel
    {
        /// <summary>
        /// pdd.logistics.ticket.problem.type.get
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

    }

}
