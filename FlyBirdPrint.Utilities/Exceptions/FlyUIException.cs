using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// 业务UI异常
    /// </summary>
    public class FlyUIException:Exception
    {
        public FlyUIException(string msg):base(msg)
        {

        }
    }
}
