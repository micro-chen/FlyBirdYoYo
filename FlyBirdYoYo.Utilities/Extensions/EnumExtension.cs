using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using FlyBirdYoYo.Utilities.RegularExpressions;

namespace System
{
    public static class EnumExtension
    {


        /// <summary>
        /// 获取 枚举名:枚举值 格式的 字符串
        /// </summary>
        /// <param name="emObj"></param>
        /// <returns></returns>
        public static string GetEnumNameValueString(this Enum emObj)
        {
            var type = emObj.GetType();
            var typeName = type.Name;
            ///var valueNames = Enum.GetNames(type, emObj);
            var flagValues = Enum.GetValues(type)
                        .Cast<Enum>()
                        .Where(m => emObj.HasFlag(m));

            List<string> lst_Names = new List<string>();
            foreach (var itemEnumValue in flagValues)
            {
                var valueName = Enum.GetName(type, itemEnumValue);
                lst_Names.Add(valueName);
            }
            string names = string.Join(":", lst_Names.ToArray());
            return string.Concat(typeName, ":", names);
        }



        /// <summary>
        /// 获取枚举值 整数值的字符串
        /// </summary>
        /// <param name="emObj"></param>
        /// <returns></returns>
        public static string GetEnumValueString(this Enum emObj)
        {
            var type = emObj.GetType();
            var name = Enum.GetName(type, emObj);
            var value = Convert.ToInt32(Enum.Parse(type, name));

            return value.ToString();
        }
        /// <summary>
        /// 获取枚举值上的Description特性的说明
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">枚举值</param>
        /// <returns>特性的说明</returns>
        public static string GetEnumDescription(this Enum emObj)
        {
            var type = emObj.GetType();
            FieldInfo field = type.GetField(Enum.GetName(type, emObj));
            DescriptionAttribute descAttr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (descAttr == null)
            {
                return string.Empty;
            }

            return descAttr.Description;
        }

        /// <summary>
        /// 值转换为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this int val)
        {
        
            var enObj = Enum.Parse(typeof(T), val.ToString());
            return (T)enObj;
        }

        /// <summary>
        /// 值转换为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string val) 
        {
            object realValue = null;
            //判断值是否为整形
            if (InPutValidate.IsNumber(val))
            {
                realValue = val.ToInt();
            }
            else
            {
                realValue = val;
            }
            if (!Enum.IsDefined(typeof(T), realValue))
            {
                throw new Exception("非法的枚举值！");
            }

            var enObj = Enum.Parse(typeof(T), val);
            return (T)enObj;
        }

        /// <summary>
        /// 枚举转换成列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EnumberEntity> ConvertEnumToList<T>()
        {
            List<EnumberEntity> list = new List<EnumberEntity>();

            var type = typeof(T);
            var values = Enum.GetValues(type);
            if (values.IsNull()||values.Length<=0)
            {
                return list;
            }
            foreach (var e in values)
            {
                EnumberEntity m = new EnumberEntity();
                var field = e.GetType().GetField(e.ToString());

                object[] objArr_Grp = field.GetCustomAttributes(typeof(GroupAttribute), true);
                if (objArr_Grp != null && objArr_Grp.Length > 0)
                {
                    GroupAttribute grp = objArr_Grp[0] as GroupAttribute;
                    m.GroupName = grp.Name;
                    m.GroupOrder = grp.Order;
                }

                object[] objArr = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.Description = da.Description;
                }
               
                m.EnumValue = Convert.ToInt32(e);
                m.EnumName = e.ToString();
                list.Add(m);
            }
            return list;
        }

    }

    public class EnumberEntity
    {
        /// <summary>  
        /// 枚举的描述  
        /// </summary>  
        public string Description { set; get; }

        /// <summary>  
        /// 枚举名称  
        /// </summary>  
        public string EnumName { set; get; }

        /// <summary>  
        /// 枚举对象的值  
        /// </summary>  
        public int EnumValue { set; get; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 分组顺序
        /// </summary>
        public int GroupOrder { get; set; }
    }

}
