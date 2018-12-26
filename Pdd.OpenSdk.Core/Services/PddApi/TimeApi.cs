using System.Threading.Tasks;
using Pdd.OpenSdk.Core.Models.Request.Time;
using Pdd.OpenSdk.Core.Models.Response.Time;
namespace Pdd.OpenSdk.Core.Services.PddApi
{
    public class TimeApi : PddCommonApi
    {
        /// <summary>
        /// 获取拼多多系统时间
        /// </summary>
        public async Task<GetTimeResponseModel> GetTimeAsync(GetTimeRequestModel getTime)
        {
            var result = await PostAsync<GetTimeRequestModel, GetTimeResponseModel>("pdd.time.get", getTime);
            return result;
        }

    }
}
