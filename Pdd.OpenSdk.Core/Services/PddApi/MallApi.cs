using System.Threading.Tasks;
using Pdd.OpenSdk.Core.Models.Request.Mall;
using Pdd.OpenSdk.Core.Models.Response.Mall;
namespace Pdd.OpenSdk.Core.Services.PddApi
{
    public class MallApi : PddCommonApi
    {
        /// <summary>
        /// 店铺信息接口
        /// </summary>
        public async Task<GetMallInfoResponseModel> GetMallInfoAsync(GetMallInfoRequestModel getMallInfo)
        {
            var result = await PostAsync<GetMallInfoRequestModel, GetMallInfoResponseModel>("pdd.mall.info.get", getMallInfo);
            return result;
        }

    }
}
