
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.AutoMapper;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.DomainEntity;
using FlyBirdYoYo.BusinessServices;

using FlyBirdYoYo.Tests;
using FlyBirdYoYo.BusinessServices.Services;
using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.Utilities.DataStructure;
using FlyBirdYoYo.Utilities;
using Taobao.Pac.Sdk.Core;
using Taobao.Pac.Sdk.Core.Request;

namespace FlyBirdYoYo.BusinessServices.Services.Tests
{
    [TestClass()]
    public class CaiNiaoSdkTests : TestBase
    {
        //https://linkdaily.tbsandbox.com/gateway/link.do?logistic_provider_id=TmpFU1ZOUGoyRnoybDZmT3lyaW9hWGR4VFNad0xNYTBUek9QZk9kamt2Z1hJMytsVkVHK0FjVW55T25wcUR1Qw==&msg_type=TMS_WAYBILL_SUBSCRIPTION_QUERY&logistics_interface=%3Crequest%3E%3CcpCode%3EGTO%3C/cpCode%3E%3C/request%3E&data_digest=2Q7WmTvDEV2CAbc38BtCzg==
        /// <summary>
        /// 测试菜鸟的sdk的接口
        /// </summary>
        [TestMethod()]
        public void GetDianZiDanHaoTest()
        {
            //加载配置
            AppSecretConfigSection configSection = ConfigHelper.GetConfigSection<AppSecretConfigSection>(AppSecretConfigSection.SectionName);

            string appkey = configSection.CaiNiao.AppKey;
            string appSecret = configSection.CaiNiao.AppSecret;
            string url = configSection.CaiNiao.PacUrl;
            string token = "TmpFU1ZOUGoyRnoybDZmT3lyaW9hWGR4VFNad0xNYTBUek9QZk9kamt2Z1hJMytsVkVHK0FjVW55T25wcUR1Qw==";
            //正式     WkFhU2ZYdnA2dkFPd2MvVlR2OTF6K2FuQ1NKejdrV0V1ZDE3VS8zTW9uQjhLOGx5VHBuUkgyMzgrc3Q1RnRZNw==
            token = "WkFhU2ZYdnA2dkFPd2MvVlR2OTF6K2FuQ1NKejdrV0V1ZDE3VS8zTW9uQjhLOGx5VHBuUkgyMzgrc3Q1RnRZNw==";
            var client = new PacClient(appkey, appSecret, url);
            string jsonParas = "{\"cpCode\":\"ZTO\"}";
            TmsWaybillSubscriptionQueryRequest parasModel = jsonParas.FromJsonToObject<TmsWaybillSubscriptionQueryRequest>();
            parasModel.SoapFormat = PacSupportFormat.JSON;

           var resp= client.Send(parasModel, token);

            Assert.IsNotNull(resp);
        }


        [TestMethod()]
        public void GetTemplate()
        {
            //加载配置
            AppSecretConfigSection configSection = ConfigHelper.GetConfigSection<AppSecretConfigSection>(AppSecretConfigSection.SectionName);

            string appkey = configSection.CaiNiao.AppKey;
            string appSecret = configSection.CaiNiao.AppSecret;
            string url = configSection.CaiNiao.PacUrl;
            string token = "";
             //测试
             //  token = "TmpFU1ZOUGoyRnoybDZmT3lyaW9hWGR4VFNad0xNYTBUek9QZk9kamt2Z1hJMytsVkVHK0FjVW55T25wcUR1Qw==";
            //正式
            token = "WkFhU2ZYdnA2dkFPd2MvVlR2OTF6K2FuQ1NKejdrV0V1ZDE3VS8zTW9uQjhLOGx5VHBuUkgyMzgrc3Q1RnRZNw==";
            //List<TemplateCompanyModel> lst = new TemplateCompanyService().GetTemplateCompanyElementsByCondition(d => d.ExCode != "OTHER");

            //foreach (TemplateCompanyModel m in lst)
            // {
            //    var client = new PacClient(appkey, appSecret, url);
            //    string jsonParas = "{\"cpCode\":\""+m.ExCode+"\"}";
            //    TmsWaybillSubscriptionQueryRequest parasModel = jsonParas.FromJsonToObject<TmsWaybillSubscriptionQueryRequest>();
            //    parasModel.SoapFormat = PacSupportFormat.XML;

            //    var resp = client.Send(parasModel, token);
            //    if (resp.Success && resp.WaybillApplySubscriptionCols[0].BranchAccountCols.Count > 0)
            //    {
            //        Assert.IsNotNull(resp);
            //    }
            // }

            var client = new PacClient(appkey, appSecret, url);
            string jsonParas = "{\"cpCode\":\"ZTO\"}";
            CloudprintStandardTemplatesRequest parasModel = jsonParas.FromJsonToObject<CloudprintStandardTemplatesRequest>();

            parasModel.SoapFormat = PacSupportFormat.JSON;

            var resp = client.Send(parasModel, token);
            if (resp.Success)
            {
                Assert.IsNotNull(resp);
            }


        }

    }
}