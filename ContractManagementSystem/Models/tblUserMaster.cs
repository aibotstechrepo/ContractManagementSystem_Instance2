//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContractManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblUserMaster
    {
        public int UserID { get; set; }
        public int UserEmployeeID { get; set; }
        public string UserEmployeeName { get; set; }
        public string UserEmployeeEmail { get; set; }
        public string UserEmployeeDesignation { get; set; }
        public string UserCategory { get; set; }
        public string UserSubCategory { get; set; }
        public bool UserRoleInitiator { get; set; }
        public bool UserRoleApprover { get; set; }
        public bool UserRoleLegal { get; set; }
        public bool UserRoleFinance { get; set; }
        public bool UserRoleAdmin { get; set; }
        public Nullable<bool> UserRoleReviewer { get; set; }
        public string UserStatus { get; set; }
        public Nullable<int> UserHodEmployeeID { get; set; }
        public string UserHodEmployeeName { get; set; }
        public string UserHodEmployeeEmailAddress { get; set; }
        public string UserPlant { get; set; }
        public bool UserRoleFinance2 { get; set; }
    }
}
