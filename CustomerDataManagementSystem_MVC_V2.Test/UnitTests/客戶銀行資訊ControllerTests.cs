using CustomerDataManagementSystem_MVC_V2.Controllers;
using CustomerDataManagementSystem_MVC_V2.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CustomerDataManagementSystem_MVC_V2.Test.UnitTests
{
    [TestClass]
    public class 客戶銀行資訊ControllerTests
    {
        private IQueryable<客戶銀行資訊> banks;
        private IDbSet<客戶銀行資訊> mockDbSet;
        private 客戶資料DBEntities mockDbContext;

        private 客戶銀行資訊Repository mockRepo;

        [TestInitialize]
        public void Initialize()
        {
            banks = new List<客戶銀行資訊>
            {
                new 客戶銀行資訊 {Id = 0, 銀行名稱 = "testBank0", 是否已刪除 = false , 客戶資料 = new 客戶資料 { Id=0, 客戶名稱="test0", Email="test0@testmail.com" , 是否已刪除=false} },
                new 客戶銀行資訊 {Id = 1, 銀行名稱 = "testBank0", 是否已刪除 = false , 客戶資料 = new 客戶資料 { Id=1, 客戶名稱="test1", Email="test1@testmail.com" , 是否已刪除=false} },
                new 客戶銀行資訊 {Id = 2, 銀行名稱 = "testBank0", 是否已刪除 = false , 客戶資料 = new 客戶資料 { Id=2, 客戶名稱="test2", Email="test2@testmail.com" , 是否已刪除=false} },
                new 客戶銀行資訊 {Id = 3, 銀行名稱 = "testBank0", 是否已刪除 = false , 客戶資料 = new 客戶資料 { Id=3, 客戶名稱="test3", Email="test3@testmail.com" , 是否已刪除=false} }
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


            mockDbContext = Substitute.For<客戶資料DBEntities>();
            mockDbContext.客戶銀行資訊.Returns(mockDbSet);

            mockRepo = Substitute.For<客戶銀行資訊Repository>();
            mockRepo.All().Returns(banks.Where(b=>b.是否已刪除==false));
            mockRepo.UnitOfWork = Substitute.For<IUnitOfWork>();
        }

        [TestMethod]
        public void Index_不顯示已刪除欄位資料()
        {
            //Assign
            var controller = new 客戶銀行資訊StubController(mockRepo);
            //Act
            var test = banks.SingleOrDefault(b => b.Id == 0);
            test.是否已刪除 = true;
            ViewResult result = controller.Index() as ViewResult;
            var data = result.Model as List<客戶銀行資訊>;
            //Assert
            Assert.AreEqual(3, data.Count);
        }

        [TestMethod]
        public void DeleteConfrimed_只在是否已刪除欄位改成true()
        {
            //Assign
            var controller = new 客戶銀行資訊StubController(mockRepo);
            //Act
            int id = 0;
            var result = controller.DeleteConfirmed(id);

            //Assert
            Assert.AreEqual(true, banks.SingleOrDefault(b => b.Id == id).是否已刪除);
        }
    }

    public class 客戶銀行資訊StubController:客戶銀行資訊Controller
    {
        public 客戶銀行資訊StubController(客戶銀行資訊Repository mockRepo)
        {
            this.bankRepo = mockRepo;
        }

        public 客戶銀行資訊StubController()
        {

        }
    }
}