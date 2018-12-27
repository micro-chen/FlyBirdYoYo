using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using FlyBirdYoYo.DbManage.CommandTree;
using System.Data.Common;

namespace FlyBirdYoYo.DbManage
{
    public interface IDbContext<TElement>
        where TElement : BaseEntity, new()
    {



        #region  Insert操作
        /// <summary>
        /// 插入 实体-并返回插入的主键
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        long Insert(TElement entity, IDbTransaction transaction = null);


        /// <summary>
        /// 基于事务安全的插入实体集合
        /// </summary>
        /// <param name="entities"></param>
        /// <returns>返回操作结果</returns>
        bool InsertMulitiEntities(IEnumerable<TElement> entities, IDbTransaction transaction = null);

        #endregion


        #region Update 更新操作
        /// <summary>
        /// 更新单个模型
        /// （更新机制为，模型载体设置的值的字段会被更新掉，不设置值 不更新）
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(TElement entity, IDbTransaction transaction = null);

        /// <summary>
        /// 更新元素 通过  符合条件的
        /// （更新机制为，模型载体设置的值的字段会被更新掉，不设置值 不更新）
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int UpdateByCondition(TElement entity, Expression<Func<TElement, bool>> predicate, IDbTransaction transaction = null);

        #endregion


        #region Select   查询操作

        /// <summary>
        ///通过主键获取单个元素
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        TElement GetElementById(long id);


        /// <summary>
        ///通过特定的条件查询出元素
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TElement GetFirstOrDefaultByCondition(Expression<Func<TElement, bool>> predicate);

        /// <summary>
        ///通过特定的条件查询出元素集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<TElement> GetElementsByCondition(Expression<Func<TElement, bool>> predicate);

         /// <summary>
         /// 基于分页查询：支持单表和多表的联接查询分页
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <param name="condition"></param>
         /// <returns></returns>
         PagedSqlDataResult<T> PageQuery<T>(PagedSqlCondition condition) where T:class;

        #endregion


        #region Delete 删除操作

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        int Delete(TElement entity, IDbTransaction transaction = null);

        /// <summary>
        /// 删除符合条件的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int DeleteByCondition(Expression<Func<TElement, bool>> predicate, IDbTransaction transaction = null);



        #endregion


        #region 聚合函数操作

        /// <summary>
        ///使用指定的条件 汇总列
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="specialColumn"></param>
        /// <returns></returns>
        decimal Sum(Expression<Func<TElement, bool>> predicate, Fields<TElement> specialColumn);

        /// <summary>
        ///【读】 统计 符合条件的行数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Count(Expression<Func<TElement, bool>> predicate);


        /// <summary>
        ///符合条件的行的 指定列的最大值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        decimal Max(Expression<Func<TElement, bool>> predicate, Fields<TElement> specialColumn);

        /// <summary>
        ///符合条件的行的 指定列的最小值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="specialColumn"></param>
        /// <returns></returns>
        decimal Min(Expression<Func<TElement, bool>> predicate, Fields<TElement> specialColumn);
        #endregion


        #region base ado.net 

        /// <summary>
        /// 获取此上下文的新的db连接对象
        /// </summary>
        /// <returns></returns>
        IDbConnection GetCurrentDbConnection();


        /// <summary>
        ///执行 CommandText 针对 Connection, ，并返回 DbDataReader。
        /// </summary>
        /// <returns></returns>
        DbDataReader ExecuteReader(string cmdText, DbParameter[] commandParameters, CommandType cmdType = CommandType.Text);

        /// <summary>
        ///执行查询 返回 DataSet
        /// </summary>
        /// <returns></returns>
        DataSet ExecuteDataSet(string cmdText, DbParameter[] commandParameters, CommandType cmdType = CommandType.Text);
        /// <summary>
        /// 执行查询，并返回由查询返回的结果集中的第一行的第一列。 其他列或行将被忽略。
        /// </summary>
        /// <returns></returns>
        object ExecuteScalar(string cmdText, DbParameter[] commandParameters, CommandType cmdType = CommandType.Text);

        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string cmdText, DbParameter[] commandParameters, CommandType cmdType = CommandType.Text);

        /// <summary>
        /// 执行查询并返回 集合对象
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<TElement> SqlQuery(string commandText, CommandType commandType, params DbParameter[] parameters);

        #endregion

        #region Sql 语句批量执行
        bool SqlBatchExcute(Dictionary<string, DbParameter[]> SqlCmdList);

        #endregion

    }
}
