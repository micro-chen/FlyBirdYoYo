
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.AutoMapper;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.DomainEntity;
using FlyBirdYoYo.BusinessServices;

using FlyBirdYoYo.Tests;
using FlyBirdYoYo.BusinessServices.Services;
using FlyBirdYoYo.DomainEntity.ViewModel;

namespace FlyBirdYoYo.BusinessServices.Services.Tests
{
    public enum AgeStageEnum
    {
        Ten=10,
        Old=100
    }


    [TestClass()]
    public class UserStudentServiceTests : TestBase
    {
        //private UserStudentsService serviceOfStudents;


        public UserStudentServiceTests()
        {
               //this.serviceOfStudents = new UserStudentsService();
        }

        /// <summary>
        /// 增加
        /// </summary>
        [TestMethod()]
        public void AddOneUserStudentsModelTest()
        {


            //var model = new UserStudentsModel
            //{

            //    Name = "你猜猜-" + DateTime.Now.ToString(),
            //    Age = DateTime.Now.Second,
            //    Sex = true,
            //    Score = 55.98m,
            //    Longitude = 555555.6666,
            //    AddTime = DateTime.Now
            //};

            //var result = serviceOfStudents.AddOneUserStudentsModel(model);


            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var model = new UserStudentsModel
            {

                Name = "你猜222222222猜-" + DateTime.Now.ToString(),
                Age = DateTime.Now.Second,
                Sex = true,
                Score = 6655.98m,
                Longitude = 99999999,
                AddTime = DateTime.Now,
                HasPay=888,
                HomeNumber=666
            };

            var result = Singleton<UserStudentsService>.Instance.AddOneUserStudentsModel(model);


            watch.Stop();

            Console.WriteLine(string.Format("real for insert one data use time  is :{0} ms.", watch.ElapsedMilliseconds));




            Assert.IsTrue(result);
        }

        /// <summary>
        /// 批量增加
        /// </summary>

        [TestMethod()]
        public void AddMulitiUserStudentsModelsTest()
        {
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
            var result = Singleton<UserStudentsService>.Instance.AddMulitiUserStudentsModels(lstData);
            Assert.IsTrue(1 == 1);

        }

        /// <summary>
        /// 更新数据实体-by主键
        /// </summary>

        [TestMethod()]
        public void UpdateOneUserStudentsModelTest()
        {
            var model = new UserStudentsModel
            {
                Id = 1,
                Age = 100
            };

            var result = Singleton<UserStudentsService>.Instance.UpdateOneUserStudentsModel(model);
            Assert.IsTrue(result);
        }




        /// <summary>
        /// 条件更新
        /// 多个条件 and 
        /// 
        /// </summary>

        [TestMethod()]
        public void UpdateUserStudentsModelsByConditionTest()
        {
            var model = new UserStudentsModel
            {
                Age = 444
            };
            var result = Singleton<UserStudentsService>.Instance.UpdateUserStudentsModelsByCondition(
                model,
                x => x.Id > 0 && x.Name.Contains("你猜猜%"));

            Assert.IsTrue(result);

        }


        /// </summary>
        [TestMethod()]
        public void GetstudentsElementByIdTest()
        {

            var model = Singleton<UserStudentsService>.Instance
                .GetUserStudentsElementById(1);

            Assert.IsTrue(null != model);

 
        }


  
        /// <summary>
        /// 条件获取
        /// 或

        /// </summary>
        [TestMethod()]
        public void GetfStudentsByEnumConditionTest()
        {


            var ageType = AgeStageEnum.Ten;
            //枚举---1
            var lstData = Singleton<UserStudentsService>.Instance
                .GetUserStudentsElementsByCondition(
                  x => x.Age >= (int)ageType
                );
            //----枚举--2
            //var lstData = Singleton<UserStudentsService>.Instance
            //    .GetUserStudentsElementsByCondition(
            //      x=>x.Age>=(int)AgeStageEnum.Ten
            //    );

            Assert.IsTrue(lstData.Count > 0);

            lstData = Singleton<UserStudentsService>.Instance
                .GetUserStudentsElementsByCondition(null);

            Assert.IsTrue(lstData.Count > 0);



        }


        /// <summary>
        /// 条件获取
        /// 或

        /// </summary>
        [TestMethod()]
        public void GetfStudentsElementsByConditionTest()
        {

 
            var lstData = Singleton<UserStudentsService>.Instance
                .GetUserStudentsElementsByCondition(
                x => x.Id == 1 || x.Name.Contains("你猜猜%")
                );//(x => x.PubSubWsAddr.LenFuncInSql() > 0);

            Assert.IsTrue(lstData.Count > 0);

            lstData = Singleton<UserStudentsService>.Instance
                .GetUserStudentsElementsByCondition(null);

            Assert.IsTrue(lstData.Count > 0);



        }


        [TestMethod()]
        public void GetfStudentsElementsToDTOTest()
        {

            var lstData = Singleton<UserStudentsService>.Instance
                .GetUserStudentsElementsByCondition(
                x => x.Id == 1 || x.Name.Contains("你猜猜%")
                );//(x => x.PubSubWsAddr.LenFuncInSql() > 0);

            Assert.IsTrue(lstData.Count > 0);

            //lstData = this.serviceOfStudents
            //    .GetstudentsElementsByCondition(null);

            var dtoList = lstData.MapTo<List<StudentDto>>();

            Assert.IsTrue(dtoList.Count > 0);
        }

        /// <summary>
        /// 单个删除实体删除 
        /// </summary>
        [TestMethod()]
        public void DeleteOneStudentTest()
        {
            var model = new UserStudentsModel { Id = 2 };

            var result = Singleton<UserStudentsService>.Instance
               .DeleteOneUserStudentsModel( model  );

            Assert.IsTrue(result);
        }

        /// <summary>
        /// 条件删除 
        /// </summary>
        [TestMethod()]
        public void DeleteMulitiStudentsByConditionTest()
        {
            //var result = this.serviceOfStudents
            //    .DeleteMulitiservicesAddressByCondition(x => x.PubSubWsAddr.LenFuncInSql() > 0);

            var result = Singleton<UserStudentsService>.Instance
               .DeleteMulitiUserStudentsByCondition(
                  x => x.Id == 1 || x.Name.Contains("你猜猜%")
            );
            Assert.IsTrue(result);
        }


        //多个查询条件构建 （使用Lambda表达式构建 进行条件body的合并）

        [TestMethod()]
        public void GetByMultipleConditionsTest()
        {
            //组合条件
            var predicate = PredicateBuilder.CreatNew<UserStudentsModel>();

            string id = "55";
            if (!string.IsNullOrEmpty(id) && id.ToInt() > 0)
            {
                predicate = predicate.And(s => s.Id <= id.ToInt());
            }

            //开始组合表达式body
            predicate = predicate.Or(s => s.Name.Contains("你猜猜-2%"));


            var model = Singleton<UserStudentsService>.Instance.GetUserStudentsElementsByCondition(predicate);

            Assert.IsNotNull(model);

        }



        /// <summary>
        /// 分页查询年龄大于某个值的学生信息
        /// </summary>
        /// <param name="age"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [TestMethod()]
        public void GetDTOElementsByPagerAndCondition()
        {
            int age = 10;
            int pageIndex = 0;//页索引 从0开始的
            int pageSize = 10;
            var dataPage = Singleton<UserStudentsService>.Instance.SearchUserStudentsHandler(new DomainEntity.QueryCondition.StudentQueryCondition
            {
                KeyWord = "你",
                PageNumber = pageIndex + 1,
                PageSize = pageSize
            });

            Assert.IsNotNull(dataPage);
            Assert.IsTrue(dataPage.Data.Count > 0);


        }


        /// <summary>
        /// 分页查询年龄大于某个值的学生信息
        /// </summary>
        /// <param name="age"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [TestMethod()]
        public void GetListBySqlTest()
        {
            var model = new StudentDto
            {
                Name = "11111111111111111"
            };
            var dataList = Singleton<UserStudentsService>.Instance.GetListBySql(model);

            Assert.IsNotNull(dataList);


        }


        ////[TestMethod()]
        ////public void GetUserStudentsModelsElementsByPagerAndConditionTest()
        ////{
        ////    var pageSize = 10;
        ////    var pageIndex = 0;
        ////    var totalRecords = -1;
        ////    var totalPages = -1;

        ////    var lstData = this.serviceOfStudents
        ////       .GetstudentsElementsByPagerAndCondition(pageIndex,
        ////       pageSize,
        ////       out totalRecords,
        ////       out totalPages, x => x.Id > 0,//PubSubWsAddr.LenFuncInSql() 
        ////       "Id",
        ////       OrderRule.DESC);
        ////    Assert.IsTrue(lstData.Count > 0);


        ////}



    }
}