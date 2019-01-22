using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using TableAttribute = System.ComponentModel.DataAnnotations.Schema.TableAttribute;

using Dapper.Contrib.Extensions;
using FlyBirdYoYo.Utilities.Caching;
using FlyBirdYoYo.DbManage.Mapping;

namespace FlyBirdYoYo.DbManage
{

    /// <summary>
    /// 属性变更事件参数
    /// </summary>
    public class EntityPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public EntityPropertyChangedEventArgs(string propertyName, object value) : base(propertyName)
        {
            this.Value = value;
        }

        /// <summary>
        /// 属性值
        /// </summary>
        public object Value { get; set; }
    }

    /// <summary>
    /// 标识列主键类
    /// </summary>
    public class EntityIdentity
    {
        /// <summary>
        /// 主键名
        /// </summary>
        public string IdentityKeyName { get; set; }

        /// <summary>
        /// 主键值
        /// </summary>
        public object IdentityValue { get; set; }
    }

    /// <summary>
    /// 主键属性
    /// </summary>
    public class PrimaryKeyAttribute : Attribute
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// Base class for entities
    /// </summary>
    [Serializable]
    public abstract class BaseEntity : IEntityObject, INotifyPropertyChanged
    {



        ///// <summary>
        ///// 实体的主键名称
        ///// </summary>
        //private  string Entity_Identity_FiledName = "Id";
        /// <summary>
        /// 创建实体的时候，一个注册表，用来表示已经设置过值的属性 字典
        /// </summary>

        internal Dictionary<string, object> __HAS_SET_VALUE_PROPERTY_DIC;

        internal EntityIdentity __ENTITY_IDENTITY;
        public BaseEntity()
        {
            __HAS_SET_VALUE_PROPERTY_DIC = new Dictionary<string, object>();
            this.PropertyChanged -= OnBaseEntity_PropertyChangedHandler;
            this.PropertyChanged += OnBaseEntity_PropertyChangedHandler;
        }



        #region 数据更新事件通知


        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChange(string propertyName, object value)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new EntityPropertyChangedEventArgs(propertyName, value));
            }
        }
        #endregion





        /// <summary>
        /// 属性变更的时候 触发事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBaseEntity_PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            var agrs = e as EntityPropertyChangedEventArgs;
            if (null != agrs)
            {
                this.AddOrChangeSettedValuePropertyFromDic(agrs.PropertyName, agrs.Value);
            }

        }


        #region 实现主键类型的访问控制


        /// <summary>
        /// 实体的唯一标识  ，如果是实体表  那么此字段必须是-有默认值的主键
        /// </summary>
        public object GetIdentityValue()
        {
            var entityIdentity = this.GetIdentity();
            var identityName = entityIdentity.IdentityKeyName;
            if (__HAS_SET_VALUE_PROPERTY_DIC.ContainsKey(identityName))
            {
                return __HAS_SET_VALUE_PROPERTY_DIC[identityName];
            }
            return null;
        }


        /// <summary>
        /// 获取主键
        /// </summary>
        /// <returns></returns>
        public EntityIdentity GetIdentity()
        {

            if (null == this.__ENTITY_IDENTITY)
            {
                var mapping = this.ResolveEntity(false);
                if (null != mapping)
                {
                    __ENTITY_IDENTITY = mapping.IdentityKey;
                }
            }
            return this.__ENTITY_IDENTITY;
        }

        #endregion


        #region 实现对设置过值的属性对象进行注册管理

        //1 添加注册
        //2 移除注册
        //3 获取注册表-属性值对

        /// <summary>
        /// 添加注册
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void AddOrChangeSettedValuePropertyFromDic(string propertyName, object value)
        {
            if (__HAS_SET_VALUE_PROPERTY_DIC.ContainsKey(propertyName))
            {
                __HAS_SET_VALUE_PROPERTY_DIC[propertyName] = value;
            }
            else
            {
                __HAS_SET_VALUE_PROPERTY_DIC.Add(propertyName, value);
            }
        }

        /// <summary>
        /// 移除注册
        /// </summary>
        /// <param name="propertyName"></param>
        public void RemoveSettedValuePropertyFromDic(string propertyName)
        {
            if (__HAS_SET_VALUE_PROPERTY_DIC.ContainsKey(propertyName))
            {
                __HAS_SET_VALUE_PROPERTY_DIC.Remove(propertyName);
            }
        }

        /// <summary>
        /// 获取注册表-属性值对
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetSettedValuePropertyDic()
        {
            return this.__HAS_SET_VALUE_PROPERTY_DIC;
        }

        #endregion


        /// <summary>
        ///当前实体的属性数组
        /// </summary>
        internal PropertyInfo[] GetCurrentEntityProperties()
        {
            PropertyInfo[] arrProps= null;

            var mapping = this.ResolveEntity(false);
            if (null!=mapping)
            {
                arrProps = mapping.Propertys;
            }
            return arrProps;

        }

        /// <summary>
        /// 获取此实体对应的sql 查询字段 带属性别名
        /// </summary>
        /// <param name="tableAilasName"></param>
        /// <returns></returns>
        public string GetSqlQueryFieldsWithAlias(string tableAilasName = "")
        {


            string tableName = this.ResolveTableName(this);
            //----------尝试从静态字典中获取结构-----------
            string cacheKey = string.Concat(tableName, ":", tableAilasName, ":", "queryFields");
            return SqlFieldMappingManager.GetSqlFieldNamesForQuery(cacheKey, () =>
            {
                var mapping = this.ResolveEntity(false);
                var fieldAlias = new List<string>();
                foreach (var item in mapping.Filelds)
                {

                    var ailasName = "";
                    if (!tableAilasName.IsNullOrEmpty())
                    {
                        ailasName = string.Format("{0}.`{1}` as `{2}`", tableAilasName, item.FieldColumnName, item.PropertyName);
                    }
                    else
                    {
                        ailasName = string.Format("`{0}` as `{1}`", item.FieldColumnName, item.PropertyName);
                    }
                    fieldAlias.Add(ailasName);
                }
                //string splitor = string.Format("{0},{0}", this.FieldWrapperChar);
                var fieldSplitString = string.Join(",", fieldAlias);

                if (!SqlFieldMappingManager.SqlFieldNamesForQuery.ContainsKey(cacheKey))
                {
                    SqlFieldMappingManager.SqlFieldNamesForQuery.TryAdd(cacheKey, fieldSplitString);
                }
                return fieldSplitString;
            });




        }

        /// <summary>
        /// 从本地缓存获取表名称
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal string ResolveTableName(BaseEntity entity)
        {
            string cacheKey = string.Concat("TableName:", entity.GetType().FullName);
            return NativeCacheManager.Current.Get<string>(cacheKey,int.MaxValue, () =>
            {
                var targetAttributes = entity.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                if (null == targetAttributes)
                {
                    throw new Exception("the model class has not mapping table!");
                }

                string tableName = (targetAttributes[0] as TableAttribute).Name;

                return tableName;
            });
        }
        /// <summary>
        /// 解析实体   解析其中的关联的表+字段+字段参数
        /// </summary>
        /// <param name="isWriteCmd">是否是写入命令生成sql参数</param>
        internal SqlFieldMapping ResolveEntity(bool isWriteCmd)
        {


            var entity = this;


            string tableName = this.ResolveTableName(this);
            //----------尝试从静态字典中获取结构-----------
            string cacheKey = string.Concat(tableName, ":", Convert.ToInt32(isWriteCmd));

            return SqlFieldMappingManager.GetEntityMapping(cacheKey, () =>
             {
                 SqlFieldMapping mapping = new SqlFieldMapping();
                 mapping.TableName = tableName;

                 #region 解析实体

                 
                 //解析主键
                 var targetPrimaryKeyAttribute = this.GetType().GetCustomAttribute<PrimaryKeyAttribute>();// this.GetType().GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                 if (targetPrimaryKeyAttribute!=null)
                 {
                     var identityKeyModel = new EntityIdentity();
                     string name = targetPrimaryKeyAttribute.Name;
                     identityKeyModel.IdentityKeyName = name;
                     mapping.IdentityKey = identityKeyModel;
                 }
             
                 //获取所有字段
                 var fullPropertys = this.GetType().GetProperties();
                 //有效的db属性
                 var lstDbUsedPropertys = new List<PropertyInfo>();

                 var lstFilelds = new List<DbField>();//[propertys.Length];
                 for (int i = 0; i < fullPropertys.Length; i++)
                 {
                     var item = fullPropertys[i];
                     //将有忽略db的字段 排除
                     if (item.GetCustomAttribute<IgnoreDbFieldAttribute>() != null)
                     {
                         continue;//忽略属性
                     }
                     if (isWriteCmd == true)
                     {
                         var writeAttr = item.GetCustomAttribute<WriteAttribute>();
                         if (null != writeAttr && writeAttr.Write == false)
                         {
                             continue;//如果是非写入参数，那么忽略此属性作为sql 参数
                         }
                     }
                     lstDbUsedPropertys.Add(item);

                     //解析属性上的标注 CloumnAttribute

                     string fieldName = item.Name;

                     var colAttr = item.GetCustomAttribute<ColumnAttribute>();
                     if (null != colAttr && !colAttr.Name.IsNullOrEmpty())
                     {
                         fieldName = colAttr.Name;//如果有自定义的ColumnAttribute 
                     }
                     lstFilelds.Add(new DbField { PropertyName = item.Name, FieldColumnName = fieldName });
                 }
                 //db 字段CLR 属性
                 mapping.Propertys = lstDbUsedPropertys.ToArray();
                 //字段
                 mapping.Filelds = lstFilelds;

                 //参数字段
                 mapping.SqlParas = mapping.Filelds.Select(x => x.PropertyName).ToArray();
                 for (int i = 0; i < mapping.SqlParas.Length; i++)
                 {
                     mapping.SqlParas[i] = string.Concat("@", mapping.SqlParas[i]);
                 }
                 #endregion

                 //保存到Mapping缓存

                 if (!SqlFieldMappingManager.Mappings.ContainsKey(cacheKey))
                 {
                     SqlFieldMappingManager.Mappings.TryAdd(cacheKey, mapping);
                 }

                 return mapping;

             });



        }


        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        private static bool IsTransient(BaseEntity obj)
        {
            return obj != null && Equals(obj.GetIdentityValue(), default(int));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(this.GetIdentityValue(), other.GetIdentityValue()))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(this.GetIdentityValue(), default(int)))
                return base.GetHashCode();
            return this.GetIdentityValue().GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }



        /// <summary>
        /// 实现实体的深度克隆（使用二进制序列化进行对象的序列化到流，然后再进行反序列化操作
        /// 对象必须是声明：Serializable
        /// ）
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            IFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, this);

            ms.Seek(0, SeekOrigin.Begin);

            var obj = bf.Deserialize(ms);

            ms.Flush();
            ms.Close();
            ms.Dispose();

            return obj;
        }
    }


}
