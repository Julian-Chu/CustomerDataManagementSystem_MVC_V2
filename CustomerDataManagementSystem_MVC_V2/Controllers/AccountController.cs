using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using CustomerDataManagementSystem_MVC_V2.Models;
using CustomerDataManagementSystem_MVC_V2.Models.ViewModels;

namespace CustomerDataManagementSystem_MVC_V2.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginVM form)
        {
            if (ModelState.IsValid)
            {
                int id = -1;
                if (form.Username == "admin")
                {
                    if (form.Password == "12345678")
                    {
                        id = 0;
                    }

                }
                else
                {
                    var repo = RepositoryHelper.Get客戶資料Repository();
                    var hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(form.Password, "SHA1");
                    var item = repo.All().SingleOrDefault(p => p.帳號 == form.Username && p.密碼 == hashPassword);
                    if (item != null)
                    {
                        id = item.Id;
                    }
                }

                if (id >= 0)
                {
                    FormsAuthentication.RedirectFromLoginPage(form.Username, false);
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Incorrect = "Incorrect account or password!";
            }
            return View();
        }

        private bool CheckLogin(LoginVM form)
        {
            if (form.Username == "admin")
            {
                if (form.Password == "123")
                    return true;
                return false;
            }
            else
            {
                var repo = RepositoryHelper.Get客戶資料Repository();
                var hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(form.Password, "SHA1");
                return repo.All().Any(p => p.帳號 == form.Username && p.密碼 == hashPassword);
            }
        }
    }
}