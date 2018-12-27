using Microsoft.AspNetCore.Http;

namespace FlyBirdYoYo.Utilities.Interface
{
    /// <summary>
    /// 授权服务接口
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// 验证登录授权
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool Authentication(IBaseLoginViewModel model, out string token);


        /// <summary>
        /// 验证是否为超管账号
        /// </summary>
        /// <returns></returns>
        bool CheckUserIsSystemAdminFromHttpContext();
        /// <summary>
        /// 从当前HttpContext上下文中获取授权用户信息
        /// </summary>
        /// <returns></returns>
        ILoginAuthedUserDTO GetAuthenticatedUserFromHttpContext();
    }
}