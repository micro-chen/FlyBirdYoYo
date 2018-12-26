using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using QimenCloud.Api.Parser;
using Top.Api;
using Top.Api.Parser;
using Top.Api.Util;
using FastJSON;
using System.Collections;
using XmlWriter = Top.Api.Util.XmlWriter;

namespace QimenCloud.Api
{
    public class DefaultQimenCloudClient : IQimenCloudClient
    {
        internal string serverUrl;
        internal string appKey;
        internal string appSecret;
        internal string format = Constants.FORMAT_JSON;

        internal string signMethod = Constants.SIGN_METHOD_HMAC_SHA256;

        internal WebUtils webUtils;
        internal ITopLogger topLogger;
        internal bool disableParser = false; // 禁用响应结果解释
        internal bool disableTrace = false; // 禁用日志调试功能
        internal bool useGzipEncoding = true;  // 是否启用响应GZIP压缩
        internal IDictionary<string, string> systemParameters; // 设置所有请求共享的系统级参数

        #region DefaultQimenCloudClient Constructors

        public DefaultQimenCloudClient(string serverUrl, string appKey, string appSecret)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
            this.serverUrl = serverUrl;
            this.webUtils = new WebUtils();
            this.topLogger = Log.Instance;
        }

        public DefaultQimenCloudClient(string serverUrl, string appKey, string appSecret, string format)
            : this(serverUrl, appKey, appSecret)
        {
            this.format = format;
        }

        #endregion

        public void SetTimeout(int timeout)
        {
            this.webUtils.Timeout = timeout;
        }

        public void SetReadWriteTimeout(int readWriteTimeout)
        {
            this.webUtils.ReadWriteTimeout = readWriteTimeout;
        }

        public void SetSignMethod(String signMethod)
        {
            this.signMethod = signMethod;
        }

        public void SetDisableParser(bool disableParser)
        {
            this.disableParser = disableParser;
        }

        public void SetDisableTrace(bool disableTrace)
        {
            this.disableTrace = disableTrace;
        }

        public void SetUseGzipEncoding(bool useGzipEncoding)
        {
            this.useGzipEncoding = useGzipEncoding;
        }

        public void SetIgnoreSSLCheck(bool ignore)
        {
            this.webUtils.IgnoreSSLCheck = ignore;
        }

        public void SetSystemParameters(IDictionary<string, string> systemParameters)
        {
            this.systemParameters = systemParameters;
        }

        #region ITopClient Members

        public virtual T Execute<T>(IQimenCloudRequest<T> request) where T : QimenCloudResponse
        {
            return DoExecute<T>(request, null, DateTime.Now);
        }

        public virtual T Execute<T>(IQimenCloudRequest<T> request, string session) where T : QimenCloudResponse
        {
            return DoExecute<T>(request, session, DateTime.Now);
        }

        public virtual T Execute<T>(IQimenCloudRequest<T> request, string session, DateTime timestamp) where T : QimenCloudResponse
        {
            return DoExecute<T>(request, session, timestamp);
        }

        #endregion

        private T DoExecute<T>(IQimenCloudRequest<T> request, string session, DateTime timestamp) where T : QimenCloudResponse
        {
            long start = DateTime.Now.Ticks;

            // 提前检查业务参数
            try
            {
                request.Validate();
            }
            catch (TopException e)
            {
                return CreateErrorResponse<T>(e.ErrorCode, e.ErrorMsg);
            }

            // 兼容 老奇门1.0 的 XML BODY
            String apiBody = null;
            Object[] requestXmlBodyType = request.GetType().GetCustomAttributes(typeof(RequestXmlBodyAttribute), false);
            Boolean isXmlBody = (requestXmlBodyType != null && requestXmlBodyType.Length > 0);
            if (isXmlBody)
            {
                XmlWriter writer = new XmlWriter(Constants.QM_ROOT_TAG_REQ, typeof(IQimenCloudRequest<T>));
                apiBody = writer.Write(request);
            } 

            // 添加协议级请求参数
            TopDictionary txtParams = new TopDictionary(request.GetParameters());
            txtParams.Add(Constants.METHOD, request.GetApiName());
            txtParams.Add(Constants.SIGN_METHOD, signMethod);
            txtParams.Add(Constants.APP_KEY, appKey);
            txtParams.Add(Constants.FORMAT, format);
            txtParams.Add(Constants.VERSION, "2.0");
            txtParams.Add(Constants.PARTNER_ID, GetSdkVersion());
            txtParams.Add(Constants.TIMESTAMP, timestamp);
            txtParams.Add(Constants.TARGET_APP_KEY, request.GetTargetAppKey());
            txtParams.Add(Constants.SESSION, session);
            txtParams.AddAll(this.systemParameters);

            // 添加签名参数
            if (isXmlBody)
            {
                txtParams.Add(Constants.SIGN, TopUtils.SignTopRequest(txtParams, apiBody, appSecret, signMethod));
            }
            else
            {
                txtParams.Add(Constants.SIGN, TopUtils.SignTopRequest(txtParams, appSecret, signMethod));
            }

            // 添加头部参数
            if (this.useGzipEncoding)
            {
                request.GetHeaderParameters()[Constants.ACCEPT_ENCODING] = Constants.CONTENT_ENCODING_GZIP;
            }

            string realServerUrl = GetServerUrl(this.serverUrl, request.GetApiName(), session);

            string reqUrl;
            if (isXmlBody)
            {
                reqUrl = WebUtils.BuildRequestUrl(realServerUrl, txtParams);
            }
            else
            {
                reqUrl = WebUtils.BuildRequestUrl(realServerUrl, txtParams);
            }
            
            try
            {
                string body;
                if (isXmlBody)
                {
                    body = webUtils.DoPost(reqUrl, Encoding.UTF8.GetBytes(apiBody), Constants.QM_CONTENT_TYPE, request.GetHeaderParameters());
                }
                else
                {
                    body = webUtils.DoPost(realServerUrl, txtParams, request.GetHeaderParameters());
                }

                // 解释响应结果
                T rsp;
                if (disableParser)
                {
                    rsp = Activator.CreateInstance<T>();
                    rsp.Body = body;
                    rsp.RequestUrl = reqUrl;
                }
                else
                {
                    if (Constants.FORMAT_XML.Equals(format))
                    {
                        ITopParser<T> tp = new QimenCloudXmlParser<T>();
                        rsp = tp.Parse(body);
                    }
                    else
                    {
                        ITopParser<T> tp = new QimenCloudSimplifyJsonParser<T>();
                        rsp = tp.Parse(body);
                    }
                    rsp.RequestUrl = reqUrl;
                }

                // 追踪错误的请求
                if (rsp.IsError)
                {
                    TimeSpan latency = new TimeSpan(DateTime.Now.Ticks - start);
                    TraceApiError(appKey, request.GetApiName(), serverUrl, txtParams, latency.TotalMilliseconds, rsp.Body);
                }
                return rsp;
            }
            catch (Exception e)
            {
                TimeSpan latency = new TimeSpan(DateTime.Now.Ticks - start);
                TraceApiError(appKey, request.GetApiName(), serverUrl, txtParams, latency.TotalMilliseconds, e.GetType() + ": " + e.Message);
                throw e;
            }
        }

        internal virtual string GetServerUrl(string serverUrl, string apiName, string session)
        {
            return serverUrl;
        }

        internal virtual string GetSdkVersion()
        {
            return Constants.SDK_VERSION;
        }

        internal T CreateErrorResponse<T>(string errCode, string errMsg) where T : QimenCloudResponse
        {
            T rsp = Activator.CreateInstance<T>();
            rsp.Code = errCode;
            rsp.Message = errMsg;

            if (Constants.FORMAT_XML.Equals(format))
            {
                XmlDocument root = new XmlDocument();
                XmlElement bodyE = root.CreateElement(Constants.ERROR_RESPONSE);
                XmlElement codeE = root.CreateElement(Constants.ERROR_CODE);
                codeE.InnerText = errCode;
                bodyE.AppendChild(codeE);
                XmlElement msgE = root.CreateElement(Constants.ERROR_MSG);
                msgE.InnerText = errMsg;
                bodyE.AppendChild(msgE);
                root.AppendChild(bodyE);
                rsp.Body = root.OuterXml;
            }
            else
            {
                IDictionary<string, object> errObj = new Dictionary<string, object>();
                errObj.Add(Constants.ERROR_CODE, errCode);
                errObj.Add(Constants.ERROR_MSG, errMsg);
                IDictionary<string, object> root = new Dictionary<string, object>();
                root.Add(Constants.ERROR_RESPONSE, errObj);

                string body = JSON.ToJSON(root);
                rsp.Body = body;
            }
            return rsp;
        }

        internal void TraceApiError(string appKey, string apiName, string url, Dictionary<string, string> parameters, double latency, string errorMessage)
        {
            if (!disableTrace)
            {
                this.topLogger.TraceApiError(appKey, apiName, url, parameters, latency, errorMessage);
            }
        }
    }
}
