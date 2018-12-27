using System;
using System.Collections.Generic;
using System.AutoMapper;
using AutoMapper;

using FlyBirdYoYo.DomainEntity;
using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.DomainEntity.Login;


namespace FlyBirdYoYo.BusinessServices.AutoMapper
{
    public class AutoMapperMappingRegister : IMappingRegister
    {

        #region 单例模式


        private static AutoMapperMappingRegister _instance;

        public static AutoMapperMappingRegister Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new AutoMapperMappingRegister();
                }
                return _instance;
            }
        }

        #endregion


        /// <summary>
        /// 类型注册委托
        /// </summary>
        /// <param name="cfg"></param>
        public void Register(IMapperConfigurationExpression cfg)
        {
            #region 类型映射配置


            cfg.CreateMap<UserStudentsModel, StudentDto>();
            cfg.CreateMap<OAuthLoginViewModel, BaseLoginViewModel>();
          

            #endregion
        }
    }
}
