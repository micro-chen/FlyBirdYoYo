using System;
using System.Collections.Generic;
using System.Text;

namespace FlyBirdYoYo.Utilities.Caching
{
    /// <summary>
    /// 文件依赖数据结构
    /// </summary>
	public class FileCacheDependency
	{
		public FileCacheDependency(string filePath)
		{
			FilePath = filePath;
		}

		public string FilePath { get; }
	}
}
