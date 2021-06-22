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
    
    public partial class tblContractModification
    {
        public int ContractID { get; set; }
        public string ContractName { get; set; }
        public string ContractType { get; set; }
        public string ContractCategory { get; set; }
        public string ContractSubCategory { get; set; }
        public byte[] UploadExistingFile { get; set; }
        public string UploadExistingContractFileName { get; set; }
        public string UploadExistingContractFileType { get; set; }
        public byte[] UploadSignOffContractFile { get; set; }
        public string UploadSignOffContractFileName { get; set; }
        public string UploadSignOffContractFileType { get; set; }
        public string Description { get; set; }
        public Nullable<int> Initiator { get; set; }
        public string InitiatorComments { get; set; }
        public string InitiatorStatus { get; set; }
        public string InitiatorDraft { get; set; }
        public string ContractDraft { get; set; }
        public string TemplateType { get; set; }
        public string Status { get; set; }
        public Nullable<int> OriginalContractID { get; set; }
        public string ContractModificationType { get; set; }
        public byte[] SupportingDoc1 { get; set; }
        public string SupportingDoc1FileName { get; set; }
        public string SupportingDoc1FileType { get; set; }
        public byte[] SupportingDoc2 { get; set; }
        public string SupportingDoc2FileName { get; set; }
        public string SupportingDoc2FileType { get; set; }
        public byte[] SupportingDoc3 { get; set; }
        public string SupportingDoc3FileName { get; set; }
        public string SupportingDoc3FileType { get; set; }
        public byte[] SupportingDoc4 { get; set; }
        public string SupportingDoc4FileName { get; set; }
        public string SupportingDoc4FileType { get; set; }
        public byte[] SupportingDoc5 { get; set; }
        public string SupportingDoc5FileName { get; set; }
        public string SupportingDoc5FileType { get; set; }
        public Nullable<System.DateTime> DateofInitiated { get; set; }
        public string Variables { get; set; }
        public Nullable<System.DateTime> LastReviewed { get; set; }
        public int Approver1ID { get; set; }
        public string Approver1Draft { get; set; }
        public string Approver1Comments { get; set; }
        public string Approver1Status { get; set; }
        public Nullable<System.DateTime> Approver1ReceivedOn { get; set; }
        public Nullable<System.DateTime> Approver1ApprovedOn { get; set; }
        public Nullable<int> Approver2ID { get; set; }
        public string Approver2Draft { get; set; }
        public string Approver2Comments { get; set; }
        public string Approver2Status { get; set; }
        public Nullable<System.DateTime> Approver2ReceivedOn { get; set; }
        public Nullable<System.DateTime> Approver2ApprovedOn { get; set; }
        public Nullable<int> Approver3ID { get; set; }
        public string Approver3Draft { get; set; }
        public string Approver3Comments { get; set; }
        public string Approver3Status { get; set; }
        public Nullable<System.DateTime> Approver3ReceivedOn { get; set; }
        public Nullable<System.DateTime> Approver3ApprovedOn { get; set; }
        public Nullable<int> Approver4ID { get; set; }
        public string Approver4Draft { get; set; }
        public string Approver4Comments { get; set; }
        public string Approver4Status { get; set; }
        public Nullable<System.DateTime> Approver4ReceivedOn { get; set; }
        public Nullable<System.DateTime> Approver4ApprovedOn { get; set; }
        public Nullable<int> Approver5ID { get; set; }
        public string Approver5Draft { get; set; }
        public string Approver5Comments { get; set; }
        public string Approver5Status { get; set; }
        public Nullable<System.DateTime> Approver5ReceivedOn { get; set; }
        public Nullable<System.DateTime> Approver5ApprovedOn { get; set; }
        public Nullable<int> Approver6ID { get; set; }
        public string Approver6Draft { get; set; }
        public string Approver6Comments { get; set; }
        public string Approver6Status { get; set; }
        public Nullable<System.DateTime> Approver6ReceivedOn { get; set; }
        public Nullable<System.DateTime> Approver6ApprovedOn { get; set; }
        public Nullable<int> Approver7ID { get; set; }
        public string Approver7Draft { get; set; }
        public string Approver7Comments { get; set; }
        public string Approver7Status { get; set; }
        public Nullable<System.DateTime> Approver7ReceivedOn { get; set; }
        public Nullable<System.DateTime> Approver7ApprovedOn { get; set; }
        public Nullable<int> Approver8ID { get; set; }
        public string Approver8Draft { get; set; }
        public string Approver8Comments { get; set; }
        public string Approver8Status { get; set; }
        public Nullable<System.DateTime> Approver8ReceivedOn { get; set; }
        public Nullable<System.DateTime> Approver8ApprovedOn { get; set; }
        public Nullable<int> Approver9ID { get; set; }
        public string Approver9Draft { get; set; }
        public string Approver9Comments { get; set; }
        public string Approver9Status { get; set; }
        public Nullable<System.DateTime> Approver9ReceivedOn { get; set; }
        public Nullable<System.DateTime> Approver9ApprovedOn { get; set; }
        public Nullable<int> Approver10ID { get; set; }
        public string Approver10Draft { get; set; }
        public string Approver10Comments { get; set; }
        public string Approver10Status { get; set; }
        public Nullable<System.DateTime> Approver10ReceivedOn { get; set; }
        public Nullable<System.DateTime> Approver10ApprovedOn { get; set; }
        public string PendingFrom { get; set; }
        public string NextApprover { get; set; }
        public string DefaultVariables { get; set; }
        public string RejectedBy { get; set; }
        public byte[] ModifiedSupportingDoc1 { get; set; }
        public string ModifiedSupportingDoc1FileName { get; set; }
        public string ModifiedSupportingDoc1FileType { get; set; }
        public string InEffectFrom { get; set; }
        public string InEffectTo { get; set; }
        public Nullable<int> ReviewerID { get; set; }
        public string PhysicalCopyLocation { get; set; }
        public Nullable<System.DateTime> ReviewedOn { get; set; }
        public string ReviewedComments { get; set; }
        public string Template { get; set; }
        public string Department { get; set; }
        public string SubDepartment { get; set; }
        public string Plant { get; set; }
    }
}
