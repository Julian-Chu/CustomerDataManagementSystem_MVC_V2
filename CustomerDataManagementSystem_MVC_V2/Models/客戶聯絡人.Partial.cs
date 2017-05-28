namespace CustomerDataManagementSystem_MVC_V2.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人 : IValidatableObject
    {
        客戶資料DBEntities db;

        public 客戶聯絡人()
        {
            db = new 客戶資料DBEntities();
        }

        public 客戶聯絡人(客戶資料DBEntities mockDbContext)
        {
            db = mockDbContext;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Id == 0)
            {
                //Create
                if (db.客戶聯絡人.Where(c => c.Email == this.Email && c.客戶Id == this.客戶Id).Any())
                    yield return new ValidationResult("Email已存在");
                yield break;
            }
            else
            {
                //Update
                if (db.客戶聯絡人.Where(c => c.Email == this.Email && c.Id != this.Id && c.客戶Id == this.客戶Id).Any())
                {
                    yield return new ValidationResult("Email已存在");
                }
                yield break;
            }
            //yield return ValidationResult.Success;
        }
    }

    public partial class 客戶聯絡人MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int 客戶Id { get; set; }

        [StringLength(50, ErrorMessage = "Max Length is 50 words")]
        [Required]
        [Display(Name = "Title")]
        public string 職稱 { get; set; }

        [StringLength(50, ErrorMessage = "Max Length is 50 words")]
        [Required]
        [Display(Name = "Name")]
        public string 姓名 { get; set; }

        [StringLength(250, ErrorMessage = "Max Length is 250 words")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "Max Length is 50 words")]
        [RegularExpression(@"\d{4}-\d{6}", ErrorMessage = "Incorrect Format: Please input XXXX-XXXXXX")]
        [Display(Name = "Mobile")]
        public string 手機 { get; set; }

        [StringLength(50, ErrorMessage = "Max Length is 50 words")]
        [Display(Name = "Tel")]
        public string 電話 { get; set; }

        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
