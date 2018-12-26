using System;
using System.Collections.Generic;
using System.Text;

namespace Alipay.AopSdk.Core
{
    public interface IAlipayService : IAopClient
    {
        IAlipayOptions Options { get; set; }

        bool RSACheckV1(Dictionary<string, string> data);
        /*
                Dictionary<string, string> RequestParamToDictionaryForHttpPost();
                Dictionary<string, string> RequestParamToDictionaryForHttpGet();*/
    }
}
