using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using FlyBirdYoYo.Utilities.Ioc;
using Microsoft.Extensions.Caching.Memory;


namespace FlyBirdYoYo.Utilities.Caching
{
	/// <summary>
	/// 缓存处理类
	/// 提供一个基于 .net  Catche对象的管理容器
	/// 实现缓存的管理
	/// </summary>
	public sealed class NativeCacheManager : ICacheManager
	{

		#region 单例实例



		/// <summary>
		/// </summary>
		private static NativeCacheManager _Default;
		/// <summary>
		/// 单例实例
		/// </summary>
		public static NativeCacheManager Current
		{
			get
			{
				if (null == _Default)
				{
					_Default = new NativeCacheManager();
				}
				return _Default;
			}

		}


		internal static IMemoryCache CacheCore
		{
			get
			{
				var memCache = ServiceLocator.GetInstance<IMemoryCache>();

				if (null == memCache)
				{
					//构建默认的内存缓存,个数限制：1000k
					memCache = new MemoryCache(new MemoryCacheOptions() { SizeLimit = 1024 * 1000 });
				}
				return memCache;

			}
		}


		#endregion

		public ReaderWriterLockSlim CacheWriteLock = new ReaderWriterLockSlim();

		private Dictionary<string, bool> DicForCacheKeys = new Dictionary<string, bool>();

		#region 构造函数

		/// <summary>
		/// 空参数的构造函数
		/// </summary>
		public NativeCacheManager()
		{

			//每次构建 这个实例  那么注册到单例集合，为了监视所有的缓存键值对
			SingletonList<NativeCacheManager>.Instance.Add(this);
		}

		///// <summary>
		///// 设置过期时间
		///// </summary>
		///// <param name="timeOut"></param>
		//public NativeCacheManager(int timeOut)
		//{
		//    this._timeOut = timeOut;
		//}

		#endregion



		///// <summary>
		///// timeout ( 默认为300s  5min过期，如果太大，会导致缓存中的数据量越来越大，降低缓存的检索效率 )
		///// </summary>
		//private int _timeOut = 300;
		///// <summary>
		///// time out (seconds)   
		///// </summary>
		//public int TimeOut
		//{
		//    set { _timeOut = value; }
		//    get { return _timeOut; }
		//}


		#region 添加缓存

		public bool IsHasSet(string key)
		{

			if (key.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(key));
			}
			object cacheValue;
			var result = CacheCore.TryGetValue(key, out cacheValue);

			return result;
		}



		/// <summary>
		/// 添加缓存 (绝对有效期)
		/// </summary>
		/// <param name="cacheKey">缓存键值</param>
		/// <param name="cacheValue">缓存内容</param>
		/// <param name="timeout">绝对有效期（单位: 秒）</param>
		public void Set(string cacheKey, object cacheValue, int timeout = CacheConfigFactory.DefaultTimeOut)
		{

			if (string.IsNullOrEmpty(cacheKey))
			{
				return;
			}

			try
			{


				if (null == cacheValue)
				{
					Remove(cacheKey);
					return;
				}
				//缓存策略
				var cacheEntryOptions = new MemoryCacheEntryOptions() { Size = 1024 };

				cacheEntryOptions.SetPriority(CacheItemPriority.Normal);

				// Pin to cache.
				// Add eviction callback
				// .RegisterPostEvictionCallback(callback: EvictionCallback, state: this);
				//绝对过期
				var timeExpire = DateTime.Now + TimeSpan.FromSeconds(timeout);
				cacheEntryOptions.SetAbsoluteExpiration(timeExpire);
				CacheCore.Set(cacheKey, cacheValue, cacheEntryOptions);


				if (!DicForCacheKeys.ContainsKey(cacheKey))
				{
					DicForCacheKeys.Add(cacheKey, true);
				}

			}
			catch (Exception ex)
			{
				Logging.Logger.Error(ex);
			}
			//if (timeout <= 0)
			//{
			//    //绝对过期
			//    policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(TimeOut);
			//    CacheCore.Add(new CacheItem(cacheKey, cacheValue), policy);
			//}
			//else
			//{
			//    //相对过期
			//    policy.SlidingExpiration = TimeSpan.FromSeconds(TimeOut);
			//    CacheCore.Add(new CacheItem(cacheKey, cacheValue), policy);
			//}
		}





		#endregion


		#region 删除缓存



		/// <summary>
		/// 删除缓存
		/// </summary>
		/// <param name="cacheKey">缓存键值</param>
		public void Remove(string cacheKey)
		{
			if (!string.IsNullOrEmpty(cacheKey))
			{
				CacheCore.Remove(cacheKey);

				if (DicForCacheKeys.ContainsKey(cacheKey))
				{
					DicForCacheKeys.Remove(cacheKey);
				}
			}
		}

		/// <summary>
		/// 清空所有缓存，不仅仅是当前的缓存实例 ，而是所有的缓存实例都要清除
		/// </summary>
		public void Clear()
		{
			var allMemCacheInstance = SingletonList<NativeCacheManager>.Instance;

			try
			{

				foreach (var itemCacheInstance in allMemCacheInstance)
				{
					try
					{
						//保持每个实例 进入锁定状态
						itemCacheInstance.CacheWriteLock.EnterReadLock();

						var keys = itemCacheInstance.DicForCacheKeys;
						foreach (var itemKey in keys)
						{
							itemCacheInstance.Remove(itemKey.Key);
						}
					}
					catch { }
					finally
					{
						itemCacheInstance.CacheWriteLock.ExitReadLock();
					}


				}

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		#endregion


		#region 获取缓存




		/// <summary>
		/// 获取缓存
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="key">The key of the value to get.</param>
		/// <returns>The value associated with the specified key.</returns>
		public T Get<T>(string cacheKey)
		{
			if (string.IsNullOrEmpty(cacheKey))
			{
				return default(T);
			}

			object cacheValue;
			var result = CacheCore.TryGetValue(cacheKey, out cacheValue);
			if (true == result)
			{
				return (T)cacheValue;
			}
			else
			{
				return default(T);
			}

		}



		/// <summary>
		/// 返回缓存键值列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetCacheKeys()
		{

			List<string> cacheKeys = new List<string>();

			var allMemCacheInstance = SingletonList<NativeCacheManager>.Instance;

			try
			{

				foreach (var itemCacheInstance in allMemCacheInstance)
				{
					try
					{
						//保持每个实例 进入锁定状态
						itemCacheInstance.CacheWriteLock.EnterWriteLock();

						var keys = itemCacheInstance.DicForCacheKeys;
						foreach (var itemKey in keys)
						{
							cacheKeys.Add(itemKey.Key);
						}
					}
					catch { }
					finally
					{
						itemCacheInstance.CacheWriteLock.ExitWriteLock();
					}


				}

			}
			catch (Exception ex)
			{
				throw ex;
			}


			return cacheKeys;
		}


		#endregion




	}
}
