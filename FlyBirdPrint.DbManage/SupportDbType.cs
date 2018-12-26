using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyBirdYoYo.DbManage
{

    /// <summary>
    /// 支持的数据库类型
    /// </summary>
    public enum SupportDbType
    {
        /// <summary>
        /// MS SQLServer
        /// </summary>
        SQLSERVER = 1,

        /// <summary>
        /// Mysql
        /// </summary>
        MYSQL=2,
        /// <summary>
        /// 暂未支持 PostgreSQL
        /// </summary>
        POSTGRESQL = 3,

        /// <summary>
        /// 暂未支持 Oracle
        /// </summary>
        ORACLE = 4

    }
}
