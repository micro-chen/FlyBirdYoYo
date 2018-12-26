using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Taobao.Pac.Sdk.Core;

namespace Taobao.Pac.Sdk.Core
{
    public interface ICaiNiaoPacService
    {
        //ICaiNiaoPacOptions Options { get; set; }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="token">商家的菜鸟授权Token</param>
        /// <returns></returns>
        T Send<T>(BaseRequest<T> request, string token) where T : BaseResponse;

        /// <summary>
        /// 异步发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="token">商家的菜鸟授权Token</param>
        /// <returns></returns>
        Task<T> SendAsync<T>(BaseRequest<T> request, string token) where T : BaseResponse;
       
    }
}
