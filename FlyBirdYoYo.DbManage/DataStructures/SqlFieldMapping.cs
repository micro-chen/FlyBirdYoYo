using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Reflection;
 
namespace FlyBirdYoYo.DbManage
{
    /// <summary>
    /// 管理本地sql 字段属性的映射
    /// </summary>
    internal class SqlFieldMappingManager
    {
        static SqlFieldMappingManager()
        {
            Mappings = new ConcurrentDictionary<string, SqlFieldMapping>();
            SqlFieldNamesForQuery = new ConcurrentDictionary<string, string>();
        }

        /// <summary>
        /// 实体字段映射集合
        /// </summary>
        public static ConcurrentDictionary<string, SqlFieldMapping> Mappings;
        /// <summary>
        /// 实体的查询字段
        /// </summary>
        public static ConcurrentDictionary<string, string> SqlFieldNamesForQuery;


        /// <summary>
        /// 从本地变量缓存读取实体的SqlFieldMapping
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="acquire"></param>
        /// <returns></returns>
        public static SqlFieldMapping GetEntityMapping(string cacheKey, Func<SqlFieldMapping> acquire)
        {
            SqlFieldMapping cacheValue = null;
            if (Mappings.ContainsKey(cacheKey))
            {
                cacheValue = Mappings[cacheKey];
                return cacheValue;
            }

            //不存在的话 ，那么执行委托并插入
            cacheValue = acquire();
            if (null!=cacheValue)
            {
                Mappings.TryAdd(cacheKey, cacheValue);
            }

            return cacheValue;
        }

        /// <summary>
        /// 从本地变量缓存读取实体的字段查询names
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="acquire"></param>
        /// <returns></returns>
        public static string GetSqlFieldNamesForQuery(string cacheKey,Func<string> acquire)
        {
            string cacheValue = string.Empty;
            if (SqlFieldNamesForQuery.ContainsKey(cacheKey))
            {
                cacheValue = SqlFieldNamesForQuery[cacheKey];
                return cacheValue;
            }

            //不存在的话 ，那么执行委托并插入
            cacheValue = acquire();
            if (!string.IsNullOrEmpty(cacheValue))
            {
                SqlFieldNamesForQuery.TryAdd(cacheKey, cacheValue);
            }

            return cacheValue;
        }

    }

    /// <summary>
    /// POCO 映射模型
    /// </summary>
    internal class SqlFieldMapping
    {


        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public EntityIdentity IdentityKey { get; set; }
        /// <summary>
        /// 实体属性集合
        /// </summary>
        public PropertyInfo[] Propertys { get; set; }
        /// <summary>
        /// 字段集合
        /// </summary>
        public List<DbField> Filelds  { get; set; }

        /// <summary>
        /// sql 参数集合
        /// </summary>
        public string[] SqlParas  { get; set; }
    }
}
