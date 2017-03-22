using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerDataManagementSystem_MVC_V2.Models.ViewModels
{
    public class LoginViewModel:IValidatableObject
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Username != "test")
            {
                yield return new ValidationResult("帳戶錯誤", new string[]{"Username" });
            }
            if (this.Password != "1234")
            {
                yield return new ValidationResult("密碼錯誤", new string[] {"Password" });
                yield break;
            }
            yield return ValidationResult.Success;
        }
    }
}