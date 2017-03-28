using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CustomerDataManagementSystem_MVC_V2.Models;
using System.Linq;
using System.Data.Entity;
using NSubstitute;
using CustomerDataManagementSystem_MVC_V2.Controllers;
using System.Web.Mvc;

namespace CustomerDataManagementSystem_MVC_V2.Test.UnitTests
{
    [TestClass]
    public class 客戶資料ControllerTests
    {
        List<客戶資料> customers;
        IDbSet<客戶資料> mockDBSet;
        客戶資料DBEntities mockDBContext;

        客戶資料Repository mockRepo;
        IUnitOfWork mockUnitOfWork;
        [TestInitialize]
        public void Initialze()
        {
            customers = new List<客戶資料>()
            {
                new 客戶資料 { Id=0, 客戶名稱="test0", Email="test0@testmail.com" , 是否已刪除=false},
                new 客戶資料 { Id=1, 客戶名稱="test1", Email="test1@testmail.com" , 是否已刪除=false},
                new 客戶資料 { Id=2, 客戶名稱="test2", Email="test2@testmail.com" , 是否已刪除=false},
                new 客戶資料 { Id=3, 客戶名稱="test3", Email="test3@testmail.com" , 是否已刪除=false}
            };

            //mockDBSet = Substitute.For<DbSet<客戶資料>, IDbSet<客戶資料>>();
            //mockDBSet.Provider.Returns(customers.AsQueryable().Provider);
            //mockDBSet.Expression.Returns(customers.AsQueryable().Expression);
            //mockDBSet.ElementType.Returns(customers.AsQueryable().ElementType);
            //mockDBSet.GetEnumerator().Returns(customers.AsQueryable().GetEnumerator());

            //mockDBSet.Find(Arg.Any<int>()).Returns(callinfo =>
            //{
            //    //int[] idValues = callinfo.Arg<int[]>();  // failed
            //    //object[] idValues = callinfo.Arg<object[]>();
            //    var idValues = callinfo.Arg<object[]>();

            //    int tempId = (int)idValues[0];
            //    return customers.FirstOrDefault(p => p.Id == tempId);
            //});

            //mockDBContext = Substitute.For<客戶資料DBEntities>();
            //mockDBContext.客戶資料.Returns(mockDBSet);

            mockRepo = Substitute.For<客戶資料Repository>();
            mockRepo.All().Returns(customers.AsQueryable());
            mockRepo.When(x => x.Delete(Arg.Any<客戶資料>())).Do(arg =>
            {
                var entity = (客戶資料)arg[0];
                entity.是否已刪除 = true;
            });


            mockUnitOfWork = Substitute.For<IUnitOfWork>();
            mockRepo.UnitOfWork = mockUnitOfWork;

            //mockRepo.Find(Arg.Any<int>()).Returns(customers[0]);

            //mockRepo.Find(Arg.Any<int>()).Returns(callinfo =>
            //{
            //    var idValues = callinfo.Arg<object[]>();
            //    int id = (int)idValues[0];
            //    return customers.FirstOrDefault(p => p.Id == id);
            //});
        }

        [TestMethod]
        public void Index_不會顯示已經標示刪除的資料()
        {
            //Assign           
            var controller = new 客戶資料Controller(mockRepo);
            //Act
            customers[0].是否已刪除 = true;
            ViewResult result = controller.Index() as ViewResult;
            List<客戶資料> data = result.Model as List<客戶資料>;
            //Assert
            Assert.AreEqual(3, data.Count);

        }

        [TestMethod]
        public void DeletedConfirmed_RedirectToIndex()
        {
            //Assign
            var controller = new 客戶資料Controller(mockRepo);
            //Act
            int id = 0;
            var result = controller.DeleteConfirmed(id);
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = result as RedirectToRouteResult;
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
        }

        [TestMethod]
        public void DeleteConfirmed_只在是否已刪除欄位改成是()
        {
            //Assign
            var controller = new 客戶資料Controller(mockRepo);
            //Act
            int id = 0;
            var result = controller.DeleteConfirmed(id);
            //Assert
            Assert.AreEqual(4, customers.Count);
            Assert.AreEqual(true, customers[0].是否已刪除);
        }
    }
}
