//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CustomerDataManagementSystem_MVC_V2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class 客戶銀行資訊
    {
        public int Id { get; set; }
        public int 客戶Id { get; set; }
        public string 銀行名稱 { get; set; }
        public int 銀行代碼 { get; set; }
        public Nullable<int> 分行代碼 { get; set; }
        public string 帳戶名稱 { get; set; }
        public string 帳戶號碼 { get; set; }
    
        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
