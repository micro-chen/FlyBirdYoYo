using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FlyBirdYoYo.Utilities.TypeFinder;

namespace FlyBirdYoYo.DbManage.Mapping
{
    public static class TypeMapper
    {
        /// <summary>
        /// 配置映射直接采用异步启动
        /// </summary>
        /// <returns></returns>
        public static Task InitializeAsync()
        {

            return Task.Factory.StartNew(() =>
            {

                //类型寻找
                var typeFinder = new AppDomainTypeFinder()
                {
                    //从指定的程序集加载Types
                    AssemblyNames = new List<string>
                {
                    "FlyBirdYoYo.DomainEntity",
                }
                };

                var entityTypes = typeFinder.FindClassesOfType(typeof(BaseEntity));


                if (entityTypes.IsNotEmpty())
                {
                    //创建指定的对象实例的Mapper
                    entityTypes.AsParallel().ForAll(type =>
                    {
                        var mapper = new ColumnAttributeTypeMapper(type);
                        SqlMapper.SetTypeMap(type, mapper);
                    });
                }


            });


        }
    }
}
