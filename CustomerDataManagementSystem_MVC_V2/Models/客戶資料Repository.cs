using System;
using System.Linq;
using CustomerDataManagementSystem_MVC_V2.Models.ViewModels;

namespace CustomerDataManagementSystem_MVC_V2.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(c => c.是否已刪除 == false);
        }

        public IQueryable<客戶資料> Get客戶資料_含篩選排序條件(客戶資料篩選條件ViewModel filter, string sortBy = "客戶名稱", bool ascent = true)
        {
            var data = this.All();
            if (!string.IsNullOrEmpty(filter.keyword))
                data = data.Where(p => p.客戶名稱.Contains(filter.keyword));
            if (!string.IsNullOrEmpty(filter.Type))
                data = data.Where(p => p.客戶分類 == filter.Type);
            data = SortBy(sortBy, ascent, data);

            return data;
        }

        private IQueryable<客戶資料> SortBy(string sortBy, bool ascent, IQueryable<客戶資料> data)
        {
            var dataEnum = data.AsEnumerable();
            if (ascent == true)
                dataEnum = dataEnum.OrderBy(p => p.GetType().GetProperty(sortBy)?.GetValue(p) ?? p.客戶名稱);

            else
                dataEnum = dataEnum.OrderByDescending(p => p.GetType().GetProperty(sortBy)?.GetValue(p) ?? p.客戶名稱);

            return  dataEnum.AsQueryable();
        }

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

        public override void Delete(客戶資料 entity)
        {
            entity.是否已刪除 = true;
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