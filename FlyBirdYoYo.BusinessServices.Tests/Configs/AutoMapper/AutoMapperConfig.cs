using System.AutoMapper;
using FlyBirdYoYo.BusinessServices.AutoMapper;

namespace FlyBirdYoYo.Web.Configs
{

    /// <summary>
    /// 配置AutoMapper的入口
    /// </summary>
    public static class AutoMapperConfig
    {

        public static void Config()
        {
            AutoMapperExtension.MappingRegisterList.Add(AutoMapperMappingRegister.Instance);
            AutoMapperExtension.Init();
        }

    }
}