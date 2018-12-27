using System;
using System.Collections.Generic;
using System.Text;

namespace FlyBirdYoYo.Utilities.Logging
{
    /// <summary>
    /// sql日志基础模型
    /// </summary>
    public class SqlLogModel:LogModel
    {

        /// <summary>
        /// 执行的sql命令
        /// </summary>
        public string  SqlCmd { get; set; }

        /// <summary>
        /// 执行的sql参数
        /// </summary>
        public object SqlParas { get; set; }


    }
}
