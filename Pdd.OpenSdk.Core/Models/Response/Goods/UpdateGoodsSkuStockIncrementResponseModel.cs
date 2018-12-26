using Newtonsoft.Json;
namespace Pdd.OpenSdk.Core.Models.Response.Goods
{
    public class SkuStockIncrementUpdateResponse
    {

        /// <summary>
        /// Examples: true
        /// </summary>
        [JsonProperty("is_success")]
        public bool IsSuccess { get; set; }
    }

    public class UpdateGoodsSkuStockIncrementResponseModel
    {

        /// <summary>
        /// Examples: {"is_success":true}
        /// </summary>
        [JsonProperty("sku_stock_increment_update_response")]
        public SkuStockIncrementUpdateResponse SkuStockIncrementUpdateResponse { get; set; }
    }


}
