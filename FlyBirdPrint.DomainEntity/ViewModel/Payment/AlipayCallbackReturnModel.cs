using FlyBirdYoYo.Utilities.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Alipay.AopSdk.Core.Response;

namespace FlyBirdYoYo.DomainEntity.ViewModel.Payment
{
    /// <summary>
    /// 支付宝同步回调参数模型
    /// https://docs.open.alipay.com/270/alipay.trade.page.pay
    /// </summary>
    public class AlipayCallbackReturnModel:BaseAlipayCallbackReturnModel
    {

        /// <summary>
        /// 支付宝请求返回的业务模型
        /// </summary>
        public AlipayTradePagePayResponse BizModel{ get; set; }




        /// <summary>
        /// 转换参数数组为业务网对象
        /// </summary>
        /// <param name="_sArray"></param>
        public override  void ParseToBizModel(Dictionary<string, string> _sArray=null)
        {
            if (null==_sArray)
            {
                _sArray = this.sArray;
            }

            this.BizModel = new AlipayTradePagePayResponse();
            this.BizModel.OutTradeNo = _sArray["out_trade_no"];
            this.BizModel.SellerId = _sArray["seller_id"];
            this.BizModel.TotalAmount = _sArray["total_amount"].ToDecimal();
            this.BizModel.TradeNo = _sArray["trade_no"];
            if (string.IsNullOrEmpty(this.BizModel.OutTradeNo))
            {
                throw new Exception("out_trade_no is empty!");
            }
            if (string.IsNullOrEmpty(this.BizModel.TradeNo))
            {
                throw new Exception("trade_no is empty!");
            }
        }
    }
}
