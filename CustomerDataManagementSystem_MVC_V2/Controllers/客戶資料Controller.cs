using CustomerDataManagementSystem_MVC_V2.ActionFilters;
using CustomerDataManagementSystem_MVC_V2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace CustomerDataManagementSystem_MVC_V2.Controllers
{
    [Authorize]
    [HandleError(View ="Error_ArgumentException",ExceptionType =typeof(ArgumentException))]
    [計算Action時間]
    public class 客戶資料Controller : Controller
    {
        protected 客戶資料Repository repo;
        private 客戶聯絡人Repository contactRepo;

        private Dictionary<string, string> customerCategoryDic = new Dictionary<string, string>();

        public 客戶資料Controller()
        {
            this.repo = RepositoryHelper.Get客戶資料Repository();
            this.contactRepo = RepositoryHelper.Get客戶聯絡人Repository(repo.UnitOfWork);

            CreateCategoryDic();
        }

        protected virtual void CreateCategoryDic()
        {
            var category = repo.All().Select(c => c.客戶分類).Distinct().ToList();
            int i = 1;
            foreach (var item in category)
            {
                customerCategoryDic.Add(i.ToString(), item);
                i++;
            }
        }

        public ActionResult 客戶資料統計()
        {
            var tempRepo = RepositoryHelper.Get客戶資料統計Repository();
            return View(tempRepo.All().ToList());
        }

        
        // GET: 客戶資料
        public ActionResult Index(string keyword = "", string sortBy = "CustomerName", bool ascent = true, string selectedId = "0")
        {
            var viewBag = ViewBag;
            var modelstate = ModelState["sortBy"];
            var data = repo.All();
            data = data.Where(p => p.是否已刪除 == false && p.客戶名稱.Contains(keyword));
            data = SortBy(sortBy, ascent, data);
            List<SelectListItem> items = CreateSelectListItems();
            ViewBag.dep = items;
            return View(data.ToList());
        }

        private IQueryable<客戶資料> SortBy(string sortBy, bool ascent, IQueryable<客戶資料> data)
        {
            switch (sortBy)
            {
                case "CustomerName":
                    if (ascent == true)
                        data = data.OrderBy(p => p.客戶名稱);
                    else
                        data = data.OrderByDescending(p => p.客戶名稱);
                    break;

                case "VAT":
                    if (ascent == true)
                        data = data.OrderBy(p => p.統一編號);
                    else
                        data = data.OrderByDescending(p => p.統一編號);
                    break;

                case "TEL":
                    if (ascent == true)
                        data = data.OrderBy(p => p.電話);
                    else
                        data = data.OrderByDescending(p => p.電話);
                    break;

                default:
                    data = data.OrderBy(p => p.客戶名稱);
                    break;
            }

            return data;
        }

        private List<SelectListItem> CreateSelectListItems()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in customerCategoryDic)
            {
                items.Add(new SelectListItem { Text = item.Value, Value = item.Key });
            }

            return items;
        }

        [HttpPost]
        public ActionResult Index(string selectedId, string keyword = "")
        {
            var modelstate = ModelState;
            var category = customerCategoryDic[selectedId];
            var data = repo.filter(keyword, category);
            List<SelectListItem> items = CreateSelectListItems();
            ViewBag.selectList = items;
            ViewBag.keyword = keyword;
            ViewBag.selectedId = selectedId;
            return View(data.ToList());
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                throw new ArgumentException("id 不存在或為空值");

            }
            客戶資料 客戶資料 = repo.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index", FormMethod.Get);
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                throw new ArgumentException("id 不存在或為空值");
            }
            客戶資料 客戶資料 = repo.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            var contacts = contactRepo.All().Where(c => c.客戶Id == 客戶資料.Id);
            ViewBag.contacts = contacts.ToList();

            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料, 
            客戶聯絡人[] contacts)
        {
            if (ModelState.IsValid)
            {
                var tempDB = repo.UnitOfWork.Context;
                tempDB.Entry(客戶資料).State = EntityState.Modified;

                foreach (var item in contacts)
                {
                    客戶聯絡人 contact = contactRepo.Find(item.Id);
                    contact.職稱 = item.職稱;
                    //contact.姓名 = item.姓名;
                    //contact.Email = item.Email;
                    contact.手機 = item.手機;
                    contact.電話 = item.電話;
                    var DB = contactRepo.UnitOfWork.Context;
                    DB.Entry(contact).State = EntityState.Modified;
                }
                
                repo.UnitOfWork.Commit();
                contactRepo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repo.Find(id);
            //客戶資料.是否已刪除 = true;
            repo.Delete(客戶資料);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult 修改客戶資料()
        {
            FormsIdentity id = (FormsIdentity)HttpContext.User.Identity;
            客戶資料 客戶資料 = repo.FindByAccount(id.Name);
            return View(客戶資料);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult 修改客戶資料(FormCollection form)
        {
            FormsIdentity id = (FormsIdentity)HttpContext.User.Identity;
            客戶資料 客戶資料 = repo.FindByAccount(id.Name);
            if(TryUpdateModel(客戶資料, new string[] {"電話","傳真","地址","Email" }))
            {
                string password = form["密碼"];
                var passwordWithSHA1 = Crypto.SHA1(password);
                客戶資料.密碼 = passwordWithSHA1;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index", "Home");
            }

            return View(客戶資料);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}