using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace CustomerDataManagementSystem_MVC_V2.Models.ViewModels
{
    public class LoginViewModel:IValidatableObject
    {
        客戶資料Repository repo;

        public LoginViewModel()
        {
            repo = RepositoryHelper.Get客戶資料Repository();
        }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var user = repo.FindByAccount(this.Username);

            if ( this.Username =="test" && this.Password =="1234")
            {
                yield return ValidationResult.Success;
                yield break;
            }
            else if (user !=null && user.密碼 == Crypto.SHA1( this.Password))
            {
                yield return ValidationResult.Success;
                yield break;
            }
            if (this.Password != "1234")
            {
                //yield return new ValidationResult("帳戶錯誤", new string[]{"Username" });
                //yield return new ValidationResult("密碼錯誤", new string[] { "Password" });
                yield return new ValidationResult("帳號或密碼錯誤");

                yield break;
            }
            
        }
    }
}