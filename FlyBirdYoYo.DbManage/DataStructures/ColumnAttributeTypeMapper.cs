using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace FlyBirdYoYo.DbManage.DataStructures
{
    /// <summary>
    /// 基于Dapper的属性标注的  自定义Mapper注册
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ColumnAttributeTypeMapper<T> : ColumnAttributeTypeMapper  
        where T :BaseEntity
    {
        public ColumnAttributeTypeMapper():base(typeof(T))
        {
        }
    }

    /// <summary>
    /// 列标注的Dapper自定义映射
    /// </summary>
    public class ColumnAttributeTypeMapper : FallBackTypeMapper
    {
        public ColumnAttributeTypeMapper(Type entityType)
            : base(new SqlMapper.ITypeMap[]
                    {
                        new CustomPropertyTypeMap(entityType,
                            (type, columnName) =>
                                type.GetProperties().FirstOrDefault(prop =>
                                    prop.GetCustomAttributes(false)
                                        .OfType<ColumnAttribute>()
                                        .Any(attribute => attribute.Name == columnName)||columnName==prop.Name
                            )
                        ),
                        new DefaultTypeMap(entityType)
                    })
        {
        }
    }
}
