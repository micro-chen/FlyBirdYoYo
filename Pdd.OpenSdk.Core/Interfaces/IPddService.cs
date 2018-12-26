using Pdd.OpenSdk.Core.Services.PddApi;

namespace Pdd.OpenSdk.Core
{
    /// <summary>
    /// 拼多多开放服务接口
    /// </summary>
    public interface IPddService
    {
        AdApi AdApi { get; set; }
        AuthApi AuthApi { get; set; }
        DdkApi DdkApi { get; set; }
        DdkOauthApi DdkOauthApi { get; set; }
        ErpApi ErpApi { get; set; }
        GoodsApi GoodsApi { get; set; }
        InvoiceApi InvoiceApi { get; set; }
        LogisticsApi LogisticsApi { get; set; }
        LogisticsCsApi LogisticsCsApi { get; set; }
        MallApi MallApi { get; set; }
        ErpApi OrderApi { get; set; }
        PromotionApi PromotionApi { get; set; }
        RefundApi RefundApi { get; set; }
        SmsApi SmsApi { get; set; }
        StockApi StockApi { get; set; }
        TimeApi TimeApi { get; set; }
        VirtualApi VirtualApi { get; set; }
        VoucherApi VoucherApi { get; set; }
    }
}