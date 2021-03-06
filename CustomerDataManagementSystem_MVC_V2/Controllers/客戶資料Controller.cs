﻿using CustomerDataManagementSystem_MVC_V2.ActionFilters;
using CustomerDataManagementSystem_MVC_V2.Models;
using CustomerDataManagementSystem_MVC_V2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace CustomerDataManagementSystem_MVC_V2.Controllers
{
    [Authorize]
    [HandleError(View = "Error_ArgumentException", ExceptionType = typeof(ArgumentException))]
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
        [客戶資料Droplist]
        public ActionResult Index(客戶資料篩選條件ViewModel filter, string sortBy = "CustomerName", bool ascent = true)
        {
            var data = repo.Get客戶資料_含篩選排序條件(filter, sortBy, ascent);

            return View(data.ToList());
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

        [HttpPost]
        public ActionResult BatchUpdateForContacts(int? id, IList<客戶聯絡人批次更新ViewModel> data)
        {
            if (ModelState.IsValid)
            {
                客戶資料 客戶資料 = repo.Find(id);

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        客戶聯絡人 客戶聯絡人 = contactRepo.Find(item.Id);
                        if (客戶聯絡人 != null)
                        {
                            客戶聯絡人.職稱 = item.職稱;
                            客戶聯絡人.手機 = item.手機;
                            客戶聯絡人.電話 = item.電話;
                        }
                    }
                    repo.UnitOfWork.Commit();
                }
                return RedirectToAction("Details", new { id = id.Value });
            }
            else
            {
                foreach (var model in ModelState)
                {
                    if (!ModelState.IsValidField(model.Key))
                    {
                        foreach (var err in model.Value.Errors)
                        {
                            Debug.WriteLine(err.ErrorMessage);
                        }
                    }
                }


                客戶資料 客戶資料 = repo.Find(id);
                if (客戶資料 == null)
                {
                    return HttpNotFound();
                }
                return View("Details", 客戶資料);
            }

        }

        // GET: 客戶資料/Create
        [客戶資料Droplist]
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [客戶資料Droplist]

        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類,帳號,密碼")] 客戶資料 客戶資料)
        {
            if (string.IsNullOrEmpty(客戶資料.密碼))
            {
                ModelState.AddModelError("密碼", "密碼必填");
                return View(客戶資料);
            }

            if (ModelState.IsValid)
            {
                客戶資料.對密碼進行雜湊();
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index", FormMethod.Get);
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        [客戶資料Droplist]
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
            客戶資料.密碼 = "";

            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [客戶資料Droplist]
        public ActionResult Edit(int id,
            客戶聯絡人[] contacts)
        {
            客戶資料 客戶資料 = repo.Find(id);
            string oldPassword = 客戶資料.密碼;
            if (TryUpdateModel(客戶資料))
            {
                if (string.IsNullOrEmpty(客戶資料.密碼))
                {
                    客戶資料.密碼 = oldPassword;
                }
                else
                {
                    客戶資料.對密碼進行雜湊();
                }


                if (contacts != null)
                {
                    foreach (var item in contacts)
                    {
                        客戶聯絡人 contact = contactRepo.Find(item.Id);
                        contact.職稱 = item.職稱;
                        contact.手機 = item.手機;
                        contact.電話 = item.電話;
                        var DB = contactRepo.UnitOfWork.Context;
                        DB.Entry(contact).State = EntityState.Modified;
                    }
                }


                var tempDB = repo.UnitOfWork.Context;
                //tempDB.Entry(客戶資料).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                contactRepo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        //public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料,
        //    客戶聯絡人[] contacts)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        if (contacts != null)
        //        {
        //            foreach (var item in contacts)
        //            {
        //                客戶聯絡人 contact = contactRepo.Find(item.Id);
        //                contact.職稱 = item.職稱;
        //                contact.手機 = item.手機;
        //                contact.電話 = item.電話;
        //                var DB = contactRepo.UnitOfWork.Context;
        //                DB.Entry(contact).State = EntityState.Modified;
        //            }
        //        }
        //        var tempDB = repo.UnitOfWork.Context;
        //        tempDB.Entry(客戶資料).State = EntityState.Modified;
        //        repo.UnitOfWork.Commit();
        //        contactRepo.UnitOfWork.Commit();

        //        return RedirectToAction("Index");
        //    }
        //    return View(客戶資料);
        //}

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
            if (TryUpdateModel(客戶資料, new string[] { "電話", "傳真", "地址", "Email" }))
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