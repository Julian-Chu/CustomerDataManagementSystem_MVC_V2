using CustomerDataManagementSystem_MVC_V2.ActionFilters;
using CustomerDataManagementSystem_MVC_V2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CustomerDataManagementSystem_MVC_V2.Controllers
{
    [計算Action時間]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login(string ReturnUrl)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginVmModel, string ReturnUrl="")
        {
            if (ModelState.IsValid)
            {
                FormsAuthentication.RedirectFromLoginPage(loginVmModel.Username, false);
                if (ReturnUrl.StartsWith("/"))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}