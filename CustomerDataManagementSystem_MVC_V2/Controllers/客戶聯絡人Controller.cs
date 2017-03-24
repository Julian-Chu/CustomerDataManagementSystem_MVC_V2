using CustomerDataManagementSystem_MVC_V2.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CustomerDataManagementSystem_MVC_V2.Controllers
{
    public class 客戶聯絡人Controller : Controller
    {
        private 客戶聯絡人Repository contactRepo;
        private 客戶資料Repository customerRepo;

        private Dictionary<string, string> jobTitleDic = new Dictionary<string, string>();

        public 客戶聯絡人Controller()
        {
            this.contactRepo = RepositoryHelper.Get客戶聯絡人Repository();
            this.customerRepo = RepositoryHelper.Get客戶資料Repository();
            CreateCaregoryDic();
        }

        private void CreateCaregoryDic()
        {
            var category = contactRepo.All().Select(c => c.職稱).Distinct().ToList();
            int i = 1;
            jobTitleDic.Add(i.ToString(), "All");
            i++;
            foreach (var item in category)
            {
                jobTitleDic.Add(i.ToString(), item);
                i++;
            }
        }

        private List<SelectListItem> CreateSelectListItems()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in jobTitleDic)
            {
                items.Add(new SelectListItem { Text = item.Value, Value = item.Key });
            }

            return items;
        }

        public 客戶聯絡人Controller(客戶聯絡人Repository mockRepo)
        {
            this.contactRepo = mockRepo;
        }

        // GET: 客戶聯絡人
        public ActionResult Index(string keyword = "", string selectedId = "")
        {
            var data = contactRepo.All().Where(contact => contact.是否已刪除 == false &&
            (contact.姓名.Contains(keyword) || contact.客戶資料.客戶名稱.Contains(keyword))).Include(客 => 客.客戶資料);

            if (jobTitleDic.ContainsKey(selectedId) && !string.IsNullOrEmpty(jobTitleDic[selectedId]))
            {
                var jobTitle = jobTitleDic[selectedId];
                if(jobTitle != "All") data = data.Where(p => p.職稱 == jobTitle);
            }

            ViewBag.jobTitles = CreateSelectListItems();
            return View(data.ToList());
        }

        //[HttpPost]
        //public ActionResult Index(, string keyword = "")
        //{
        //    var data = contactRepo.All().Where(contact => contact.是否已刪除 == false &&
        //    (contact.姓名.Contains(keyword) || contact.客戶資料.客戶名稱.Contains(keyword))).Include(客 => 客.客戶資料);

        //    var jobTitle = jobTitleDic[selectedId];
        //    data = data.Where(p => p.職稱 == jobTitle);

        //    ViewBag.jobTitles = CreateSelectListItems();
        //    return View(data.ToList());
        //}

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = contactRepo.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                contactRepo.Add(客戶聯絡人);
                contactRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = contactRepo.Find(id);

            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                contactRepo.UnitOfWork.Context.Entry(客戶聯絡人).State = EntityState.Modified;
                contactRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = contactRepo.Find(id);

            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = contactRepo.Find(id);

            客戶聯絡人.是否已刪除 = true;
            contactRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var db = contactRepo.UnitOfWork.Context;
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}