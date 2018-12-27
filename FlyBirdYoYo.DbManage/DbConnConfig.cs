using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyBirdYoYo.DbManage
{


    public class DbConnConfig
    {

        public DbConnConfig(string connStr)
        {
            this.ConnString = connStr;
        }
        /// <summary>
        /// 连接字符串的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 代号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 链接字符串
        /// </summary>
        public string ConnString { get; private set; }


        private string _Database;
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Database
        {
            get
            {
                if (string.IsNullOrEmpty(this._Database))
                {
                    
                    if (!this.ConnString.IsNullOrEmpty())
                    {
                        switch (this.DbType)
                        {
                            case SupportDbType.SQLSERVER:
                                var builder_SqlServer = new System.Data.SqlClient.SqlConnectionStringBuilder(this.ConnString);

                                //string server = builder_SqlServer.DataSource;
                               this. _Database = builder_SqlServer.InitialCatalog;
                                break;
                            case SupportDbType.MYSQL:
                                var builder_Mysql = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(this.ConnString);
                                this._Database = builder_Mysql.Database;
                                break;
                            case SupportDbType.POSTGRESQL:
                                throw new NotImplementedException();
                                //break;
                            case SupportDbType.ORACLE:
                                throw new NotImplementedException();
                               // break;
                            default:
                                break;
                        }

                    }
                }
               
                return _Database;
            }
        }
        private string _ProviderName;
        /// <summary>
        /// 默认是 Sqlserver
        /// 支持 Sqlserver /Mysql
        /// </summary>
        public string ProviderName
        {
            get
            {
                if (string.IsNullOrEmpty(_ProviderName))
                {
                    _ProviderName = SupportDbType.SQLSERVER.ToString();
                }
                return _ProviderName;
            }

            set
            {
                _ProviderName = value;
            }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否输出执行sql--调试用，线上慎用
        /// </summary>
        public bool SqlOutPut { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public SupportDbType DbType
        {
            get
            {
                if (string.IsNullOrEmpty(ProviderName))
                {
                    throw new Exception("请为连接字符串添加 providerName 配置。sqlserver or mysql?");
                }
                return (SupportDbType)Enum.Parse(typeof(SupportDbType), ProviderName.ToUpper());
            }
        }


    }
}
