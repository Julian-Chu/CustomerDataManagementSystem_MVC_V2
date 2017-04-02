using AutoMapper;
using CustomerDataManagementSystem_MVC_V2.Models;
using CustomerDataManagementSystem_MVC_V2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerDataManagementSystem_MVC_V2.Controllers
{
    public class 客戶專區Controller : Controller
    {
        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
        // GET: 客戶專區
        public ActionResult Index()
        {
            var data = repo.Find(Convert.ToInt32(User.Identity.Name));

            Mapper.Initialize(cfg => cfg.CreateMap<客戶資料, 客戶專區_資料修改VM>());
            客戶專區_資料修改VM model = Mapper.Map<客戶專區_資料修改VM>(data);
            model.密碼 = "";



            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(客戶專區_資料修改VM item)
        {
            if (ModelState.IsValid)
            {
                var data = repo.Find(Convert.ToInt32(User.Identity.Name));
                
                Mapper.Initialize(cfg => cfg.CreateMap<客戶專區_資料修改VM, 客戶資料>());
                Mapper.Map(item, data, typeof(客戶專區_資料修改VM), typeof(客戶資料));

                if(!string.IsNullOrEmpty(data.密碼))
                {
                    data.對密碼進行雜湊();
                }
                repo.UnitOfWork.Commit();

                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}