using FastJSON;
using System;
using System.Collections;
using QimenCloud.Api;
using Top.Api;
using Top.Api.Parser;

namespace QimenCloud.Api.Parser
{
    public class QimenCloudSimplifyJsonParser<T> : QimenCloudJsonParser<T> where T : QimenCloudResponse
    {
        public override T Parse(string body)
        {
            T rsp = null;

            IDictionary rootJson = JSON.Parse(body) as IDictionary;
            if (rootJson != null)
            {
                IDictionary data = rootJson;
                if (rootJson.Contains(Constants.QIMEN_CLOUD_ERROR_RESPONSE))
                {
                    data = rootJson[Constants.QIMEN_CLOUD_ERROR_RESPONSE] as IDictionary;
                }

                if (data != null)
                {
                    ITopReader reader = new TopSimplifyJsonReader(data);
                    rsp = (T)FromJson(reader, typeof(T));
                }
            }

            if (rsp == null)
            {
                rsp = Activator.CreateInstance<T>();
            }

            if (rsp != null)
            {
                rsp.Body = body;
            }

            return rsp;
        }
    }
}
