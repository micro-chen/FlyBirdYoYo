using System;
using System.Collections.Generic;
using System.Text;

namespace FlyBirdYoYo.Utilities.Caching
{
	public enum CacheItemPolicy
	{
		AbsoluteExpiration,
		SlidingExpiration
	}

	public class TimeCacheDependency
	{
		public TimeCacheDependency(TimeSpan time, CacheItemPolicy policy = CacheItemPolicy.AbsoluteExpiration)
		{
			Time = time;
		}

		public TimeSpan Time { get; }

		public CacheItemPolicy Policy { get; }
	}
}
