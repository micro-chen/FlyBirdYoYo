using Microsoft.Extensions.Configuration;

namespace Taobao.Pac.Sdk.Core
{
    public interface ICaiNiaoPacOptions
    {
        /// <summary>
        /// 远程调用地址
        /// </summary>
        string PacUrl { get; set; }


        string AppKey { get; set; }
        string AppSecret { get; set; }
      

        void SetOption(ICaiNiaoPacOptions options);
        void SetOption(IConfigurationSection section);
    }
}