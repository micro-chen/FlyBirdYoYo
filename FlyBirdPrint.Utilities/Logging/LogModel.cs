using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace FlyBirdYoYo.Utilities.Logging
{
    /// <summary>
    /// 日志基础信息模型
    /// </summary>
    public class LogModel
    {
        /// <summary>
        /// 当前调用堆栈信息
        /// </summary>
        public StackFrame[] CallingFrames { get; set; }

    }
}
