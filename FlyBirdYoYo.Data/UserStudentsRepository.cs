using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using Dapper;
using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.DomainEntity;
using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.Utilities.Interface;

namespace FlyBirdYoYo.Data.Repository
{
    public class UserStudentsRepository : BaseRepository<UserStudentsModel>, IDbContext<UserStudentsModel>, IRepository
    {
        public UserStudentsRepository()//:base("Db_SqlServer")
        {
            /*示范代码：
             在写这一层的时候，自己写的sql 请把sql  执行日志 参数输出
               conn.SqlOutPutToLogAsync(sql, model);
             */
            //1 切换数据库上下文
            //this.dbContext = GetDbContext("Db_SqlServer");

            //2 dapper orm
            //var model = new StudentsModel();
            //this.Insert(model, transaction);

            // 3 ado.net
            // this.ExecuteNonQuery()

            //4 dapper 
            //using (var conn = this.GetDbConnection())
            //{
            //    conn.ExecuteScalar<int>(sql, paras);
            //} 



        }

        #region 自定义数据交互方法


        /// <summary>
        /// 分页查询年龄大于某个值的学生信息
        /// 支持参数化查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedSqlDataResult<StudentDto> GetStudentssByPagerAndCondition(string name, int pageIndex, int pageSize)
        {
            //防止sql 注入
            //condition.CompanyCode = condition.CompanyCode.ToSafeSqlString();
            //condition.ShopCode = condition.ShopCode.ToSafeSqlString();


            string sqlDataSet = $@"SELECT a.id,a.Name,a.age,b.class_name as ClassName 
from user_students a 
inner join school_class b on a.age=b.age where a.Name like @Name and sex=@Sex";

            try
            {

                var pagerCond = new PagedSqlCondition
                {
                    PageNumber = pageIndex,
                    PageSize = pageSize,
                    TableNameOrSqlCmd = sqlDataSet,
                    SqlParameters=new { Name= name, Sex=0},//参数化查询参数设置
                    TableOptions = PageTableOptions.SqlScripts,
                    SortField = "id",
                    IsDesc = false
                };

                var data = this.PageQuery<StudentDto>(pagerCond);

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        public IEnumerable<StudentDto> GetListBySql(StudentDto model)
        {
            using (var conn = this.GetCurrentDbConnection())
            {
                string sql = @"SELECT a.id,a.`Name`,a.Age,b.ClassName from students a 
                                    inner join schoolClass b on a.Age = b.Age where 1=1 ";
                if(model.ClassName!=null)
                {
                    sql += " and b.ClassName=@ClassName";
                }
                if (model.Name != null)
                {
                    sql += " and a.Name=@Name";
                }

                //异步输出日志
                conn.SqlOutPutToLogAsync(sql, model);


                return conn.Query<StudentDto>(sql, model);
            }


        }



        #endregion

    }
}