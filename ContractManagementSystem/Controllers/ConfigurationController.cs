using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContractManagementSystem.Models;
using NLog;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Web.Configuration;

namespace ContractManagementSystem.Controllers
{
    [Authorize(Roles = "admin")]
    public class ConfigurationController : Controller
    {
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();

        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        readonly string ApplicationLink = WebConfigurationManager.AppSettings["ApplicationLink"];

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

        //public ActionResult AlertsNotification()
        //{
        //    return View();
        //}
        public ActionResult Index()
        {
            if (TempData["newstatus"] != null)
            {
                ViewBag.Status = "AlertsNotificationSuccess";
                TempData.Remove("newstatus");
            }
            Logger.Info("Attempt Configuration Index");

            Logger.Info("Accessing DB for saving  Alert System Details");
            //tblAlertSystem tblAlertSystem = db.tblAlertSystems.Find(1);


            //if (tblAlertSystem == null)
            //{
            //    //Logger.Info("Accesed DB, Checking AlertSystem Details: Details not Found");
            //    //return HttpNotFound();
            //    return View();
            //}
            Logger.Info("Accesed DB, checking  AlertSystem Details: Details Found");

            Logger.Info("Redirecting to AlertSystem Details Page");
            return View();
        }

        [HttpPost]
        public ActionResult GetAlertSystemDetails()
        {
            var result = from tblAlertSystem in db.tblAlertSystems select tblAlertSystem;
            return Json(result);
        }

        [HttpPost]
        public ActionResult SaveContractExpiryAlert(int Rem1Date, int Rem2Date, int Rem3Date, int Rem4Date, int Rem5Date, string Rem1Duration,string Rem2Duration,
            string Rem3Duration, string Rem4Duration, string Rem5Duration,int ID = 0 )
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Alerts And Notification SaveAlertRecord");
            try
            {
                Logger.Info("Accessing DB for saving  AlertSystem Record");
                tblAlertSystem m = db.tblAlertSystems.Find(ID);

                if (m == null)
                {
                    tblAlertSystem alert = new tblAlertSystem();
                    
                    alert.ContractExpiryReminder1Date = Rem1Date;
                    alert.ContractExpiryReminder1Duration = HttpUtility.HtmlEncode(Rem1Duration);
                    alert.ContractExpiryReminder2Date = Rem2Date;
                    alert.ContractExpiryReminder2Duration = HttpUtility.HtmlEncode(Rem2Duration);
                    alert.ContractExpiryReminder3Date = Rem3Date;
                    alert.ContractExpiryReminder3Duration = HttpUtility.HtmlEncode(Rem3Duration);
                    alert.ContractExpiryReminder4Date = Rem4Date;
                    alert.ContractExpiryReminder4Duration = HttpUtility.HtmlEncode(Rem4Duration);
                    alert.ContractExpiryReminder5Date = Rem5Date;
                    alert.ContractExpiryReminder5Duration = HttpUtility.HtmlEncode(Rem5Duration);


                    alert.TemplateLegal = "5";
                    alert.ContractFinance = "5";
                    alert.ContractLegal = "5";
                    alert.ContractHod = "5";

                    db.tblAlertSystems.Add(alert);
                    db.SaveChanges();

                    Logger.Info("Accesed DB, AlertSystem Details Saved");
                }
                else
                {
                    string OldValues = "";
                    string NewValues = "";


                    if (m.ContractExpiryReminder1Date != Rem1Date)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder5 Date : " + m.ContractExpiryReminder1Date + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder5 Date : " + Rem1Date + " , ";
                    }
                    if (m.ContractExpiryReminder1Duration != Rem1Duration)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder5 Duration : " + m.ContractExpiryReminder1Duration + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder5 Duration : " + Rem1Duration + " , ";
                    }
                    if (m.ContractExpiryReminder2Date != Rem2Date)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder4 Date : " + m.ContractExpiryReminder2Date + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder4 Date : " + Rem2Date + " , ";
                    }
                    if (m.ContractExpiryReminder2Duration != Rem2Duration)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder4 Duration : " + m.ContractExpiryReminder2Duration + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder4 Duration : " + Rem2Duration + " , ";
                    }
                    if (m.ContractExpiryReminder3Date != Rem3Date)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder3 Date : " + m.ContractExpiryReminder3Date + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder3 Date : " + Rem3Date + " , ";
                    }
                    if (m.ContractExpiryReminder3Duration != Rem3Duration)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder3 Duration : " + m.ContractExpiryReminder3Duration + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder3 Duration : " + Rem3Duration + " , ";
                    }
                    if (m.ContractExpiryReminder4Date != Rem4Date)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder2 Date : " + m.ContractExpiryReminder4Date + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder2 Date : " + Rem4Date + " , ";
                    }
                    if (m.ContractExpiryReminder4Duration != Rem4Duration)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder2 Duration : " + m.ContractExpiryReminder4Duration + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder2 Duration : " + Rem4Duration + " , ";
                    }
                    if (m.ContractExpiryReminder5Date != Rem5Date)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder1 Date : " + m.ContractExpiryReminder5Date + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder1 Date : " + Rem5Date + " , ";
                    }
                    if (m.ContractExpiryReminder5Duration != Rem5Duration)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder1 Duration : " + m.ContractExpiryReminder5Duration + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder1 Duration : " + Rem5Duration + " , ";
                    }

                   

                   
                    //if (m.OldContractDate != model.OldContractDate)
                    //{
                    //    OldValues = OldValues + "Old Contract Date : " + m.OldContractDate + " , ";
                    //    NewValues = NewValues + "Old Contract Date : " + model.OldContractDate + " , ";
                    //}
                    //if (m.OldContractDuration != model.OldContractDuration)
                    //{
                    //    OldValues = OldValues + "Old Contract Duration : " + m.OldContractDuration + " , ";
                    //    NewValues = NewValues + "Old Contract Duration : " + model.OldContractDuration + " , ";
                    //}

                    m.ContractExpiryReminder1Date = Rem1Date;
                    m.ContractExpiryReminder1Duration = HttpUtility.HtmlEncode(Rem1Duration);
                    m.ContractExpiryReminder2Date = Rem2Date;
                    m.ContractExpiryReminder2Duration = HttpUtility.HtmlEncode(Rem2Duration);
                    m.ContractExpiryReminder3Date = Rem3Date;
                    m.ContractExpiryReminder3Duration = HttpUtility.HtmlEncode(Rem3Duration);
                    m.ContractExpiryReminder4Date = Rem4Date;
                    m.ContractExpiryReminder4Duration = HttpUtility.HtmlEncode(Rem4Duration);
                    m.ContractExpiryReminder5Date = Rem5Date;
                    m.ContractExpiryReminder5Duration = HttpUtility.HtmlEncode(Rem5Duration);
                   
                    //m.OldContractDate = model.OldContractDate;
                    //m.OldContractDuration = HttpUtility.HtmlEncode(model.OldContractDuration);

                     db.Entry(m).State = EntityState.Modified;

                     if (OldValues.Length > 0)
                        {
                            tblAlertLog log = new tblAlertLog();
                            log.LogAlertUID = m.AlertID;
                            log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                            log.LogActivity = "Modified";
                            log.ChangedFrom = OldValues;
                            log.ChangedTo = NewValues;
                            log.DateandTime = DateTime.Now.ToString();

                            db.tblAlertLogs.Add(log);
                        }

                        db.SaveChanges();
                        Logger.Info("Accesed DB, AlertSystem Details Saved");
                }
                return Json("success");
            }

            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'SaveAlertRecord' Action HTTP POST Main exception");
                return View(Ex.Message);

            }
        }


        [HttpPost]
        public ActionResult SaveApprovalAlerts(string TemLegal, string ContractHod, string ContractLegal, string ContractFinance, int ID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            try
            {
                Logger.Info("Accessing DB for saving  AlertSystem Record");
                tblAlertSystem m = db.tblAlertSystems.Find(ID);

                if (m == null)
                {
                    tblAlertSystem alert = new tblAlertSystem();

                    alert.TemplateLegal = HttpUtility.HtmlEncode(TemLegal);
                    //alert.TemplateLegal = HttpUtility.HtmlEncode(model.TemplateLegal);
                    //alert.TemplateFinance = HttpUtility.HtmlEncode(model.TemplateFinance);
                    alert.ContractHod = HttpUtility.HtmlEncode(ContractHod);
                    alert.ContractFinance = HttpUtility.HtmlEncode(ContractFinance);
                    alert.ContractLegal = HttpUtility.HtmlEncode(ContractLegal);

                    alert.ContractExpiryReminder1Date = 1;
                    alert.ContractExpiryReminder1Duration = "Day";
                    alert.ContractExpiryReminder2Date = 2;
                    alert.ContractExpiryReminder2Duration = "Day";
                    alert.ContractExpiryReminder3Date = 3;
                    alert.ContractExpiryReminder3Duration = "Day";
                    alert.ContractExpiryReminder4Date = 4;
                    alert.ContractExpiryReminder4Duration = "Day";
                    alert.ContractExpiryReminder5Date = 5;
                    alert.ContractExpiryReminder5Duration = "Day";


                    db.tblAlertSystems.Add(alert);
                    db.SaveChanges();

                    Logger.Info("Accesed DB, AlertSystem Details Saved");
                }
                else
                {
                    string OldValues = "";
                    string NewValues = "";


                   

                    if (m.TemplateLegal != TemLegal)
                    {
                        OldValues = OldValues + "Template Legal : " + m.TemplateLegal + " , ";
                        NewValues = NewValues + "Template Legal : " + TemLegal + " , ";
                    }

                    if (m.ContractHod != ContractHod)
                    {
                        OldValues = OldValues + "Contract Hod : " + m.ContractHod + " , ";
                        NewValues = NewValues + "Contract Hod : " + ContractHod + " , ";
                    }
                    if (m.ContractFinance != ContractFinance)
                    {
                        OldValues = OldValues + "Contract Finance : " + m.ContractFinance + " , ";
                        NewValues = NewValues + "Contract Finance : " + ContractFinance + " , ";
                    }
                    if (m.ContractLegal != ContractLegal)
                    {
                        OldValues = OldValues + "Contract Legal : " + m.ContractLegal + " , ";
                        NewValues = NewValues + "Contract Legal : " + ContractLegal + " , ";
                    }



                    //if (m.OldContractDate != model.OldContractDate)
                    //{
                    //    OldValues = OldValues + "Old Contract Date : " + m.OldContractDate + " , ";
                    //    NewValues = NewValues + "Old Contract Date : " + model.OldContractDate + " , ";
                    //}
                    //if (m.OldContractDuration != model.OldContractDuration)
                    //{
                    //    OldValues = OldValues + "Old Contract Duration : " + m.OldContractDuration + " , ";
                    //    NewValues = NewValues + "Old Contract Duration : " + model.OldContractDuration + " , ";
                    //}

                    m.TemplateLegal = HttpUtility.HtmlEncode(TemLegal);
                    m.ContractHod = HttpUtility.HtmlEncode(ContractHod);
                    m.ContractFinance = HttpUtility.HtmlEncode(ContractFinance);
                    m.ContractLegal = HttpUtility.HtmlEncode(ContractLegal);

                    //m.OldContractDate = model.OldContractDate;
                    //m.OldContractDuration = HttpUtility.HtmlEncode(model.OldContractDuration);

                    db.Entry(m).State = EntityState.Modified;

                    if (OldValues.Length > 0)
                    {
                        tblAlertLog log = new tblAlertLog();
                        log.LogAlertUID = m.AlertID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified";
                        log.ChangedFrom = OldValues;
                        log.ChangedTo = NewValues;
                        log.DateandTime = DateTime.Now.ToString();

                        db.tblAlertLogs.Add(log);
                    }

                    db.SaveChanges();
                    Logger.Info("Accesed DB, AlertSystem Details Saved");
                }
                return Json("success");
            }

            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'SaveAlertRecord' Action HTTP POST Main exception");
                return View(Ex.Message);

            }
        }


        [HttpPost]
        public ActionResult getLogDetail(int ID = 0)
        {
            Logger.Info("Attempt Configuration getLogDetail");
            try
            {
                Logger.Info("Accessing DB for LogDetails");
                var result = from tblAlertLog in db.tblAlertLogs select tblAlertLog;
                Logger.Info("Accesed DB, LogDetails  Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'getLogDetail' Action HTTP POST Main exception");
                return View(Ex.Message);

            }
        }

        [HttpPost]
        public ActionResult SaveLog(string details, int ID, string initialvalue, string UserID)
        {
            Logger.Info("Attempt Configuration SaveLog");
           
            try
            {
                Logger.Info("Accessing DB for saving AlertLog Details");

                tblAlertLog log = new tblAlertLog
                {
                    LogAlertUID = ID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Modified",
                    ChangedFrom = initialvalue,
                    ChangedTo = details,
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblAlertLogs.Add(log);
                db.SaveChanges();
                Logger.Info("Accesed DB, AlertLog  Records Saved");
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'SaveLog' Action HTTP POST Main exception");
                return Json(Ex.Message);
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitPageSetUp(string PaperTypeValue,string PaperSizeValue, string TopValue, string LeftValue,string BottomValue, string RightValue)
        {
            //int ID = 0;
            
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            try
            {
                Logger.Info("Accessing DB for Configuration Details");
                var pagesetupDetails = from tblConfiguration in db.tblConfigurations.Where(x => x.PaperType == PaperTypeValue)
                                       .Where(x => x.PaperSize == PaperSizeValue)
                                       select tblConfiguration;

               
                if (pagesetupDetails.ToList().Count > 0)
                {
                    
                    foreach(var eachItem in pagesetupDetails)
                    {
                        string OldValues = "";
                        string NewValues = "";
                        if (eachItem.MarginTop != TopValue)
                        {
                            OldValues = OldValues + "Margin Top : " + eachItem.MarginTop + " , ";
                            NewValues = NewValues + "Margin Top : " + TopValue + " , ";
                        }
                        if (eachItem.MarginLeft != LeftValue)
                        {
                            OldValues = OldValues + "Margin Left : " + eachItem.MarginLeft + " , ";
                            NewValues = NewValues + "Margin Left : " + LeftValue + " , ";
                        }
                        if (eachItem.MarginBottom != BottomValue)
                        {
                            OldValues = OldValues + "Margin Bottom : " + eachItem.MarginBottom + " , ";
                            NewValues = NewValues + "Margin Bottom : " + BottomValue + " , ";
                        }
                        if (eachItem.MarginRight != RightValue)
                        {
                            OldValues = OldValues + "Margin Right : " + eachItem.MarginRight + " , ";
                            NewValues = NewValues + "Margin Right : " + RightValue + " , ";
                        }
                        eachItem.MarginTop = TopValue;
                        eachItem.MarginLeft = LeftValue;
                        eachItem.MarginBottom = BottomValue;
                        eachItem.MarginRight = RightValue;

                        db.Entry(eachItem).State = EntityState.Modified;

                        if (OldValues.Length > 0)
                        {
                            tblAlertLog log = new tblAlertLog();
                            log.LogAlertUID = eachItem.ID;
                            log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                            log.LogActivity = "Modified";
                            log.ChangedFrom = OldValues;
                            log.ChangedTo = NewValues;
                            log.DateandTime = DateTime.Now.ToString();

                            db.tblAlertLogs.Add(log);
                        }

                    }

                    db.SaveChanges();
                    Logger.Info("Accessed DB, Record Saved");
                }
                else
                {
                  
                    tblConfiguration pagesetup = new tblConfiguration();
                    pagesetup.PaperType = PaperTypeValue;
                    pagesetup.PaperSize = PaperSizeValue;
                    pagesetup.MarginTop = TopValue;
                    pagesetup.MarginLeft = LeftValue;
                    pagesetup.MarginBottom = BottomValue;
                    pagesetup.MarginRight = RightValue;

                    db.tblConfigurations.Add(pagesetup);
                    db.SaveChanges();

                    //tblAlertLog log = new tblAlertLog();
                    //    log.LogAlertUID = pagesetup.ID;
                    //    log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                    //    log.LogActivity = "Creation";
                    //    log.ChangedFrom = "-";
                    //    log.ChangedTo = "-";
                    //    log.DateandTime = DateTime.Now.ToString();

                    //    db.tblAlertLogs.Add(log);
                    
                    //db.SaveChanges();
                    Logger.Info("Accessed DB, Record Saved");
                }

                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'SubmitPageSetUp' Action HTTP POST Main exception");

                return Json("errors");
            }
        }


        [HttpPost]
        public ActionResult GetConfigurationTable(string PaperType, string PaperSize)
        {
            Logger.Info("Attempt Configuration GetConfigurationTable");
            try
            {
                Logger.Info("Accessing DB for Configuration Details : Configuration match");
                var TableConfig = db.tblConfigurations.Where(x => x.PaperType == PaperType).Where(x => x.PaperSize == PaperSize).Select(x => new { x.PaperType, x.PaperSize,x.MarginTop,x.MarginRight,x.MarginLeft,x.MarginBottom });

                string[] eachItem = new string[9];
                foreach (var r in TableConfig)
                {
                    eachItem[0] = r.PaperType;
                    eachItem[1] = r.PaperSize;
                    eachItem[2] = r.MarginTop.ToString();
                    eachItem[3] = r.MarginLeft.ToString();
                    eachItem[4] = r.MarginBottom.ToString();
                    eachItem[5] = r.MarginRight.ToString();

                }
                Logger.Info("Accessed DB, Checking Configuration Details : Details Found");
                return Json(eachItem);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'GetConfigurationTable' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        //[HttpPost]
        //public JsonResult GetPlantName(int CurrentUser)
        //{
        //    Logger.Info("Attempt Configuration getDepartment");
        //    try
        //    {
        //        var EmployeePlant = "";
        //        EmployeePlant = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == CurrentUser) select tblUserMaster.UserPlant).First();

        //        return Json(EmployeePlant);
        //    }
        //    catch (Exception Ex)
        //    {
        //        Logger.Error(Ex, "'Configuration' Controller , 'getDepartment' Action HTTP POST Main exception");
        //        return Json("error");
        //    }
        //}

        [HttpPost]
        public JsonResult GetDeptAndSubDept(int CurrentUser)
        {
            Logger.Info("Attempt Configuration GetDeptAndSubDept");
            try
            {

                string[] Info = new string[3];
                Info[0] = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == CurrentUser) select tblUserMaster.UserPlant).First();
                Info[1] = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == CurrentUser) select tblUserMaster.UserCategory).First();
                Info[2] = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == CurrentUser) select tblUserMaster.UserSubCategory).First();

                Logger.Info("Accessed DB, Checking Department List: Department Found");
                return Json(Info);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'GetDeptAndSubDept' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult GetEmployeeOnAppEscalation(string Plant, string Dept, string SubDept)
        {
            Logger.Info("Attempt Configuration GetEmployeeOnAppEscalation");
            try
            {
                try
                {
                    string[] Info = new string[3];
                    Info[0] = (from tblApprovalEscalation in db.tblApprovalEscalations.Where(x => x.Plant == Plant)
                               .Where(x => x.Department == Dept).Where(x => x.SubDepartment == SubDept)
                               select tblApprovalEscalation.EmployeeID.ToString()).First();

                    var EmpId = Info[0];


                    Info[1] = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == EmpId)
                               select tblUserMaster.UserEmployeeName).First();

                    Info[2] = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == EmpId)
                               select tblUserMaster.UserEmployeeEmail).First();

                    Logger.Info("Accessed DB, Checking Department List: Department Found");
                    return Json(Info);
                }
                catch
                {
                    return Json("Error");
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'GetEmployeeOnAppEscalation' Action HTTP POST Main exception");
                return Json("error");
            }
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
                var result = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserPlant == EmployeePlant).Where(x => x.UserEmployeeID.ToString().Contains(EmployeeDetails)).Select(x => new
                {
                    x.UserCategory,
                    x.UserEmployeeDesignation,
                    x.UserEmployeeEmail,
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleFinance2,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserSubCategory,
                    x.UserPlant
                }); //select tblUserMaster;
                return Json(result);
            }
            else if (OptionToSearch == "Employee Name")
            {
                var result = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserPlant == EmployeePlant).Where(x => x.UserEmployeeName.Contains(EmployeeDetails)).Select(x => new
                {
                    x.UserCategory,
                    x.UserEmployeeDesignation,
                    x.UserEmployeeEmail,
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleFinance2,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserSubCategory,
                    x.UserPlant
                }); //select tblUserMaster; //select tblUserMaster;
                return Json(result);
            }
            else if (OptionToSearch == "Employee Email Address")
            {
                var result = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserPlant == EmployeePlant).Where(x => x.UserEmployeeEmail.Contains(EmployeeDetails)).Select(x => new
                {
                    x.UserCategory,
                    x.UserEmployeeDesignation,
                    x.UserEmployeeEmail,
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleFinance2,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserSubCategory,
                    x.UserPlant
                }); //select tblUserMaster; //select tblUserMaster;
                return Json(result);
            }
            else
            {
                return Json("");
            }
        }
        [HttpPost]
        public JsonResult getDepartment()
        {
            Logger.Info("Attempt Configuration getDepartment");
            try
            {
                int CurrentUser = 0;
                try
                {
                    CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                }
                catch { }

                var EmployeePlant = "";
                EmployeePlant = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == CurrentUser) select tblUserMaster.UserPlant).First();


                Logger.Info("Accessing DB for Department List");
                var result = from tblDepartment in db.tblDepartments.Where(x => x.PlantName == EmployeePlant) select tblDepartment.DepartmentName;
                Logger.Info("Accessed DB, Checking Department List: Department Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'getDepartment' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [HttpPost]
        public JsonResult getSubDepartment(string user_category_id)
        {
            Logger.Info("Attempt Configuration getSubDepartment");
            try
            {
                Logger.Info("Accessing DB for SubDepartment List");

                var result = from tblSubDepartment in db.tblSubDepartments.Where(x => x.DepartmentName == user_category_id) select tblSubDepartment.SubDepartmentName;
                Logger.Info("Accessed DB, Checking SubDepartment List: SubDepartment Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'getSubDepartment' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitApprovalEscalation(string Plant, string Department, string SubDepartment, int EmployeeID = 0)
        { 
            int ID = 0;
            //EmployeeID = EmployeeID.Trim();

            //int EmpID = Convert.ToInt32(EmployeeID);

            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            try
            {
                Logger.Info("Accessing DB for Configuration Details");
                var EscalationDetails = from tblApprovalEscalation in db.tblApprovalEscalations.Where(x => x.Plant == Plant)
                                       .Where(x => x.Department == Department).Where(x => x.SubDepartment == SubDepartment)
                                       select tblApprovalEscalation;


                if (EscalationDetails.ToList().Count > 0)
                {

                    foreach (var eachItem in EscalationDetails)
                    {
                        string OldValues = "";
                        string NewValues = "";

                        ID = eachItem.UID;

                        if (eachItem.EmployeeID != EmployeeID)
                        {
                            OldValues = OldValues + "Escalation EmployeeID : " + eachItem.EmployeeID + " , ";
                            NewValues = NewValues + "Escalation EmployeeID : " + EmployeeID + " , ";
                        }

                        eachItem.EmployeeID = EmployeeID;

                        db.Entry(eachItem).State = EntityState.Modified;

                        if (OldValues.Length > 0)
                        {
                            tblAlertLog log = new tblAlertLog();
                            log.LogAlertUID = ID;
                            log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                            log.LogActivity = "Modified";
                            log.ChangedFrom = OldValues;
                            log.ChangedTo = NewValues;
                            log.DateandTime = DateTime.Now.ToString();

                            db.tblAlertLogs.Add(log);
                        }

                    }

                    db.SaveChanges();
                    Logger.Info("Accessed DB, Record Saved");
                }
                else
                {

                    tblApprovalEscalation eachEscalation = new tblApprovalEscalation();
                    eachEscalation.Plant = Plant;
                    eachEscalation.Department = Department;
                    eachEscalation.SubDepartment = SubDepartment;
                    eachEscalation.EmployeeID = EmployeeID;

                    db.tblApprovalEscalations.Add(eachEscalation);
                    db.SaveChanges();

                    tblAlertLog log = new tblAlertLog();
                    log.LogAlertUID = eachEscalation.UID;
                    log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                    log.LogActivity = "Approval Escalation Created";
                    log.ChangedFrom = "-";
                    log.ChangedTo = "-";
                    log.DateandTime = DateTime.Now.ToString();

                    db.tblAlertLogs.Add(log);

                    db.SaveChanges();
                    Logger.Info("Accessed DB, Record Saved");
                }

                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'SubmitApprovalEscalation' Action HTTP POST Main exception");

                return Json("errors");
            }
        }

        void ContractTimelineNotePad(int NextApprover, string ContractID, string ContractName, int DaysToApprove)
        {
            var dir = Server.MapPath("~/Uploads/");
            
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var file = Path.Combine(dir, NextApprover + ".csv");

            if (!(System.IO.File.Exists(file)))
            {
                System.IO.File.Create(file).Close();
            }

            System.IO.File.AppendAllText(file, ContractID + " , " + ContractName + " , " + DaysToApprove + Environment.NewLine);

            //if (System.IO.File.Exists(file))
            //{
                
            //}
            //else
            //{
            //    System.IO.File.Create(file).Close();
            //    System.IO.File.AppendAllText(file, ContractID + " , " + ContractName + " , " + DaysToApprove + Environment.NewLine);
            //}

        }

        
        void ContractApprovalTimeline()
        {
            var DaysToApprove_Approver = "";
            var DaysToApprove_Finance = "";
            var DaysToApprove_Legal = "";
            DaysToApprove_Approver = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractHod.ToString()).First();
            DaysToApprove_Finance = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractFinance.ToString()).First();
            DaysToApprove_Legal = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractLegal.ToString()).First();

            int Approver_Days = Convert.ToInt32(DaysToApprove_Approver);
            int Finance_Days = Convert.ToInt32(DaysToApprove_Finance);
            int Legal_Days = Convert.ToInt32(DaysToApprove_Legal);

            var ContractTable = from tblContractMaster in db.tblContractMasters.Where(x => x.Status == "Pending Approval") select tblContractMaster;
            foreach (var eachContract in ContractTable)
            {
                
                string NextApprover = eachContract.NextApprover;
                string ContractID = eachContract.ContractID.ToString();
                string ContractName = eachContract.ContractName;

                string UserRole = "";
                var UserTable = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == NextApprover) select tblUserMaster;
                foreach (var item in UserTable)
                {
                    if (item.UserRoleApprover == true)
                    {
                        UserRole = UserRole + "Approver";
                    }
                    if (item.UserRoleFinance == true)
                    {
                        if (UserRole.Length > 0)
                        {
                            UserRole = UserRole + ", Finance";
                        }
                        else
                        {
                            UserRole = UserRole + "Finance";
                        }
                    }
                    if (item.UserRoleLegal == true)
                    {
                        if (UserRole.Length > 0)
                        {
                            UserRole = UserRole + ", Legal";
                        }
                        else
                        {
                            UserRole = UserRole + "Legal";
                        }
                    }
                }

                if (UserRole.Contains("Approver") && UserRole.Contains("Legal") && UserRole.Contains("Finance"))
                {
                    int highest = Math.Max(Approver_Days, Math.Max(Finance_Days, Legal_Days));
                    ContractTimelineNotePad(Convert.ToInt32(NextApprover),ContractID,ContractName,highest);
                }
                else if (UserRole.Contains("Approver") && UserRole.Contains("Finance"))
                {
                    int highest = Math.Max(Approver_Days, Finance_Days);
                    ContractTimelineNotePad(Convert.ToInt32(NextApprover), ContractID, ContractName, highest);
                }
                else if (UserRole.Contains("Finance") && UserRole.Contains("Legal"))
                {
                    int highest = Math.Max(Finance_Days, Legal_Days);
                    ContractTimelineNotePad(Convert.ToInt32(NextApprover), ContractID, ContractName, highest);
                }
                else if (UserRole.Contains("Legal") && UserRole.Contains("Approver"))
                {
                    int highest = Math.Max(Legal_Days, Approver_Days);
                    ContractTimelineNotePad(Convert.ToInt32(NextApprover), ContractID, ContractName, highest);
                }
                else if (UserRole == "Approver")
                {
                    ContractTimelineNotePad(Convert.ToInt32(NextApprover), ContractID, ContractName, Approver_Days);
                }
                else if (UserRole == "Finance")
                {
                    ContractTimelineNotePad(Convert.ToInt32(NextApprover), ContractID, ContractName, Finance_Days);
                }
                else if (UserRole == "Legal")
                {
                    ContractTimelineNotePad(Convert.ToInt32(NextApprover), ContractID, ContractName, Legal_Days);
                }

            }
        }

        void ContractEscalationNotePad(int NextApprover, string ContractID, string ContractName, string EscalateDate, string From)
        {
            var dir = Server.MapPath("~/Uploads/");
            var file = Path.Combine(dir, NextApprover + ".csv");

            if (!(System.IO.File.Exists(file)))
            {
                System.IO.File.Create(file).Close();
            }
            System.IO.File.AppendAllText(file, ContractID + " , " + ContractName + " , " + EscalateDate + " , " + From + Environment.NewLine);

        }


        void EscalationAlerts()
        {
            try
            {
                var Escalation_Approver = "";
                var Escalation_Finance = "";
                var Escalation_Legal = "";
                Escalation_Approver = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.EsclationHod.ToString()).First();
                Escalation_Finance = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.EsclationFinance.ToString()).First();
                Escalation_Legal = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.EsclationLegal.ToString()).First();

                int Approver_Days = Convert.ToInt32(Escalation_Approver);
                int Finance_Days = Convert.ToInt32(Escalation_Finance);
                int Legal_Days = Convert.ToInt32(Escalation_Legal);

                var ContractTable = from tblContractMaster in db.tblContractMasters.Where(x => x.Status == "Pending Approval") select tblContractMaster;
                foreach (var eachContract in ContractTable)
                {
                    string ContractID = eachContract.ContractID.ToString();
                    string ContractName = eachContract.ContractName;

                    string Dept = eachContract.Department;
                    string SubDept = eachContract.SubDepartment;

                    if (eachContract.Approver1ID.ToString().Length > 1)
                    {
                        if (eachContract.Approver1Status == "Pending Approval")
                        {
                            var From = eachContract.Approver1ID.ToString();
                            if (eachContract.Approver2ID.ToString().Length > 1)
                            {
                                string UserRole = "";
                                var UserTable = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == eachContract.Approver1ID) select tblUserMaster;
                                foreach (var item in UserTable)
                                {
                                    if (item.UserRoleApprover == true)
                                    {
                                        UserRole = UserRole + "Approver";
                                    }
                                    if (item.UserRoleFinance == true)
                                    {
                                        if (UserRole.Length > 0)
                                        {
                                            UserRole = UserRole + ", Finance";
                                        }
                                        else
                                        {
                                            UserRole = UserRole + "Finance";
                                        }
                                    }
                                    if (item.UserRoleLegal == true)
                                    {
                                        if (UserRole.Length > 0)
                                        {
                                            UserRole = UserRole + ", Legal";
                                        }
                                        else
                                        {
                                            UserRole = UserRole + "Legal";
                                        }
                                    }
                                }

                                if (UserRole.Contains("Approver") && UserRole.Contains("Legal") && UserRole.Contains("Finance"))
                                {
                                    int highest = Math.Max(Approver_Days, Math.Max(Finance_Days, Legal_Days));

                                    //DateTime ReceivedOn = DateTime.ParseExact(eachContract.Approver1ReceivedOn.ToString(), "dd/MM/yyyy", null);
                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver2ID), ContractID, ContractName, ReceivedOn.ToString(), From);
                                }
                                else if (UserRole.Contains("Approver") && UserRole.Contains("Finance"))
                                {
                                    int highest = Math.Max(Approver_Days, Finance_Days);

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver2ID), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole.Contains("Finance") && UserRole.Contains("Legal"))
                                {
                                    int highest = Math.Max(Finance_Days, Legal_Days);

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver2ID), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole.Contains("Legal") && UserRole.Contains("Approver"))
                                {
                                    int highest = Math.Max(Approver_Days, Legal_Days);

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver2ID), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole == "Approver")
                                {

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(Approver_Days);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver2ID), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole == "Finance")
                                {
                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(Finance_Days);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver2ID), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole == "Legal")
                                {
                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(Legal_Days);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver2ID), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                            }
                            else
                            {
                                string Plant = "";
                                Plant = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == eachContract.Approver1ID) select tblUserMaster.UserPlant).First();

                                string EmployeeID_Escalation = "";
                                EmployeeID_Escalation = (from tblApprovalEscalation in db.tblApprovalEscalations.Where(x => x.Plant == Plant)
                                                         .Where(x => x.Department == Dept).Where(x => x.SubDepartment == SubDept)
                                                         select tblApprovalEscalation.EmployeeID.ToString()).First();

                                string UserRole = "";
                                var UserTable = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == eachContract.Approver1ID) select tblUserMaster;
                                foreach (var item in UserTable)
                                {
                                    if (item.UserRoleApprover == true)
                                    {
                                        UserRole = UserRole + "Approver";
                                    }
                                    if (item.UserRoleFinance == true)
                                    {
                                        if (UserRole.Length > 0)
                                        {
                                            UserRole = UserRole + ", Finance";
                                        }
                                        else
                                        {
                                            UserRole = UserRole + "Finance";
                                        }
                                    }
                                    if (item.UserRoleLegal == true)
                                    {
                                        if (UserRole.Length > 0)
                                        {
                                            UserRole = UserRole + ", Legal";
                                        }
                                        else
                                        {
                                            UserRole = UserRole + "Legal";
                                        }
                                    }
                                }

                                if (UserRole.Contains("Approver") && UserRole.Contains("Legal") && UserRole.Contains("Finance"))
                                {
                                    int highest = Math.Max(Approver_Days, Math.Max(Finance_Days, Legal_Days));

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);
                                }
                                else if (UserRole.Contains("Approver") && UserRole.Contains("Finance"))
                                {
                                    int highest = Math.Max(Approver_Days, Finance_Days);

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole.Contains("Finance") && UserRole.Contains("Legal"))
                                {
                                    int highest = Math.Max(Finance_Days, Legal_Days);

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole.Contains("Legal") && UserRole.Contains("Approver"))
                                {
                                    int highest = Math.Max(Approver_Days, Legal_Days);

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole == "Approver")
                                {

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(Approver_Days);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole == "Finance")
                                {
                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(Finance_Days);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole == "Legal")
                                {
                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                                    ReceivedOn.AddDays(Legal_Days);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }

                            }
                        }
                    }
                    if (eachContract.Approver2ID.ToString().Length > 1)
                    {
                        if (eachContract.Approver2Status == "Pending Approval")
                        {
                            var From = eachContract.Approver2ID.ToString();
                            if (eachContract.Approver3ID.ToString().Length > 1)
                            {
                                string UserRole = "";
                                var UserTable = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == eachContract.Approver2ID) select tblUserMaster;
                                foreach (var item in UserTable)
                                {
                                    if (item.UserRoleApprover == true)
                                    {
                                        UserRole = UserRole + "Approver";
                                    }
                                    if (item.UserRoleFinance == true)
                                    {
                                        if (UserRole.Length > 0)
                                        {
                                            UserRole = UserRole + ", Finance";
                                        }
                                        else
                                        {
                                            UserRole = UserRole + "Finance";
                                        }
                                    }
                                    if (item.UserRoleLegal == true)
                                    {
                                        if (UserRole.Length > 0)
                                        {
                                            UserRole = UserRole + ", Legal";
                                        }
                                        else
                                        {
                                            UserRole = UserRole + "Legal";
                                        }
                                    }
                                }

                                if (UserRole.Contains("Approver") && UserRole.Contains("Legal") && UserRole.Contains("Finance"))
                                {
                                    int highest = Math.Max(Approver_Days, Math.Max(Finance_Days, Legal_Days));

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);

                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver3ID), ContractID, ContractName, ReceivedOn.ToString(), From);
                                }
                                else if (UserRole.Contains("Approver") && UserRole.Contains("Finance"))
                                {
                                    int highest = Math.Max(Approver_Days, Finance_Days);

                                     DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);

                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver3ID), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole.Contains("Finance") && UserRole.Contains("Legal"))
                                {
                                    int highest = Math.Max(Finance_Days, Legal_Days);

                                     DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);

                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver3ID), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole.Contains("Legal") && UserRole.Contains("Approver"))
                                {
                                    int highest = Math.Max(Approver_Days, Legal_Days);

                                     DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);

                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver3ID), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole == "Approver")
                                {

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);

                                    ReceivedOn.AddDays(Approver_Days);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver3ID), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole == "Finance")
                                {
                                     DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);

                                    ReceivedOn.AddDays(Finance_Days);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver3ID), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole == "Legal")
                                {
                                     DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);

                                    ReceivedOn.AddDays(Legal_Days);

                                    ContractEscalationNotePad(Convert.ToInt32(eachContract.Approver3ID), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                            }
                            else
                            {
                                string Plant = "";
                                Plant = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == eachContract.Approver2ID) select tblUserMaster.UserPlant).First();

                                string EmployeeID_Escalation = "";
                                EmployeeID_Escalation = (from tblApprovalEscalation in db.tblApprovalEscalations.Where(x => x.Plant == Plant)
                                                         .Where(x => x.Department == Dept).Where(x => x.SubDepartment == SubDept)
                                                         select tblApprovalEscalation.EmployeeID.ToString()).First();

                                string UserRole = "";
                                var UserTable = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == eachContract.Approver2ID) select tblUserMaster;
                                foreach (var item in UserTable)
                                {
                                    if (item.UserRoleApprover == true)
                                    {
                                        UserRole = UserRole + "Approver";
                                    }
                                    if (item.UserRoleFinance == true)
                                    {
                                        if (UserRole.Length > 0)
                                        {
                                            UserRole = UserRole + ", Finance";
                                        }
                                        else
                                        {
                                            UserRole = UserRole + "Finance";
                                        }
                                    }
                                    if (item.UserRoleLegal == true)
                                    {
                                        if (UserRole.Length > 0)
                                        {
                                            UserRole = UserRole + ", Legal";
                                        }
                                        else
                                        {
                                            UserRole = UserRole + "Legal";
                                        }
                                    }
                                }

                                if (UserRole.Contains("Approver") && UserRole.Contains("Legal") && UserRole.Contains("Finance"))
                                {
                                    int highest = Math.Max(Approver_Days, Math.Max(Finance_Days, Legal_Days));

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);
                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);
                                }
                                else if (UserRole.Contains("Approver") && UserRole.Contains("Finance"))
                                {
                                    int highest = Math.Max(Approver_Days, Finance_Days);

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);
                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole.Contains("Finance") && UserRole.Contains("Legal"))
                                {
                                    int highest = Math.Max(Finance_Days, Legal_Days);

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);
                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole.Contains("Legal") && UserRole.Contains("Approver"))
                                {
                                    int highest = Math.Max(Approver_Days, Legal_Days);

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);
                                    ReceivedOn.AddDays(highest);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole == "Approver")
                                {

                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);
                                    ReceivedOn.AddDays(Approver_Days);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole == "Finance")
                                {
                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);
                                    ReceivedOn.AddDays(Finance_Days);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }
                                else if (UserRole == "Legal")
                                {
                                    DateTime ReceivedOn = Convert.ToDateTime(eachContract.Approver2ReceivedOn);
                                    ReceivedOn.AddDays(Legal_Days);

                                    ContractEscalationNotePad(Convert.ToInt32(EmployeeID_Escalation), ContractID, ContractName, ReceivedOn.ToString(), From);

                                }

                            }
                        }
                    }
                }
            }
            catch(Exception ex) { }
        }


        
    }
}