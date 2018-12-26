using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.Utilities.Ioc;
using FlyBirdYoYo.Utilities.Logging;
using Pdd.OpenSdk.Core;
using Pdd.OpenSdk.Core.Models.Response.Erp;
using FlyBirdYoYo.Utilities.SystemEnum;
using FlyBirdYoYo.DomainEntity.QueryCondition;

using FlyBirdYoYo.DomainEntity.ViewModel;


namespace FlyBirdYoYo.BusinessServices.Helpers
{
    /// <summary>
    /// 沙箱环境辅助类
    /// </summary>
    public static class SandBoxHelper
    {

        /// <summary>
        /// 是否启用沙箱环境
        /// </summary>
        public static bool IsUseSandBoxMode
        {
            get
            {
                bool result = false;
                result = ConfigHelper.AppSettingsConfiguration.GetConfigBool("IsUseSandBoxMode");
                return result;
            }
        }

        private static string _FilePath_PddOrderList;
        /// <summary>
        /// 拼多多订单列表json文件路径
        /// </summary>
        public static string FilePath_PddOrderList
        {
            get
            {
                if (string.IsNullOrEmpty(_FilePath_PddOrderList))
                {
                    _FilePath_PddOrderList = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SandBox", "sandbox_orderlist.json");
                }
                return _FilePath_PddOrderList;
            }
        }
        /// <summary>
        /// 读取沙箱环境的拼多多订单列表
        /// </summary>
        /// <returns></returns>
        public static GetOrderListResponseModel GetAllPddOrderList_SandBox()
        {
            GetOrderListResponseModel dataList = null;

            try
            {
                if (!File.Exists(FilePath_PddOrderList))
                {
                    throw new FileNotFoundException("模拟拼多多订单列表json文件未找到在指定的路径！");
                }

                string jsonContent = File.ReadAllText(FilePath_PddOrderList);

                dataList = jsonContent.FromJsonToObject<GetOrderListResponseModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataList;
        }
    }
}
