using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlyBirdYoYo.DbManage
{
    /// <summary>
    /// 动态拼接Lambda表达式 构建容器
    /// 基于静态实体模型的泛型，进行属性与表达式的构建
    /// </summary>
    public static class PredicateBuilder
    {
        /*使用示范：
          //var predicate = PredicateBuilder.True<Student>();
        //predicate=predicate.And(s => s.ID > 1200);
        // predicate=predicate.Or(s => s.ID< 1000);
        // var result = db.Students.Where(predicate).ToList();  */




        ///////////// <summary>
        ///////////// 生成 true 筛选语句
        ///////////// 防止多条件中的判断  导致无条件 ；实现类似sql 拼接条件的where 1=1
        ///////////// </summary>
        ///////////// <typeparam name="T"></typeparam>
        ///////////// <returns></returns>
        //////////private static Expression<Func<T, bool>> True<T>() { return f => true; }
        //////////// public static Expression<Func<T, bool>> False<T>() { return f => false; }

        /// <summary>
        /// 创建新的条件表达式容器实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreatNew<T>()
        {
            var a=new Dictionary<int,int>();
            a[1] = 1;//用来生成1=1
            return f => a.Keys.First()== 1; ;
        }

        /// <summary>
        /// 将表达式合并
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="merge"></param>
        /// <returns></returns>
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
    }
}
