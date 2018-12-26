using System;
using System.Web;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlyBirdYoYo.Web.Mvc;

namespace FlyBirdYoYo.Web.Controllers
{
    public class HomeController : BaseMvcController
    {
        public IActionResult Index()
        {
            //var r = this.HttpContext.Request;
            //var id = Request.GetQuery<string>("id");
            //var ip = Request.GetIP();

            //var dataContainer = new BusinessViewModelContainer<HomePageViewModel>();
            //         var viewModel = new HomePageViewModel();
            //         //热搜词汇- 从检索的5分钟的词列表中获取
            //         viewModel.HotWords = HotWordService.GetHotWords();

            //         dataContainer.Data = viewModel;

            //         return View(dataContainer);

            return View();
        }



        //public IActionResult Help()
        //{
        //    ViewData["Message"] = "this is help page";

        //    return View();
        //}

        //public IActionResult About()
        //{
        //    ViewData["Message"] = "this is about page";

        //    return View();
        //}

        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}


    }
}
