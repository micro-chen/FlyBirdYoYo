using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


using FlyBirdYoYo.Utilities;

using FlyBirdYoYo.Utilities.Logging;
using FlyBirdYoYo.DbManage;

using FlyBirdYoYo.BusinessServices.Message;
using FlyBirdYoYo.Web.Configs;
using FlyBirdYoYo.Utilities.DataStructure;
using FlyBirdYoYo.Utilities.Interface;
using FlyBirdYoYo.BusinessServices.Authentication;
using FlyBirdYoYo.Utilities.Ioc;

using FlyBirdYoYo.DbManage.Mapping;

namespace FlyBirdYoYo.Web
{
    public class Startup
    {
        /// <summary>
        /// 程序启动 加载入口配置
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            //配置注册gb2312编码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //保证配置的全局
            ConfigHelper.AppSettingsConfiguration = configuration;


            //设置AutoMapper
            AutoMapperConfig.Config();

            //设置mvc基础类型
            Contanst.CallingEntryPointBasetype = typeof(Controller);


            //设置数据库连接
            InitDatabase.Init(isAsync: true);
            //初始化 dapper的  基于Column标识的映射
            TypeMapper.InitializeAsync();


        }

        #region 注意ConfigureServices 方法的调用顺序

        //https://developer.telerik.com/featured/understanding-asp-net-core-initialization/
        /*https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-2.1#the-configureservices-method
 The ConfigureServices method
The ConfigureServices method is:
Optional
Called by the web host before the Configure method to configure the app's services.
Where configuration options are set by convention
note: IServiceCollection services 变量并不是 application级的全局引用，而是一个 .临时变量
             */
        #endregion

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region CORS 支持


            //支持跨域请求CORS 
            var urls = ConfigHelper.AppSettingsConfiguration.GetConfig("CORS-Domin").Split(',');
            services.AddCors(options => options.AddPolicy(
              "AllowAllDomain",
            builder => builder.WithOrigins(urls)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            )
            );
            services.AddSession(options =>
            {
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });


            // configure the application cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            #endregion

            //内存缓存支持
            services.AddMemoryCache();
            //返回json  大小写控制
            services.AddMvc().AddJsonOptions(op => op.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()); ;

            //将http 上下文中间件 添加的依赖注册容器
            // HttpContext在整个中间件贯穿。基于构造函数依赖：public UserRepository(IHttpContextAccessor httpContextAccessor)            2.1 之前手工写法services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-2.1
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1
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
                    options.AlipayPublicKey = alipayConfig.AppPublicKey;// "支付宝开放平台的应用公钥";
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


            //菜鸟打印ISV依赖注册----ICaiNiaoPacService
            services.AddCaiNiaoPac(options =>
            {
                //注册 id  secret
                options.AppKey = configSection.CaiNiao.AppKey;
                options.AppSecret = configSection.CaiNiao.AppSecret;
                options.PacUrl = configSection.CaiNiao.PacUrl;

            });

            #endregion


          

        }


        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="svp">依赖注册的方式 ，内置DI容器</param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider svp, ILoggerFactory loggerFactory)
        {

            //配置mvc 环境
            //将全局内置的DI容器暴露
            ServiceLocator.ServiceProvider = app.ApplicationServices; // svp;
            WorkContext.HostingEnvironment = env;



            var logger = loggerFactory as LoggerFactory;
            Logger.LogFactory = loggerFactory;
            logger.AddLog4Net();//注册log4日志记录组件




            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //通过异常自定义的 过滤器  记录日志
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseStaticFiles();



            //mvc  route config 。注意：从2.1 之后，webapi 的路由必须通过RouteAttribute的方式标注到控制器上
            app.UseMvc(routes =>
            {
                ///mvc and webapi is in one  so add one route at here.
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default-webapi",
                    template: "api/{controller}/{action}/{id?}");

                //注册Area
                routes.MapRoute(
                 name: "areas",
                 template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
               );
            });

            //配置 使用 ForwardedHeaders
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });

            #region CORS
            //跨域请求策略
            app.UseCors("AllowAllDomain");
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None
            });
            #endregion


            //消息处理控制台开启
            if (ConfigHelper.AppSettingsConfiguration.GetConfigBool("UseMessageWorkBench"))
            {
                MessageWorkBench.Start();
            }

        }
    }
}
