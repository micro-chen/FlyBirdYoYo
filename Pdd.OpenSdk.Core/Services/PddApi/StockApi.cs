using System.Threading.Tasks;
using Pdd.OpenSdk.Core.Models.Request.Stock;
using Pdd.OpenSdk.Core.Models.Response.Stock;
namespace Pdd.OpenSdk.Core.Services.PddApi
{
    public class StockApi : PddCommonApi
    {
        /// <summary>
        /// 家电分仓库存-库存信息调整
        /// </summary>
        public async Task<MoveStockWareResponseModel> MoveStockWareAsync(MoveStockWareRequestModel moveStockWare)
        {
            var result = await PostAsync<MoveStockWareRequestModel, MoveStockWareResponseModel>("pdd.stock.ware.move", moveStockWare);
            return result;
        }

    }
}
