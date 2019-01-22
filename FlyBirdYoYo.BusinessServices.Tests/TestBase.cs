using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FlyBirdYoYo.DomainEntity;
using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.Web.Configs;
using FlyBirdYoYo.Utilities.Interface;
using FlyBirdYoYo.BusinessServices;
using FlyBirdYoYo.DomainEntity.Login;
using FlyBirdYoYo.Utilities.SystemEnum;
using Microsoft.Extensions.DependencyInjection;
using FlyBirdYoYo.Utilities.Ioc;
using FlyBirdYoYo.BusinessServices.Authentication;
using FlyBirdYoYo.Utilities.DataStructure;
using FlyBirdYoYo.DbManage.Mapping;

namespace FlyBirdYoYo.Tests
{
    [TestClass()]

    public class TestBase : IUnitTestBase
    {

        long _CurrentUserId;
        /// <summary>
        /// 当前固定模拟登录用户Id
        /// </summary>
        protected long CurrentUserId
        {
            get
            {
                return _CurrentUserId;
            }
            set
            {
                _CurrentUserId = value;
                this.MockLogin(_CurrentUserId);
            }
        }



        [TestInitialize]
        public void Init()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            //设置配置加载
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            ConfigHelper.AppSettingsConfiguration = builder.Build();

            //设置数据库连接
            InitDatabase.Init();
            TypeMapper.InitializeAsync();

            //设定发起调用的基类
            Contanst.CallingEntryPointBasetype = typeof(TestBase);

            //配置automapper 的对象映射
            AutoMapperConfig.Config();

            this.MockIoc();

            watch.Stop();

            System.Diagnostics.Debug.WriteLine($"初始化耗时：{watch.ElapsedMilliseconds} ms");

        }


        private void MockIoc()
        {
            IServiceCollection services = new ServiceCollection();
            //注册服务
            //内存缓存支持
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            //依赖注入验证服务
            services.AddTransient<IAuthenticationService, AuthenticationService>();


            #region 平台设置依赖注册组件


            //支付宝配置加载---注册：
            var alipayConfig = ConfigHelper.GetConfigSection<AlipayConfigSection>(AlipayConfigSection.SectionName);
            if (null != alipayConfig)
            {
                services.AddAlipay(options =>
                {
                    options.AlipayPublicKey = alipayConfig.AppPublicKey;// "支付宝公钥";
                    options.AppId = alipayConfig.AppId.ToString();// "应用ID";
                    options.CharSet = alipayConfig.CharSet;// "密钥编码";
                    options.Gatewayurl = alipayConfig.Gatewayurl;// "支付网关";
                    options.PrivateKey = alipayConfig.PrivateKey;// "商家私钥";
                    options.SignType = alipayConfig.SignType;// "签名方式 RSA/RSA2";
                    options.Uid = alipayConfig.Uid.ToString();// "商户ID";
                });
            }



            // 批多多服务依赖注册---IPddService
            AppSecretConfigSection configSection = ConfigHelper.GetConfigSection<AppSecretConfigSection>(AppSecretConfigSection.SectionName);
            services.AddPdd(options =>
            {
                //注册 id  secret
                options.ClientId = configSection.Pdd.client_id;
                options.ClientSecret = configSection.Pdd.client_secret;
                options.CallbackUrl = configSection.Pdd.redirect_uri;

            });

            #endregion



            IServiceProvider serviceProvider = services.BuildServiceProvider();
            ServiceLocator.ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// 模拟用户登录
        /// </summary>
        protected void MockLogin(long _userId)
        {
            if (_userId <= 0)
            {
                return;
            }

            try
            {


                //var user = Singleton<UserService>.Instance.GetUserElementById(_userId);
                //LoginAuthedUserDTO dtoModel = new LoginAuthedUserDTO
                //{
                //    GroupId = -1,//用户所在分组
                //    Platform = (PlatformEnum)user.PlatformId,
                //    ShopId = user.ShopId,
                //    ShopName = user.ShopName,
                //    UserId = user.Id,
                //    UserName = user.Nick,
                //    Access_token = userTokenModel.AccessToken,
                //    //TokenExpireTime = userTokenModel.AddTime.Value.
                //};
                //设定模拟登录用户
                ApplicationContext.Current.User = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
