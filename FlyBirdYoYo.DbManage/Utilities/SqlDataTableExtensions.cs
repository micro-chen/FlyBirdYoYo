using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

using FlyBirdYoYo.Utilities.TypeFinder;
using Dapper.Contrib.Extensions;

namespace FlyBirdYoYo.DbManage.Utilities
{
    public static class SqlDataTableExtensions
    {

        /// <summary>    
        /// 将泛型集合类转换成DataTable    
        /// </summary>    
        /// <typeparam name="T">集合项类型</typeparam>    
        /// <param name="list">集合</param>    
        /// <param name="propertys">列名 属性集合</param>    
        /// <returns>数据集(表)</returns>    
        public static DataTable ConvertListToDataTable<T>(IEnumerable<T> list, ref PropertyInfo[] propertys) where T:BaseEntity
        {

            DataTable result = new DataTable();
            var totalCount = list.Count();
            if (totalCount > 0)
            {
                //如果未传递属性 那么属性Property  反射出来
                if (null == propertys || propertys.Length <= 0)
                {
                    //propertys = list.ElementAt(0).GetType().GetProperties();
                    var mapping = list.ElementAt(0).ResolveEntity(true);
                    propertys = mapping.Propertys;
                }


                foreach (PropertyInfo pi in propertys)
                {

                    string fieldName = pi.Name;

                    var colAttr = pi.GetCustomAttribute<ColumnAttribute>();
                    if (null != colAttr && !colAttr.Name.IsNullOrEmpty())
                    {
                        fieldName = colAttr.Name;//如果有自定义的ColumnAttribute 
                    }


                    if (pi.PropertyType.IsGenericType
                        && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        //泛型 Nullable 属性
                        var realType = pi.PropertyType.GetGenericArguments()[0];
                        
                        result.Columns.Add(fieldName, realType);
                    }
                    else
                    {
                        //普通属性
                        result.Columns.Add(fieldName, pi.PropertyType);
                    }


                }

                for (int i = 0; i < totalCount; i++)
                {
                    var model = list.ElementAt(i);
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {

                        object obj = ReflectionHelper.GetPropertyValue(model, pi);
                        tempList.Add(obj);

                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }


    }


}
