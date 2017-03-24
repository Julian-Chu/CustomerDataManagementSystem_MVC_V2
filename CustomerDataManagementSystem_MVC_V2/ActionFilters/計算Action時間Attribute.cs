using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerDataManagementSystem_MVC_V2.ActionFilters
{
    public class 計算Action時間Attribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.StartTime = DateTime.Now;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.EndTime = DateTime.Now;
            filterContext.Controller.ViewBag.DurationTime =
                 filterContext.Controller.ViewBag.EndTime - filterContext.Controller.ViewBag.StartTime;

            TimeSpan time = (TimeSpan)filterContext.Controller.ViewBag.DurationTime;
            

            Debug.WriteLine("Duraction Time:"+ time.ToString());
        }

    }
}