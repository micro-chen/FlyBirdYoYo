using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace FlyBirdYoYo.DbManage
{
    /// <summary>
    /// 分页sql结果对象
    /// </summary>
    public class PagedSqlDataResult<T>
    {

        public PagedSqlDataResult()
        {

        }

        public PagedSqlDataResult(List<T> dataList)
        {
            this.DataList = dataList;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRows { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 查询的结果集
        /// </summary>
        public List<T> DataList { get; set; }
    }

    /// <summary>
    /// 分页结果集类型
    /// </summary>
    public enum PageTableOptions {
        /// <summary>
        /// 表or视图类型
        /// </summary>
        TableOrView=0,
        /// <summary>
        /// 动态sql查询脚本
        /// </summary>
        SqlScripts=1
    }
    /// <summary>
    /// 执行分页查询sql的条件
    /// </summary>
    public class PagedSqlCondition
    {
        public PagedSqlCondition()
        {
            this.PrimaryKey = "Id";
            this.SelectFields = "*";
            this.ConditionWhere = "";
        }
        #region 分页查询需要设置的属性

       
        /// <summary>
        /// 页码--从1开始
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 主键：默认Id  
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 表名称/视图名称/sql查询语句
        /// </summary>
        public string TableNameOrSqlCmd { get; set; }

        /// <summary>
        /// 结果集类型。默认为NULL
        /// </summary>
        public PageTableOptions? TableOptions { get; set; }

        /////////// <summary>
        /////////// 参数化sql :执行的sql 查询参数
        /////////// 请传递一个包含参数属性Model实体对象
        /////////// </summary>
        ////////public object SqlParameters { get; set; }

        /// <summary>
        /// 子集字段，默认为：*
        /// </summary>
        public string SelectFields { get; set; }
        /// <summary>
        /// 子集的过滤条件，默认为：空
        /// </summary>
        public string ConditionWhere { get; set; }

        /// <summary>
        /// 排序字段--字段名逗号分隔字符串
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 是否倒序
        /// </summary>
        public bool IsDesc { get; set; }

       


        #endregion



        public bool IsValid(out string msg)
        {
            msg = "";
            bool result = false;

            if (this.PageNumber<0)
            {
                msg = "分页页索引为非有效值！";
                return result;
            }
            if (this.PageSize<=0)
            {
                msg = "分页页大小为非有效值！";
                return result;
            }
            if (string.IsNullOrEmpty(this.TableNameOrSqlCmd))
            {
                msg = "分页查询表名或者sql查询不能为空！";
                return result;
            }
            if (string.IsNullOrEmpty(this.SortField))
            {
                msg = "分页需要指定排序字段，无法对集合行进行位置区间定位！";
                return result;
            }


            return result=true;
        }

 

    }
}
