using System.Threading.Tasks;
using Pdd.OpenSdk.Core.Models.Request.Virtual;
using Pdd.OpenSdk.Core.Models.Response.Virtual;
namespace Pdd.OpenSdk.Core.Services.PddApi
{
    public class VirtualApi : PddCommonApi
    {
        /// <summary>
        /// 虚拟类目发货通知接口
        /// </summary>
        public async Task<NotifyVirtualMobileChargeResponseModel> NotifyVirtualMobileChargeAsync(NotifyVirtualMobileChargeRequestModel notifyVirtualMobileCharge)
        {
            var result = await PostAsync<NotifyVirtualMobileChargeRequestModel, NotifyVirtualMobileChargeResponseModel>("pdd.virtual.mobile.charge.notify", notifyVirtualMobileCharge);
            return result;
        }

    }
}
