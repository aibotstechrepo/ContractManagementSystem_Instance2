using ContractManagementSystem.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace ContractManagementSystem.Controllers
{
    public class ContractExpiry : IJob
    {
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        readonly string ApplicationLink = WebConfigurationManager.AppSettings["ApplicationLink"];
        Task IJob.Execute(IJobExecutionContext context)
        {
            DeligateUser();
            ContractExpiryMethod();
            return null;
        }

        void ContractExpiryMethod()
        {
            try
            {
                int Found = 1;
                var ContractTable = from tblContractMaster in db.tblContractMasters.Where(x => x.Status == "In Effect") select tblContractMaster;
                foreach (var eachContract in ContractTable)
                {
                    if (!string.IsNullOrWhiteSpace(eachContract.InEffectTo))
                    {
                       
                        var dateAndTime = DateTime.Now;
                        var date = dateAndTime.Date;

                        DateTime ExpieryDate = DateTime.ParseExact(eachContract.InEffectTo, "dd/MM/yyyy", null);

                        Found = DateTime.Compare(ExpieryDate, date);

                        if (Found < 0)
                        {
                            eachContract.Status = "Expired";
                        }
                    }
                    db.Entry(eachContract).State = EntityState.Modified;

                }
                db.SaveChanges();


                var ModifiedContractTable = from tblContractModification in db.tblContractModifications.Where(x => x.Status == "In Effect") select tblContractModification;
                foreach (var eachContract in ModifiedContractTable)
                {
                    if (!string.IsNullOrWhiteSpace(eachContract.InEffectTo))
                    {
                        var dateAndTime = DateTime.Now;
                        var date = dateAndTime.Date;

                        DateTime ExpieryDate = DateTime.ParseExact(eachContract.InEffectTo, "dd/MM/yyyy", null);

                        Found = DateTime.Compare(ExpieryDate, date);

                        if (Found < 0)
                        {
                            eachContract.Status = "Expired";
                        }
                    }
                    db.Entry(eachContract).State = EntityState.Modified;

                }
                db.SaveChanges();

            }
            catch(Exception Ex) { }
        }

        void DeligateUser()
        {
            try
            {
                var DeligationTable = from tblDeligationMatrix in db.tblDeligationMatrices select tblDeligationMatrix;
                foreach (var eachItem in DeligationTable)
                {
                    int Found = 1;
                    DateTime startDate = DateTime.ParseExact(eachItem.StartDate, "dd/MM/yyyy", null);
                    Found = DateTime.Compare(startDate, DateTime.Now);

                    if (Found <= 0)
                    {
                        Found = 1;
                        DateTime endDate = DateTime.ParseExact(eachItem.EndDate, "dd/MM/yyyy", null);
                        Found = DateTime.Compare(endDate, DateTime.Now);
                        if (Found < 0)
                        {
                            var ID = eachItem.ID;

                            tblDeligationMatrix Deligate = db.tblDeligationMatrices.Find(ID);
                            db.tblDeligationMatrices.Remove(Deligate);

                        }
                        else
                        {
                            try
                            {
                                DeligationMethod(Convert.ToInt32(eachItem.DeligateFrom), Convert.ToInt32(eachItem.DeligateTo));

                            }
                            catch
                            { }
                        }

                    }

                }
                db.SaveChanges();
            }
            catch(Exception Ex) {
               // throw Ex;
            }
        }

        bool DeligationMethod(int DeligationFromID = 0, int DeligationToID=0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            //try
            //{
            //    CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
            //    CurrentUserName = User.Identity.Name.Split('|')[0];
            //}
            //catch { }
           
            
            try
            {
                using (var context = new ContractManagementSystemDBEntities())
                {
                   var Template = from tblTemplateMaster in context.tblTemplateMasters select tblTemplateMaster;
                    foreach (var item in Template)
                    {
                        string OldValues = "";
                        string NewValues = "";

                        if (item.Status == "Pending Approval" || item.Status == "Rejected")
                        {
                            if ((item.Approver1ID == DeligationFromID) && (item.Approver1Status == "Pending Approval" || item.Approver1Status == "Rejected" || item.Approver1Status == null))
                            {

                                item.Approver1ID = DeligationToID;

                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.TemplateID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 1 in Template (" + item.TemplateID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                            }
                            if ((item.Approver2ID == DeligationFromID) && (item.Approver2Status == "Pending Approval" || item.Approver2Status == "Rejected" || item.Approver2Status == null))
                            {

                                item.Approver2ID = DeligationToID;

                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.TemplateID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 2 in Template (" + item.TemplateID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                

                            }
                            if ((item.Approver3ID == DeligationFromID) && (item.Approver3Status == "Pending Approval" || item.Approver3Status == "Rejected" || item.Approver3Status == null))
                            {

                                item.Approver3ID = DeligationToID;

                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.TemplateID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 3 in Template (" + item.TemplateID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                            }
                            if ((item.Approver4ID == DeligationFromID) && (item.Approver4Status == "Pending Approval" || item.Approver4Status == "Rejected" || item.Approver4Status == null))
                            {
                                item.Approver4ID = DeligationToID;

                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.TemplateID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 4 in Template (" + item.TemplateID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                            }
                            if ((item.Approver5ID == DeligationFromID) && (item.Approver5Status == "Pending Approval" || item.Approver5Status == "Rejected" || item.Approver5Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.TemplateID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 5 in Template (" + item.TemplateID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                

                                item.Approver5ID = DeligationToID;
                            }
                            if ((item.Approver6ID == DeligationFromID) && (item.Approver6Status == "Pending Approval" || item.Approver6Status == "Rejected" || item.Approver6Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.TemplateID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 6 in Template (" + item.TemplateID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                

                                item.Approver6ID = DeligationToID;
                            }
                            if ((item.Approver7ID == DeligationFromID) && (item.Approver7Status == "Pending Approval" || item.Approver7Status == "Rejected" || item.Approver7Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.TemplateID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 7 in Template (" + item.TemplateID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                

                                item.Approver7ID = DeligationToID;
                            }
                            if ((item.Approver8ID == DeligationFromID) && (item.Approver8Status == "Pending Approval" || item.Approver8Status == "Rejected" || item.Approver8Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.TemplateID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 8 in Template (" + item.TemplateID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                

                                item.Approver8ID = DeligationToID;
                            }
                            if ((item.Approver9ID == DeligationFromID) && (item.Approver9Status == "Pending Approval" || item.Approver9Status == "Rejected" || item.Approver9Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.TemplateID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 9 in Template (" + item.TemplateID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                

                                item.Approver9ID = DeligationToID;
                            }
                            if ((item.Approver10ID == DeligationFromID) && (item.Approver10Status == "Pending Approval" || item.Approver10Status == "Rejected" || item.Approver10Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.TemplateID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 10 in Template (" + item.TemplateID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                

                                item.Approver10ID = DeligationToID;
                            }
                            if (item.NextApprover == DeligationFromID.ToString())
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.TemplateID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Next Approver in Template (" + item.TemplateID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                

                                item.NextApprover = DeligationToID.ToString();
                            }
                            if (item.Initiator == DeligationFromID)
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.TemplateID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Initiator in Template (" + item.TemplateID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                

                                item.Initiator = DeligationToID;
                            }
                            if (item.RejectedBy == DeligationFromID.ToString())
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.TemplateID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Rejected By in Template (" + item.TemplateID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                

                                item.RejectedBy = DeligationToID.ToString();
                            }

                        }
                        context.Entry(item).State = EntityState.Modified;

                    }
                    context.SaveChanges();

                    var Contract = from tblContractMaster in context.tblContractMasters select tblContractMaster;
                    foreach (var item in Contract)
                    {
                        string OldValues1 = "";
                        string NewValues1 = "";
                        if (item.Status == "Pending Approval" || item.Status == "Rejected")
                        {
                            if ((item.Approver1ID == DeligationFromID) && (item.Approver1Status == "Pending Approval" || item.Approver1Status == "Rejected" || item.Approver1Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 1 in Contract (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                

                                item.Approver1ID = DeligationToID;
                                //tblTemplateMaster eachData = context.tblTemplateMasters.Find(item.TemplateID);
                                //eachData.Approver1ID = DeligationToID;
                                //context.Entry(eachData).State = EntityState.Modified;
                            }
                            if ((item.Approver2ID == DeligationFromID) && (item.Approver2Status == "Pending Approval" || item.Approver2Status == "Rejected" || item.Approver2Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 2 in Contract (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                

                                item.Approver2ID = DeligationToID;
                            }
                            if ((item.Approver3ID == DeligationFromID) && (item.Approver3Status == "Pending Approval" || item.Approver3Status == "Rejected" || item.Approver3Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 3 in Contract (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver3ID = DeligationToID;
                            }
                            if ((item.Approver4ID == DeligationFromID) && (item.Approver4Status == "Pending Approval" || item.Approver4Status == "Rejected" || item.Approver4Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 4 in Contract (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver4ID = DeligationToID;
                            }
                            if ((item.Approver5ID == DeligationFromID) && (item.Approver5Status == "Pending Approval" || item.Approver5Status == "Rejected" || item.Approver5Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 5 in Contract (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver5ID = DeligationToID;
                            }
                            if ((item.Approver6ID == DeligationFromID) && (item.Approver6Status == "Pending Approval" || item.Approver6Status == "Rejected" || item.Approver6Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 6 in Contract (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver6ID = DeligationToID;
                            }
                            if ((item.Approver7ID == DeligationFromID) && (item.Approver7Status == "Pending Approval" || item.Approver7Status == "Rejected" || item.Approver7Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 7 in Contract (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver7ID = DeligationToID;
                            }
                            if ((item.Approver8ID == DeligationFromID) && (item.Approver8Status == "Pending Approval" || item.Approver8Status == "Rejected" || item.Approver8Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 8 in Contract (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver8ID = DeligationToID;
                            }
                            if ((item.Approver9ID == DeligationFromID) && (item.Approver9Status == "Pending Approval" || item.Approver9Status == "Rejected" || item.Approver9Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 9 in Contract (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver9ID = DeligationToID;
                            }
                            if ((item.Approver10ID == DeligationFromID) && (item.Approver10Status == "Pending Approval" || item.Approver10Status == "Rejected" || item.Approver10Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 10 in Contract (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver10ID = DeligationToID;
                            }
                            if (item.NextApprover == DeligationFromID.ToString())
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Next Approver in Contract (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.NextApprover = DeligationToID.ToString();
                            }
                            if (item.Initiator == DeligationFromID)
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Initiator in Contract (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Initiator = DeligationToID;
                            }
                            if (item.RejectedBy == DeligationFromID.ToString())
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Rejected By in Contract (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.RejectedBy = DeligationToID.ToString();
                            }

                        }
                        context.Entry(item).State = EntityState.Modified;

                       
                    }
                    context.SaveChanges();

                    var ContractModification = from tblContractModification in context.tblContractModifications select tblContractModification;
                    foreach (var item in ContractModification)
                    {

                        if (item.Status == "Pending Approval" || item.Status == "Rejected")
                        {
                            if ((item.Approver1ID == DeligationFromID) && (item.Approver1Status == "Pending Approval" || item.Approver1Status == "Rejected" || item.Approver1Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 1 in Contract Modification (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver1ID = DeligationToID;
                                //tblTemplateMaster eachData = context.tblTemplateMasters.Find(item.TemplateID);
                                //eachData.Approver1ID = DeligationToID;
                                //context.Entry(eachData).State = EntityState.Modified;
                            }
                            if ((item.Approver2ID == DeligationFromID) && (item.Approver2Status == "Pending Approval" || item.Approver2Status == "Rejected" || item.Approver2Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 2 in Contract Modification (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver2ID = DeligationToID;
                            }
                            if ((item.Approver3ID == DeligationFromID) && (item.Approver3Status == "Pending Approval" || item.Approver3Status == "Rejected" || item.Approver3Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 3 in Contract Modification (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver3ID = DeligationToID;
                            }
                            if ((item.Approver4ID == DeligationFromID) && (item.Approver4Status == "Pending Approval" || item.Approver4Status == "Rejected" || item.Approver4Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 4 in Contract Modification (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver4ID = DeligationToID;
                            }
                            if ((item.Approver5ID == DeligationFromID) && (item.Approver5Status == "Pending Approval" || item.Approver5Status == "Rejected" || item.Approver5Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 5 in Contract Modification (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver5ID = DeligationToID;
                            }
                            if ((item.Approver6ID == DeligationFromID) && (item.Approver6Status == "Pending Approval" || item.Approver6Status == "Rejected" || item.Approver6Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 6 in Contract Modification (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver6ID = DeligationToID;
                            }
                            if ((item.Approver7ID == DeligationFromID) && (item.Approver7Status == "Pending Approval" || item.Approver7Status == "Rejected" || item.Approver7Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 7 in Contract Modification (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver7ID = DeligationToID;
                            }
                            if ((item.Approver8ID == DeligationFromID) && (item.Approver8Status == "Pending Approval" || item.Approver8Status == "Rejected" || item.Approver8Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 8 in Contract Modification (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver8ID = DeligationToID;
                            }
                            if ((item.Approver9ID == DeligationFromID) && (item.Approver9Status == "Pending Approval" || item.Approver9Status == "Rejected" || item.Approver9Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 9 in Contract Modification (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver9ID = DeligationToID;
                            }
                            if ((item.Approver10ID == DeligationFromID) && (item.Approver10Status == "Pending Approval" || item.Approver10Status == "Rejected" || item.Approver10Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 10 in Contract Modification (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Approver10ID = DeligationToID;
                            }
                            if (item.NextApprover == DeligationFromID.ToString())
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Next Approver in Contract Modification (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.NextApprover = DeligationToID.ToString();
                            }
                            if (item.Initiator == DeligationFromID)
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Initiator in Contract Modification (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.Initiator = DeligationToID;
                            }
                            if (item.RejectedBy == DeligationFromID.ToString())
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Rejected By in Contract Modification (" + item.ContractID + ")";
                                logs.ChangedFrom = DeligationFromID.ToString();
                                logs.ChangedTo = DeligationToID.ToString();
                                logs.DateandTime = DateTime.Now.ToString();
                                context.tblDeligationLogs.Add(logs);
                                
                                item.RejectedBy = DeligationToID.ToString();
                            }

                        }
                        context.Entry(item).State = EntityState.Modified;

                       
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception Ex)
            { }
            return false;
        }
    }
}