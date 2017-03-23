using System;
using System.Linq;
using System.Collections.Generic;
	
namespace CustomerDataManagementSystem_MVC_V2.Models
{   
	public  class sysdiagramsRepository : EFRepository<sysdiagrams>, IsysdiagramsRepository
	{

	}

	public  interface IsysdiagramsRepository : IRepository<sysdiagrams>
	{

	}
}