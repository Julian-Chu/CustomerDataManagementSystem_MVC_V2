using System.ComponentModel.DataAnnotations;

namespace CustomerDataManagementSystem_MVC_V2.Models.ViewModels
{
    public class 客戶聯絡人批次更新ViewModel
    {
        public int Id { get; set; }
        public string 職稱 { get; set; }
        [Required]
        public string 手機 { get; set; }
        [Required]
        public string 電話 { get; set; }
    }
}