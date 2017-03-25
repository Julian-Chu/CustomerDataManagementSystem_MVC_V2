using System;
using System.Linq;
using System.Collections.Generic;

namespace CustomerDataManagementSystem_MVC_V2.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public 客戶資料 Find(int? id)
        {
            return this.All().SingleOrDefault(customer => customer.Id == id);
        }

        public 客戶資料 FindByName(string name)
        {
            return this.All().SingleOrDefault(customer => customer.客戶名稱 == name);
        }

        public 客戶資料 FindByAccount(string account)
        {
            return this.All().SingleOrDefault(customer => customer.帳號 == account);
        }

        public IQueryable<客戶資料> filter(string keyword, string category)
        {
            var data = this.All().Where(p => p.是否已刪除 == false);
            if (!String.IsNullOrEmpty(keyword)) data = data.Where(p => p.客戶名稱.Contains(keyword));
            if (!String.IsNullOrEmpty(category)) data = data.Where(c => c.客戶分類.Contains(category));

            return data;
        }
    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}