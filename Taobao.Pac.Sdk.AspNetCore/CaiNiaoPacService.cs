using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Taobao.Pac.Sdk.Core;

namespace Taobao.Pac.Sdk.AspNetCore
{
    /// <summary>
    /// 菜鸟打印ISV服务
    /// </summary>
    public class CaiNiaoPacService : ICaiNiaoPacService
    {

        private PacClient client;


        private readonly IOptions<CaiNiaoPacOptions> _options;

        public CaiNiaoPacService(IOptions<CaiNiaoPacOptions> options)
        {
            _options = options;

            client = new PacClient(_options.Value.AppKey, _options.Value.AppSecret, _options.Value.PacUrl);
        }



        public T Send<T>(BaseRequest<T> request, string token) where T : BaseResponse
        {
            return this.client.Send<T>(request, token);
        }

        public Task<T> SendAsync<T>(BaseRequest<T> request, string token) where T : BaseResponse
        {
            return this.client.SendAsync<T>(request, token);
        }


        
    }
}
