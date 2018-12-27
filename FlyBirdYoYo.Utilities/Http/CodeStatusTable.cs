using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FlyBirdYoYo.Utilities.Http
{

    /// <summary>
    ///  业务处理错误码一览表枚举
    /// code=0 为正确  其他都是需要处理的
    /// code>0  需要开发者调整参数业务
    /// 4000-4999为【项目错误码】
    /// 后续错误请自定义添加
    /// </summary>
    public  enum CodeStatusTable
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success =0,
        /// <summary>
        /// 无权限
        /// </summary>
        [Description("无权限访问！")]
        NotHaveAuth=4000
    }

    
}
