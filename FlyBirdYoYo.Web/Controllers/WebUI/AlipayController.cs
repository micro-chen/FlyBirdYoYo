using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlyBirdYoYo.Web.Mvc;
using FlyBirdYoYo.Utilities.Logging;

using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.DomainEntity.QueryCondition;
using FlyBirdYoYo.DomainEntity.ViewModel.Payment;
using FlyBirdYoYo.BusinessServices;
using FlyBirdYoYo.Utilities;
using System.Net;

namespace FlyBirdYoYo.Web.Controllers.WebUI
{
    /// <summary>
    /// 支付宝相关业务控制器
    /// 文档：https://docs.open.alipay.com/270/105902/
    /// </summary>
    public class AlipayController : BaseApiControllerNoAuth
    {

        ////////        必须保证服务器异步通知页面（notify_url）上无任何字符，如空格、HTML标签、开发系统自带抛出的异常提示信息等；
        ////////支付宝是用POST方式发送通知信息，因此该页面中获取参数的方式，如：request.Form(“out_trade_no”)、$_POST[‘out_trade_no’]；
        ////////支付宝主动发起通知，该方式才会被启用；
        ////////只有在支付宝的交易管理中存在该笔交易，且发生了交易状态的改变，支付宝才会通过该方式发起服务器通知（即时到账交易状态为“等待买家付款”的状态默认是不会发送通知的）；
        ////////服务器间的交互，不像页面跳转同步通知可以在页面上显示出来，这种交互方式是不可见的；
        ////////第一次交易状态改变（即时到账中此时交易状态是交易完成）时，不仅会返回同步处理结果，而且服务器异步通知页面也会收到支付宝发来的处理结果通知；
        ////////程序执行完后必须打印输出“success”（不包含引号）。如果商户反馈给支付宝的字符不是success这7个字符，支付宝服务器会不断重发通知，直到超过24小时22分钟。一般情况下，25小时以内完成8次通知（通知的间隔频率一般是：4m,10m,10m,1h,2h,6h,15h）；
        ////////程序执行完成后，该页面不能执行页面跳转。如果执行页面跳转，支付宝会收不到success字符，会被支付宝服务器判定为该页面程序运行出现异常，而重发处理结果通知；
        ////////cookies、session等在此页面会失效，即无法获取这些数据；
        ////////该方式的调试与运行必须在服务器上，即互联网上能访问；
        ////////该方式的作用主要防止订单丢失，即页面跳转同步通知没有处理订单更新，它则去处理；
        ////////当商户收到服务器异步通知并打印出success时，服务器异步通知参数notify_id才会失效。也就是说在支付宝发送同一条异步通知时（包含商户并未成功打印出success导致支付宝重发数次通知），服务器异步通知参数notify_id是不变的。

        ///// 1. 确认您使用的接口是用notify_url还是return_url。 
        //////2. notify_url为服务器通知，支付宝可以保证99.9999%的通知到达率，前提是您的网络通畅。 
        //////3. return_url为网页重定向通知，是由客户的浏览器触发的一个通知，若客户去网银支付，也会受银行接口影响，由于各种影响因素特别多，所以该种类型的通知支付宝不保证其到达率。 
        //////买家付款成功后,会跳到 return_url所在的页面, 这个页面可以展示给客户看, 这个页面只有付款成功才会跳转, 并且只跳转一次.. 
        //////notify_url: 服务器后台通知,这个页面是支付宝服务器端自动调用这个页面的链接地址,这个页面根据支付宝反馈过来的信息修改网站的定单状态,更新完成后需要返回一个success给支付宝.,不能含有任何其它的字符包括html语言.
        //////流程:买家付完款(trade_status= WAIT_SELLER_SEND_GOODS)--->支付宝通知 notify_url--->如果反馈给支付宝的是success(表示成功, 这个状态下不再反馈, 如果不是继续通知, 一般第一次发送和第二次发送的时间间隔是3分钟)
        //////剩下的过程,卖家发货,买家确认收货,交易成功都是这个流程 

        /// <summary>
        /// 回调响应成功code
        /// </summary>
        const string ALIPAY_SUCCESS_CODE = "success";
        /// <summary>
        /// 回调失败code
        /// </summary>
        const string ALIPAY_FAILED_CODE = "fail";
        /// <summary>
        /// 支付宝异步回调通知地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("notify_url")]
        public string NotifyUrlOfNetwork()
        {

            string apiResult = ALIPAY_FAILED_CODE;
            try
            {
                //先要验证回调的签名
                AlipayNotifyReturnModel callBackModel = new AlipayNotifyReturnModel();
                var validSignRresult = callBackModel.ValidateRequest(HttpContext.Request);
                if (true == validSignRresult)
                {

                    //验证签名成功，更新支付状态
                    //var result = Singleton<PayOrderService>.Instance.NotifyPaymentSuccess_AliPay(callBackModel.BizModel.OutTradeNo, callBackModel.BizModel.TotalAmount, callBackModel.BizModel.TradeNo);
                    //if (true == result)
                    //{
                    //    apiResult = ALIPAY_SUCCESS_CODE;
                    //}

                }



                string tradNo = Request.GetQuery<string>("trade_no");
                string bizOrderNo = Request.GetQuery<string>("out_trade_no");
                string msgInfo = $"充值续费notify_url，执行状态：{apiResult}，业务单号：{bizOrderNo} ; 交易流水号：{tradNo} ;";
                Logger.Info(msgInfo);//充值异步通知消息保留到日志


            }
            catch (Exception ex)
            {
                Logger.Error("Alipay : notify_url 调用失败了！" + ex.ToString());
            }

            return apiResult;
        }



        /// <summary>
        /// 支付宝回调地址
        /// </summary>
        /// <returns></returns>
        [ActionName("call_back_handler")]
        [HttpGet]
        [HttpPost]
        public IActionResult AlipayCallBackHander()
        {
            bool validSignRresult = false;
            string apiResult = string.Empty;
            try
            {
                //先要验证回调的签名
                AlipayCallbackReturnModel callBackModel = new AlipayCallbackReturnModel();
                validSignRresult = callBackModel.ValidateRequest(HttpContext.Request);
                if (true == validSignRresult)
                {
                    ////验证签名成功，更新支付状态
                    //var result = Singleton<PayOrderService>.Instance.NotifyPaymentSuccess_AliPay(callBackModel.BizModel.OutTradeNo, callBackModel.BizModel.TotalAmount, callBackModel.BizModel.TradeNo);

                    //if (true == result)
                    //{
                    //    return Redirect(ConfigHelper.AppSettingsConfiguration.GetConfig("DomainNameForUI"));
                    //}
                    //else
                    //{
                    //    string tradNo = Request.GetQuery<string>("trade_no");
                    //    string bizOrderNo = Request.GetQuery<string>("out_trade_no");
                    //    apiResult = $"充值续费失败，业务单号：{bizOrderNo} ; 交易流水号：{tradNo} ；请联系客服人员！";
                    //    Logger.Error(apiResult);//充值失败的保留到日志
                    //}
                }


            }
            catch (FlyUIException uiExp)
            {
                apiResult = uiExp.Message;
            }
            catch (Exception ex)
            {
                Logger.Error("Alipay： call_back_handler  调用失败了！" + ex.ToString());
            }

            //如果没有成功支付，那么返回错误提示
            return new ContentResult() { Content = apiResult, ContentType = "text/plain", StatusCode = (int)HttpStatusCode.OK };
        }

    }
}
