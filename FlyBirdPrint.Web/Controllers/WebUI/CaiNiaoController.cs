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
using FlyBirdYoYo.Utilities.Ioc;
using FlyBirdYoYo.Utilities.Http;
using FlyBirdYoYo.BusinessServices;

namespace FlyBirdYoYo.Web.Controllers.WebUI
{
    /// <summary>
    /// 菜鸟 Link  ISV的回调业务
    /// 需要用户登录
    /// </summary>
    [AuthAttributeFilter(IsCheck = false)]
    public class CaiNiaoController : BaseMvcController
    {

        public IActionResult Index()
        {
            //////var curentUser = ApplicationContext.Current.User;
            //////if (null==curentUser)
            //////{
            //////    return null;//安全性校验
            //////}

            var model = new CaiNiaoIndexViewModel();

            var configSection = ConfigHelper.GetConfigSection<AppSecretConfigSection>(AppSecretConfigSection.SectionName);

            if (string.IsNullOrEmpty(configSection.CaiNiao.redirect_uri))
            {
                throw new Exception("配置节点缺少了回调地址：redirect_uri ！");
            }

            string baseCodePath = configSection.CaiNiao.ShopOAuthAddress;
            var keyPair = new NameValueCollection();
            keyPair.Add("isvAppKey", configSection.CaiNiao.AppKey);

            //时间戳+签名
            long timeStamp = DateTime.Now.ToTimeStampMilliseconds();
            string sign = StringExtension.DEFAULT_ENCRYPT_KEY.GetCrossSiteSign(timeStamp);
            string dataStr = string.Concat(timeStamp, this.SplitChar, sign);
            keyPair.Add("ext", dataStr);
            keyPair.Add("redirectUrl", configSection.CaiNiao.redirect_uri);



            //---字典转为字符串---
            string paramString = keyPair.ToQueryString();

            model.AuthCodeAddress = string.Concat(baseCodePath, paramString);

            return View(model);
        }



        /// <summary>
        /// 菜鸟的回调处理
        /// 发起地址：
        ///// https://open.taobao.com/doc.htm?docId=107149&docType=1
        /// //http://lcp.cloud.cainiao.com/permission/isv/grantpage.do?isvAppKey=<YourAppKey>&ext=<YourExtentionInfo>&redirectUrl=http://www.<YourRedirectUrl>.com/auth/callback.do
        /// //http://lcp.cloud.cainiao.com/permission/isv/grantpage.do?isvAppKey=472333&ext=66666666666&redirectUrl=http://api.flybirdyoyo.com/cainiao/call_back_handler
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [ActionName("call_back_handler")]
        public async Task<IActionResult> AuthCallBack(string accessCode, string appkey, string ext)
        {
            BusinessViewModelContainer<string> msgResult = new BusinessViewModelContainer<string>();

            if (string.IsNullOrEmpty(accessCode) || string.IsNullOrEmpty(appkey) || string.IsNullOrEmpty(ext))
            {
                msgResult.SetFalied("参数丢失！");
                return new JsonResult(msgResult);
            }

            var configSection = ConfigHelper.GetConfigSection<AppSecretConfigSection>(AppSecretConfigSection.SectionName);

            //回调的跟发起授权的key不一样，返回错误
            if (appkey != configSection.CaiNiao.AppKey)
            {
                msgResult.SetFalied("非法请求！");
                return new JsonResult(msgResult);
            }

            if (null == ApplicationContext.Current.User)
            {
                msgResult.SetFalied("当前用户未登录，请登录！！");
                return new JsonResult(msgResult);

            }

            //1 验证state的数据
            //时间戳+签名
            var array_State = ext.Split(this.SplitChar);
            long timeStamp = array_State[0].ToLong();

            string signInner = array_State[1];//
            if (timeStamp <= 0 || signInner.IsNullOrEmpty())
            {
                msgResult.SetFalied("拼多多回调缺少参数！！");
                return new JsonResult(msgResult);
            }

            var serverSign = StringExtension.DEFAULT_ENCRYPT_KEY.GetCrossSiteSign(timeStamp);
            if (!string.Equals(signInner, serverSign))
            {
                msgResult.SetFalied("拼多多回调签名验证失败！");
                return new JsonResult(msgResult);
            }



            //--------验证通过--------
            // 发起请求去交换当前用户的Token

            try
            {


                string baseExtenPath = configSection.CaiNiao.ShopExtenTokenAddress;
                var keyPair = new NameValueCollection();
                
                     keyPair.Add("accessCode", accessCode);
                keyPair.Add("isvAppKey", configSection.CaiNiao.AppKey);

                //菜鸟的签名
                //sign：签名，证明ISV合法，签名规则：md5(accessCode + "," + appKey + "," + appSecret); Note: 别忘了逗号是英文状态下的逗号，签名方法：
                var lstSignParas = new List<string> { accessCode, configSection.CaiNiao.AppKey, configSection.CaiNiao.AppSecret };
                string signCaiNiaoData = string.Join(',', lstSignParas).GetMD5().ToLower();
                keyPair.Add("sign", signCaiNiaoData);



                //---字典转为字符串---
                string paramString = keyPair.ToQueryString();

                string extentTokenAddress = string.Concat(baseExtenPath, paramString);

                var resp = await Singleton<HttpServerProxy>.Instance.GetAsync(extentTokenAddress);
                if (null!=resp&&resp.IsSuccessStatusCode)
                {

                    string jsonContent = await resp.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(jsonContent))
                    {
                        msgResult.IsSuccess = false;
                        msgResult.Msg = "菜鸟授权未成功，请重试！";
                       return new JsonResult(msgResult);
                    }

              
                  
                }
            
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }


            return new JsonResult(msgResult);
        }
    }
}
