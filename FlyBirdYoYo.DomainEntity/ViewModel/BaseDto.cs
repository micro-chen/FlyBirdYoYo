using System;
using FlyBirdYoYo.Utilities.SystemEnum;

namespace FlyBirdYoYo.DomainEntity.ViewModel
{
   /// <summary>
   /// dto 抽象基础类
   /// </summary>
   public abstract  class BaseDto
    {

     
        /// <summary> 
        /// 销售平台
        /// </summary>
        public PlatformEnum Platform { get; set; }

    }
}
