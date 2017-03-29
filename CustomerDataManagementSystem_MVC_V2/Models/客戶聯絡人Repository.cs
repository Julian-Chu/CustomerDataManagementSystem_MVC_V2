using System;
using System.Linq;
using System.Collections.Generic;
	
namespace CustomerDataManagementSystem_MVC_V2.Models
{
    public class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
    {
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(contact => contact.是否已刪除 == false );
        }
        internal 客戶聯絡人 Find(int? id)
        {
            return this.All().SingleOrDefault(c => c.Id == id);
        }
    }

    public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}