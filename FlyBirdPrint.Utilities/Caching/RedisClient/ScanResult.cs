using System;
using System.Collections.Generic;
using System.Text;

namespace FlyBirdYoYo.Utilities.Caching.RedisClient
{
    /// <summary>
    /// 键扫描结果模型
    /// </summary>
    public class ScanResult
    {
        public long Cursor { get; set; }
        public List<string> Results { get; set; }
    }
}
