namespace CustomerDataManagementSystem_MVC_V2.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(客戶銀行資訊MetaData))]
    public partial class 客戶銀行資訊
    {
    }

    public partial class 客戶銀行資訊MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int 客戶Id { get; set; }

        [StringLength(50, ErrorMessage = "Max Length is 50 words")]
        [Required]
        [Display(Name = "Bank")]
        public string 銀行名稱 { get; set; }
        [Required]
        [Display(Name = "Bank Code")]
        public int 銀行代碼 { get; set; }
        [Display(Name = "Branch Code")]
        public Nullable<int> 分行代碼 { get; set; }

        [StringLength(50, ErrorMessage = "Max Length is 50 words")]
        [Required]
        [Display(Name = "Account Owner")]
        public string 帳戶名稱 { get; set; }

        [StringLength(20, ErrorMessage = "Max Length is 50 words")]
        [Required]
        [Display(Name = "Account Number")]
        public string 帳戶號碼 { get; set; }

        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
