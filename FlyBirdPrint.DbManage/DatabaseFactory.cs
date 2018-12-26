using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace FlyBirdYoYo.DbManage
{
    public static class DatabaseFactory
    {

        public const string Default_ConnName = "Default";

        #region Db 交互实例 工厂


        /// <summary>
        /// 根据连接字符串名称  获取连接字符串配置
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static DbConnConfig GetDbConfig(string connName = Default_ConnName)
        {
            DbConnConfig dbConfig = null;
            if (string.IsNullOrEmpty(connName))
            {
                //必须有连接配置，如果没有 那么抛出异常
                dbConfig = GlobalDBConnection.AllDbConnConfigs.FirstOrDefault().Value;
            }
            else
            {
                //检测是否有name
                if (!GlobalDBConnection.AllDbConnConfigs.ContainsKey(connName))
                {
                    throw new Exception("指定的数据库连接名称不存在配置中！Name：" + connName);
                }
                dbConfig = GlobalDBConnection.AllDbConnConfigs[connName];

            }

            return dbConfig;
        }

        /// <summary>
        /// 根据连接字符串名称获取DbConnection，db连接对象
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static DbConnection GetDbConnection(string connName = Default_ConnName)
        {
            var dbConfig = GetDbConfig(connName);
            return GetDbConnection(dbConfig);
        }
        /// <summary>
        /// 获取数据连接
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetDbConnection(DbConnConfig dbConnConfig)
        {
            DbConnection conn = null;
            if (null == dbConnConfig)
            {
                throw new Exception("置数据库连接配置不能为空！");

            }

            switch (dbConnConfig.DbType)
            {
                case SupportDbType.SQLSERVER:
                    conn = new SqlConnection(dbConnConfig.ConnString);
                    break;
                case SupportDbType.MYSQL:
                    conn = new MySqlConnection(dbConnConfig.ConnString);
                    break;
                case SupportDbType.POSTGRESQL:
                case SupportDbType.ORACLE:
                default: throw new NotImplementedException();
            }

            return conn;

        }

        /// <summary>
        /// 获取 DbCommand
        /// </summary>
        /// <returns></returns>
        public static DbCommand GetDbDbCommand(DbConnConfig dbConnConfig)
        {
            DbCommand cmd = null;
            if (null == dbConnConfig)
            {
                throw new Exception("置数据库连接配置不能为空！");

            }

            switch (dbConnConfig.DbType)
            {
                case SupportDbType.SQLSERVER:

                    cmd = new SqlCommand();
                    break;
                case SupportDbType.MYSQL:
                    cmd = new MySqlCommand();
                    break;
                case SupportDbType.POSTGRESQL:
                case SupportDbType.ORACLE:
                default: throw new NotImplementedException();


            }



            return cmd;
        }
        /// <summary>
        /// 获取一个 DataAdapter
        /// </summary>
        /// <returns></returns>
        public static DbDataAdapter GetDbDataAdapter(DbCommand selectCommand, DbConnConfig dbConnConfig)
        {
            DbDataAdapter dataAdapter = null;
            if (null == dbConnConfig)
            {
                throw new Exception("置数据库连接配置不能为空！");

            }

            switch (dbConnConfig.DbType)
            {
                case SupportDbType.SQLSERVER:
                    dataAdapter = new SqlDataAdapter((SqlCommand)selectCommand);
                    break;
                case SupportDbType.MYSQL:
                    dataAdapter = new MySqlDataAdapter((MySqlCommand)selectCommand);
                    break;
                case SupportDbType.POSTGRESQL:
                case SupportDbType.ORACLE:
                default: throw new NotImplementedException();

            }




            return dataAdapter;

        }



        #endregion

    }
}
