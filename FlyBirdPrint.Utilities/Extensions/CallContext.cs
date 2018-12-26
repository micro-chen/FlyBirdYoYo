using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace  System
{
    /// <summary>
    /// 模拟当前调用的上下文
    /// </summary>
    public static class CallContext
    {
        static ConcurrentDictionary<string, AsyncLocal<object>> state = new ConcurrentDictionary<string, AsyncLocal<object>>();

        public static void SetData(string name, object data)
        {
            state.GetOrAdd(name, _ => new AsyncLocal<object>()).Value = data;
        }

        public static object GetData(string name)
        {
            return state.TryGetValue(name, out AsyncLocal<object> data) ? data.Value : null;
        }
        public static void FreeNamedDataSlot(string key)
        {
             state.TryRemove(key, out AsyncLocal<object> data);
        }

        

    }
}
