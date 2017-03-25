using ClosedXML.Excel;
using CustomerDataManagementSystem_MVC_V2.Models;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace CustomerDataManagementSystem_MVC_V2.Controllers
{
    public class FileController : Controller
    {
        private 客戶資料Repository 客戶資料repo;

        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult 客戶資料Excel()

        {
            客戶資料repo = RepositoryHelper.Get客戶資料Repository();
            List<客戶資料> items = 客戶資料repo.All().ToList();
            var dt = ConvertToDataTable(items);

            //var wb = new XLWorkbook();
            //var ws = wb.Worksheets.Add("客戶資料");
            //ws.Cell(1, 1).Value = dataTable.AsEnumerable();
            //wb.SaveAs("Content/Excel/客戶資料.xlsx");
            //return File(Server.MapPath(@"~/Content/Excel/\客戶資料.xlsx"), "application / vnd.openxmlformats - officedocument.spreadsheetml.sheet","test");

            ExportExcel(dt, "客戶資料");
            return RedirectToAction("客戶資料Excel", "File");
        }

        public ActionResult 客戶聯絡人Excel()
        {
            客戶聯絡人Repository repo = RepositoryHelper.Get客戶聯絡人Repository();
            List<客戶聯絡人> items = repo.All().ToList();
            var dt = ConvertToDataTable(items);
            ExportExcel(dt, "客戶聯絡人");
            return RedirectToAction("客戶聯絡人Excel", "File");
        }

        public ActionResult 客戶銀行資訊Excel()
        {
            客戶銀行資訊Repository repo = RepositoryHelper.Get客戶銀行資訊Repository();
            List<客戶銀行資訊> items = repo.All().ToList();
            var dt = ConvertToDataTable(items);
            ExportExcel(dt, "客戶銀行資訊");
            return RedirectToAction("客戶銀行資訊Excel", "File");
        }

        public ActionResult 客戶資料統計Excel()
        {
            客戶資料統計Repository repo = RepositoryHelper.Get客戶資料統計Repository();
            List<客戶資料統計> items = repo.All().ToList();
            var dt = ConvertToDataTable(items);
            ExportExcel(dt, "客戶資料統計");
            return RedirectToAction("客戶資料統計Excel", "File");
        }


        private void ExportExcel(DataTable dt, string filename)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", $"attachment;filename= {filename}.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        public DataTable ConvertToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, System.Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }
    }
}