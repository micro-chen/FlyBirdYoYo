------------------------------------------------------------------------
异步控制器示范：
  [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await Task.Run(() =>
            {

                return new List<TodoItem>
                {
                    new TodoItem{ Id=1, Name="11111111111"},
                     new TodoItem{ Id=2, Name="22222222222"},
                      new TodoItem{ Id=3, Name="33333333333333"},
                     new TodoItem{ Id=4, Name="44444444444444444"},
                };
            });

           
        }


        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            int x = 0;
            return await Task.Factory.StartNew<TodoItem>(() =>
            {
                return new TodoItem { Id = 1, Name = "11111111111" };
            });

        }
------------------------------------------------------------------------------------------
            //获取配置文件配置   并注册配置的变更事件
            //var config = ConfigHelper.GetCustomConfiguration(Contanst.Global_Config_Hosting, args);
            //ConfigHelper.HostingConfiguration = config;
            //ConfigHelper.OnHostingConfigChangedEvent += WorkContext.ConfigHelper_OnHostingConfigChangedEvent;

这个是测试KEY 你可以试一下 你的API
地址：https://linkdaily.tbsandbox.com/gateway/link.do
或者 https://linkdaily.tbsandbox.com/gateway/pac_message_receiver.do 
（测试环境不稳定，时有调整，两个地址都试一下，哪个可以用哪个）

AppKey：766654

SecretKey：F53eqq903jQySV100Z8w06f9g914A13Z

Token：TmpFU1ZOUGoyRnoybDZmT3lyaW9hWGR4VFNad0xNYTBUek9QZk9kamt2Z1hJMytsVkVHK0FjVW55T25wcUR1Qw==

日常测试环境使用以上提供的AppKey、SecretKey、Token，不能使用自己申请的。
测试环境不支持配置请求、返回报文格式，默认格式xml格式；不可新增订购关系，
只能使用提供的快递公司获取面单号，支持的快递公司通过订购关系查询接口获取。
请参照LINK接入白皮书1.10中的代码示例进行联调测试，
测试环境只供调通接口，具体数据没有参考意义，数据准确性请到正式环境验证


菜鸟集成授权
https://support-cnkuaidi.taobao.com/docs/doc.htm?spm=a21da.7629140.0.0.NFb31C&treeId=492&articleId=108314&docType=1
更新时间：2018/03/13 访问次数：27084
ISV可以在自己的系统或者应用中集成授权平台的功能，在用户登录授权后可以通过重定向自动把AccessCode发送给到ISV的系统中，ISV的系统可以自动根据AccessCode换取AccessToken并访问用户的资源，请参考文档 集成授权。

注意事项：

1、使用集成授权方式，必须到应用管理中配置ISV服务器地址，重定向的地址需与应用中配置的ISV服务器一致；

2、集成授权必须以普通身份的菜鸟账号登录，否则下一步会是空页面；

3、授权后得到token有效期为1年，建议不要每次调用接口都重新获取token。
