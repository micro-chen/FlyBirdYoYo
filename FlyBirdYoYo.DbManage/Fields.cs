

using System;
using System.Collections.Generic;

using System.Linq.Expressions;


using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.DbManage.Utilities;

namespace FlyBirdYoYo.DbManage
{


    /// <summary>
    /// 字段 容器 ，可以使用Lambda表达式 进行字段的包含
    /// 解析表达式树  然后将树中包含的字段放置解析到容器中
    /// </summary>
    /// <typeparam name="TStructuralType"> The type to be configured. </typeparam>
    public class Fields<TStructuralType>
        where TStructuralType : BaseEntity
    {





        private  List<string> _Container_Fileds;

        /// <summary>
        /// 已经包含的字段容器
        /// </summary>
        internal  List<string> Container_Fileds
        {
            get
            {
                if (null == _Container_Fileds)
                {
                    _Container_Fileds = new List<string>();
                }

                return _Container_Fileds;
            }
        }

        public Fields()
        {

        }

        /// <summary>
        /// 创建一个字段容器实例
        /// </summary>
        /// <returns></returns>
        public static Fields<TStructuralType> Build()
        {
            return new Fields<TStructuralType>();
        }
        /// <summary>
        ///包含强类型 int  fload 等struct 等
        /// </summary>
        /// <typeparam name="T"> The type of the property being configured. </typeparam>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>

        public  Fields<TStructuralType> WithField<T>(
            Expression<Func<TStructuralType, T>> propertyExpression)
            where T : struct
        {
            // Decompose the expression tree.-----------解析表达式树
            // ParameterExpression param = (ParameterExpression)propertyExpression.Parameters[0];
            //BinaryExpression operation = (BinaryExpression)propertyExpression.Body;
            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            var memberExp = (MemberExpression)propertyExpression.Body;
            var filedName = memberExp.Member.Name;
            ///如果不包含此字段了 那么添加进入容器
            if (!Container_Fileds.Contains(filedName))
            {
                Container_Fileds.Add(filedName);
            }
            return this;
        }

        /// <summary>
        /// 包含struct可为空的数据类型
        /// </summary>
        /// <typeparam name="T"> The type of the property being configured. </typeparam>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        public Fields<TStructuralType> WithField<T>(
            Expression<Func<TStructuralType, T?>> propertyExpression)
            where T : struct
        {
            // Decompose the expression tree.-----------解析表达式树
            // ParameterExpression param = (ParameterExpression)propertyExpression.Parameters[0];
            //BinaryExpression operation = (BinaryExpression)propertyExpression.Body;
            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            var memberExp = (MemberExpression)propertyExpression.Body;
            var filedName = memberExp.Member.Name;
            ///如果不包含此字段了 那么添加进入容器
            if (!Container_Fileds.Contains(filedName))
            {
                Container_Fileds.Add(filedName);
            }
            return this;
        }

        /// <summary>
        ///包含字符串类型的字段
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        public Fields<TStructuralType> WithField(Expression<Func<TStructuralType, object>> propertyExpression)
        {
            // Decompose the expression tree.-----------解析表达式树
            // ParameterExpression param = (ParameterExpression)propertyExpression.Parameters[0];
            //var exType = propertyExpression.Body.NodeType;
            //BinaryExpression operation = (BinaryExpression)propertyExpression.Body;
            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            var memberExp = (MemberExpression)propertyExpression.Body;
            var filedName = memberExp.Member.Name;
            ///如果不包含此字段了 那么添加进入容器
            if (!Container_Fileds.Contains(filedName))
            {
                Container_Fileds.Add(filedName);
            }
            return this;
        }

        /// <summary>
        ///包含字符串类型的字段
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        public Fields<TStructuralType> WithField(Expression<Func<TStructuralType, string>> propertyExpression)
        {
            // Decompose the expression tree.-----------解析表达式树
            // ParameterExpression param = (ParameterExpression)propertyExpression.Parameters[0];
            //var exType = propertyExpression.Body.NodeType;
            //BinaryExpression operation = (BinaryExpression)propertyExpression.Body;
            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            var memberExp=(MemberExpression)propertyExpression.Body;
            var filedName = memberExp.Member.Name;
            ///如果不包含此字段了 那么添加进入容器
            if (!Container_Fileds.Contains(filedName))
            {
                Container_Fileds.Add(filedName);
            }
            return this;
        }

        /// <summary>
        /// 包含字节类型的字段
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        public Fields<TStructuralType> WithField(Expression<Func<TStructuralType, byte[]>> propertyExpression)
        {
            // Decompose the expression tree.-----------解析表达式树
            // ParameterExpression param = (ParameterExpression)propertyExpression.Parameters[0];
            //BinaryExpression operation = (BinaryExpression)propertyExpression.Body;
            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            var memberExp = (MemberExpression)propertyExpression.Body;
            var filedName = memberExp.Member.Name;
            ///如果不包含此字段了 那么添加进入容器
            if (!Container_Fileds.Contains(filedName))
            {
                Container_Fileds.Add(filedName);
            }
            return this;
        }

        /// <summary>
        /// 包含decimal类型的字段
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        public Fields<TStructuralType> WithField(Expression<Func<TStructuralType, decimal>> propertyExpression)
        {
            // Decompose the expression tree.-----------解析表达式树
            // ParameterExpression param = (ParameterExpression)propertyExpression.Parameters[0];
            //BinaryExpression operation = (BinaryExpression)propertyExpression.Body;
            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            var memberExp = (MemberExpression)propertyExpression.Body;
            var filedName = memberExp.Member.Name;
            ///如果不包含此字段了 那么添加进入容器
            ///如果不包含此字段了 那么添加进入容器
            if (!Container_Fileds.Contains(filedName))
            {
                Container_Fileds.Add(filedName);
            }
            return this;
        }

        /// <summary>
        /// 包含decimal?类型的字段
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        public Fields<TStructuralType> WithField(Expression<Func<TStructuralType, decimal?>> propertyExpression)
        {
            // Decompose the expression tree.-----------解析表达式树
            // ParameterExpression param = (ParameterExpression)propertyExpression.Parameters[0];
            //BinaryExpression operation = (BinaryExpression)propertyExpression.Body;
            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            var memberExp = (MemberExpression)propertyExpression.Body;
            var filedName = memberExp.Member.Name;
            ///如果不包含此字段了 那么添加进入容器
            if (!Container_Fileds.Contains(filedName))
            {
                Container_Fileds.Add(filedName);
            }
            return this;

        }
        /// <summary>
        ///包含DateTime类型的字段
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>

        public Fields<TStructuralType> WithField(Expression<Func<TStructuralType, DateTime>> propertyExpression)
        {
            // Decompose the expression tree.-----------解析表达式树
            // ParameterExpression param = (ParameterExpression)propertyExpression.Parameters[0];
            //BinaryExpression operation = (BinaryExpression)propertyExpression.Body;
            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            var memberExp = (MemberExpression)propertyExpression.Body;
            var filedName = memberExp.Member.Name;
            ///如果不包含此字段了 那么添加进入容器
            if (!Container_Fileds.Contains(filedName))
            {
                Container_Fileds.Add(filedName);
            }
            return this;
        }

        /// <summary>
        /// 包含DateTime?类型的字段
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        public Fields<TStructuralType> WithField(Expression<Func<TStructuralType, DateTime?>> propertyExpression)
        {
            // Decompose the expression tree.-----------解析表达式树
            // ParameterExpression param = (ParameterExpression)propertyExpression.Parameters[0];
            //BinaryExpression operation = (BinaryExpression)propertyExpression.Body;
            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            var memberExp = (MemberExpression)propertyExpression.Body;
            var filedName = memberExp.Member.Name;
            ///如果不包含此字段了 那么添加进入容器
            if (!Container_Fileds.Contains(filedName))
            {
                Container_Fileds.Add(filedName);
            }
            return this;
        }

        /// <summary>
        /// 包含DateTimeOffset类型的字段
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>

        public Fields<TStructuralType> WithField(
            Expression<Func<TStructuralType, DateTimeOffset>> propertyExpression)
        {
            // Decompose the expression tree.-----------解析表达式树
            // ParameterExpression param = (ParameterExpression)propertyExpression.Parameters[0];
            //BinaryExpression operation = (BinaryExpression)propertyExpression.Body;
            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            var memberExp = (MemberExpression)propertyExpression.Body;
            var filedName = memberExp.Member.Name;
            ///如果不包含此字段了 那么添加进入容器
            if (!Container_Fileds.Contains(filedName))
            {
                Container_Fileds.Add(filedName);
            }
            return this;
        }

        /// <summary>
        /// 包含DateTimeOffset?类型的字段
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>

        public Fields<TStructuralType> WithField(
            Expression<Func<TStructuralType, DateTimeOffset?>> propertyExpression)
        {
            // Decompose the expression tree.-----------解析表达式树
            // ParameterExpression param = (ParameterExpression)propertyExpression.Parameters[0];
            //BinaryExpression operation = (BinaryExpression)propertyExpression.Body;
            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            var memberExp = (MemberExpression)propertyExpression.Body;
            var filedName = memberExp.Member.Name;
            ///如果不包含此字段了 那么添加进入容器
            if (!Container_Fileds.Contains(filedName))
            {
                Container_Fileds.Add(filedName);
            }
            return this;
        }

        /// <summary>
        /// 包含TimeSpan类型的字段
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        public Fields<TStructuralType> WithField(Expression<Func<TStructuralType, TimeSpan>> propertyExpression)
        {
            // Decompose the expression tree.-----------解析表达式树
            // ParameterExpression param = (ParameterExpression)propertyExpression.Parameters[0];
            //BinaryExpression operation = (BinaryExpression)propertyExpression.Body;
            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            var memberExp = (MemberExpression)propertyExpression.Body;
            var filedName = memberExp.Member.Name;
            ///如果不包含此字段了 那么添加进入容器
            if (!Container_Fileds.Contains(filedName))
            {
                Container_Fileds.Add(filedName);
            }
            return this;
        }

        /// <summary>
        /// 包含TimeSpan?类型的字段
        /// </summary>
        /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t => t.MyProperty VB.Net: Function(t) t.MyProperty </param>
        /// <returns> A configuration object that can be used to configure the property. </returns>
        public Fields<TStructuralType> WithField(Expression<Func<TStructuralType, TimeSpan?>> propertyExpression)
        {
            // Decompose the expression tree.-----------解析表达式树
            // ParameterExpression param = (ParameterExpression)propertyExpression.Parameters[0];
            //BinaryExpression operation = (BinaryExpression)propertyExpression.Body;
            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            var memberExp = (MemberExpression)propertyExpression.Body;
            var filedName = memberExp.Member.Name;
            ///如果不包含此字段了 那么添加进入容器
            if (!Container_Fileds.Contains(filedName))
            {
                Container_Fileds.Add(filedName);
            }
            return this;
        }

    }
}
