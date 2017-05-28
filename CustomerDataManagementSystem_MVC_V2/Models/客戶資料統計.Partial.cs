namespace CustomerDataManagementSystem_MVC_V2.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(客戶資料統計MetaData))]
    public partial class 客戶資料統計
    {
    }

    public partial class 客戶資料統計MetaData
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Max Length is 50 words")]
        [Required]
        [Display(Name = "Customer")]
        public string 客戶名稱 { get; set; }
        [Display(Name = "number of contacts")]
        public Nullable<int> 聯絡人數量 { get; set; }
        [Display(Name = "number of bank accounts")]
        public Nullable<int> 銀行帳戶數量 { get; set; }
    }
}
