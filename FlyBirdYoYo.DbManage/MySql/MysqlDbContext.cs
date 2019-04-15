
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using System.Linq.Expressions;
using Dapper;
using MySql.Data.MySqlClient;

using FlyBirdYoYo.DbManage.Utilities;
using FlyBirdYoYo.DbManage.CommandTree;
using FlyBirdYoYo.Utilities.TypeFinder;
using FlyBirdYoYo.Utilities;

namespace FlyBirdYoYo.DbManage
{
    public class MySqlDbContext<TElement> : BaseSqlOperation<TElement>, IDbContext<TElement>, IDisposable
       where TElement : BaseEntity, new()
    {



        #region Construction and fields




        /// <summary>
        /// 实体的主键名称
        /// </summary>
        private string EntityIdentityFiledName = new TElement().GetIdentity().IdentityKeyName;

        /// <summary>
        /// 覆盖基类字段包裹
        /// </summary>
        public override string FieldWrapperChar { get; set; }


        public MySqlDbContext()
        {
            this.FieldWrapperChar = "`";
        }
        /// <summary>
        /// 数据上下文构造函数
        /// </summary>
        /// <param name="dbConfig"></param>
        public MySqlDbContext(DbConnConfig dbConfig) : this()
        {
            this.DbConfig = dbConfig;
        }


        #endregion



        #region 对提供映射字段配置的数据，进行ORM对象获取



        #region  Insert操作
        /// <summary>
        /// 插入 实体
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="transaction">db事务</param>
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

            string splitor = string.Format("{0},{0}", this.FieldWrapperChar);
            var fieldSplitString = string.Concat(this.FieldWrapperChar, string.Join(splitor, noIdentityFileds), this.FieldWrapperChar);//返回逗号分隔的字符串 例如：`ProvinceCode`,`ProvinceName`
            var parasSplitString = string.Join(",", noIdentityParas);//参数   数组 的逗号分隔


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
        /// 单次批量多次插入多个实体,并返回执行的记录数目---参数化的形式
        /// 第二种使用db事务  ，使用代码事务 using(var tran=new TranstionScope()){ your  code.......}
        /// </summary>
        /// <param name="entities">实体集合</param>
        public bool InsertMulitiEntities(IEnumerable<TElement> entities, IDbTransaction transaction = null)
        {

            string tableInDbName;
            System.Reflection.PropertyInfo[] propertys;
            string[] filelds;
            string[] paras;
            ResolveEntity(entities.First(), true, out tableInDbName, out propertys, out filelds, out paras);

            this.SqlOutPutToLogAsync("InsertMulitiEntities", entities);

            using (var bcp = new MysqlBuckCopy<TElement>(this.DbConfig.ConnString))
            {
                return bcp.WriteToServer(entities, tableInDbName, transaction);
            }


        }


        #endregion


        #region Update 更新操作

        /// <summary>
        /// 更新单个模型
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
            var sqlFieldMapping= ResolveEntity(entity, true, out tableInDbName, out propertys, out filelds, out paras);
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
                string where = ResolveLambdaTreeToCondition.ConvertLambdaToCondition<TElement>(predicate,   sqlFieldMapping,wrapperChar: this.FieldWrapperChar);
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
            var sqlFieldMapping=ResolveEntity(entity, false, out tableInDbName, out propertys, out filelds, out paras);
            if (filelds.Length <= 1)
            {
                //除主键后 没有其他字段
                return null;
                throw new Exception("未指定除主键后其他字段！");
            }
            //List<string> fieldAlias = new List<string>();
            //foreach (var item in sqlFieldMapping.Filelds)
            //{
            //    var ailasName = string.Format("`{0}` as `{1}`", item.FieldColumnName, item.PropertyName);
            //    fieldAlias.Add(ailasName);
            //}
            //string splitor = string.Format("{0},{0}", this.FieldWrapperChar);
            var fieldSplitString = "*";//entity.GetSqlQueryFieldsWithAlias();//string.Join(",", fieldAlias);

            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.AppendFormat("select  {0} ", fieldSplitString);
            sb_Sql.AppendFormat(" from {0} ", tableInDbName);
            sb_Sql.AppendFormat(" where {2}{0}{2}={1};", EntityIdentityFiledName, id, this.FieldWrapperChar);

            var sqlCmd = sb_Sql.ToString();

            sb_Sql.Clear();
            sb_Sql = null;

            this.SqlOutPutToLogAsync(sqlCmd, entity);

            try
            {
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
            //    var ailasName = string.Format("`{0}` as `{1}`", item.FieldColumnName, item.PropertyName);
            //    fieldAlias.Add(ailasName);
            //}
            var fieldSplitString = "*";// entity.GetSqlQueryFieldsWithAlias(); //string.Join(",", fieldAlias);
            //解析查询条件
            string whereStr = "1=1";
            if (null != predicate)
            {
                whereStr = ResolveLambdaTreeToCondition.ConvertLambdaToCondition<TElement>(predicate, sqlFieldMapping, wrapperChar: this.FieldWrapperChar);
            }



            StringBuilder sb_Sql = new StringBuilder();
            sb_Sql.AppendFormat("select  {0} ", fieldSplitString);
            sb_Sql.AppendFormat(" from {0} ", tableInDbName);
            sb_Sql.AppendFormat(" where {0} ", whereStr);
            sb_Sql.AppendFormat(" limit {0};", 1);


            var sqlCmd = sb_Sql.ToString();

            sb_Sql.Clear();
            sb_Sql = null;
            this.SqlOutPutToLogAsync(sqlCmd, entity);

            try
            {
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
            var sqlFieldMapping=ResolveEntity(entity, false, out tableInDbName, out propertys, out filelds, out paras);
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
            //    var ailasName = string.Format("`{0}` as `{1}`", item.FieldColumnName, item.PropertyName);
            //    fieldAlias.Add(ailasName);
            //}
            var fieldSplitString = "*";//entity.GetSqlQueryFieldsWithAlias(); //string.Join(",", fieldAlias);
            //解析查询条件
            string whereStr = "1=1";
            if (null != predicate)
            {
                whereStr = ResolveLambdaTreeToCondition.ConvertLambdaToCondition<TElement>(predicate,sqlFieldMapping, wrapperChar: this.FieldWrapperChar);
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

                if (condition.PageNumber<=0)
                {
                    condition.PageNumber = 1;
                }
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
                    string sqlCmd = condition.TableNameOrSqlCmd.Trim();
                    if (sqlCmd[sqlCmd.Length-1]==';')
                    {
                        sqlCmd = sqlCmd.TrimEnd(';');
                    }
                    condition.TableNameOrSqlCmd = string.Format(" ( {0} ) as  tmpTable ", condition.TableNameOrSqlCmd);
                }



                //# 分页sql 字符串

                string countSql = string.Format("select count(*) as TotalRows, 0 as TotalPages  from {0}  {1} ;", condition.TableNameOrSqlCmd, condition.ConditionWhere);

                string bodySql = string.Concat(
                    "select  "
                    , condition.SelectFields
                    , " from "
                    , condition.TableNameOrSqlCmd
                    , condition.ConditionWhere
                    , " order by "
                    , condition.SortField.IsNullOrEmpty() ? condition.PrimaryKey : condition.SortField
                    , condition.IsDesc ? " DESC " : " ASC " 
                    , "  limit  " 
                    , (condition.PageNumber-1)* condition.PageSize
                    , ','
                    , condition.PageSize
                );



               string pagerSql = string.Concat(countSql, bodySql,";");
                 



                //string 

                using (var conn = DatabaseFactory.GetDbConnection(this.DbConfig))
                {



                    //多部分结果
                    var multiResult = conn.QueryMultiple(pagerSql);//////, condition.SqlParameters);
                    if (null != multiResult)
                    {
                        pageData = multiResult.ReadFirstOrDefault<PagedSqlDataResult<T>>();//分页信息

                        if (pageData.TotalRows>0)
                        {
                            //计算分页总页数
                            if (pageData.TotalRows<condition.PageSize)
                            {
                                pageData.TotalPages = 1;
                            }
                            else
                            {
                                if (pageData.TotalRows%condition.PageSize>0)
                                {
                                    pageData.TotalPages = pageData.TotalRows / condition.PageSize + 1;
                                }
                                else
                                {
                                    pageData.TotalPages = pageData.TotalRows / condition.PageSize;
                                }
                            }
                        }


                        if (null != pageData)
                        {
                            pageData.DataList = multiResult.Read<T>().AsList();//结果行
                        }
                    }



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
            if (null==identityKey)
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
                    var result = conn.Execute(sqlCmd, null, transaction);
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
                whereStr = ResolveLambdaTreeToCondition.ConvertLambdaToCondition<TElement>(predicate, sqlFieldMapping, wrapperChar: this.FieldWrapperChar);
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
                    var result = conn.Execute(sqlCmd, null, transaction);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



        #endregion


        #endregion


        #region Other Methods


        #endregion



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



    }

}
