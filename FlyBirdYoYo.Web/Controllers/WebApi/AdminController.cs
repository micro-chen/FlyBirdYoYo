using System;
using System.AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlyBirdYoYo.BusinessServices;
using FlyBirdYoYo.DomainEntity;
using FlyBirdYoYo.DomainEntity.Login;
using FlyBirdYoYo.DomainEntity.QueryCondition.Admin;
using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.DomainEntity.ViewModel.Admin;
using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.Utilities.Logging;
using FlyBirdYoYo.Utilities.SystemEnum;
using FlyBirdYoYo.Web.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlyBirdYoYo.Web.Controllers.WebApi
{
    /// <summary>
    /// 超管控制器
    /// </summary>
    [AuthSysAdminAttributeFilter(IsCheck = true)]
    public class AdminController : BaseApiControllerAuth
    {
        /// <summary>
        /// 加载全部的支持平台列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public BusinessViewModelContainer<List<EnumKeyValueViewModel>> LoadSupportPlatformsListHandler()
        {
            BusinessViewModelContainer<List<EnumKeyValueViewModel>> res = new BusinessViewModelContainer<List<EnumKeyValueViewModel>>();
            try
            {
                var lstPlatforms = EnumExtension.ConvertEnumToList<PlatformEnum>();
                res.Data = lstPlatforms.Where(x => x.EnumValue > 0).Select(item =>
                  {
                      return new EnumKeyValueViewModel { Label = item.Description, Value = item.EnumValue };
                  }).ToList();


            }
            catch (Exception ex)
            {

                res.SetFalied(ex.Message);
            }

            return res;
        }

       
       
    }
}