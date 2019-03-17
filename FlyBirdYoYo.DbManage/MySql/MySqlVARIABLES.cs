using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace FlyBirdYoYo.DbManage
{
    /// <summary>
    /// mysql  环境变量
    /// </summary>
    internal sealed class MySqlVARIABLES
    {
        #region 字段 常量
        private string _connString;

        /// <summary>
        /// 查询 secure-file-priv 变量
        /// </summary>
        private const string CMD_QUERY_secure_file_priv = "SELECT @@global.secure_file_priv as secure_file_priv;";
        #endregion


        #region 构造函数


        public MySqlVARIABLES(string connString)
        {
            if (string.IsNullOrEmpty(connString))
            {
                throw new Exception("Please Special Mysql ConnectString Not Empty!");
            }

            this._connString = connString;
        }

        #endregion

        #region 属性


        private string _secure_file_priv;

        /// <summary>
        /// secure-file-priv 变量
        /// </summary>
        public string Secure_file_priv
        {
            get
            {
                if (string.IsNullOrEmpty(_secure_file_priv))
                {
                    _secure_file_priv = this.LoadVariables<string>(CMD_QUERY_secure_file_priv);
                }
                return _secure_file_priv;
            }

        }

        #endregion

        #region 方法

        /// <summary>
        /// 查询mysql 的环境变量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlCmd"></param>
        /// <returns></returns>
        private T LoadVariables<T>(string sqlCmd)
        {
            T result = default(T);
            if (sqlCmd.IsEmpty())
            {
                return result;
            }
            using (var conn = new MySqlConnection(this._connString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                result = conn.ExecuteScalar<T>(sqlCmd);

            }

            return result;
        }
        #endregion

    }
}
