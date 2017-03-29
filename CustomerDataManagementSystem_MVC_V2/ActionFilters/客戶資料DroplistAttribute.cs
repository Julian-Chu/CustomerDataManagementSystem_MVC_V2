using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CustomerDataManagementSystem_MVC_V2.ActionFilters
{
    internal class 客戶資料DroplistAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            List<SelectListItem> items = CreateSelectListItems();
            filterContext.Controller.ViewBag.客戶分類 = new SelectList(items, "Value", "Text");
        }

        private List<SelectListItem> CreateSelectListItems()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            //foreach (var item in customerCategoryDic)
            //{
            //    items.Add(new SelectListItem { Text = item.Value, Value = item.Key });
            //}

            items.Add(new SelectListItem { Text = "資訊業", Value = "資訊業" });
            items.Add(new SelectListItem { Text = "服務業", Value = "服務業" });


            return items;
        }
    }
}