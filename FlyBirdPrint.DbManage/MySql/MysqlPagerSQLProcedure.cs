using System;
using MySql.Data.MySqlClient;
using FlyBirdYoYo.DbManage.Utilities;
using FlyBirdYoYo.Utilities;

namespace FlyBirdYoYo.DbManage
{

    /// <summary>
    /// 检测是否存在需要的分页存储过程
    /// </summary>
    public static class MySqlPagerSQLProcedure
    {

        //检查存储过程存在--命令
        private static readonly string Cmd_Check_Target_Procedure = @"select count(1) from mysql.proc
        where db = '{0}'
        and `type` = 'PROCEDURE'
        and `name`='{1}'";


        //internal static void CheckAndCreatePagerSQLProcedure(DbConnConfig dbConfig)
        //{
        //    var connStr = dbConfig.ConnString;

        //    //1检查数据库是否存在存储过程   //2创建
        //    //分页调用入口
        //    var isHasExist = CheckIsHasExistProcedure(connStr, Contanst.PageSql_Call_Name);
        //    if (isHasExist == true)
        //    {
        //        return;
        //    }
        //    CreateSQLProcedure(connStr, PageSql_Call_MySqlCommand);

        //}

        /// <summary>
        /// 检查指定的存储过程是否存在
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        public static bool CheckIsHasExistProcedure(string connStr, string procName)
        {
            var result = false;

            //这里不执行异常的输出！
            try
            {

                using (var conn = new MySqlConnection(connStr))
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    var dbName = conn.Database;
                    if (string.IsNullOrEmpty(dbName))
                    {
                        throw new Exception("数据库实例不能为空！");
                    }

                    var sqlCmd = string.Format(Cmd_Check_Target_Procedure, dbName, procName);

                    var cmd = new MySqlCommand(sqlCmd, conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    result = Convert.ToInt32(cmd.ExecuteScalar()) > 0 ? true : false;

                }

            }
            catch
            {
            }

            return result;
        }



        private static void CreateSQLProcedure(string connStr, string MySqlCommand)
        {
            if (string.IsNullOrEmpty(connStr))
            {
                throw new Exception("数据库连接异常，请检查连接字符串的配置！");
            }



            using (var conn = new MySqlConnection(connStr))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                MySqlScript cmdScript = new MySqlScript(conn);
                cmdScript.Query = MySqlCommand;
                cmdScript.Delimiter = "??";//设定结束符

                int result = cmdScript.Execute();
            }
        }

        

    }
}
