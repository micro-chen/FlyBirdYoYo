using System;
using Taobao.Pac.Sdk.Core;
using Microsoft.Extensions.Configuration;

namespace Taobao.Pac.Sdk.AspNetCore
{
    public class CaiNiaoPacOptions : ICaiNiaoPacOptions
    {
        public string PacUrl { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }



        public void SetOption(ICaiNiaoPacOptions options)
        {
            if (string.IsNullOrEmpty(options.PacUrl))
            {
                throw new Exception("服务调用 PacUrl 不能为空！");
            }
            if (string.IsNullOrEmpty(options.AppKey))
            {
                throw new Exception("服务调用 AppKey 不能为空！");
            }

            if (string.IsNullOrEmpty(options.AppSecret))
            {
                throw new Exception("服务调用 AppSecret 不能为空！");
            }

            PacClient.PacUrl = options.PacUrl;
            PacClient.AppKey = options.AppKey;
            PacClient.AppSecret = options.AppSecret;

        }

        public void SetOption(IConfigurationSection section)
        {
            if (section == null)
            {
                throw new ArgumentException(nameof(section));
            }

            var options = section.Get<CaiNiaoPacOptions>();
            SetOption(options);
        }
    }
}