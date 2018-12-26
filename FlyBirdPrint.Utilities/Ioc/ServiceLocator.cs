using System;

namespace FlyBirdYoYo.Utilities.Ioc
{
    /// <summary>
    /// .net core 内置的DI容器(IServiceProvider)的包装
    /// </summary>
    public class ServiceLocator
	{


        /// <summary>
        /// 内置的DI容器
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }
		 

		/// <summary>
		/// T 必须是引用类型 ，类或者接口
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetInstance<T>() where T : class
		{

			if (null == ServiceProvider)
			{
				return default(T);
			}
			object typedInstance = ServiceProvider.GetService(typeof(T));

			return (T)typedInstance;

		}
	}
}
