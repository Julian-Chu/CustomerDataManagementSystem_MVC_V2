﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CustomerDataManagementSystem_MVC_V2.Controllers;
using CustomerDataManagementSystem_MVC_V2.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CustomerDataManagementSystem_MVC_V2.Test.UnitTests
{
    [TestClass]
    public class 客戶聯絡人ControllerTests
    {
        private IQueryable<客戶聯絡人> contacts;
        private IDbSet<客戶聯絡人> mockDbSet;
        private 客戶資料DBEntities mockDBContext;
        private 客戶聯絡人Repository mockRepo;

        [TestInitialize]
        public void Initialze()
        {
            contacts = new List<客戶聯絡人>
            {
                new 客戶聯絡人 { Id = 0, 姓名="test0", 是否已刪除 = false, 客戶資料 = new 客戶資料 { Id=0, 客戶名稱="test0", Email="test0@testmail.com" , 是否已刪除=false} },
                new 客戶聯絡人 { Id = 1, 姓名="test1", 是否已刪除 = false, 客戶資料 = new 客戶資料 { Id=1, 客戶名稱="test1", Email="test1@testmail.com" , 是否已刪除=false} },
                new 客戶聯絡人 { Id = 2, 姓名="test2", 是否已刪除 = false, 客戶資料 = new 客戶資料 { Id=2, 客戶名稱="test2", Email="test2@testmail.com" , 是否已刪除=false} },
                new 客戶聯絡人 { Id = 3, 姓名="test3", 是否已刪除 = false, 客戶資料 = new 客戶資料 { Id=3, 客戶名稱="test3", Email="test3@testmail.com" , 是否已刪除=false} }
            }.AsQueryable();

            mockDbSet = Substitute.For<DbSet<客戶聯絡人>, IDbSet<客戶聯絡人>>();
            mockDbSet.Provider.Returns(contacts.Provider);
            mockDbSet.Expression.Returns(contacts.Expression);
            mockDbSet.ElementType.Returns(contacts.ElementType);
            mockDbSet.GetEnumerator().Returns(contacts.GetEnumerator());

            mockDbSet.Find(Arg.Any<int>()).Returns(callinfo =>
            {
                var idValues = callinfo.Arg<object[]>();
                int id = (int)idValues[0];
                return contacts.SingleOrDefault(c => c.Id == id);
            });

            mockDbSet.Include("test").ReturnsForAnyArgs(mockDbSet);

            mockDBContext = Substitute.For<客戶資料DBEntities>();
            mockDBContext.客戶聯絡人.Returns(mockDbSet);

            mockRepo = Substitute.For<客戶聯絡人Repository>();
            mockRepo.All().Returns(contacts.Where(contact => contact.是否已刪除 == false));
            mockRepo.UnitOfWork = Substitute.For<IUnitOfWork>();
        }

        [TestMethod]
        public void Index_不會顯示已經標示刪除的資料()
        {
            //Assign
            //var controller = new 客戶聯絡人Controller(mockDBContext);
            var controller = new 客戶聯絡人Controller(mockRepo);

            //Act
            contacts.FirstOrDefault(c => c.Id == 0).是否已刪除 = true;
            var result = controller.Index(null) as ViewResult;
            var data = result.Model as List<客戶聯絡人>;
            //Assert
            Assert.AreEqual(3, data.Count);
        }

        [TestMethod]
        public void DeleteConfirmed_RedirectToIndex()
        {
            //Assign
            //var controller = new 客戶聯絡人Controller(mockDBContext);
            var controller = new 客戶聯絡人Controller(mockRepo);

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
            //var controller = new 客戶聯絡人Controller(mockDBContext);
            var controller = new 客戶聯絡人Controller(mockRepo);

            //Act
            int id = 0;
            var result = controller.DeleteConfirmed(id);
            //Assert
            Assert.AreEqual(4, contacts.Count());
            var contactList = contacts.ToList();
            Assert.AreEqual(true, contactList[0].是否已刪除);
        }
    }
}