using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContractManagementSystem.Models;
using NLog;

namespace ContractManagementSystem.Controllers
{
    public class DeligationController : Controller
    {
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();

        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        // GET: AlertsAndNotification
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue // Use this value to set your maximum size for all of your Requests
            };
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult getHodDetails(string EmployeeDetails, string OptionToSearch)
        {
            int CurrentUser = 0;
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
            }
            catch { }

            var EmployeePlant = "";
            EmployeePlant = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == CurrentUser) select tblUserMaster.UserPlant).First();



            OptionToSearch = OptionToSearch.Trim();
            if (OptionToSearch == "Employee ID")
            {
                var result =/* from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserPlant == EmployeePlant).Where(x => x.UserEmployeeID.ToString().Contains(EmployeeDetails)).Select(x => new {
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserEmployeeDesignation,
                    x.UserCategory,
                    x.UserSubCategory,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleFinance2,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserEmployeeEmail,
                    x.UserStatus
                }); /*select tblUserMaster;*/
                return Json(result);
            }
            else if (OptionToSearch == "Employee Name")
            {
                var result = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserPlant == EmployeePlant).Where(x => x.UserEmployeeName.Contains(EmployeeDetails)).Select(x => new {
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserEmployeeDesignation,
                    x.UserCategory,
                    x.UserSubCategory,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleFinance2,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserEmployeeEmail,
                    x.UserStatus
                }); /*select tblUserMaster;*/
                return Json(result);
            }
            else if (OptionToSearch == "Employee Email Address")
            {
                var result = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserPlant == EmployeePlant).Where(x => x.UserEmployeeEmail.Contains(EmployeeDetails)).Select(x => new {
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserEmployeeDesignation,
                    x.UserCategory,
                    x.UserSubCategory,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleFinance2,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserEmployeeEmail,
                    x.UserStatus
                }); /*select tblUserMaster;*/
                return Json(result);
            }
            else
            {
                return Json("");
            }
        }


        [HttpPost]
        public JsonResult GetEmployeeDetails(string employeeid)
        {
            Logger.Info("Attempt Deligation GetEmployeeDetails");

            try
            {
                string[] UserInfo = { "", "User Not Found", "User Not Found", "User Not Found", "User Not Found", "", "", "" };

                if (!string.IsNullOrWhiteSpace(employeeid))
                {
                    employeeid = employeeid.Trim();
                    int EMPID = Convert.ToInt32(employeeid);

                    Logger.Info("Accessed DB, Checking UserMaster Details: EmployeeID match");
                    var result = db.tblUserMasters.Where(x => x.UserEmployeeID == EMPID).Select(x => new {x.UserEmployeeID, x.UserEmployeeName, x.UserEmployeeEmail, x.UserRoleAdmin, x.UserRoleApprover, x.UserRoleFinance, x.UserRoleInitiator, x.UserRoleLegal, x.UserRoleReviewer });/*select tblUserMaster;*/

                    foreach (var r in result)
                    {
                        string UserRole = "";
                        Logger.Info("Accessed DB, Checking User Details: Checking Status");
                        if (r.UserRoleAdmin == true)
                        {
                            Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                            UserRole = UserRole + "Admin";
                        }
                        Logger.Info("Accessed DB, Checking User Details: Checking Status");
                        if (r.UserRoleInitiator == true)
                        {

                            if (UserRole.Length > 0)
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                                UserRole = UserRole + ", Initiator";
                            }
                            else
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Not Active");
                                UserRole = UserRole + "Initiator";
                            }
                        }
                        Logger.Info("Accessed DB, Checking User Details: Checking Status");
                        if (r.UserRoleApprover == true)
                        {
                            if (UserRole.Length > 0)
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                                UserRole = UserRole + ", Approver";
                            }
                            else
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Not Active");
                                UserRole = UserRole + "Approver";
                            }
                        }
                        Logger.Info("Accessed DB, Checking User Details: Checking Status");
                        if (r.UserRoleFinance == true)
                        {
                            if (UserRole.Length > 0)
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                                UserRole = UserRole + ", Finance Approver";
                            }
                            else
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Not Active");
                                UserRole = UserRole + "Finance Approver";
                            }
                        }

                        Logger.Info("Accessed DB, Checking User Details: Checking Status");
                        if (r.UserRoleLegal == true)
                        {
                            if (UserRole.Length > 0)
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                                UserRole = UserRole + ", Legal Approver";
                            }
                            else
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Not Active");
                                UserRole = UserRole + "Legal Approver";
                            }
                        }
                        UserInfo[0] = "success";
                        UserInfo[1] = r.UserEmployeeName;
                        UserInfo[2] = r.UserEmployeeEmail;
                       
                        UserInfo[3] = UserRole;
                      
                        Logger.Info("Accessed DB, Checking UserMaster Details: User Details Found");
                        return Json(UserInfo);
                    }
                    return Json(UserInfo);
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Contract' Controller , 'GetUserById' Action HTTP POST Main exception");
                string[] errors = { "error" };
                return Json(errors);
            }
            Logger.Info("Accessed DB, Checking UserMaster Details: User Details Not Found");
            string[] failures = { "failure" };
            return Json(failures);
        }

        public static bool DeligationMethod(int DeligationFromID, int DeligationToID)
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
                    string DeligateName_To = "";
                    DeligateName_To = (from tblUserMaster in context.tblUserMasters.Where(x => x.UserEmployeeID == DeligationToID) select tblUserMaster.UserEmployeeName).First();


                    var Template = from tblTemplateMaster in context.tblTemplateMasters select tblTemplateMaster;
                    foreach(var item in Template)
                    {
                        //string Activity = "";
                        //string OldValues = "";
                        //string NewValues = "";

                        if (item.Status == "Pending Approval" || item.Status == "Rejected")
                        {
                            if((item.Approver1ID == DeligationFromID) && (item.Approver1Status == "Pending Approval" || item.Approver1Status == "Rejected" || item.Approver1Status == null))
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

                    var Contract = from tblContractMaster in context.tblContractMasters select tblContractMaster;
                    foreach (var item in Contract)
                    {
                        //string Activity1 = "";
                        //string OldValues1 = "";
                        //string NewValues1 = "";
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

                    var ContractModification = from tblContractModification in context.tblContractModifications select tblContractModification;
                    foreach (var item in ContractModification)
                    {
                        //string OldValues1 = "";
                        //string NewValues1 = "";
                        if (item.Status == "Pending Approval" || item.Status == "Rejected")
                        {
                            if ((item.Approver1ID == DeligationFromID) && (item.Approver1Status == "Pending Approval" || item.Approver1Status == "Rejected" || item.Approver1Status == null))
                            {
                                tblDeligationLog logs = new tblDeligationLog();
                                logs.LogDeligationUID = item.ContractID;
                                logs.ModifiedBy = "SYSTEM";
                                logs.LogActivity = "Deligating  Approver 1 in Contract Modified (" + item.ContractID + ")";
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
                                logs.LogActivity = "Deligating  Approver 2 in Contract Modified (" + item.ContractID + ")";
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
                                logs.LogActivity = "Deligating  Approver 3 in Contract Modified (" + item.ContractID + ")";
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
                                logs.LogActivity = "Deligating  Approver 4 in Contract Modified (" + item.ContractID + ")";
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
                                logs.LogActivity = "Deligating  Approver 5 in Contract Modified (" + item.ContractID + ")";
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
                                logs.LogActivity = "Deligating  Approver 6 in Contract Modified (" + item.ContractID + ")";
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
                                logs.LogActivity = "Deligating  Approver 7 in Contract Modified (" + item.ContractID + ")";
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
                                logs.LogActivity = "Deligating  Approver 8 in Contract Modified (" + item.ContractID + ")";
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
                                logs.LogActivity = "Deligating  Approver 9 in Contract Modified (" + item.ContractID + ")";
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
                                logs.LogActivity = "Deligating  Approver 10 in Contract Modified (" + item.ContractID + ")";
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
                                logs.LogActivity = "Deligating  Next Approver in Contract Modified (" + item.ContractID + ")";
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
                                logs.LogActivity = "Deligating  Initiator in Contract Modified (" + item.ContractID + ")";
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
                                logs.LogActivity = "Deligating  Rejected By in Contract Modified (" + item.ContractID + ")";
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
            catch(Exception Ex)
            {  }
            return false;
        }

        [HttpPost]
        public ActionResult SaveDeligationToDB(int EmployeeID, int DeligateID, string DeligateDate, string DeligateFromDate)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            //string OldValues = "";
            //string NewValues = "";

            Logger.Info("Attempt Deligation SaveDeligationToDB");
            try
            {
                Logger.Info("Accessing DB for Saving the Deligation Details");

                var result = from tblDeligationMatrix in db.tblDeligationMatrices.Where(x => x.DeligateFrom == EmployeeID) select tblDeligationMatrix;

                if(result.ToList().Count > 0)
                {
                    int ID = 0;
                    foreach(var item in result)
                    {
                        ID = item.ID;

                        tblDeligationMatrix oldData = db.tblDeligationMatrices.Find(ID);
                        //if(oldData.DeligateFrom != EmployeeID)
                        //{
                        //    OldValues = OldValues + "Employee ID : " + oldData.DeligateFrom;
                        //    NewValues = NewValues + "Employee ID : " + EmployeeID;
                        //}
                        //if (oldData.DeligateTo != DeligateID)
                        //{
                        //    OldValues = OldValues + "Employee ID : " + oldData.DeligateTo;
                        //    NewValues = NewValues + "Employee ID : " + EmployeeID;
                        //}
                        oldData.DeligateFrom = EmployeeID;
                        oldData.DeligateTo = DeligateID;
                        oldData.StartDate = DeligateFromDate;
                        oldData.EndDate = DeligateDate;

                        db.Entry(oldData).State = EntityState.Modified;

                        tblDeligationLog log = new tblDeligationLog();
                        log.LogDeligationUID = ID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified";
                        log.ChangedFrom = EmployeeID.ToString();
                        log.ChangedTo = DeligateID.ToString();
                        log.DateandTime = DateTime.Now.ToString();

                        db.tblDeligationLogs.Add(log);
                    }
                       
                }
                else
                {
                    tblDeligationMatrix deligate = new tblDeligationMatrix
                    {
                        DeligateFrom = EmployeeID,
                        DeligateTo = DeligateID,
                        StartDate = DeligateFromDate,
                        EndDate = DeligateDate,
                    };
                    db.tblDeligationMatrices.Add(deligate);

                   
                        tblDeligationLog log = new tblDeligationLog();
                        log.LogDeligationUID = deligate.ID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Deligating";
                        log.ChangedFrom = EmployeeID.ToString();
                        log.ChangedTo = DeligateID.ToString();
                        log.DateandTime = DateTime.Now.ToString();

                        db.tblDeligationLogs.Add(log);
                   
                }


                db.SaveChanges();
                Logger.Info("Accessed DB, Deligation Details Saved");
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
                                //DeligationMethod(Convert.ToInt32(eachItem.DeligateFrom), Convert.ToInt32(eachItem.DeligateTo));
                                DeligationMethod(Convert.ToInt32(EmployeeID), Convert.ToInt32(DeligateID));
                            }
                            catch
                            { }
                        }

                    }
                }
                db.SaveChanges();

                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Deligation' Controller , 'SaveDeligationToDB' Action HTTP POST Main exception");
                return Json("error");
            }
        }
        

        [HttpPost]
        public JsonResult DeligationExistCheck(int employeeid)
        {
            Logger.Info("Attempt Deligation DeligationExistCheck");
            try
            {
                string[] DeligationData = new string[4]; 
                Logger.Info("Accessing DB for Deligation Details");

                var result = from tblDeligationMatrix in db.tblDeligationMatrices.Where(x => x.DeligateFrom == employeeid) select tblDeligationMatrix;
                foreach(var item in result)
                {
                    DeligationData[0] = "success";
                    DeligationData[1] = item.DeligateTo.ToString();
                    DeligationData[2] = item.EndDate;
                    DeligationData[3] = item.StartDate;

                }
               
                    Logger.Info("Accessed DB, Checking Deligation List: Mapping Found");
                    return Json(DeligationData);
                
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Deligation' Controller , 'DeligationExistCheck' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult RemoveDeligationFromDB(int EmployeeID)
        {
            Logger.Info("Attempt Deligation RemoveDeligationFromDB");
            try
            {
                int CurrentUser = 0;
                string CurrentUserName = "";
                try
                {
                    CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                    CurrentUserName = User.Identity.Name.Split('|')[0];
                }
                catch { }

                Logger.Info("Accessing DB for Removing the Deligation Details");
                int ID;
                ID = (from tblDeligationMatrix in db.tblDeligationMatrices.Where(x => x.DeligateFrom == EmployeeID) select tblDeligationMatrix.ID).First();
                string IDTo = "";
                IDTo = (from tblDeligationMatrix in db.tblDeligationMatrices.Where(x => x.DeligateFrom == EmployeeID) select tblDeligationMatrix.DeligateTo.ToString()).First();

                tblDeligationMatrix deligate = db.tblDeligationMatrices.Find(ID);
                db.tblDeligationMatrices.Remove(deligate);

                tblDeligationLog log = new tblDeligationLog();
                log.LogDeligationUID = ID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Deleted";
                log.ChangedFrom = EmployeeID.ToString();
                log.ChangedTo = IDTo;
                log.DateandTime = DateTime.Now.ToString();

                db.tblDeligationLogs.Add(log);

                db.SaveChanges();

                Logger.Info("Accessed DB, Deligation Details Removed");
                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Deligation' Controller , 'RemoveDeligationFromDB' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult getLogDetail(string DeligationID)
        {
            Logger.Info("Attempt Deligation getLogDetail");
            try
            {
                Logger.Info("Accessing DB for LogDetails");
                var result = from tblDeligationLog in db.tblDeligationLogs.Where(x => x.ChangedFrom == DeligationID || x.ChangedTo == DeligationID) select tblDeligationLog;
                Logger.Info("Accesed DB, LogDetails  Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Deligation' Controller , 'getLogDetail' Action HTTP POST Main exception");
                return View(Ex.Message);

            }
        }

        [HttpPost]
        public ActionResult DeligateIDName(string From, string To)
        {
            Logger.Info("Attempt Deligation DeligateIDName");
            try
            {
                string[] result = new string[3];
                Logger.Info("Accessing DB for DeligateIDName");
                
                result[0] = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == From) select tblUserMaster.UserEmployeeName).First();
                result[1] = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == To) select tblUserMaster.UserEmployeeName).First();

                Logger.Info("Accesed DB, LogDetails  Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Deligation' Controller , 'DeligateIDName' Action HTTP POST Main exception");
                return View(Ex.Message);

            }
        }
    }
}