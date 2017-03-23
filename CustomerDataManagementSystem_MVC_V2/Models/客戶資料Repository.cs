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
    }

    public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}