using CustomerDataManagementSystem_MVC_V2.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NSubstitute;
using System.Data.Entity;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace CustomerDataManagementSystem_MVC_V2.Test.UnitTests
{
    [TestClass]
    public class 客戶聯絡人ModelTests
    {
        List<客戶聯絡人> contacts;
        客戶資料DBEntities mockDbContext;
        IDbSet<客戶聯絡人> mockDbSet;

        [TestInitialize]
        public void Initialze()
        {
            contacts = new List<客戶聯絡人>()
            {
                new 客戶聯絡人 { Id = 1, 客戶Id = 1, Email ="test@test.com" }
            };

            mockDbContext = Substitute.For<客戶資料DBEntities>();
            mockDbSet = Substitute.For<DbSet<客戶聯絡人>, IDbSet<客戶聯絡人>>();
            mockDbSet.Provider.Returns(contacts.AsQueryable().Provider);
            mockDbSet.Expression.Returns(contacts.AsQueryable().Expression);
            mockDbSet.ElementType.Returns(contacts.AsQueryable().ElementType);
            mockDbSet.GetEnumerator().Returns(contacts.AsQueryable().GetEnumerator());

            mockDbContext.客戶聯絡人.Returns(mockDbSet);
        }

        [TestMethod]
        public void 新建客戶聯絡人資料時_輸入同一客戶下已存在email_拋出錯誤訊息()
        {
            //Assign
            var model = new 客戶聯絡人(mockDbContext)
            {
                Id = 0,
                客戶Id = 1,
                Email = "test@test.com"
            };
            //Act
            var result = model.Validate(null).ToArray();
            //Assert

            Assert.IsTrue(result[0].ErrorMessage.Contains("已存在"));
        }

        [TestMethod]
        public void 更新資料時_同一客戶下不同聯絡人輸入已存在email_拋出錯誤訊息()
        {
            //Assign
            var model = new 客戶聯絡人(mockDbContext)
            {
                Id = 2,
                客戶Id = 1,
                Email = "test@test.com"
            };
            //Act
            var result = model.Validate(null).ToArray();
            //Assert

            Assert.IsTrue(result[0].ErrorMessage.Contains("已存在"));
        }
    }
}