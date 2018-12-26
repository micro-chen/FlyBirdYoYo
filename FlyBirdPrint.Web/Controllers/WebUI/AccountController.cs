using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlyBirdYoYo.Web.Mvc;
using FlyBirdYoYo.Utilities.Logging;
using FlyBirdYoYo.Utilities.DEncrypt;

using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.DomainEntity.QueryCondition;
using FlyBirdYoYo.BusinessServices;
using FlyBirdYoYo.DomainEntity.Login;
using FlyBirdYoYo.Utilities;
using TwoFactorAuthNet;
using FlyBirdYoYo.DomainEntity;
using FlyBirdYoYo.Utilities.SystemEnum;

namespace FlyBirdYoYo.Web.Controllers.WebUI
{
    public class AccountController : BaseApiControllerNoAuth
    {


        /// <summary>
        /// 登录验证-第一阶段-验证用户名密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public BusinessViewModelContainer<LoginSystemAdminResultViewModel> LoginCheckUser( PasswordLoginViewModel model)
        {


            BusinessViewModelContainer<LoginSystemAdminResultViewModel> viewModel = new BusinessViewModelContainer<LoginSystemAdminResultViewModel>();
            try
            {
                //先去检查用户名密码
                string uName = model.UserName;
                string pwd = DESEncrypt.Encrypt(model.Pwd);
                if (string.IsNullOrEmpty(uName) || string.IsNullOrEmpty(pwd))
                {
                    viewModel.Data = new LoginSystemAdminResultViewModel { Message = "用户名密码不能为空！" };
                    return viewModel;
                }


                var sysUser = Singleton<SysAdminService>.Instance
                     .GetSysAdminFirstOrDefaultByCondition(x => x.Uname == uName
                     && x.Upassword == pwd && x.State == true);

                if (null != sysUser)
                {
                    string next_step = "2";//验证通过后，加密的签名进行第二步操作--动态口令验证
                    var lstSignParas = new string[]
                    {
                        DateTime.Now.ToTimeStampMilliseconds().ToString(),
                        next_step,
                        uName,
                       pwd
                    };

                    string sign = DESEncrypt.Encrypt(string.Join('|', lstSignParas));

                    viewModel.Data = new LoginSystemAdminResultViewModel
                    {
                        Message = "登录成功！请进行二阶验证！",
                        IsSuccess = true,
                        Step = 1,//标识第一步验证通过
                        Sign = sign//自定义签名
                    };

                }
                else
                {
                    viewModel.Data = new LoginSystemAdminResultViewModel { Message = "用户名密码输入错误！" };
                }


            }
            catch (Exception ex)
            {
                viewModel.SetFalied("调用失败了！");
                Logger.Error(ex);
            }

            return viewModel;
        }


        /// <summary>
        /// 登录验证---第二阶验证（Google身份验证）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public BusinessViewModelContainer<bool> LoginCheckDyCode(PasswordLoginViewModel model)
        {


            BusinessViewModelContainer<bool> viewModel = new BusinessViewModelContainer<bool>();
             
            try
            {
                if (model.CheckCode.IsNullOrEmpty())
                {
                    return viewModel;
                }
                if (model.Sign.IsNullOrEmpty())
                {
                    viewModel.SetFalied("签名不能为空！");
                    return viewModel;
                }
                string deSign = string.Empty;
                try
                {
                    deSign = DESEncrypt.Decrypt(model.Sign);
                }
                catch
                { }
                if (deSign.IsNullOrEmpty())
                {
                    viewModel.SetFalied("签名错误！");
                    return viewModel;
                }
                string[] arrSign = deSign.Split('|');
                long timeSnamp = arrSign[0].ToLong();
                int step = arrSign[1].ToInt();

                //时间戳之间的间隔不能过长-不可超过5分钟
                if ((DateTime.Now.ToTimeStampMilliseconds() - timeSnamp) / 1000 > 5 * 60)
                {
                    viewModel.SetFalied("登录超时！请重新输入用户名密码！");
                    return viewModel;
                }

                if (step != 2 || arrSign.Length < 4)
                {
                    viewModel.SetFalied("登录必须输入密码！请重新输入用户名密码！");
                    return viewModel;
                }
                string uName = arrSign[2];
                string pwd = arrSign[3];
                if (string.IsNullOrEmpty(uName) || string.IsNullOrEmpty(pwd))
                {
                    viewModel.SetFalied("登录必须输入密码！请重新输入用户名密码！");
                    return viewModel;
                }


                var sysUser = Singleton<SysAdminService>.Instance
                     .GetSysAdminFirstOrDefaultByCondition(x => x.Uname == uName
                     && x.Upassword == pwd && x.State == true);

                if (null == sysUser)
                {
                    viewModel.SetFalied("未知用户！");
                    return viewModel;
                }
                if (string.IsNullOrEmpty(sysUser.PublicKey))
                {
                    viewModel.SetFalied("用户密钥已经失效！请联系管理员！");
                    return viewModel;
                }


                //进行谷歌身份验证，如果验证通过，那么写入系统用户Cookie
                //写入凭证
                //todo:进行谷歌二阶验证
                var tfaProvider = new TwoFactorAuth();

                bool validateResult = false;
                try
                {
                    validateResult = tfaProvider.VerifyCode(sysUser.PublicKey, model.CheckCode);
                }
                catch 
                { }

                if (true == validateResult)
                {
                    //验证通过
                    //1 记录登录日志：
                    var logModel = new SysLogModel
                    {
                        Level =1,
                        SysUserId = sysUser.Id,
                        //LogType = (int)SysLogTypeEnum.Login,
                        LogContent = $"超管账号：{sysUser.Uname} , 登录系统！",
                        CreateTime = DateTime.Now,
                        IpAddress = base.IpAddress
                    };
                    Singleton<SysLogService>.Instance.AddOneSysLogModel(logModel);

                    //2 客户端授权并进入后台页面
                    viewModel.Msg = "成功登录！";
                    viewModel.Data = true;


                    var sysUserLoginModel = new LoginSystemAdminResultViewModel
                    {
                        AdminUserId = sysUser.Id,
                        IsSuccess = true,
                        Step = 3,
                        Sign = model.Sign//自定义签名
                    };


                    string authJson = sysUserLoginModel.ToJson();
                    //写入用户基本信息Cookie
                    HttpContext.SetCookie(Contanst.Global_Site_Domain_Cookie, Contanst.Login_Cookie_SystemAdminUserInfo, authJson);

                }
                else
                {
                    viewModel.SetFalied("口令已经过期，请重新输入！");

                }


            }
            catch (Exception ex)
            {
                viewModel.SetFalied("验证失败！");
                Logger.Error(ex);
            }

            return viewModel;
        }


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public BusinessViewModelContainer<bool> Logout()
        {
            BusinessViewModelContainer<bool> viewModel = new BusinessViewModelContainer<bool>();

            try
            {
                //系统管理员退出的清理
                bool isCookieOfAdmin = Request.Cookies.ContainsKey(Contanst.Login_Cookie_SystemAdminUserInfo);
                if (isCookieOfAdmin)
                {
                    //清除cookie
                    HttpContext.RemoveCookie(Contanst.Login_Cookie_SystemAdminUserInfo, Contanst.Global_Site_Domain_Cookie);
                    HttpContext.RemoveCookie(Contanst.Login_Cookie_Client_Key, Contanst.Global_Site_Domain_Cookie);
                    //写入用户基本信息Cookie
                    HttpContext.RemoveCookie(Contanst.Login_Cookie_UserInfo, Contanst.Global_Site_Domain_Cookie);
                  

                }



                //普通商户退出登录
                //清除cookie
                HttpContext.RemoveCookie(Contanst.Login_Cookie_Client_Key, Contanst.Global_Site_Domain_Cookie);
                //写入用户基本信息Cookie
                HttpContext.RemoveCookie(Contanst.Login_Cookie_UserInfo, Contanst.Global_Site_Domain_Cookie);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return viewModel;

        }




    }
}
