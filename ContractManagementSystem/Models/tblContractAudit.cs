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
    
    public partial class tblContractAudit
    {
        public int ID { get; set; }
        public int AuditContractD { get; set; }
        public string AuditContractComments { get; set; }
        public Nullable<int> AuditEmployeeID { get; set; }
        public System.DateTime AuditLastReviewed { get; set; }
        public string AuditStatus { get; set; }
    
        public virtual tblContractMaster tblContractMaster { get; set; }
    }
}
