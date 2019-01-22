using FlyBirdYoYo.DbManage.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace FlyBirdYoYo.DbManage.CommandTree
{
    /// <summary>
    /// 将表达式树 解析为字符串格式的条件语句
    /// 可以实现  And  Or  Like NotLike In 等值判断
    /// 参考：EF的  QueryParameterExpression类
    /// </summary>
    public static class ResolveLambdaTreeToCondition
    {
        /// <summary>
        /// 解析Lambda 到Where条件表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="sqlFieldMapping"></param>
        /// <param name="wrapperChar">字段的包裹符号</param>
        /// <returns></returns>
        internal static string ConvertLambdaToCondition<T>(Expression<Func<T, bool>> func, SqlFieldMapping sqlFieldMapping, string wrapperChar = "") where T : BaseEntity
        {
            var whereStr = string.Empty;
            if (func.Body is BinaryExpression)
            {
                BinaryExpression be = ((BinaryExpression)func.Body);
                Expression originalLeft = be.Left;
                Expression originalRight = be.Right;
                Expression afterProcessLeft = ProcessNoneValueBoolenExpression<T>(originalLeft);
                Expression afterProcessRight = ProcessNoneValueBoolenExpression<T>(originalRight);
                whereStr = BinarExpressionProvider(afterProcessLeft, afterProcessRight, be.NodeType, sqlFieldMapping, wrapperChar);

                //whereStr = BinarExpressionProvider(be.Left, be.Right, be.NodeType);
            }
            if (func.Body is MethodCallExpression)
            {
                MethodCallExpression mce = ((MethodCallExpression)func.Body);

                string methodCondition = ExpressionRouter(mce, sqlFieldMapping);
                if (!string.IsNullOrEmpty(methodCondition))
                {
                    if (whereStr != string.Empty)
                    {
                        whereStr += " and ";
                        whereStr += methodCondition;
                    }
                    else
                    {
                        whereStr = methodCondition;
                    }
                }
            }

            return whereStr;
        }

        /// <summary>
        /// 处理没有值的bool 表达式 x.isDelete==true  写成  x.isDelete
        /// </summary>
        /// <param name="exp"></param>
        /// 
        /// <returns></returns>
        private static Expression ProcessNoneValueBoolenExpression<T>(Expression exp)
        {
            Expression result = null;

            if (exp.Type == typeof(bool) && (exp is MemberExpression) && !exp.ToString().Contains("=="))
            {
                //没有包含值 那么默认是True
                ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
                var propertyName = ((MemberExpression)exp).Member.Name;
                //左侧部分
                Expression left = Expression.Property(parameter, propertyName);

                //值
                ConstantExpression compareValue = Expression.Constant(true);

                //右侧部分
                Expression afterRight = null;
                afterRight = Expression.Equal(left, compareValue);

                result = afterRight;
            }
            if (null == result)
            {
                //返回默认的原始表达式
                return exp;
            }
            return result;
        }
        public static bool In<T>(this T obj, T[] array) where T : BaseEntity
        {
            return true;
        }
        public static bool NotIn<T>(this T obj, T[] array) where T : BaseEntity
        {
            return true;
        }
        public static bool Like(this string str, string likeStr)
        {
            return true;
        }
        public static bool NotLike(this string str, string likeStr)
        {
            return true;
        }


        static string BinarExpressionProvider(Expression left, Expression right, ExpressionType type, SqlFieldMapping sqlFieldMapping, string wrapperChar = "")
        {
            string sb = "(";
            //先处理左边
            sb += ExpressionRouter(left, sqlFieldMapping, wrapperChar);//左边的字段 传递包裹符号

            sb += ExpressionTypeCast(type);

            //再处理右边
            string tmpStr = ExpressionRouter(right, sqlFieldMapping, wrapperChar);//左边的字段 传递包裹符号
            if (tmpStr == "null")
            {
                if (sb.EndsWith(" ="))
                    sb = sb.Substring(0, sb.Length - 2) + " is null";
                else if (sb.EndsWith("<>"))
                    sb = sb.Substring(0, sb.Length - 2) + " is not null";
            }
            else
                sb += tmpStr;
            return sb += ")";
        }

        /// <summary>
        /// 从mapping 中寻找指定的属性名  如果找不到 返回属性名
        /// </summary>
        /// <param name="sqlFieldMapping"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        internal static string SearchPropertyMappingField(SqlFieldMapping sqlFieldMapping, string propName)
        {
            if (null == sqlFieldMapping)
            {
                return propName;
            }

            var field = sqlFieldMapping.Filelds.FirstOrDefault(x => x.PropertyName == propName);
            if (null == field)
            {
                return propName;
            }

            return field.FieldColumnName;
        }
        static string ExpressionRouter(Expression exp, SqlFieldMapping sqlFieldMapping, string wrapperChar = "")
        {
            string sb = string.Empty;
            if (exp is BinaryExpression)
            {
                BinaryExpression be = ((BinaryExpression)exp);
                return BinarExpressionProvider(be.Left, be.Right, be.NodeType, sqlFieldMapping, wrapperChar);
            }
            else if (exp is MemberExpression)
            {


                if (!exp.ToString().StartsWith("value("))
                {
                    MemberExpression me = ((MemberExpression)exp);
                    string filedName = SearchPropertyMappingField(sqlFieldMapping, me.Member.Name);
                    return string.Concat(wrapperChar, filedName, wrapperChar);
                }
                else
                {


                    ConstantExpression conExpression = null;
                    TryEvaluatePath(exp, out conExpression);
                    var result = conExpression.Value;

                    if (result == null)
                        return "null";
					if (result is ValueType && !(result is Guid) && !(result is DateTime))//guid 虽然是值类型的 --长整型，datetime 是struct  也是继承值类型 但是在SQL操作的时候 是按照字符串处理的
					{
						if (result is Enum)
						{
							return ((int)result).ToString();//枚举类型的  要转换为具体的int  值
						}
						return result.ToString();

					}
					else if (result is string || result is DateTime || result is char || result is Guid)
					{
						if (result is DateTime)
						{
							//将时间进行统一 yyyy-MM-dd HH:mm:ss
							return string.Format("'{0}'", ((DateTime)result).ToOfenTimeString());
						}
						return string.Format("'{0}'", result.ToString());
					} 

                }


            }
            else if (exp is NewArrayExpression)
            {
                NewArrayExpression ae = ((NewArrayExpression)exp);
                StringBuilder tmpstr = new StringBuilder();
                foreach (Expression ex in ae.Expressions)
                {
                    tmpstr.Append(ExpressionRouter(ex, null));
                    tmpstr.Append(",");
                }
                return tmpstr.ToString(0, tmpstr.Length - 1);
            }
            else if (exp is MethodCallExpression)
            {
                MethodCallExpression mce = (MethodCallExpression)exp;
                if (mce.Method.Name == "Like")
                    return string.Format("({2}{0}{2} like {1})", ExpressionRouter(mce.Arguments[0], sqlFieldMapping), ExpressionRouter(mce.Arguments[1], null), wrapperChar);

                else if (mce.Method.Name == "Contains")
                    return string.Format("({2}{0}{2}  like {1})", ExpressionRouter(mce.Object, sqlFieldMapping), ExpressionRouter(mce.Arguments[0], null), wrapperChar);

                else if (mce.Method.Name == "NotLike")
                    return string.Format("({2}{0}{2}  Not like {1})", ExpressionRouter(mce.Arguments[0], sqlFieldMapping), ExpressionRouter(mce.Arguments[1], null), wrapperChar);
                else if (mce.Method.Name == "In")
                    return string.Format("{{2}{0}{2}  In ({1})", ExpressionRouter(mce.Arguments[0], sqlFieldMapping), ExpressionRouter(mce.Arguments[1], null), wrapperChar);
                else if (mce.Method.Name == "NotIn")
                    return string.Format("{2}{0}{2}  Not In ({1})", ExpressionRouter(mce.Arguments[0], sqlFieldMapping), ExpressionRouter(mce.Arguments[1], null), wrapperChar);
                else if (mce.Method.Name == "LenFuncInSql")
                    return string.Format("len({1}{0}{1})", ExpressionRouter(mce.Arguments[0], sqlFieldMapping), wrapperChar);
                else
                {
                    //实例方法或者静态方法的调用解析
                    if (null != mce.Object)
                    {
						//如果不是自定义的内置sql函数方法 活着扩展， 那么只取属性;例如 x.Id.ToString() .那么仅仅返回id属性即可

						var mceValue = ExpressionRouter(mce.Object, sqlFieldMapping);
						if (null != mceValue)
						{
							return mceValue; //string.Format("{1}{0}{1}", mceValue, wrapperChar);
						}
						else
						{
							//解析不是基础类型 时候 尝试执行表达式
							object result = Expression.Lambda(mce).Compile().DynamicInvoke();
							ConstantExpression consExpress = Expression.Constant(result);
							return ExpressionRouter(consExpress, sqlFieldMapping);
						}

						
                    }
                    else
                    {
                        //如果是System 处理函数 调用 那么执行函数调用的结果

                        object result = Expression.Lambda(mce).Compile().DynamicInvoke();
                        ConstantExpression consExpress = Expression.Constant(result);
                        return ExpressionRouter(consExpress, sqlFieldMapping);

                    }

                }
            }
            else if (exp is ConstantExpression)
            {
                ConstantExpression ce = ((ConstantExpression)exp);
                if (ce.Value == null)
                    return "null";
                else if (ce.Value is ValueType)
                {
                    if (ce.Value is Guid  || ce.Value is DateTime)//处理guid值
                    {
                        return string.Format("'{0}'", ce.Value.ToString());
                    }

                    return ce.Value.ToString();//处理bool值 ce.Value is bool
                }
                else if (ce.Value is string || ce.Value is DateTime || ce.Value is char)
                    return string.Format("'{0}'", ce.Value.ToString());
            }
            else if (exp is UnaryExpression)
            {
                UnaryExpression ue = ((UnaryExpression)exp);
                return ExpressionRouter(ue.Operand, sqlFieldMapping);
            }
            return null;
        }

        static string ExpressionTypeCast(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.Equal:
                    return " =";
                case ExpressionType.GreaterThan:
                    return " >";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return " Or ";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                default:
                    return null;
            }
        }

        /// <summary>
        /// 查询成员表达式  转化为根具有值的表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="constantExpression"></param>
        /// <returns></returns>
        private static bool TryEvaluatePath(Expression expression, out ConstantExpression constantExpression)
        {
            var me = expression as MemberExpression;
            constantExpression = null;
            if (me != null)
            {
                ///初始化一个堆栈 遍历成员中的表达式 到最后一个节点
                var stack = new Stack<MemberExpression>();
                stack.Push(me);
                while ((me = me.Expression as MemberExpression) != null)
                {
                    stack.Push(me);
                }
                me = stack.Pop();//找到最根的表达式  
                var ce = me.Expression as ConstantExpression;
                if (ce != null)
                {
                    object memberVal;
                    if (!TryGetFieldOrPropertyValue(me, ((ConstantExpression)me.Expression).Value, out memberVal))
                    {
                        return false;
                    }
                    if (stack.Count > 0)
                    {
                        foreach (var rec in stack)
                        {
                            if (!TryGetFieldOrPropertyValue(rec, memberVal, out memberVal))
                            {
                                return false;
                            }
                        }
                    }

                    //构建值表达式
                    constantExpression = Expression.Constant(memberVal, expression.Type);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 从字段或者属性中 获取值
        /// </summary>
        /// <param name="me"></param>
        /// <param name="instance"></param>
        /// <param name="memberValue"></param>
        /// <returns></returns>
        private static bool TryGetFieldOrPropertyValue(MemberExpression me, object instance, out object memberValue)
        {
            var result = false;
            memberValue = null;

            try
            {
                if (me.Member.MemberType
                    == MemberTypes.Field)
                {
                    var fieldInfo = ((FieldInfo)me.Member);

                    memberValue = fieldInfo.GetValue(instance);
                   

                    result = true;
                }
                else if (me.Member.MemberType
                         == MemberTypes.Property)
                {
                    memberValue = ((PropertyInfo)me.Member).GetValue(instance, null);
                    result = true;
                }
                return result;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

    }


}
