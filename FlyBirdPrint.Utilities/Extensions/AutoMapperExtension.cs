using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Data;
using AutoMapper.EquivalencyExpression;


namespace  System.AutoMapper
{

    /// <summary>
    /// 类型注册接口
    /// </summary>
    public interface IMappingRegister
    {
        void Register(IMapperConfigurationExpression cfg);
    }

    /// <summary>
    /// AutoMapper扩展帮助类
    /// 自动映射规则：1 同名属性 （不区分大小写） 2Get前缀的同名属性/方法 3 class 
    /// 其他自定义规则：请参考：http://automapper.readthedocs.io/en/latest/Getting-started.html
    /// When you configure a source/destination type pair in AutoMapper, the configurator attempts to match properties and methods on the source type to properties on the destination type. If for any property on the destination type a property, method, or a method prefixed with “Get” does not exist on the source type, AutoMapper splits the destination member name into individual words (by PascalCase conventions).
    /// </summary>
    public static class AutoMapperExtension
    {


        private static bool _isHasInit = false;
        private static object _locker_init = new object();

        /// <summary>
        /// 是否初始化完毕
        /// </summary>
        public static bool Initialized
        {
            get
            {
                return _isHasInit;
            }
        }


        public static readonly List<IMappingRegister> MappingRegisterList = new List<IMappingRegister>();

        static IMapperConfigurationExpression _MapperConfiguration;

        /// <summary>
        /// automapper的全局配置
        /// </summary>
        public static IMapperConfigurationExpression MapperConfiguration
        {
            get
            {
                return _MapperConfiguration;
            }
        }
        /// <summary>
        /// AutoMapper的自初始化
        /// https://automapper.readthedocs.io/en/latest/Getting-started.html
        /// If you’re using the static Mapper method, configuration should only happen once per AppDomain. 
        /// One “feature” of AutoMapper allowed you to modify configuration at runtime. That caused many problems, so the new static API does not allow you to do this. 
        /// You’ll need to move all your Mapper.CreateMap calls into a profile, and into a Mapper.Initialize.
        /// </summary>
        public static void Init()
        {
            lock (_locker_init)
            {
                if (_isHasInit == false)
                {
                    Mapper.Initialize(cfg =>
                    {
                        cfg.AllowNullCollections = true;//如果source 集合为null ，那么dest集合为null
                        cfg.AddDataReaderMapping();
                        cfg.AddCollectionMappers();
                        //cfg.CreateMap<Foo, FooDto>();
                        //cfg.CreateMap<Bar, BarDto>();
                        //类型注册
                        MappingRegisterList.AsParallel().ForAll(x =>
                        {
                            x.Register(cfg);
                        });
                        //保存配置到静态属性
                        _MapperConfiguration = cfg;
                    });

                    _isHasInit = true;
                }
            }


        }



        /// <summary>
        ///  类型映射
        ///  创建新的目标实例
        /// </summary>
        public static TDestination MapTo<TDestination>(this object obj)
            where TDestination : class

        {
            if (obj == null) return default(TDestination);
            if (!Initialized)
            {
                throw new Exception("请先配置AutoMapper!");
            }
            return Mapper.Map<TDestination>(obj);
        }

        /// <summary>
        /// 类型映射
        /// 已有的目标实例
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destObj)
            where TSource : class
            where TDestination : class
        {

            if (null == source)
            {
                return null;
            }
            if (!Initialized)
            {
                throw new Exception("请先配置AutoMapper!");
            }
            return Mapper.Map<TSource, TDestination>(source, destObj);
        }

        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
            where TSource : class
            where TDestination : class
        {
            //IEnumerable<T> 类型需要创建元素的映射
            if (!source.Any())
            {
                return null;
            }
            if (!Initialized)
            {
                throw new Exception("请先配置AutoMapper!");
            }
            return Mapper.Map<List<TDestination>>(source);
        }



        /// <summary>
        /// DataReader映射
        /// </summary>
        public static IEnumerable<TDestination> DataReaderMapTo<TDestination>(this IDataReader reader)
            where TDestination : class
        {
            if (null == reader || reader.IsClosed || reader.FieldCount <= 0)
            {
                return null;
            }
            if (!Initialized)
            {
                throw new Exception("请先配置AutoMapper!");
            }
            return Mapper.Map<IDataReader, IEnumerable<TDestination>>(reader);
        }


    }
}
