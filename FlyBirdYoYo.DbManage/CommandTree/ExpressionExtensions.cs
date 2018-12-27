
namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {

        #region  条件表达式 node 

        public static Expression AndAlso(this Expression left, Expression constant)
        {
            return Expression.AndAlso(left, constant);
        }
        public static Expression GreaterThan(this Expression left, Expression constant)
        {
            return Expression.GreaterThan(left, constant);
        }
        public static Expression GreaterThanOrEqual(this Expression left, Expression constant)
        {
            return Expression.GreaterThanOrEqual(left, constant);
        }
        public static Expression And(this Expression left, Expression constant)
        {
            return Expression.And(left, constant);
        }
        public static Expression Or(this Expression left, Expression constant)
        {
            return Expression.Or(left, constant);
        }


        public static Expression Equal(this Expression left, Expression constant)
        {
            return Expression.Equal(left, constant);
        }
        public static Expression NotEqual(this Expression left, Expression constant)
        {
            return Expression.NotEqual(left, constant);
        }
        public static Expression LessThan(this Expression left, Expression constant)
        {
            return Expression.LessThan(left, constant);
        }
        public static Expression LessThanOrEqual(this Expression left, Expression constant)
        {
            return Expression.LessThanOrEqual(left, constant);
        }
      
        public static Expression OrElse(this Expression left, Expression constant)
        {
            return Expression.OrElse(left, constant);
        }

        //是否是nul
        public static Expression IsNull(this Expression left, Expression constant)
        {
            return
                Expression.Call(typeof(object).GetMethod("Equals")).Equal(constant);
        }
        //是Like
        public static Expression Contain(this Expression left, Expression constant)
        {
            var method = typeof(LikeExtension).GetMethod("Like", new[] { typeof(string), typeof(string) });
            return
                Expression.Call(method, left, constant);
        }

        //否Like
        public static Expression NotContain(this Expression left, Expression constant)
        {
            var method = typeof(LikeExtension).GetMethod("NotLike", new[] { typeof(string), typeof(string) });
            return
              Expression.Call(method, left, constant);
        }

        //是In
        public static Expression In(this Expression left, Expression constant)
        {
            var method = typeof(LikeExtension).GetMethod("In");

            return
                Expression.Call(method,left,constant);
        }
        //Not In
        public static Expression NotIn(this Expression left, Expression constant)
        {
            var method = typeof(LikeExtension).GetMethod("NotIn");
            return
                    Expression.Call(method, left, constant);
        }

        //相加
        public static Expression Add(this Expression left, Expression constant)
        {
            return Expression.Add(left, constant);
        }
        //相加
        public static Expression AddChecked(this Expression left, Expression constant)
        {
            return Expression.AddChecked(left, constant);
        }
        //相减
        public static Expression Subtract(this Expression left, Expression constant)
        {
            return Expression.Subtract(left, constant);
        }
        //相减
        public static Expression SubtractChecked(this Expression left, Expression constant)
        {
            return Expression.SubtractChecked(left, constant);
        }
        //除以
        public static Expression Divide(this Expression left, Expression constant)
        {
            return Expression.Divide(left, constant);
        }
        //乘以
        public static Expression Multiply(this Expression left, Expression constant)
        {
            return Expression.Multiply(left, constant);
        }
        //乘以
        public static Expression MultiplyChecked(this Expression left, Expression constant)
        {
            return Expression.MultiplyChecked(left, constant);
        }
        #endregion

        public static Expression Call(this Expression instance, string methodName, params Expression[] arguments)
        {
            return Expression.Call(instance, instance.Type.GetMethod(methodName), arguments);
        }
        public static Expression Property(this Expression expression, string propertyName)
        {
            return Expression.Property(expression, propertyName);
        }

        public static Expression<TDelegate> ToLambda<TDelegate>(this Expression body, params ParameterExpression[] parameters)
        {
            return Expression.Lambda<TDelegate>(body, parameters);
        }

        ////***********下面是一个动态构建表达式树的例子*************************
        //        // Add the following using directive to your code file:
        //// using System.Linq.Expressions;

        //// Create an expression tree.
        //Expression<Func<int, bool>> exprTree = num => num < 5;

        //// Decompose the expression tree.
        //ParameterExpression param = (ParameterExpression)exprTree.Parameters[0];
        //BinaryExpression operation = (BinaryExpression)exprTree.Body;
        //ParameterExpression left = (ParameterExpression)operation.Left;
        //ConstantExpression right = (ConstantExpression)operation.Right;

        //Console.WriteLine("Decomposed expression: {0} => {1} {2} {3}",
        //                  param.Name, left.Name, operation.NodeType, right.Value);

        //// This code produces the following output:

        //// Decomposed expression: num => num LessThan 5   

       
    }
}
