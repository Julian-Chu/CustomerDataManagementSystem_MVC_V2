using System;
using System.Linq;
using System.Collections.Generic;
	
namespace CustomerDataManagementSystem_MVC_V2.Models
{
    public class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
    {
        internal 客戶銀行資訊 Find(int? id)
        {
            return this.All().SingleOrDefault(b => b.Id == id);
        }

        public override IQueryable<客戶銀行資訊> All()
        {
            return this.All().Where(b => b.是否已刪除 == false);
        }
    }

    public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}