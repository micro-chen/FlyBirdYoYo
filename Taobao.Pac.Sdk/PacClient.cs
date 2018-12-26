
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Taobao.Pac.Sdk.Core
{


    public class PacClient
    {
        #region 字段


        private string appKey;
        private string appSecret;
        private string pacUrl;


        protected static HttpClient client = new HttpClient();

        #endregion


        #region 属性
        public static string PacUrl;
        public static string AppKey;
        public static string AppSecret;
        #endregion

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PacClient() : this(AppKey, AppSecret, PacUrl)
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="pacUrl"></param>
        public PacClient(string appKey, string appSecret, string pacUrl = Constants.PacUrl)
        {
            if (string.IsNullOrEmpty(appKey))
            {
                throw new PacException("应用ID的appkey不能为空!");
            }
            if (string.IsNullOrEmpty(appSecret))
            {
                throw new PacException("物流合作伙伴密钥不能为空appSecret!");
            }



            this.appKey = appKey;
            this.appSecret = appSecret;
            if (!string.IsNullOrEmpty(pacUrl))
            {
                this.pacUrl = pacUrl;
            }

        }


        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }


        public virtual T Send<T>(BaseRequest<T> request, string token) where T : BaseResponse
        {
            return DoSendAsync<T>(request, token).Result;
        }

        public virtual Task<T> SendAsync<T>(BaseRequest<T> request, string token) where T : BaseResponse
        {
            return DoSendAsync<T>(request, token);
        }



        private async Task<T> DoSendAsync<T>(BaseRequest<T> request, string token) where T : BaseResponse
        {
            // long start = DateTime.Now.Ticks;

            T resultModel = default(T);


            // 提前检查业务参数
            try
            {
                if (null == request)
                {
                    throw new PacException("请求数据对象request不能为空!");
                }
                if (string.IsNullOrEmpty(token))
                {
                    throw new PacException("商户的 token 不能为空!");
                }

                request.Validate();

                //提交报文参数

                string reqBodyContent = string.Empty;
                if (request.SoapFormat == PacSupportFormat.JSON)
                {
                    reqBodyContent = JsonConvert.SerializeObject(request);
                }
                else if (request.SoapFormat == PacSupportFormat.XML)
                {
                    reqBodyContent = XmlHelper.SerializeObject(request.GetType(), request);
                }


                //生成参数签名
                string digSign = SignHelper.MakeSign(reqBodyContent, appSecret);


                //先对内容进行Url编码 ，并移除UTF8的BOM

                //请求提交的表单公共参数
                Dictionary<string, string> formParas = new Dictionary<string, string>();
                formParas.Add("msg_type", request.MsgType);
                formParas.Add("logistic_provider_id", token);
                formParas.Add("data_digest", digSign);
                if (!string.IsNullOrEmpty(request.ToCode))
                {
                    formParas.Add("to_code", request.ToCode);
                }
                formParas.Add("logistics_interface", reqBodyContent);



                //包装提交的数据
                var targetUri = new Uri(this.pacUrl);
                if (targetUri.Scheme == "https")
                {
                    //开启 https 默认证书验证
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                }


                string respResult = string.Empty;

                using (HttpContent content = new FormUrlEncodedContent(formParas))
                {

                    var response = await client.PostAsync(targetUri, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new PacException("菜鸟SDK ISV接口调用返回异常！" + response.StatusCode);
                    }

                    // 响应的报文
                    respResult = await response.Content.ReadAsStringAsync();

                }

                //根据不同的格式 进行反序列化
                if (request.SoapFormat == PacSupportFormat.JSON//注意啊：菜鸟的响应反复无常，只能这样完整的限制格式
                    && !respResult.Contains("<response>")
                    &&!respResult.Contains("<success>"))
                {
                    
                        resultModel = JsonConvert.DeserializeObject<T>(respResult);
                }
                else //////////if (request.SoapFormat == PacSupportFormat.XML)----菜鸟的响应反复无常，不得已只能不限制xml格式
                {
                    //追加xml头
                    string fullXml = string.Concat("<?xml version=\"1.0\" encoding=\"utf-8\"?>", respResult);
                    resultModel = XmlHelper.Deserialize<T>(fullXml);
                }


                //////if (null== resultModel||resultModel.Success== false)
                //////{
                //////    throw new PacException("菜鸟SDK ISV接口调用失败：未能正确获取响应的结果！" + respResult);
                //////}


                return resultModel;

            }
            catch (PacException ex)
            {
                throw ex;
            }

        }



    }
}
