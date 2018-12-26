using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;
namespace FlyBirdYoYo.DbManage
{
    /// <summary>
    /// 全部数据库连接配置
    /// </summary>
    public  class GlobalDBConnection: Dictionary<string,DbConnConfig>
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public GlobalDBConnection()
        {
        }



        private static GlobalDBConnection _AllDbConnConfigs;
        /// <summary>
        /// 所有的数据库连接配置
        /// </summary>
        public static GlobalDBConnection AllDbConnConfigs
        {
            get
            {
                if (null==_AllDbConnConfigs)
                {
                    _AllDbConnConfigs = new GlobalDBConnection();
                }
                return _AllDbConnConfigs;
            }


        }


       


        internal static void InitDataBase(Dictionary<string,DbConnConfig> dbConfigs)
        {


          
                try
                {
                    foreach (var itemConfig in dbConfigs.Values)
                    {
                        switch (itemConfig.DbType)
                        {
                            case SupportDbType.SQLSERVER:
                                //1 创建必须的分页存储过程等全局操作
                                //PagerSQLProcedure.CheckAndCreatePagerSQLProcedure(itemConfig);
                                break;
                            case SupportDbType.MYSQL:
                                //1 创建必须的分页存储过程等全局操作
                                //MySqlPagerSQLProcedure.CheckAndCreatePagerSQLProcedure(itemConfig);
                                break;
                            case SupportDbType.POSTGRESQL:

                                break;
                            case SupportDbType.ORACLE:

                                break;
                            default:
                                throw new NotImplementedException();

                        }
                    }



                    //2 Other Need Initial Operations....

                }
                catch (Exception ex)
                {

                    throw ex;
                }


         

        }
    }
}
