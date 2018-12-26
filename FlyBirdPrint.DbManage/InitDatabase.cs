
using FlyBirdYoYo.Utilities;
using System;
using System.Configuration;
using System.Threading.Tasks;


namespace FlyBirdYoYo.DbManage
{
    /// <summary>
    /// 设置数据库连接
    /// </summary>
    public class InitDatabase
    {
        /// <summary>
        ///  初始化数据库连接配置，从 配置文件：ConnectionStringSection节点
        /// </summary>
        /// <param name="isNeedConfigDb">是否需要配置数据库</param>
        /// <param name="isAsync">是否异步模式配置数据库</param>
        public static void Init(bool isNeedConfigDb = true, bool isAsync = true)
        {

            //设置数据库连接
            var connStringSection = ConfigHelper.GetConnectionStringSection();
            if (null == connStringSection)
            {
                throw new Exception("未发现数据库连接配置节点：ConnectionStringSection");
            }

            //将数据库连接节点装载到静态集合
            foreach (var itemConnString in connStringSection)
            {
                if (itemConnString.ConnectionString.Contains(@".\SQLEXPRESS"))
                {
                    continue;
                }
                var config = new DbConnConfig(itemConnString.ConnectionString)
                {
                    Name = itemConnString.Name,
                    Code=itemConnString.Code,
                    //ConnString = ,
                    ProviderName = itemConnString.ProviderName,
                    SqlOutPut= itemConnString.SqlOutPut

                };

                GlobalDBConnection.AllDbConnConfigs.Add(config.Name, config);
            }

            if (isNeedConfigDb == true)
            {
                //初始化-数据库
                //异步初始化
                var tsk_init = Task.Factory.StartNew(() =>
                  {
                      GlobalDBConnection.InitDataBase(GlobalDBConnection.AllDbConnConfigs);
                  });

                if (isAsync == false
                    && null != tsk_init
                    && tsk_init.IsCompleted == false)
                {
                    tsk_init.Wait();//等待此任务完成
                }
            }


        }

    }
}