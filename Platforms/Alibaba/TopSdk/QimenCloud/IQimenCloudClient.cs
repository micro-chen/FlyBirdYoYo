using System;

namespace QimenCloud.Api
{
    /// <summary>
    /// 奇门客户端。
    /// </summary>
    public interface IQimenCloudClient
    {
        /// <summary>
        /// 执行奇门公开API请求。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="request">具体的奇门API请求</param>
        /// <returns>领域对象</returns>
        T Execute<T>(IQimenCloudRequest<T> request) where T : QimenCloudResponse;

        /// <summary>
        /// 执行TOP隐私API请求。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="request">具体的TOP API请求</param>
        /// <param name="session">用户会话码</param>
        /// <returns>领域对象</returns>
        T Execute<T>(IQimenCloudRequest<T> request, string session) where T : QimenCloudResponse;

        /// <summary>
        /// 执行TOP隐私API请求。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="request">具体的TOP API请求</param>
        /// <param name="session">用户会话码</param>
        /// <param name="timestamp">请求时间戳</param>
        /// <returns>领域对象</returns>
        T Execute<T>(IQimenCloudRequest<T> request, string session, DateTime timestamp) where T : QimenCloudResponse;
    }
}
