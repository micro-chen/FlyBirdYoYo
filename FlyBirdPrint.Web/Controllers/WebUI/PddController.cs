using System;
using System.AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlyBirdYoYo.Web.Mvc;
using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.Utilities.DataStructure;

using FlyBirdYoYo.Utilities.Logging;
using FlyBirdYoYo.Utilities.SystemEnum;
using FlyBirdYoYo.DomainEntity.Login;
using Pdd.OpenSdk.Core.Models.Request.Mall;
using Pdd.OpenSdk.Core;
using FlyBirdYoYo.Utilities.Ioc;
using Pdd.OpenSdk.Core.Models;
using Microsoft.AspNetCore.Http;

namespace FlyBirdYoYo.Web.Controllers.WebUI
{
    /// <summary>
    /// 拼多多平台页面控制器
    /// 跳转接收等
    /// </summary>
    public class PddController : BaseMvcController
    {

        #region 字段
        #endregion

  

        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = new PddIndexViewModel();

            var configSection = ConfigHelper.GetConfigSection<AppSecretConfigSection>(AppSecretConfigSection.SectionName);

            string baseCodePath = configSection.Pdd.code_address;
            var keyPair = new NameValueCollection();
            keyPair.Add("client_id", configSection.Pdd.client_id);
            keyPair.Add("response_type", "code");
            //时间戳+签名
            long timeStamp = DateTime.Now.ToTimeStampMilliseconds();
            string sign = StringExtension.DEFAULT_ENCRYPT_KEY.GetCrossSiteSign(timeStamp);

            string dataStr = string.Concat(timeStamp, this.SplitChar, sign);
            keyPair.Add("state", dataStr);
            keyPair.Add("redirect_uri", configSection.Pdd.redirect_uri);

            

            //---字典转为字符串---
            string paramString = keyPair.ToQueryString();

            model.AuthCodeAddress = string.Concat(baseCodePath, paramString);

            return View(model);
        }


        /// <summary>
        /// 拼多多登录授权跳转接收
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("call_back_handler")]
        public async Task<IActionResult> AuthCallBack(string code, string state)
        {

            BusinessViewModelContainer<string> msgResult = new BusinessViewModelContainer<string>();


            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
            {
                msgResult.SetFalied("参数丢失！");

                return new JsonResult(msgResult);
            }

            try
            {
                //1 验证state的数据
                //时间戳+签名
                var array_State = state.Split(this.SplitChar);
                long timeStamp = array_State[0].ToLong();

                string sign = array_State[1];//
                if (timeStamp <= 0 || sign.IsNullOrEmpty())
                {
                    msgResult.SetFalied("拼多多回调缺少参数！！");
                    return new JsonResult(msgResult);
                }

                var serverSign = StringExtension.DEFAULT_ENCRYPT_KEY.GetCrossSiteSign(timeStamp);
                if (!string.Equals(sign, serverSign))
                {
                    msgResult.SetFalied("拼多多回调签名验证失败！");
                    return new JsonResult(msgResult);
                }



                //--------验证通过--------
                //0 根据code  请求 access_token
                //1 获取refresh_token--根据数据库里的信息，检查是否过期，如果没有过期，那么不用重新拉取
                //2 获取用户信息，并将 access_token  生成cookie凭据。然后跳转到后台管理页面。
                //3 进入后台后，将授权cookie（7天过期） 放置在头.进行jwt的验证授权。一旦过期，那么进行刷新token，并生成新的凭证
                var configSection = ConfigHelper.GetConfigSection<AppSecretConfigSection>(AppSecretConfigSection.SectionName);


                //获取token结果模型
                var tokenModel = await this.PddSdkService.AuthApi.GetAccessTokenAsync(code, state);
                if (null == tokenModel)
                {
                    msgResult.SetFalied("拼多多获取Access_Token失败！");
                    return new JsonResult(msgResult);
                }


                //调用查询商家接口信息
                GetMallInfoRequestModel cond_pdd_mall = new GetMallInfoRequestModel { AccessToken = tokenModel.AccessToken };
                ////////查询物流信息
                //////var wuliuList = await this.PddSdkService.LogisticsApi.GetLogisticsCompaniesAsync(
                //////    new Pdd.OpenSdk.Core.Models.Request.Logistics.GetLogisticsCompaniesRequestModel { AccessToken = tokenModel.AccessToken });

                //////////查询订单信息
                //////////var cond = new PddOrderListQueryConditon(tokenModel.AccessToken);
                //////////var orderMouduleProvider = QueryOrderModuleProviderFactory.LoadProvider(cond);
                //////////var orderList = orderMouduleProvider.GetOrderList(cond);
                ////////////查询类目信息
                //////////var catList = await this.PddSdkService.GoodsApi.CatsGoodsAuthorizationAsync(new Pdd.OpenSdk.Core.Models.Request.Goods.CatsGoodsAuthorizationRequestModel
                //////////{
                //////////    AccessToken = tokenModel.AccessToken,
                //////////    ParentCatId = 0

                //////////});



                var mallResult = await this.PddSdkService.MallApi.GetMallInfoAsync(cond_pdd_mall);
                if (null == mallResult)
                {
                    //仅仅输出日志即可--不重要的信息
                    Logger.Info("商家信息查询失败！未能成功返回商家信息！" + mallResult.ToJson() + cond_pdd_mall.ToJson());
                }
                string ticket = "";
                var loginModel = new OAuthLoginViewModel
                {
                    Platform = PlatformEnum.Pdd,
                    ShopId = tokenModel.OwnerId,
                    UserName = tokenModel.OwnerName,
                    Access_token = tokenModel.AccessToken,
                    Refresh_token = tokenModel.RefreshToken,
                    ExpireTime = DateTime.Now.AddSeconds(tokenModel.ExpiresIn),
                    AuthJson = mallResult.ToJson(),
                    Code = code
                };

                if (null != mallResult)
                {
                    loginModel.ShopName = mallResult.MallInfoGetResponse.MallName;
                }

                bool result = this.AuthenticationServiceImpl.Authentication(loginModel, out ticket);
                if (result == true)
                {
                    //登录用户基本信息
                    var baseUserInfo = loginModel.MapTo<BaseLoginViewModel>();


                    //写入授权凭据到cookie
                    HttpContext.SetCookie(Contanst.Global_Site_Domain_Cookie, Contanst.Login_Cookie_Client_Key, ticket);
                    //写入用户基本信息Cookie
                    HttpContext.SetCookie(Contanst.Global_Site_Domain_Cookie, Contanst.Login_Cookie_UserInfo, baseUserInfo.ToJson());

                    return Redirect(ConfigHelper.AppSettingsConfiguration.GetConfig("DomainNameForUI"));
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            //没有验证通的模式进入到登录首页
            return Redirect("/pdd/index");
        }


       
    }
}
