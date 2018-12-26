using FlyBirdYoYo.Utilities.TypeFinder;
using System;
using System.Data.Common;

namespace FlyBirdYoYo.DbManage.Utilities
{
    public static class SqlDataReaderExtensions
    {
        /// <summary>
        /// 将IDataReader中的列 转化成指定的实体
        /// </summary>
        /// <typeparam name="TElemet"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static TElement ConvertDataReaderToEntity<TElement>(this DbDataReader dr)
            where TElement : BaseEntity, new()
        {


            if (dr.FieldCount <= 0)
            {
                return default(TElement);
            }

            var model = new TElement();
            //1 获取实体中所有的属性Property  
            var propertys = model.ResolveEntity(true).Propertys;
          
            //2  判断属性类型 转化成对应 的数据类型  赋值
            foreach (var p in propertys)
            {

                if (dr[p.Name] != null && dr[p.Name].ToString() != "")
                {
                    var filedValue = dr[p.Name];
                    ReflectionHelper.SetPropertyValue(model, p, filedValue);
                }
                else
                {
                    ReflectionHelper.SetPropertyValue(model, p, null);
                }

            }


            //3 返回赋值的实体对象
            return model;
        }
    }
}
