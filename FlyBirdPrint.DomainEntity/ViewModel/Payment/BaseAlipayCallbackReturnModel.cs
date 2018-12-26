using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using Alipay.AopSdk.Core.Util;
using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.Utilities.DataStructure;
using Microsoft.AspNetCore.Http;

namespace FlyBirdYoYo.DomainEntity.ViewModel.Payment
{
    public abstract class BaseAlipayCallbackReturnModel
    {


        protected Dictionary<string, string> sArray;

        /// <summary>
        /// 转换参数数组为业务网对象
        /// </summary>
        /// <param name="_sArray"></param>
        public abstract void ParseToBizModel(Dictionary<string, string> _sArray=null);

        /// <summary>
        /// 验证请求签名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool ValidateRequest(HttpRequest request)
        {
            bool flag = false;
            /* 实际验证过程建议商户添加以下校验。
      1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
      2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
      3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
      4、验证app_id是否为该商户本身。
      */
             this.sArray = this.GetRequestGet(request);
            if (sArray.Count > 0)
            {
                var alipayConfig = ConfigHelper.GetConfigSection<AlipayConfigSection>(AlipayConfigSection.SectionName);
                //注意：这里一定要使用支付宝公钥，不用应用的公钥
                flag = AlipaySignature.RSACheckV1(sArray, alipayConfig.AlipayPublicKey, alipayConfig.CharSet, alipayConfig.SignType, false);
            }
            if (true==flag)
            {
                this.ParseToBizModel();
            }
            return flag;
        }

        private Dictionary<string, string> GetRequestGet(HttpRequest request)
        {

            Dictionary<string, string> sArray = new Dictionary<string, string>();
            foreach (var key in request.Query.Keys)
            {
                var requestItem = request.Query[key];
                sArray.Add(key, requestItem[0]);
            }
            return sArray;

        }
    }
}
