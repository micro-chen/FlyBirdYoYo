using Microsoft.Extensions.Configuration;

namespace Alipay.AopSdk.Core
{
    public interface IAlipayOptions
    {
        string AlipayPublicKey { get; set; }
        string AppId { get; set; }
        string CharSet { get; set; }
        string Gatewayurl { get; set; }
        bool IsKeyFromFile { get; set; }
        string PrivateKey { get; set; }
        string SignType { get; set; }
        string Uid { get; set; }

        void SetOption(IAlipayOptions options);
        void SetOption(IConfigurationSection section);
    }
}