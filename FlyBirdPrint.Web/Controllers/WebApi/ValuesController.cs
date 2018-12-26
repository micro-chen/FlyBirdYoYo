using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Transactions;
using System.Net;
using System.Text;
using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.Web.Mvc;
using FlyBirdYoYo.DomainEntity;
using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.BusinessServices;
using FlyBirdYoYo.Utilities.Logging;

/// <summary>
/// 示范的 Web API 地址。
/// 已经在 Setup  启动中 进行了全局路由规则限制，除非特定场景 否则不适用标注的形式 添加单个控制器
/// /api/values/get
/// </summary>
namespace FlyBirdYoYo.Web.Controllers.WebApi
{
    public class ValuesController : BaseApiControllerNoAuth
    {
        // GET: api/values/get
        [HttpGet]
        public string Get()
        {
            return DateTime.Now.ToOfenTimeString();
        }

       

        [HttpPost]
        public async Task<ActionResult<StudentDto>> TestPost(StudentDto studentDto)
        {
            int x = 0;
            return await Task.Run(() =>
            {
                return new StudentDto
                {
                    Id = 1,
                    Age = 18,
                    Name = "AAA"
                };
            });

        }


      

        /// <summary>
        /// 示范接口查询数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public BusinessViewModelContainer<List<UserStudentsModel>> GetStudents()
        {


            var viewModel = new BusinessViewModelContainer<List<UserStudentsModel>>();
            try
            {
                //下面是一个查询数据库的示范---在实际场景中请一定要把数据放到Data 属性中。
                var demoData = Singleton<UserStudentsService>
                    .Instance
                    .GetUserStudentsElementsByCondition(x => x.Id >= 10);
                if (demoData.IsNotEmpty())
                {
                    viewModel.Data = demoData;
                }

            }
            catch (Exception ex)
            {
                viewModel.SetFalied("调用失败了！");
                Logger.Error(ex);
            }

            return viewModel;
        }


        [HttpGet]
        public BusinessViewModelContainer<bool> AddMulitiUserStudentsModelsTest()
        {

            var viewModel = new BusinessViewModelContainer<bool>();



            var lstData = new List<UserStudentsModel>();
            var rand = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 100; i++)
            {
                var model = new UserStudentsModel
                {
                    Name = "你猜猜-" + Guid.NewGuid().ToString(),
                    Age = rand.Next(1, 100),
                    Sex = false,
                    Score = 33355.98m,
                    Longitude = 59595959,
                    AddTime = DateTime.Now,
                    HasPay = 888,
                    HomeNumber = 666
                };
                lstData.Add(model);
            }

            using (var tran = new TransactionScope())
            {
                var lstStudents = Singleton<UserStudentsService>.Instance.GetUserStudentsElementById(1);
                var result = Singleton<UserStudentsService>.Instance.AddMulitiUserStudentsModels(lstData);

                viewModel.Data = result;

                tran.Complete();
            }


            return viewModel;
        }


   
    }
}
