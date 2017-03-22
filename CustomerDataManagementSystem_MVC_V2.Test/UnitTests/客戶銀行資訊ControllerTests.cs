using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using NSubstitute;
using CustomerDataManagementSystem_MVC_V2.Models;
using System.Collections.Generic;
using CustomerDataManagementSystem_MVC_V2.Controllers;

namespace CustomerDataManagementSystem_MVC_V2.Test.UnitTests
{
    [TestClass]
    public class 客戶銀行資訊ControllerTests
    {
        IQueryable<客戶銀行資訊> banks;
        IDbSet<客戶銀行資訊> mockDbSet;
        客戶資料DBEntities mockDbContext;

        [TestInitialize]
        public void Initialize()
        {
            banks = new List<客戶銀行資訊>
            {
                new 客戶銀行資訊 {Id = 0, 是否已刪除 = false },
                new 客戶銀行資訊 {Id = 1, 是否已刪除 = false },
                new 客戶銀行資訊 {Id = 2, 是否已刪除 = false },
                new 客戶銀行資訊 {Id = 3, 是否已刪除 = false }
            }.AsQueryable();

            mockDbSet = Substitute.For<DbSet<客戶銀行資訊>, IDbSet<客戶銀行資訊>>();
            mockDbSet.Provider.Returns(banks.Provider);
            mockDbSet.Expression.Returns(banks.Expression);
            mockDbSet.ElementType.Returns(banks.ElementType);
            mockDbSet.GetEnumerator().Returns(banks.GetEnumerator());

            mockDbSet.Find(Arg.Any<int>()).Returns(
                callinfo =>
            {
                object[] idValues = callinfo.Arg<object[]>();
                int id = (int)idValues[0];
                return banks.SingleOrDefault(b => b.Id == id);

            });

            mockDbSet.Include("test").ReturnsForAnyArgs(mockDbSet);

            mockDbContext = Substitute.For<客戶資料DBEntities>();
            mockDbContext.客戶銀行資訊.Returns(mockDbSet);
        }

        [TestMethod]
        public void Index_不顯示已刪除欄位資料()
        {
            //Assign
            var controller = new 客戶銀行資訊Controller(mockDbContext);
            //Act
            banks.SingleOrDefault(b => b.Id == 0).是否已刪除 = true;
            ViewResult result = controller.Index() as ViewResult;
            var data = result.Model as List<客戶銀行資訊>;
            //Assert
            Assert.AreEqual(3, data.Count);

        }

        [TestMethod]
        public void DeleteConfrimed_只在是否已刪除欄位改成true()
        {
            //Assign
            var controller = new 客戶銀行資訊Controller(mockDbContext);
            //Act
            int id = 0;
            var result = controller.DeleteConfirmed(id);

            //Assert
            Assert.AreEqual(true, banks.SingleOrDefault(b => b.Id == id).是否已刪除);
        }
    }
}
