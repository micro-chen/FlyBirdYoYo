
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using Dapper.Contrib.Extensions;
using System.Threading.Tasks;
using Dapper;
using FlyBirdYoYo.DbManage.CommandTree;
using FlyBirdYoYo.DbManage.Utilities;
using FlyBirdYoYo.Utilities.Logging;

using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.DbManage.Mapping;

namespace FlyBirdYoYo.DbManage
{
    public abstract class BaseSqlOperation<TElement> where TElement : BaseEntity, new()
    {
        /// <summary>
        /// 由于ADO.NET  最多可以传递2100参数 我们设置阈值略小于这个值
        /// </summary>
        protected const int MAX_SQL_PARAS_LIMIT = 2000;


        /// <summary>
        /// 字段包裹符号
        /// 用来区分不同的数据库的字段 当有关键词的时候 ，需要包裹。比如mysql ： ` 
        /// </summary>
        private string _FieldWrapperChar = "";
        public virtual string FieldWrapperChar
        {
            get
            {
                return _FieldWrapperChar;
            }
            set
            {
                _FieldWrapperChar = value;
            }
        }

        private DbConnConfig _dbConfig;
        /// <summary>
        /// 数据库连接配置
        /// </summary>
        public DbConnConfig DbConfig
        {
            get
            {
                return this._dbConfig;
            }
            set
            {
                this._dbConfig = value;
            }
        }

        public BaseSqlOperation()
        {

        }



        /// <summary>
        /// 错误的数据操作结果标志
        /// </summary>
        public const int Error_Opeation_Result = -1;


        /// <summary>
        /// 打出sql命令参数
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <param name="sqlParas"></param>
        public  void SqlOutPutToLogAsync(string sqlCmd, object sqlParas = null)
        {
            if (this.DbConfig.SqlOutPut == true)
            {
                DbConnectionExtensions.SqlOutPutToLogAsync(sqlCmd, sqlParas);
            }
        }

        #region  聚合函数实现  SUM COUNT  MAX  MIN

        /// <summary>
        /// 使用指定的条件 汇总列
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="specialColumn"></param>
        /// <returns></returns>
        public decimal Sum(Expression<Func<TElement, bool>> predicate, Fields<TElement> specialColumn)
        {
            TElement entity = new TElement();


            string tableInDbName;
            System.Reflection.PropertyInfo[] propertys;
            string[] filelds;
            string[] paras;
            var sqlFieldMapping=ResolveEntity(entity, false, out tableInDbName, out propertys, out filelds, out paras);
            if (filelds.Length <= 1)
            {
                //除主键后 没有其他字段
                throw new Exception("未指定除主键后其他字段！");
            }

            //解析查询条件
            string whereStr = "1=1";
            if (null != predicate)
            {
                whereStr = ResolveLambdaTreeToCondition.ConvertLambdaToCondition<TElement>(predicate, sqlFieldMapping, FieldWrapperChar);
            }



            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.AppendFormat("SELECT SUM({2}{0}{2}) FROM  {1} ", specialColumn.Container_Fileds.FirstOrDefault(), tableInDbName, FieldWrapperChar);
            if (!string.IsNullOrEmpty(whereStr))
            {
                sb_Sql.AppendFormat("WHERE {0} ", whereStr);
            }


            var sqlCmd = sb_Sql.ToString();

            //清理字符串构建
            sb_Sql.Clear();
            sb_Sql = null;
            DbDataReader reader = null;
            try
            {
                reader = this.ExecuteReader(sqlCmd, null, CommandType.Text);
                if (reader.Read())
                {
                    if (reader[0].ToString().IsEmpty())
                    {
                        return 0;
                    }
                    var colValue = reader.GetFieldValue<decimal>(0);
                    return colValue;
                }
                else
                {
                    throw new Exception("can not  excute sql cmd: " + sqlCmd);
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (null != reader)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// 统计 符合条件的行数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Count(Expression<Func<TElement, bool>> predicate)
        {
            TElement entity = new TElement();


            string tableInDbName;
            System.Reflection.PropertyInfo[] propertys;
            string[] filelds;
            string[] paras;
            var sqlFieldMapping = ResolveEntity(entity, false, out tableInDbName, out propertys, out filelds, out paras);
            if (filelds.Length <= 1)
            {
                //除主键后 没有其他字段
                throw new Exception("未指定除主键后其他字段！");
            }

            //解析查询条件
            string whereStr = "1=1";
            if (null != predicate)
            {
                whereStr = ResolveLambdaTreeToCondition.ConvertLambdaToCondition<TElement>(predicate,sqlFieldMapping, FieldWrapperChar);
            }


            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.AppendFormat("SELECT COUNT(*) FROM  {0} ", tableInDbName);
            if (!string.IsNullOrEmpty(whereStr))
            {
                sb_Sql.AppendFormat("WHERE {0} ", whereStr);
            }

            var sqlCmd = sb_Sql.ToString();

            //清理字符串构建
            sb_Sql.Clear();
            sb_Sql = null;
            DbDataReader reader = null;
            try
            {

                reader = this.ExecuteReader(sqlCmd, null, CommandType.Text);
                if (reader.Read())
                {
                    var colValue = reader[0].ToString().ToInt();
                    return colValue;
                }
                else
                {
                    throw new Exception("can not  excute sql cmd: " + sqlCmd);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (null != reader)
                {
                    reader.Close();
                }
            }
        }


        /// <summary>
        /// 符合条件的行的 指定列的最大值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public decimal Max(Expression<Func<TElement, bool>> predicate, Fields<TElement> specialColumn)
        {
            TElement entity = new TElement();


            string tableInDbName;
            System.Reflection.PropertyInfo[] propertys;
            string[] filelds;
            string[] paras;
            var sqlFieldMapping = ResolveEntity(entity, false, out tableInDbName, out propertys, out filelds, out paras);
            if (filelds.Length <= 1)
            {
                //除主键后 没有其他字段
                throw new Exception("未指定除主键后其他字段！");
            }

            //解析查询条件
            string whereStr = "1=1";
            if (null != predicate)
            {
                whereStr = ResolveLambdaTreeToCondition.ConvertLambdaToCondition<TElement>(predicate, sqlFieldMapping, FieldWrapperChar);
            }


            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.AppendFormat("SELECT MAX({2}{0}{2}) FROM  {1} ", specialColumn.Container_Fileds.FirstOrDefault(), tableInDbName, FieldWrapperChar);
            if (!string.IsNullOrEmpty(whereStr))
            {
                sb_Sql.AppendFormat("WHERE {0} ", whereStr);
            }

            var sqlCmd = sb_Sql.ToString();


            //清理字符串构建
            sb_Sql.Clear();
            sb_Sql = null;

            DbDataReader reader = null;
            try
            {

                reader = this.ExecuteReader(sqlCmd, null, CommandType.Text);
                if (reader.Read())
                {
                    if (reader[0].ToString().IsEmpty())
                    {
                        return 0;
                    }
                    var colValue = reader.GetFieldValue<decimal>(0);
                    return colValue;
                }
                else
                {
                    throw new Exception("can not  excute sql cmd: " + sqlCmd);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (null != reader)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// 符合条件的行的 指定列的最小值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="specialColumn"></param>
        /// <returns></returns>
        public decimal Min(Expression<Func<TElement, bool>> predicate, Fields<TElement> specialColumn)
        {
            TElement entity = new TElement();


            string tableInDbName;
            System.Reflection.PropertyInfo[] propertys;
            string[] filelds;
            string[] paras;
            var sqlFieldMapping = ResolveEntity(entity, false, out tableInDbName, out propertys, out filelds, out paras);
            if (filelds.Length <= 1)
            {
                //除主键后 没有其他字段
                throw new Exception("未指定除主键后其他字段！");
            }

            //解析查询条件
            string whereStr = "1=1";
            if (null != predicate)
            {
                whereStr = ResolveLambdaTreeToCondition.ConvertLambdaToCondition<TElement>(predicate, sqlFieldMapping,FieldWrapperChar);
            }


            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.AppendFormat("SELECT MIN({2}{0}{2}) FROM  {1} ", specialColumn.Container_Fileds.FirstOrDefault(), tableInDbName, FieldWrapperChar);
            if (!string.IsNullOrEmpty(whereStr))
            {
                sb_Sql.AppendFormat("WHERE {0} ", whereStr);
            }

            var sqlCmd = sb_Sql.ToString();


            //清理字符串构建
            sb_Sql.Clear();
            sb_Sql = null;
            DbDataReader reader = null;
            try
            {

                reader = this.ExecuteReader(sqlCmd, null, CommandType.Text);
                if (reader.Read())
                {
                    if (reader[0].ToString().IsEmpty())
                    {
                        return 0;
                    }
                    var colValue = reader.GetFieldValue<decimal>(0);
                    return colValue;
                }
                else
                {
                    throw new Exception("can not  excute sql cmd: " + sqlCmd);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (null != reader)
                {
                    reader.Close();
                }
            }
        }

        #endregion

        #region ADO.NET 相关


        /// <summary>
        /// 获取此上下文的新的db连接对象
        /// </summary>
        /// <returns></returns>
       public  IDbConnection GetCurrentDbConnection()
        {
            return DatabaseFactory.GetDbConnection(this._dbConfig);
        }


        /// <summary>
        /// 执行查询 返回 DataSet
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param> 
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string cmdText, DbParameter[] commandParameters, CommandType cmdType = CommandType.Text)
        {

            DataSet resultData = new DataSet();

            this.SqlOutPutToLogAsync(cmdText, commandParameters);

            using (DbConnection conn = DatabaseFactory.GetDbConnection(this._dbConfig))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (DbCommand cmd = DatabaseFactory.GetDbDbCommand(this._dbConfig))
                {


                    try
                    {
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                        var dataAdapter = DatabaseFactory.GetDbDataAdapter(cmd, this._dbConfig);
                        dataAdapter.Fill(resultData);
                        cmd.Parameters.Clear();

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    return resultData;
                }

            }
        }



        /// <summary>
        /// 执行命令 并返回影响行数
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>  
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, DbParameter[] commandParameters, CommandType cmdType = CommandType.Text)
        {
            this.SqlOutPutToLogAsync(cmdText, commandParameters);


            using (DbConnection conn = DatabaseFactory.GetDbConnection(this._dbConfig))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (DbCommand cmd = DatabaseFactory.GetDbDbCommand(this._dbConfig))
                {

                    int val = -1;
                    try
                    {
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                        val = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    return val;
                }

            }


        }



        /// <summary>
        ///  执行命令  并返回一个可读的Reader
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string cmdText, DbParameter[] commandParameters, CommandType cmdType = CommandType.Text)
        {

            this.SqlOutPutToLogAsync(cmdText, commandParameters);

            try
            {
                DbConnection conn = DatabaseFactory.GetDbConnection(this._dbConfig);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                DbCommand cmd = DatabaseFactory.GetDbDbCommand(this._dbConfig);

                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                DbDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return rdr;
            }
            catch (Exception ex)
            {
                throw ex;
            }





        }



        /// <summary>
        ///执行命令 并返回首列结果
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public object ExecuteScalar(string cmdText, DbParameter[] commandParameters, CommandType cmdType = CommandType.Text)
        {
            object val = null;

            this.SqlOutPutToLogAsync(cmdText, commandParameters);

            using (DbConnection conn = DatabaseFactory.GetDbConnection(this._dbConfig))

            {
                DbCommand cmd = DatabaseFactory.GetDbDbCommand(this._dbConfig);
                try
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }


                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }


                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }

            return val;
        }




        #endregion


        #region Sql 语句批量执行





        /// <summary>
        ///  Sql 语句批量执行
        /// </summary>
        /// <param name="SqlCmdList"></param>
        /// <returns></returns>
        public bool SqlBatchExcute(Dictionary<string, DbParameter[]> sqlCmdList)
        {

            bool result = false;
            if (null == sqlCmdList || sqlCmdList.Count <= 0)
            {
                return result;
            }

            using (DbConnection conn = DatabaseFactory.GetDbConnection(this._dbConfig))
            {
                conn.Open();
                using (DbTransaction trans = conn.BeginTransaction())
                {

                    try
                    {

                        var entor = sqlCmdList.GetEnumerator();

                        StringBuilder sbSqlCmdBagList = new StringBuilder();
                        var sqlParasBagList = new List<DbParameter>();
                        int pos = 0;
                        while (entor.MoveNext() || pos >= sqlCmdList.Count)
                        {
                            //到达sql参数上限或者达到命令列表边界  执行sql 包
                            if (sqlParasBagList.Count >= MAX_SQL_PARAS_LIMIT || pos >= sqlCmdList.Count)
                            {

                                using (DbCommand cmd = conn.CreateCommand())
                                {
                                    //到达最大参数临界的时候 执行sql 批语句
                                    cmd.Transaction = trans;
                                    cmd.CommandText = sbSqlCmdBagList.ToString();
                                    cmd.Parameters.AddRange(sqlParasBagList.ToArray());
                                    cmd.ExecuteNonQuery();
                                }
                                //重置
                                sbSqlCmdBagList.Clear();
                                sqlParasBagList.Clear();

                                if (pos >= sqlCmdList.Count)
                                {
                                    break;//超出临界  跳出循环
                                }
                            }

                            var cmdPair = entor.Current;
                            if (string.IsNullOrEmpty(cmdPair.Key))
                            {
                                continue;
                            }

                            string sqlCmd = cmdPair.Key;
                            DbParameter[] sqlParas = cmdPair.Value;
                            if (!sqlCmd.TrimEnd().EndsWith(";"))
                            {
                                sqlCmd += ";";
                            }
                            //处理sql 命令，和参数，对sql命令中的@ 和参数中的@的进行匹配识别
                            if (null == sqlParas || sqlParas.Length <= 0)
                            {
                                sbSqlCmdBagList.Append(sqlCmd);
                            }
                            else
                            {
                                //按照计数游标 对参数进行处理
                                foreach (DbParameter itemDbPara in sqlParas)
                                {
                                    var paraName = itemDbPara.ParameterName;
                                    string nameWithPos = string.Concat(paraName, pos);
                                    itemDbPara.ParameterName = nameWithPos;
                                    sqlCmd = sqlCmd.Replace(paraName, nameWithPos);
                                }

                                sbSqlCmdBagList.Append(sqlCmd);//追加格式化 后缀的sql 命令
                                sqlParasBagList.AddRange(sqlParas);
                            }

                            pos += 1;//计数后移

                        }
                        trans.Commit();
                        result = true;

                    }
                    catch (Exception ex)
                    {
                        result = false;
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
            return result;


        }

        #endregion

        /// <summary>
        ///  执行一个SQL查询语句，进行参数化查询
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public List<TElement> SqlQuery(string cmdText, CommandType commandType, params DbParameter[] commandParameters)
        {

            var dataLst = new List<TElement>();

            this.SqlOutPutToLogAsync(cmdText, commandParameters);

            //获取返回的结果 作为DataTable  解析其中的Row 到特定的实体
            DbDataReader reader = null;
            try
            {
                reader = this.ExecuteReader(cmdText, commandParameters, commandType);
                if (null == reader)
                {
                    return null;
                }
                while (reader.Read())
                {
                    var model = reader.ConvertDataReaderToEntity<TElement>();
                    dataLst.Add(model);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != reader)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }

            return dataLst;
        }


        public abstract PagedSqlDataResult<T> PageQuery<T>(PagedSqlCondition condition) where T : class;

        /// <summary>
        /// 提取参数化查询sql 中的参数列表
        /// </summary>
        /// <param name="inputSql"></param>
        /// <returns></returns>
        internal string GetParamSqlTokenToSqlParas(string inputSql,out string[] sqlParaToken)
        {
            string result = string.Empty;
            sqlParaToken = null;
            if (inputSql.IsNullOrEmpty())
            {
                return result;
            }

            SqlStatementTokenizer statementTokenizer = new SqlStatementTokenizer(inputSql);
            statementTokenizer.ReturnComments = true;
            statementTokenizer.SqlServerMode = true;

            var isParamSql = statementTokenizer.IsParamedSql();
            if (isParamSql == false)
            {
                //非参数话的查询 直接返回true
                return result;
            }


            //如果是参数话的查询 那么检测参数
            var allTokens = statementTokenizer.GetAllParamedTokens();
            sqlParaToken = allTokens.ToArray();

            return string.Join(",", allTokens);
        }


        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        public List<TElement> ExecuteStoredProcedureList(string commandText, params DbParameter[] parameters)
        {
            //获取返回的结果 作为DataTable  解析其中的Row 到特定的实体
            var dataList = this.SqlQuery(commandText, CommandType.StoredProcedure, parameters);
            return dataList;
        }



        /// <summary>
        /// 检测是否是参数化查询SQL
        /// </summary>
        /// <param name="conn">基于的数据库连接</param>
        /// <param name="inputSql">执行的SQL  语句</param>
        ///<param name = "cmdParms" > 执行的sql 参数/param>
        /// <returns></returns>
        private bool IsValidParamedSqlQuery(DbConnection conn, string inputSql, IEnumerable<DbParameter> cmdParms)
        {
            var result = false;
            //    MySqlConnectionStringBuilder cb = new MySqlConnectionStringBuilder(
            //conn.ConnectionString);
            //    bool sqlServerMode = cb.SqlServerMode;

            SqlStatementTokenizer statementTokenizer = new SqlStatementTokenizer(inputSql);
            statementTokenizer.ReturnComments = true;
            statementTokenizer.SqlServerMode = true;

            var isParamSql = statementTokenizer.IsParamedSql();
            if (isParamSql == false)
            {
                //非参数话的查询 直接返回true
                return true;
            }


            //如果是参数话的查询 那么检测参数
            //基于参数的查询，匹配是否有参数

            if (cmdParms == null || cmdParms.Count() <= 0)
            {
                throw new Exception("基于参数化查询的SQL命令，请必须提供参数！");
            }


            //通过了检测 
            result = true;
            statementTokenizer = null;

            return result;
        }



        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">DbCommand object</param>
        /// <param name="conn">DbConnection object</param>
        /// <param name="trans">DbTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">DbParameter to use in the command</param>
        protected void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, IEnumerable<DbParameter> cmdParms)
        {



            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;


            //检测本次查询的有效性，如果是参数查询，但是不提供参数，那么抛出异常信息
            if (!IsValidParamedSqlQuery(conn, cmdText, cmdParms))
            {
                throw new ArgumentException("非法的参数化查询！请检测SQL的正确性！");
            }

            if (cmdParms != null)
            {
                foreach (var parm in cmdParms)
                {
                    if (parm.Value == null)
                    {
                        parm.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parm);
                }


            }
        }


        /// <summary>
        /// 解析实体   解析其中的关联的表+字段+字段参数
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isWriteCmd">是否是写入命令生成sql参数</param>
        /// <param name="tableInDbName"></param>
        /// <param name="propertys"></param>
        /// <param name="filelds">数据库字段列表</param>
        /// <param name="paras">sql  参数集合</param>
        internal SqlFieldMapping ResolveEntity(TElement entity, bool isWriteCmd, out string tableInDbName, out System.Reflection.PropertyInfo[] propertys, out string[] filelds, out string[] paras)
        {
            //调用实体的解析实体

            var mapping = entity.ResolveEntity(isWriteCmd);
            tableInDbName = mapping.TableName;
            propertys = mapping.Propertys;
            filelds = mapping.Filelds.Select(x=>x.FieldColumnName).ToArray();
            paras = mapping.SqlParas;
            return mapping;



        }
    }
}
