
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FlyBirdYoYo.DbManage;
using System.Data.Common;
using System.Data;
using System.Linq.Expressions;
using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.Utilities.Interface;
using FlyBirdYoYo.Utilities.Ioc;

namespace FlyBirdYoYo.Data
{
    public class BaseRepository<TElement> where TElement : BaseEntity, new()
    {

        protected IDbContext<TElement> dbContext;


        /// <summary>
        /// 当前登录用户
        /// </summary>
        public ILoginAuthedUserDTO LoginUser
        {
            get
            {
                return ApplicationContext.Current.User;
            }
        }

        /// <summary>
        /// 构造数据仓储对象
        /// </summary>
        /// <param name="connName">连接字符串Name，默认为：Default；为空将返回默认第一个数据库连接</param>
        public BaseRepository(string connName = DatabaseFactory.Default_ConnName)
        {
            long tenantId = 0;
            //判断是否为单元测试项目发起的调用
            if (!typeof(IUnitTestBase).IsAssignableFrom(Contanst.CallingEntryPointBasetype))
            {
                var loginUserDto = this.LoginUser;
                if (null != loginUserDto)
                {
                    tenantId = loginUserDto.GroupId;
                }
            }
            
            ///------------根据租户id  对应的数据库code  找到对应的连接字符串--------
            this.dbContext = GetDbContext(connName);
        }

        /// <summary>
        /// 自定义数据库连接对象
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        protected IDbConnection GetDbConnection(string connName = DatabaseFactory.Default_ConnName)
        {
            return DatabaseFactory.GetDbConnection(connName);
        }
        /// <summary>
        /// 获取当前上下文中的数据库连接对象
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetCurrentDbConnection()
        {
            return this.dbContext.GetCurrentDbConnection();
        }


        /// <summary>
        /// /// <summary>
        /// 获取当前数据库上下文
        /// 根据写库的类型进行数据库类型的判断
        /// 支持多数据库类型-工厂拆分
        /// </summary>
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        protected  IDbContext<TElement> GetDbContext(string connName = DatabaseFactory.Default_ConnName)
        {
            IDbContext<TElement> dbContext = GetDbContext<TElement>(connName);

            return dbContext;
        }



        /// <summary>
        /// /// <summary>
        /// 获取当前数据库上下文
        /// 根据写库的类型进行数据库类型的判断
        /// 支持多数据库类型-工厂拆分
        /// </summary>
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static IDbContext<T> GetDbContext<T>(string connName = DatabaseFactory.Default_ConnName) where T: BaseEntity, new()
        {
            IDbContext<T> dbContext = null;

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

            switch (dbConfig.DbType)
            {
                case SupportDbType.SQLSERVER:
                    dbContext = new SqlDbContext<T>(dbConfig);
                    break;
                case SupportDbType.MYSQL:
                    dbContext = new MySqlDbContext<T>(dbConfig);
                    break;
                case SupportDbType.POSTGRESQL:
                case SupportDbType.ORACLE:
                default: throw new NotImplementedException();

            }

            return dbContext;
        }



        #region entity  orm 



        #region 查询实体


        /// <summary>
        /// id单个查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TElement GetElementById(long id)
        {
            return this.dbContext.GetElementById(id);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>

        public TElement GetFirstOrDefaultByCondition(Expression<Func<TElement, bool>> predicate)
        {
            return this.dbContext.GetFirstOrDefaultByCondition(predicate);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>

        public List<TElement> GetElementsByCondition(Expression<Func<TElement, bool>> predicate)
        {
            return this.dbContext.GetElementsByCondition(predicate);
        }
        

        /// <summary>
        /// 基于分页查询：支持单表和多表的联接查询分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        public PagedSqlDataResult<T> PageQuery<T>(PagedSqlCondition condition) where T : class
        {
            var pageData = this.dbContext.PageQuery<T>(condition);

            return pageData;
        }

        #endregion

        #region 插入实体

        /// <summary>
        /// 单个插入
        /// </summary>
        /// <param name="entity"></param>
        ///<param name="transaction"></param> 
        /// <returns></returns>
        public long Insert(TElement entity, IDbTransaction transaction = null)
        {
            return this.dbContext.Insert(entity, transaction);
        }
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities"></param>
        ///<param name="transaction"></param> 
        /// <returns></returns>
        public bool InsertMulitiEntities(IEnumerable<TElement> entities, IDbTransaction transaction = null)
        {
            return this.dbContext.InsertMulitiEntities(entities, transaction);
        }
        #endregion

        #region 删除实体

        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="entity"></param>
        ///<param name="transaction"></param> 
        /// <returns></returns>
        public int Delete(TElement entity, IDbTransaction transaction = null)
        {
            return this.dbContext.Delete(entity, transaction);
        }
        /// <summary>
        /// 条件删除
        /// </summary>
        /// <param name="predicate"></param>
        ///<param name="transaction"></param> 
        /// <returns></returns>
        public int DeleteByCondition(Expression<Func<TElement, bool>> predicate, IDbTransaction transaction = null)
        {
            return this.dbContext.DeleteByCondition(predicate, transaction);
        }

        #endregion

        #region 更新实体

        /// <summary>
        /// 单个更新
        /// </summary>
        /// <param name="entity"></param>
        ///<param name="transaction"></param> 
        /// <returns></returns>

        public int Update(TElement entity, IDbTransaction transaction = null)
        {
            return this.dbContext.Update(entity, transaction);
        }


        /// <summary>
        /// 条件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        ///<param name="transaction"></param> 
        /// <returns></returns>
        public int UpdateByCondition(TElement entity, Expression<Func<TElement, bool>> predicate, IDbTransaction transaction = null)
        {
            return this.dbContext.UpdateByCondition(entity, predicate, transaction);
        }

        #endregion

        #region 聚合函数

        /// <summary>
        /// 最大值-列
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="specialColumn"></param>
        /// <returns></returns>
        public decimal Max(Expression<Func<TElement, bool>> predicate, Fields<TElement> specialColumn)
        {
            return this.dbContext.Max(predicate, specialColumn);
        }

        /// <summary>
        /// 最小值-列
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="specialColumn"></param>
        /// <returns></returns>

        public decimal Min(Expression<Func<TElement, bool>> predicate, Fields<TElement> specialColumn)
        {
            return this.dbContext.Min(predicate, specialColumn);
        }

        /// <summary>
        /// 求和-列
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="specialColumn"></param>
        /// <returns></returns>

        public decimal Sum(Expression<Func<TElement, bool>> predicate, Fields<TElement> specialColumn)
        {
            return this.dbContext.Sum(predicate, specialColumn);
        }


        /// <summary>
        /// 统计计数-条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>

        public int Count(Expression<Func<TElement, bool>> predicate)
        {
            return this.dbContext.Count(predicate);
        }


        #endregion

        /// <summary>
        /// 批量执行SQL命令
        /// </summary>
        /// <param name="SqlCmdList"></param>
        /// <returns></returns>
        public bool SqlBatchExcute(Dictionary<string, DbParameter[]> SqlCmdList)
        {
            return this.dbContext.SqlBatchExcute(SqlCmdList);
        }



        #endregion

        #region base ado.net 

        /// <summary>
        /// 执行查询 返回 DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string cmdText, DbParameter[] commandParameters = null, CommandType cmdType = CommandType.Text)
        {
            return this.dbContext.ExecuteDataSet(cmdText, commandParameters, cmdType);
        }

        public DbDataReader ExecuteReader(string cmdText, DbParameter[] commandParameters = null, CommandType cmdType = CommandType.Text)
        {
            return this.dbContext.ExecuteReader(cmdText, commandParameters, cmdType);
        }

        public object ExecuteScalar(string cmdText, DbParameter[] commandParameters = null, CommandType cmdType = CommandType.Text)
        {
            return this.dbContext.ExecuteScalar(cmdText, commandParameters, cmdType);
        }

        public int ExecuteNonQuery(string cmdText, DbParameter[] commandParameters = null, CommandType cmdType = CommandType.Text)
        {
            return this.dbContext.ExecuteNonQuery(cmdText, commandParameters, cmdType);
        }

        public List<TElement> SqlQuery(string commandText, CommandType commandType, params DbParameter[] parameters)
        {
            return this.dbContext.SqlQuery(commandText, commandType, parameters);

        }

        #endregion
    }
}
