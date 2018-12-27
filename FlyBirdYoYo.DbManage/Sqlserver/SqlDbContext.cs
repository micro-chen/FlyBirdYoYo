using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using FlyBirdYoYo.Utilities.TypeFinder;
using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.DbManage.Utilities;
using FlyBirdYoYo.DbManage.CommandTree;
using System.Reflection;

namespace FlyBirdYoYo.DbManage
{


    /// <summary>
    /// 连接数据库的  上下文  用来执行与数据库进行交互
    /// </summary>
    public class SqlDbContext<TElement> : BaseSqlOperation<TElement>, IDbContext<TElement>, IDisposable
        where TElement : BaseEntity, new()
    {
        #region Construction and fields




        /// <summary>
        /// 实体的主键名称
        /// </summary>
        private string EntityIdentityFiledName = new TElement().GetIdentity().IdentityKeyName;



        /// <summary>
        /// 数据上下文 构造函数
        /// </summary>
        /// <param name="dbConfig"></param>
        public SqlDbContext(DbConnConfig dbConfig)
        {
            this.DbConfig = dbConfig;
        }



        #endregion


        #region Context methods


        #region  Insert操作
        /// <summary>
        /// 插入 实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public long Insert(TElement entity, IDbTransaction transaction = null)
        {
            string tableInDbName;
            System.Reflection.PropertyInfo[] propertys;
            string[] filelds;
            string[] paras;
            ResolveEntity(entity, true, out tableInDbName, out propertys, out filelds, out paras);

            ///不含主键的属性
            var noIdentityPropertys = propertys.Remove(x => x.Name == EntityIdentityFiledName);
            var noIdentityFileds = filelds.Remove(x => x == EntityIdentityFiledName);
            var noIdentityParas = paras.Remove(x => x.ToLower() == string.Format("@{0}", EntityIdentityFiledName.ToLower()));

            var fieldSplitString = String.Join(",", noIdentityFileds);//返回逗号分隔的字符串 例如：ProvinceCode,ProvinceName,Submmary
            var parasSplitString = String.Join(",", noIdentityParas);//参数   数组 的逗号分隔


            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.Append(string.Format("insert into {0}(", tableInDbName));
            sb_Sql.Append(string.Format("{0})", fieldSplitString));
            sb_Sql.Append(" values (");
            sb_Sql.Append(string.Format("{0})", parasSplitString));
            sb_Sql.Append(";select @@IDENTITY;");


            var sqlCmd = sb_Sql.ToString();


            ///清理掉字符串拼接构造器
            sb_Sql.Clear();
            sb_Sql = null;

            this.SqlOutPutToLogAsync(sqlCmd, entity);

            using (var conn = DatabaseFactory.GetDbConnection(this.DbConfig))
            {
                var result = conn.ExecuteScalar<long>(sqlCmd, entity, transaction);
                return result;
            }
        }



        /// <summary>
        /// 单次批量多次插入多个实体
        /// (注意：sqlbuck插入，高效率sqlbuck方式插入)
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool InsertMulitiEntities(IEnumerable<TElement> entities, IDbTransaction transaction = null)
        {
            var result = -1;


            var count_entities = entities.Count();
            if (count_entities <= 0)
            {
                return false;
            }


            string tableInDbName;
            System.Reflection.PropertyInfo[] propertys;
            string[] filelds;
            string[] paras;
            ResolveEntity(entities.First(), true, out tableInDbName, out propertys, out filelds, out paras);

            try
            {

                this.SqlOutPutToLogAsync("InsertMulitiEntities", entities);

                ///不含主键的属性
                var noIdentityPropertys = propertys.Remove(x => x.Name.ToLower() == EntityIdentityFiledName.ToLower());

                using (var conn = DatabaseFactory.GetDbConnection(this.DbConfig))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }



                    if (null == transaction)
                    {
                        transaction = conn.BeginTransaction();
                    }

                    using (var bulk = new SqlBulkCopy(conn as SqlConnection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction))
                    {
                        bulk.BulkCopyTimeout = 120;//命令超时时间
                        bulk.BatchSize = 1000;
                        //指定写入的目标表
                        bulk.DestinationTableName = tableInDbName;
                        //数据源中的列名与目标表的属性的映射关系
                        //bulk.ColumnMappings.Add("ip", "ip");
                        //bulk.ColumnMappings.Add("port", "port");
                        //bulk.ColumnMappings.Add("proto_name", "proto_name");
                        //bulk.ColumnMappings.Add("strategy_id", "strategy_id");
                        //init mapping
                        foreach (var pi in noIdentityPropertys)
                        {
                            bulk.ColumnMappings.Add(pi.Name, pi.Name);
                        }

                        DataTable dt = SqlDataTableExtensions.ConvertListToDataTable<TElement>(entities, ref noIdentityPropertys);//数据源数据

                        //DbDataReader reader = dt.CreateDataReader();
                        bulk.WriteToServer(dt);

                        if (null != transaction)
                        {
                            transaction.Commit();
                        }
                    }

                }


                result = 1;

            }
            catch (Exception ex)
            {

                if (null != transaction)
                {
                    transaction.Rollback();
                }
                //抛出Native 异常信息
                throw ex;
            }


            var isSuccess = result > 0 ? true : false;


            return isSuccess;


        }

        #endregion


        #region Update 更新操作

        /// <summary>
        /// 更新单个模型
        /// （更新机制为，模型载体设置的值的字段会被更新掉，不设置值 不更新）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int Update(TElement entity, IDbTransaction transaction = null)
        {
            string tableInDbName;
            System.Reflection.PropertyInfo[] propertys;
            string[] filelds;
            string[] paras;
            var sqlFieldMapping = ResolveEntity(entity, true, out tableInDbName, out propertys, out filelds, out paras);
            if (filelds.Length <= 1)
            {
                //除主键后 没有其他字段
                return -1;
                throw new Exception("未指定除主键后其他字段！");
            }

            StringBuilder sb_FiledParaPairs = new StringBuilder("");


            var settedValueDic = entity.GetSettedValuePropertyDic();

            foreach (var item in settedValueDic)
            {
                var keyProperty = item.Key;
                //var value = item.Value;
                if (keyProperty != EntityIdentityFiledName)
                {
                    string fieldName = ResolveLambdaTreeToCondition.SearchPropertyMappingField(sqlFieldMapping, keyProperty);
                    sb_FiledParaPairs.AppendFormat("{1}{0}{1}=@{2},", fieldName, this.FieldWrapperChar, keyProperty);
                }
            }

            //移除最后一个逗号
            var str_FiledParaPairs = sb_FiledParaPairs.ToString();
            str_FiledParaPairs = str_FiledParaPairs.Remove(str_FiledParaPairs.Length - 1);

            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.Append(string.Format("update {0} set ", tableInDbName));//Set Table
            sb_Sql.Append(str_FiledParaPairs);//参数对

            sb_Sql.AppendFormat(" where {0}=@{0}", EntityIdentityFiledName);//主键


            var sqlCmd = sb_Sql.ToString();
            ///清理掉字符串拼接构造器
            sb_FiledParaPairs.Clear();
            sb_FiledParaPairs = null;
            sb_Sql.Clear();
            sb_Sql = null;

            this.SqlOutPutToLogAsync(sqlCmd, entity);

            using (var conn = DatabaseFactory.GetDbConnection(this.DbConfig))
            {
                var result = conn.Execute(sqlCmd, entity, transaction);
                return result;
            }
        }

        /// <summary>
        /// 更新元素 通过  符合条件的
        /// （更新机制为，模型载体设置的值的字段会被更新掉，不设置值 不更新）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int UpdateByCondition(TElement entity, Expression<Func<TElement, bool>> predicate, IDbTransaction transaction = null)
        {
            string tableInDbName;
            System.Reflection.PropertyInfo[] propertys;
            string[] filelds;
            string[] paras;
            var sqlFieldMapping = ResolveEntity(entity, true, out tableInDbName, out propertys, out filelds, out paras);
            if (filelds.Length <= 1)
            {
                //除主键后 没有其他字段
                return -1;
                throw new Exception("未指定除主键后其他字段！");
            }


            StringBuilder sb_FiledParaPairs = new StringBuilder("");
            ///解析要更新的列
            var settedValueDic = entity.GetSettedValuePropertyDic();

            foreach (var item in settedValueDic)
            {
                var keyProperty = item.Key;
                //var value = item.Value;
                if (keyProperty != EntityIdentityFiledName)
                {
                    string fieldName = ResolveLambdaTreeToCondition.SearchPropertyMappingField(sqlFieldMapping, keyProperty);
                    sb_FiledParaPairs.AppendFormat("{1}{0}{1}=@{2},", fieldName, this.FieldWrapperChar, keyProperty);
                }
            }
            //移除最后一个逗号
            var str_FiledParaPairs = sb_FiledParaPairs.ToString();
            str_FiledParaPairs = str_FiledParaPairs.Remove(str_FiledParaPairs.Length - 1);

            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.Append(string.Format("update {0} set ", tableInDbName));//Set Table
            sb_Sql.Append(str_FiledParaPairs);//参数对



            if (null != predicate)
            {
                string where = ResolveLambdaTreeToCondition.ConvertLambdaToCondition<TElement>(predicate,sqlFieldMapping);
                sb_Sql.Append(" where ");//解析条件
                sb_Sql.Append(where);//条件中带有参数=值的  拼接字符串
            }


            var sqlCmd = sb_Sql.ToString();

            ///清理字符串构建
            sb_FiledParaPairs.Clear();
            sb_FiledParaPairs = null;
            sb_Sql.Clear();
            sb_Sql = null;

            this.SqlOutPutToLogAsync(sqlCmd, entity);

            using (var conn = DatabaseFactory.GetDbConnection(this.DbConfig))
            {
                var result = conn.Execute(sqlCmd, entity, transaction);
                return result;
            }
        }

        #endregion


        #region Select   查询操作

        /// <summary>
        /// 通过主键获取单个元素
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public TElement GetElementById(long id)
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
                return null;
                throw new Exception("未指定除主键后其他字段！");
            }
            //获取字段
            //List<string> fieldAlias = new List<string>();
            //foreach (var item in sqlFieldMapping.Filelds)
            //{
            //    var ailasName = string.Format("{0} as {1}", item.FieldColumnName, item.PropertyName);
            //    fieldAlias.Add(ailasName);
            //}
            var fieldSplitString = "*";//entity.GetSqlQueryFieldsWithAlias();// String.Join(",", fieldAlias);//返回逗号分隔的字符串 例如：ProvinceCode,ProvinceName,Submmary

            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.AppendFormat("select {0} ", fieldSplitString);
            sb_Sql.AppendFormat(" from {0} ", tableInDbName);//WITH (NOLOCK) 由于不锁定表执行的事务锁-会有数据脏读
            sb_Sql.AppendFormat(" where {0}={1};", EntityIdentityFiledName, id);

            var sqlCmd = sb_Sql.ToString();

            sb_Sql.Clear();
            sb_Sql = null;

            try
            {
                this.SqlOutPutToLogAsync(sqlCmd);
                using (var conn = DatabaseFactory.GetDbConnection(this.DbConfig))
                {
                    entity = conn.QueryFirstOrDefault<TElement>(sqlCmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entity;
        }

        /// <summary>
        /// 通过特定的条件查询出元素
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TElement GetFirstOrDefaultByCondition(Expression<Func<TElement, bool>> predicate)
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
                return null;
                throw new Exception("未指定除主键后其他字段！");
            }
            //获取字段
            //List<string> fieldAlias = new List<string>();
            //foreach (var item in sqlFieldMapping.Filelds)
            //{
            //    var ailasName = string.Format("{0} as {1}", item.FieldColumnName, item.PropertyName);
            //    fieldAlias.Add(ailasName);
            //}
            //获取字段
            var fieldSplitString = "*";// entity.GetSqlQueryFieldsWithAlias();//String.Join(",", fieldAlias);//返回逗号分隔的字符串 例如：ProvinceCode,ProvinceName,Submmary


            //解析查询条件
            string whereStr = "1=1";
            if (null != predicate)
            {
                whereStr = ResolveLambdaTreeToCondition.ConvertLambdaToCondition<TElement>(predicate, sqlFieldMapping);
            }



            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.AppendFormat("select top 1  {0} ", fieldSplitString);
            sb_Sql.AppendFormat(" from {0} ", tableInDbName);
            sb_Sql.AppendFormat(" where {0};", whereStr);


            var sqlCmd = sb_Sql.ToString();

            sb_Sql.Clear();
            sb_Sql = null;

            try
            {
                this.SqlOutPutToLogAsync(sqlCmd);
                using (var conn = DatabaseFactory.GetDbConnection(this.DbConfig))
                {
                    entity = conn.QueryFirstOrDefault<TElement>(sqlCmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entity;
        }

        /// <summary>
        /// 通过特定的条件查询出元素集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<TElement> GetElementsByCondition(Expression<Func<TElement, bool>> predicate)
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
                return null;
                throw new Exception("未指定除主键后其他字段！");
            }
            //获取字段
            //List<string> fieldAlias = new List<string>();
            //foreach (var item in sqlFieldMapping.Filelds)
            //{
            //    var ailasName = string.Format("{0} as {1}", item.FieldColumnName, item.PropertyName);
            //    fieldAlias.Add(ailasName);
            //}
            //获取字段
            var fieldSplitString = "*";//entity.GetSqlQueryFieldsWithAlias();// String.Join(",", fieldAlias);//返回逗号分隔的字符串 例如：ProvinceCode,ProvinceName,Submmary


            //解析查询条件
            string whereStr = "1=1";
            if (null != predicate)
            {
                whereStr = ResolveLambdaTreeToCondition.ConvertLambdaToCondition<TElement>(predicate,sqlFieldMapping);
            }



            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.AppendFormat("select  {0} ", fieldSplitString);
            sb_Sql.AppendFormat(" from {0} ", tableInDbName);
            sb_Sql.AppendFormat(" where {0};", whereStr);


            var sqlCmd = sb_Sql.ToString();

            sb_Sql.Clear();
            sb_Sql = null;

            List<TElement> dataLst = null;
            try
            {
                this.SqlOutPutToLogAsync(sqlCmd);

                using (var conn = DatabaseFactory.GetDbConnection(this.DbConfig))
                {
                    dataLst = conn.Query<TElement>(sqlCmd).AsList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataLst;
        }

 

        /// <summary>
        /// 执行分页查询的核心方法：支持单表和多表分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">执行条件</param>
        /// <returns></returns>
        public override PagedSqlDataResult<T> PageQuery<T>(PagedSqlCondition condition)
        {
            PagedSqlDataResult<T> pageData = null;
            if (null == condition)
            {
                return pageData;
            }
            string errMsg = "";
            if (!condition.IsValid(out errMsg))
            {
                throw new Exception("分页查询错误：" + errMsg);
            }

            //记录打出日志
            this.SqlOutPutToLogAsync(Contanst.PageSql_Call_Name, condition);


            try
            {


                //判断结果集类型
                if (null == condition.TableOptions)
                {
                    //简单sql语句类型判断
                    if (condition.TableNameOrSqlCmd.ToLower().Contains("select") && condition.TableNameOrSqlCmd.ToLower().Contains("from"))
                    {
                        condition.TableOptions = PageTableOptions.SqlScripts;
                    }
                    else
                    {
                        condition.TableOptions = PageTableOptions.TableOrView;
                    }
                }

                //动态查询需要包装外层
                if (condition.TableOptions == PageTableOptions.SqlScripts)
                {
                    //condition.TableNameOrSqlCmd = condition.TableNameOrSqlCmd.Replace("'", "''");
                    condition.TableNameOrSqlCmd = string.Format(" ( {0} ) as  tmpTable ", condition.TableNameOrSqlCmd);
                }

                //完整的sql参数转化
                var fullPagerSqlParas = new DynamicParameters();
                fullPagerSqlParas.Add("@PageIndex", condition.PageNumber - 1);
                fullPagerSqlParas.Add("@PageSize", condition.PageSize);
                fullPagerSqlParas.Add("@PrimaryKey", condition.PrimaryKey);//主键
                fullPagerSqlParas.Add("@TableNameOrSqlCmd", condition.TableNameOrSqlCmd);//将查询结果结合作为分页的表数据
                fullPagerSqlParas.Add("@SortField", condition.SortField);
                fullPagerSqlParas.Add("@SelectFields", condition.SelectFields);
                fullPagerSqlParas.Add("@ConditionWhere", condition.ConditionWhere);
                fullPagerSqlParas.Add("@IsDesc", condition.IsDesc == true ? 1 : 0);



                string pageSqlTemplate = PagerSQLProcedure.PAGE_SQL_CORE;
                string pagerSql = string.Empty;

                if (null != condition.SqlParameters)
                {

                    //参数列表字符串--声明
                    //参数列表 -逗号分隔
                    string[] sqlParaToken = null;
                    string parasStr = this.GetParamSqlTokenToSqlParas(condition.TableNameOrSqlCmd, out sqlParaToken);
                    string paraKeyValueString = string.Empty;

                    string paraDeinfine = this.GetSqlServerParamDefineString(condition.SqlParameters, sqlParaToken,out paraKeyValueString);

                    string defWithWraper = string.Format("N'{0}'", paraDeinfine);
                    string defDotWraper= string.Format("N',{0}'", paraDeinfine);

                    if (!string.IsNullOrEmpty(paraDeinfine))
                    {
                        pagerSql = string.Format(pageSqlTemplate, " ", defWithWraper, defDotWraper, paraKeyValueString);//需要带参数

                    }
                    else
                    {
                        pagerSql = string.Format(pageSqlTemplate, "--", "N''", "N''", " ");//不用带参数
                    }


                    //将参数整体注入到动态参数
                    fullPagerSqlParas.AddDynamicParams(condition.SqlParameters);
                }
                else
                {
                    //fullPagerSqlParas.Add("@IsParamQuery", false);
                    pagerSql = string.Format(pageSqlTemplate, "--", "N''", "N''", " ");//不用带参数
                }




                //string 

                using (var conn = DatabaseFactory.GetDbConnection(this.DbConfig))
                {


                    //多部分结果
                    var multiResult = conn.QueryMultiple(pagerSql, fullPagerSqlParas);
                    if (null != multiResult)
                    {
                        pageData = multiResult.ReadFirstOrDefault<PagedSqlDataResult<T>>();//分页信息
                        if (null != pageData)
                        {
                            pageData.DataList = multiResult.Read<T>().AsList();//结果行
                        }
                    }


                    //---------废弃的存储过程----------
                    ////////////////////var dataList = conn.Query<T>(pagerSql, fullPagerSqlParas, commandType: CommandType.Text).AsList();
                    ////////////////////if (dataList.IsNotEmpty())
                    ////////////////////{
                    ////////////////////    pageData = new PagedSqlDataResult<T>(dataList);

                    ////////////////////    //查询完毕后 根据输出参数 返回总记录数 总页数
                    ////////////////////    pageData.TotalRows = fullPagerSqlParas.Get<int>("@TotalRecords");
                    ////////////////////    pageData.TotalPages = fullPagerSqlParas.Get<int>("@TotalPageCount");

                    ////////////////////}



                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pageData;
        }

        #endregion


        #region Delete 删除操作

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int Delete(TElement entity, IDbTransaction transaction = null)
        {
            string tableInDbName;
            System.Reflection.PropertyInfo[] propertys;
            string[] filelds;
            string[] paras;
            var sqlFieldMapping = ResolveEntity(entity, true, out tableInDbName, out propertys, out filelds, out paras);

            var identityKey = sqlFieldMapping.Filelds.Where(x => x.FieldColumnName == EntityIdentityFiledName).FirstOrDefault();
            if (null == identityKey)
            {
                //除主键后 没有其他字段
                return -1;
                throw new Exception("未指定主键字段！");
            }


            var primaryValue = ReflectionHelper.GetPropertyValue(entity, identityKey.PropertyName);

            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.AppendFormat("delete from {0} ", tableInDbName);
            sb_Sql.AppendFormat(" where {0}={1};", EntityIdentityFiledName, primaryValue);


            var sqlCmd = sb_Sql.ToString();

            //清理构建器
            sb_Sql.Clear();
            sb_Sql = null;

            try
            {
                this.SqlOutPutToLogAsync(sqlCmd, entity);

                using (var conn = DatabaseFactory.GetDbConnection(this.DbConfig))
                {
                    var result = conn.Execute(sqlCmd, transaction);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除符合条件的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int DeleteByCondition(Expression<Func<TElement, bool>> predicate, IDbTransaction transaction = null)
        {
            TElement entity = new TElement();


            string tableInDbName;
            System.Reflection.PropertyInfo[] propertys;
            string[] filelds;
            string[] paras;
            var sqlFieldMapping = ResolveEntity(entity, true, out tableInDbName, out propertys, out filelds, out paras);
            if (filelds.Length <= 1)
            {
                //除主键后 没有其他字段
                return -1;
                throw new Exception("未指定除主键后其他字段！");
            }

            //解析查询条件
            var whereStr = "1=1";
            if (null != predicate)
            {
                whereStr = ResolveLambdaTreeToCondition.ConvertLambdaToCondition<TElement>(predicate,sqlFieldMapping);
            }
            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.AppendFormat("delete from {0} ", tableInDbName);
            if (null != predicate)
            {
                sb_Sql.AppendFormat("where  {0}  ", whereStr);
            }

            var sqlCmd = sb_Sql.ToString();
            try
            {
                this.SqlOutPutToLogAsync(sqlCmd);

                using (var conn = DatabaseFactory.GetDbConnection(this.DbConfig))
                {
                    var result = conn.Execute(sqlCmd, transaction);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



        #endregion


        /// <summary>
        /// 针对指定的参数实体，从中抽取特定的参数占位符的声明
        /// EXEC SP_EXECUTESQL @sql,N'@Nums INT OUT,@Score INT',@OUT_Nums OUTPUT,@IN_Score
        /// 生成中间的 参数声明部分：N'@Nums INT OUT,@Score INT'
        /// </summary>
        /// <param name="paraObj"></param>
        /// <param name="paraTokens"></param>
        /// <param name="paraKeyValueString"></param>
        /// <returns></returns>
        private string GetSqlServerParamDefineString(object paraObj, string[] paraTokens, out string paraKeyValueString)
        {
            StringBuilder sb_Para = new StringBuilder();
            StringBuilder sb_KeyValue = new StringBuilder(",");
            paraKeyValueString = string.Empty;


            if (paraTokens.IsEmpty())
            {
                return null;
            }

            var paraType = paraObj.GetType().GetTypeInfo();

            //去除公开的字段或者属性
            var arr_PubProps = paraType.DeclaredProperties;

            try
            {
          

                int counter = 0;
                foreach (var item in paraTokens)
                {
                    string pureName = item.Substring(1);//取@之后的作为参数名



                    PropertyInfo propty = arr_PubProps.FirstOrDefault(m => m.Name.ToLower().Equals(pureName.ToLower()));
                    if (null == propty)
                    {
                        throw new Exception("参数化查询缺少参数！参数名：" + item);
                    }

                    //根据type 获取sqlserver对应的字段类型
                    string sqlDef = DbTypeAndCLRType.GetSqlServerDbDefine(propty.PropertyType);
                    object value = ReflectionHelper.FastGetValue(propty, paraObj);



                    //日期 guid  字符串进行 括号包裹
                    if (value is string || value is DateTime || value is char || value is Guid)
                    {
                        if (value is DateTime)
                        {
                            //将时间进行统一 yyyy-MM-dd HH:mm:ss
                            value = string.Format("'{0}'", ((DateTime)value).ToOfenTimeString());
                        }
                        value = string.Format("'{0}'", value.ToString());
                    }
                    else if (value is bool)
                    {
                        //bool 特殊处理
                        if ((bool)value == true)
                        {
                            value = 1;
                        }
                        else
                        {
                            value = 0;
                        }
                    }

                    if (sqlDef.IsNotEmpty())
                    {
                        if (counter == 0)
                        {
                            sb_Para.Append(item).Append(" ").Append(sqlDef);//如：@a int

                            sb_KeyValue.Append(item).Append(" = ").Append(value);
                        }
                        else
                        {
                            sb_Para.Append(",").Append(item).Append(" ").Append(sqlDef);//如：,@b nvarchar

                            sb_KeyValue.Append(",").Append(item).Append(" = ").Append(value);
                        }

                    }

                    counter += 1;
                }
              
          

            }
            catch (Exception ex)
            {
                throw ex;
            }

            paraKeyValueString = sb_KeyValue.ToString();
            
            return sb_Para.ToString();
        }

        #region Disposable


        //是否回收完毕
        bool _disposed;
        public void Dispose()
        {

            Dispose(true);
            // This class has no unmanaged resources but it is possible that somebody could add some in a subclass.
            GC.SuppressFinalize(this);

        }
        //这里的参数表示示是否需要释放那些实现IDisposable接口的托管对象
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return; //如果已经被回收，就中断执行
            if (disposing)
            {
                //TODO:释放那些实现IDisposable接口的托管对象

            }
            //TODO:释放非托管资源，设置对象为null
            _disposed = true;
        }


        #endregion


        #endregion

    }
}
