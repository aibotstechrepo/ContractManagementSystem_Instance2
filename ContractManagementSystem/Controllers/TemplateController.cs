using System;
using System.Collections;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Windows.Forms;
using ContractManagementSystem.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

using System.Collections.Generic;
using NLog;
using System.Diagnostics;

namespace ContractManagementSystem.Controllers
{

    public class TemplateController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        readonly string ApplicationLink = WebConfigurationManager.AppSettings["ApplicationLink"];

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

        [Authorize(Roles = "admin, legal, finance")]
        public ActionResult New()
        {
            Logger.Info("Accessing Template New Page");
            // return View(db.tblApprovalMasters.ToList());
            CommonModel model = new CommonModel
            {
                Template = db.tblTemplateMasters,
                Clause = db.tblClauseMasters,
                Variables = db.tblVariables,
                TemplateLogs = db.tblTemplateLogs,
                VariableDatas = db.tblVariableDatas
            };
            return View(model);
        }
        // GET: Template
        public ActionResult Repository()
        {
            Logger.Info("Accessing Template Repository Page");
            Logger.Info("Accessing DB for Repository");

            List<tblTemplateMaster> ContractsList = db.tblTemplateMasters.ToList();
            ContractsList.Reverse();
            return View(ContractsList);
        }
        public ActionResult Index()
        {
            return RedirectToAction("Repository");
        }


        public ActionResult Details()
        {
            return RedirectToAction("Repository");
        }

        [Route("Template/DraftView/{id:int}")]
        public ActionResult DraftView(int id)
        {
            int CurrentUser = 0;
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
            }
            catch { }
            Logger.Info("Accessing DB for Template Details");
            tblTemplateMaster tblTemplateMaster = db.tblTemplateMasters.Find(id);


            if (tblTemplateMaster.Status == "Draft" && tblTemplateMaster.Initiator == CurrentUser)
            {

                Logger.Info("Accessing Template DraftView Page");
                CommonModel model = new CommonModel
                {
                    Template = db.tblTemplateMasters,
                    Contract = db.tblContractMasters,
                    ContractModification = db.tblContractModifications,
                    Clause = db.tblClauseMasters,
                    Variables = db.tblVariables,
                    TemplateLogs = db.tblTemplateLogs,
                    ContractLogs = db.tblContractLogs,
                    VariableDatas = db.tblVariableDatas
                };
                return View(model);
            }
            else if (((tblTemplateMaster.Status == "Pending Approval") || (tblTemplateMaster.Status == "Rejected")) && ((tblTemplateMaster.NextApprover == CurrentUser.ToString()) || (tblTemplateMaster.Initiator == CurrentUser) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver1Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver2Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver3Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver4Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver5Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver6Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver7Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver8Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver9Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver10Status)))
            {
                Logger.Info("Accessing Template DraftView Page");
                CommonModel model = new CommonModel
                {
                    Template = db.tblTemplateMasters,
                    Contract = db.tblContractMasters,
                    ContractModification = db.tblContractModifications,
                    Clause = db.tblClauseMasters,
                    Variables = db.tblVariables,
                    TemplateLogs = db.tblTemplateLogs,
                    ContractLogs = db.tblContractLogs,
                    VariableDatas = db.tblVariableDatas
                };
                return View(model);
            }
            else if (((tblTemplateMaster.Status == "Approved") && ((tblTemplateMaster.Initiator == CurrentUser) || (tblTemplateMaster.Approver1ID == CurrentUser) || (tblTemplateMaster.Approver2ID == CurrentUser) || (tblTemplateMaster.Approver3ID == CurrentUser) || (tblTemplateMaster.Approver4ID == CurrentUser) || (tblTemplateMaster.Approver5ID == CurrentUser) || (tblTemplateMaster.Approver6ID == CurrentUser) || (tblTemplateMaster.Approver7ID == CurrentUser) || (tblTemplateMaster.Approver8ID == CurrentUser) || (tblTemplateMaster.Approver9ID == CurrentUser) || (tblTemplateMaster.Approver10ID == CurrentUser))))
            {

                Logger.Info("Accessing Template DraftView Page");
                CommonModel model = new CommonModel
                {
                    Template = db.tblTemplateMasters,
                    Contract = db.tblContractMasters,
                    ContractModification = db.tblContractModifications,
                    Clause = db.tblClauseMasters,
                    Variables = db.tblVariables,
                    TemplateLogs = db.tblTemplateLogs,
                    ContractLogs = db.tblContractLogs,
                    VariableDatas = db.tblVariableDatas
                };
                return View(model);
            }
            //else if (((tblTemplateMaster.Status == "In Effect") && ((tblTemplateMaster.Initiator == CurrentUser) || (tblTemplateMaster.Approver1ID == CurrentUser) || (tblTemplateMaster.Approver2ID == CurrentUser) || (tblTemplateMaster.Approver3ID == CurrentUser) || (tblTemplateMaster.Approver4ID == CurrentUser) || (tblTemplateMaster.Approver5ID == CurrentUser) || (tblTemplateMaster.Approver6ID == CurrentUser) || (tblTemplateMaster.Approver7ID == CurrentUser) || (tblTemplateMaster.Approver8ID == CurrentUser) || (tblTemplateMaster.Approver9ID == CurrentUser) || (tblTemplateMaster.Approver10ID == CurrentUser))))
            else if (tblTemplateMaster.Status == "In Effect")
            {
                Logger.Info("Accessing Template DraftView Page");
                CommonModel model = new CommonModel
                {
                    Template = db.tblTemplateMasters,
                    Contract = db.tblContractMasters,
                    ContractModification = db.tblContractModifications,
                    Clause = db.tblClauseMasters,
                    Variables = db.tblVariables,
                    TemplateLogs = db.tblTemplateLogs,
                    ContractLogs = db.tblContractLogs,
                    VariableDatas = db.tblVariableDatas
                };
                return View(model);
            }

            else
            {
                Logger.Info("Unauthorizrd access for draft Template");
                return HttpNotFound();
            }

        }
        public ActionResult DraftView()
        {
            return RedirectToAction("Repository");
        }

        [HttpPost] 
        public ActionResult SaveApproversToDB(string TemplateName, string TemplateType, string TemplateDescription, int[] UID, int CurrentUserID, string Category,string SubCategory,string Department,string SubDepartment, string TemplateSequence)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Template SaveApproversToDB");
            try
            {
                Logger.Info("Accessing DB for Saving the Template Records");
                tblTemplateMaster eachTemplate = new tblTemplateMaster();
                try
                {
                    if (UID.Length > 0)
                    {
                        eachTemplate.Approver1ID = UID[0];
                    }
                    else
                    {
                        eachTemplate.Approver1ID = 0;
                    }
                    if (UID.Length > 1)
                    {
                        eachTemplate.Approver2ID = UID[1];
                    }
                    else
                    {
                        eachTemplate.Approver2ID = 0;
                    }
                    if (UID.Length > 2)
                    {
                        eachTemplate.Approver3ID = UID[2];
                    }
                    else
                    {
                        eachTemplate.Approver3ID = 0;
                    }
                    if (UID.Length > 3)
                    {
                        eachTemplate.Approver4ID = UID[3];
                    }
                    else
                    {
                        eachTemplate.Approver4ID = 0;
                    }
                    if (UID.Length > 4)
                    {
                        eachTemplate.Approver5ID = UID[4];
                    }
                    else
                    {
                        eachTemplate.Approver5ID = 0;
                    }
                    if (UID.Length > 5)
                    {
                        eachTemplate.Approver6ID = UID[5];
                    }
                    else
                    {
                        eachTemplate.Approver6ID = 0;
                    }
                    if (UID.Length > 6)
                    {
                        eachTemplate.Approver7ID = UID[6];
                    }
                    else
                    {
                        eachTemplate.Approver7ID = 0;
                    }
                    if (UID.Length > 7)
                    {
                        eachTemplate.Approver8ID = UID[7];
                    }
                    else
                    {
                        eachTemplate.Approver8ID = 0;
                    }
                    if (UID.Length > 8)
                    {
                        eachTemplate.Approver9ID = UID[8];
                    }
                    else
                    {
                        eachTemplate.Approver9ID = 0;
                    }
                    if (UID.Length > 9)
                    {
                        eachTemplate.Approver10ID = UID[9];
                    }
                    else
                    {
                        eachTemplate.Approver10ID = 0;
                    }
                }
                catch { }

                
                int templatesequence = Convert.ToInt32(TemplateSequence);
                eachTemplate.Name = HttpUtility.HtmlEncode(TemplateName);
                eachTemplate.Type = HttpUtility.HtmlEncode(TemplateType);
                eachTemplate.Category = HttpUtility.HtmlEncode(Category);

                if(TemplateType == "Category")
                {
                    eachTemplate.SubCategory = "";
                }
                else
                {
                    eachTemplate.SubCategory = HttpUtility.HtmlEncode(SubCategory);
                }
                
                eachTemplate.Department = HttpUtility.HtmlEncode(Department);
                eachTemplate.SubDepartment = HttpUtility.HtmlEncode(SubDepartment);
                eachTemplate.TemplateSequence = templatesequence;


                eachTemplate.Description = HttpUtility.HtmlEncode(TemplateDescription);

                eachTemplate.Status = "Draft";
                eachTemplate.Initiator = CurrentUserID;
                db.tblTemplateMasters.Add(eachTemplate);
                Logger.Info("Accessed DB, Template Record Saved");

                Logger.Info("Accessing DB for Saving the Template Log Details");

                db.SaveChanges();
                tblTemplateLog log = new tblTemplateLog
                {
                    LogTemplateUID = eachTemplate.TemplateID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Created",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };
                db.tblTemplateLogs.Add(log);
                db.SaveChanges();
                Logger.Info("Accessed DB, Template Log Record Saved");

                string[] response = new string[2];
                response[0] = "success";
                response[1] = "" + eachTemplate.TemplateID;

                return Json(response);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'SaveApproversToDB' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [Route("Template/Details/{id:int}")]
        public ActionResult Details(int id)
        {
            int CurrentUser = 0;
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
            }
            catch { }
            Logger.Info("Accessing DB for Template Details");
            tblTemplateMaster tblTemplateMaster = db.tblTemplateMasters.Find(id);

            Logger.Info("Accessed DB, Checking Template Details: Checking Status");
            if (tblTemplateMaster == null)
            {
                Logger.Info("Unauthorizrd access for draft Template");
                return HttpNotFound();
            }
            if (tblTemplateMaster.Status == "Draft" && tblTemplateMaster.Initiator == CurrentUser)
            {

                Logger.Info("Accessed DB, Checking Template Details: Details Found");

                Logger.Info("Redirecting to Template Details Page");
                return View(tblTemplateMaster);
            }
            else if (((tblTemplateMaster.Status == "Pending Approval") || (tblTemplateMaster.Status == "Rejected")) && ((tblTemplateMaster.NextApprover == CurrentUser.ToString()) || (tblTemplateMaster.Initiator == CurrentUser) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver1Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver2Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver3Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver4Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver5Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver6Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver7Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver8Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver9Status) || string.IsNullOrWhiteSpace(tblTemplateMaster.Approver10Status)))
            {

                Logger.Info("Accessed DB, Checking Template Details: Details Found");

                Logger.Info("Redirecting to Template Details Page");
                return View(tblTemplateMaster);
            }

            else if (((tblTemplateMaster.Status == "Approved") && ((tblTemplateMaster.Initiator == CurrentUser) || (tblTemplateMaster.Approver1ID == CurrentUser) || (tblTemplateMaster.Approver2ID == CurrentUser) || (tblTemplateMaster.Approver3ID == CurrentUser) || (tblTemplateMaster.Approver4ID == CurrentUser) || (tblTemplateMaster.Approver5ID == CurrentUser) || (tblTemplateMaster.Approver6ID == CurrentUser) || (tblTemplateMaster.Approver7ID == CurrentUser) || (tblTemplateMaster.Approver8ID == CurrentUser) || (tblTemplateMaster.Approver9ID == CurrentUser) || (tblTemplateMaster.Approver10ID == CurrentUser))))
            {

                Logger.Info("Accessed DB, Checking Template Details: Details Found");

                Logger.Info("Redirecting to Template Details Page");
                return View(tblTemplateMaster);
            }
            //else if (((tblTemplateMaster.Status == "In Effect") && ((tblTemplateMaster.Initiator == CurrentUser) || (tblTemplateMaster.Approver1ID == CurrentUser) || (tblTemplateMaster.Approver2ID == CurrentUser) || (tblTemplateMaster.Approver3ID == CurrentUser) || (tblTemplateMaster.Approver4ID == CurrentUser) || (tblTemplateMaster.Approver5ID == CurrentUser) || (tblTemplateMaster.Approver6ID == CurrentUser) || (tblTemplateMaster.Approver7ID == CurrentUser) || (tblTemplateMaster.Approver8ID == CurrentUser) || (tblTemplateMaster.Approver9ID == CurrentUser) || (tblTemplateMaster.Approver10ID == CurrentUser))))
            else if (tblTemplateMaster.Status == "In Effect")
            {

                Logger.Info("Accessed DB, Checking Template Details: Details Found");

                Logger.Info("Redirecting to Template Details Page");
                return View(tblTemplateMaster);
            }

            else
            {
                Logger.Info("Unauthorizrd access of Template");
                return HttpNotFound();
            }
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult UpdateTemplateInDB(int[] UID, string TemplateName, int TemplateID, string TemplateType, string TemplateDescription, int CurrentUserID, string Category, string SubCategory, string Department, string SubDepartment,string TemplateSequence)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Template UpdateTemplateInDB");
            try
            {
                Logger.Info("Accessing DB for Updating the Template Records");
                tblTemplateMaster eachTemplate = db.tblTemplateMasters.Find(TemplateID);

                string OldValues = "";
                string NewValues = "";
               
                if (eachTemplate.Name != TemplateName)
                {
                    OldValues = OldValues + "Template Name : " + eachTemplate.Name + " , ";
                    NewValues = NewValues + "Template Name : " + TemplateName + " , ";
                }
                if (eachTemplate.Type != TemplateType)
                {
                    OldValues = OldValues + "Template For : " + eachTemplate.Type + " , ";
                    NewValues = NewValues + "Template For : " + TemplateType + " , ";
                }

                if (eachTemplate.Description != TemplateDescription)
                {
                    OldValues = OldValues + "Description : " + eachTemplate.Description + " , ";
                    NewValues = NewValues + "Description : " + TemplateDescription + " , ";
                }
                if (eachTemplate.TemplateSequence.ToString() != TemplateSequence)
                {
                    OldValues = OldValues + "Template Sequence : " + eachTemplate.TemplateSequence + " , ";
                    NewValues = NewValues + "Template Sequence : " + TemplateSequence + " , ";
                }
                if (eachTemplate.Category != Category)
                {
                    OldValues = OldValues + "Category : " + eachTemplate.Category + " , ";
                    NewValues = NewValues + "Category : " + Category + " , ";
                }
                if (eachTemplate.SubCategory != SubCategory)
                {
                    OldValues = OldValues + "Sub Category : " + eachTemplate.SubCategory + " , ";
                    NewValues = NewValues + "Sub Category : " + SubCategory + " , ";
                }
                if (eachTemplate.Department != Department)
                {
                    OldValues = OldValues + "Department : " + eachTemplate.Department + " , ";
                    NewValues = NewValues + "Department : " + Department + " , ";
                }
                if (eachTemplate.SubDepartment != SubDepartment)
                {
                    OldValues = OldValues + "Sub Department : " + eachTemplate.SubDepartment + " , ";
                    NewValues = NewValues + "Sub Department : " + SubDepartment + " , ";
                }

                try
                {
                    if (eachTemplate.Approver1ID != UID[0])
                    {
                        OldValues = OldValues + "Approver1 ID : " + eachTemplate.Approver1ID + " , ";
                        NewValues = NewValues + "Approver1 ID : " + UID[0] + " , ";
                    }

                    if (eachTemplate.Approver2ID != UID[1])
                    {
                        OldValues = OldValues + "Approver2 ID : " + eachTemplate.Approver2ID + " , ";
                        NewValues = NewValues + "Approver2 ID : " + UID[1] + " , ";
                    }

                    if (eachTemplate.Approver3ID != UID[2])
                    {
                        OldValues = OldValues + "Approver3 ID : " + eachTemplate.Approver3ID + " , ";
                        NewValues = NewValues + "Approver3 ID : " + UID[2] + " , ";
                    }

                    if (eachTemplate.Approver4ID != UID[3])
                    {
                        OldValues = OldValues + "Approver4 ID : " + eachTemplate.Approver4ID + " , ";
                        NewValues = NewValues + "Approver4 ID : " + UID[3] + " , ";
                    }

                    if (eachTemplate.Approver5ID != UID[4])
                    {
                        OldValues = OldValues + "Approver5 ID : " + eachTemplate.Approver5ID + " , ";
                        NewValues = NewValues + "Approver5 ID : " + UID[4] + " , ";
                    }

                    if (eachTemplate.Approver6ID != UID[5])
                    {
                        OldValues = OldValues + "Approver6 ID : " + eachTemplate.Approver6ID + " , ";
                        NewValues = NewValues + "Approver6 ID : " + UID[5] + " , ";
                    }

                    if (eachTemplate.Approver7ID != UID[6])
                    {
                        OldValues = OldValues + "Approver7 ID : " + eachTemplate.Approver7ID + " , ";
                        NewValues = NewValues + "Approver7 ID : " + UID[6] + " , ";
                    }

                    if (eachTemplate.Approver8ID != UID[7])
                    {
                        OldValues = OldValues + "Approver8 ID : " + eachTemplate.Approver8ID + " , ";
                        NewValues = NewValues + "Approver8 ID : " + UID[7] + " , ";
                    }

                    if (eachTemplate.Approver9ID != UID[8])
                    {
                        OldValues = OldValues + "Approver9 ID : " + eachTemplate.Approver9ID + " , ";
                        NewValues = NewValues + "Approver9 ID : " + UID[8] + " , ";
                    }

                    if (eachTemplate.Approver10ID != UID[9])
                    {
                        OldValues = OldValues + "Approver10 ID : " + eachTemplate.Approver10ID + " , ";
                        NewValues = NewValues + "Approver10 ID : " + UID[9] + " , ";
                    }
                }
                catch { }


                try
                {
                    if (UID.Length > 0)
                    {
                        eachTemplate.Approver1ID = UID[0];
                    }
                    else
                    {
                        if (eachTemplate.Approver1ID != 0)
                        {
                            OldValues = OldValues + "Approver1 ID : " + eachTemplate.Approver1ID + " , ";
                            NewValues = NewValues + "Approver1 ID : " + "0" + " , ";
                        }
                        eachTemplate.Approver1ID = 0;
                    }
                    if (UID.Length > 1)
                    {
                        eachTemplate.Approver2ID = UID[1];
                    }
                    else
                    {
                        if (eachTemplate.Approver2ID != 0)
                        {
                            OldValues = OldValues + "Approver2 ID : " + eachTemplate.Approver2ID + " , ";
                            NewValues = NewValues + "Approver2 ID : " + "0" + " , ";
                        }
                        eachTemplate.Approver2ID = 0;
                    }
                    if (UID.Length > 2)
                    {
                        eachTemplate.Approver3ID = UID[2];
                    }
                    else
                    {
                        if (eachTemplate.Approver3ID != 0)
                        {
                            OldValues = OldValues + "Approver3 ID : " + eachTemplate.Approver3ID + " , ";
                            NewValues = NewValues + "Approver3 ID : " + "0" + " , ";
                        }
                        eachTemplate.Approver3ID = 0;
                    }
                    if (UID.Length > 3)
                    {
                        eachTemplate.Approver4ID = UID[3];
                    }
                    else
                    {
                        if (eachTemplate.Approver4ID != 0)
                        {
                            OldValues = OldValues + "Approver4 ID : " + eachTemplate.Approver4ID + " , ";
                            NewValues = NewValues + "Approver4 ID : " + "0" + " , ";
                        }
                        eachTemplate.Approver4ID = 0;
                    }
                    if (UID.Length > 4)
                    {
                        eachTemplate.Approver5ID = UID[4];
                    }
                    else
                    {
                        if (eachTemplate.Approver5ID != 0)
                        {
                            OldValues = OldValues + "Approver5 ID : " + eachTemplate.Approver5ID + " , ";
                            NewValues = NewValues + "Approver5 ID : " + "0" + " , ";
                        }
                        eachTemplate.Approver5ID = 0;
                    }
                    if (UID.Length > 5)
                    {
                        eachTemplate.Approver6ID = UID[5];
                    }
                    else
                    {
                        if (eachTemplate.Approver6ID != 0)
                        {
                            OldValues = OldValues + "Approver6 ID : " + eachTemplate.Approver6ID + " , ";
                            NewValues = NewValues + "Approver6 ID : " + "0" + " , ";
                        }
                        eachTemplate.Approver6ID = 0;
                    }
                    if (UID.Length > 6)
                    {
                        eachTemplate.Approver7ID = UID[6];
                    }
                    else
                    {
                        if (eachTemplate.Approver7ID != 0)
                        {
                            OldValues = OldValues + "Approver7 ID : " + eachTemplate.Approver7ID + " , ";
                            NewValues = NewValues + "Approver7 ID : " + "0" + " , ";
                        }
                        eachTemplate.Approver7ID = 0;
                    }
                    if (UID.Length > 7)
                    {
                        eachTemplate.Approver8ID = UID[7];
                    }
                    else
                    {
                        if (eachTemplate.Approver8ID != 0)
                        {
                            OldValues = OldValues + "Approver8 ID : " + eachTemplate.Approver8ID + " , ";
                            NewValues = NewValues + "Approver8 ID : " + "0" + " , ";
                        }
                        eachTemplate.Approver8ID = 0;
                    }
                    if (UID.Length > 8)
                    {
                        eachTemplate.Approver9ID = UID[8];
                    }
                    else
                    {
                        if (eachTemplate.Approver9ID != 0)
                        {
                            OldValues = OldValues + "Approver9 ID : " + eachTemplate.Approver9ID + " , ";
                            NewValues = NewValues + "Approver9 ID : " + "0" + " , ";
                        }
                        eachTemplate.Approver9ID = 0;
                    }
                    if (UID.Length > 9)
                    {
                        eachTemplate.Approver10ID = UID[9];
                    }
                    else
                    {
                        if (eachTemplate.Approver10ID != 0)
                        {
                            OldValues = OldValues + "Approver10 ID : " + eachTemplate.Approver10ID + " , ";
                            NewValues = NewValues + "Approver10 ID : " + "0" + " , ";
                        }
                        eachTemplate.Approver10ID = 0;
                    }
                }
                catch { }

                int templatesequence = Convert.ToInt32(TemplateSequence);
                eachTemplate.Name = HttpUtility.HtmlEncode(TemplateName);
                eachTemplate.Type = HttpUtility.HtmlEncode(TemplateType);
                eachTemplate.Category = HttpUtility.HtmlEncode(Category);
                if (TemplateType == "Category")
                {
                    eachTemplate.SubCategory = "";
                }
                else
                {
                    eachTemplate.SubCategory = HttpUtility.HtmlEncode(SubCategory);
                }
                eachTemplate.Department = HttpUtility.HtmlEncode(Department);
                eachTemplate.SubDepartment = HttpUtility.HtmlEncode(SubDepartment);
                eachTemplate.Description = HttpUtility.HtmlEncode(TemplateDescription);
                eachTemplate.TemplateSequence = templatesequence;

               

                // eachTemplate.Status = "Draft";
                db.Entry(eachTemplate).State = EntityState.Modified;

                if (OldValues.Length > 0)
                {
                    tblTemplateLog log = new tblTemplateLog();
                    log.LogTemplateUID = eachTemplate.TemplateID;
                    log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                    log.LogActivity = "Modified";
                    log.ChangedFrom = OldValues;
                    log.ChangedTo = NewValues;
                    log.DateandTime = DateTime.Now.ToString();

                    db.tblTemplateLogs.Add(log);
                }

                db.SaveChanges();
                Logger.Info("Accessed DB, Template Record Updated");


                string[] response = new string[2];
                response[0] = "success";
                response[1] = "" + eachTemplate.TemplateID;

                return Json(response);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'UpdateTemplateInDB' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [Authorize(Roles = "admin, legal, finance")]
        public ActionResult DeleteTemplateFromDB(int TemplateID, int CurrentUserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Template DeleteTemplateFromDB");
            try
            {
                Logger.Info("Accessing DB for Deleting the Template Records");
                tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
                db.tblTemplateMasters.Remove(template);
                db.SaveChanges();
                Logger.Info("Accessed DB, Template Record Deleted");

                Logger.Info("Accessing DB for Saving the Template Log Details");
                tblTemplateLog log = new tblTemplateLog
                {
                    LogTemplateUID = TemplateID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Deleted",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblTemplateLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, Template Log Record Saved");
                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'DeleteTemplateFromDB' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult getCategory()
        {
            Logger.Info("Attempt Template getCategory");
            try
            {
                Logger.Info("Accessing DB for Category List");
                var result = from tblCategory in db.tblCategories select tblCategory.CategoryName;
                Logger.Info("Accessed DB, Checking Category List: Category Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'getCategory' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [HttpPost]
        public JsonResult getSubCategory(string user_category_id)
        {
            Logger.Info("Attempt Template getSubCategory");
            try
            {
                Logger.Info("Accessing DB for SubCategory List");

                var result = from tblSubCategory in db.tblSubCategories.Where(x => x.CategoryName == user_category_id) select tblSubCategory.SubCategoryName;
                Logger.Info("Accessed DB, Checking SubCategory List: SubCategory Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'getSubCategory' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [HttpPost]
        public JsonResult getDepartment()
        {
            Logger.Info("Attempt Template getDepartment");
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
                Logger.Error(Ex, "'Template' Controller , 'getDepartment' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [HttpPost]
        public JsonResult getSubDepartment(string user_category_id)
        {
            Logger.Info("Attempt Template getSubDepartment");
            try
            {
                Logger.Info("Accessing DB for SubDepartment List");

                var result = from tblSubDepartment in db.tblSubDepartments.Where(x => x.DepartmentName == user_category_id) select tblSubDepartment.SubDepartmentName;
                Logger.Info("Accessed DB, Checking SubDepartment List: SubDepartment Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'getSubDepartment' Action HTTP POST Main exception");
                return Json("error");
            }
        }

      

        [HttpPost]
        public JsonResult GetUserById(string EmployeeID)
        {
            Logger.Info("Attempt Template GetUserById");

            try
            {
                string[] UserInfo = { "", "User Not Found", "User Not Found", "User Not Found", "User Not Found", "", "", "" };

                if (!string.IsNullOrWhiteSpace(EmployeeID))
                {
                    EmployeeID = EmployeeID.Trim();
                    int EMPID = Convert.ToInt32(EmployeeID);
                    Logger.Info("Accessed DB, Checking UserMaster Details: EmployeeID match");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == EMPID) select tblUserMaster;
                    
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
                        UserInfo[3] = r.UserEmployeeDesignation;
                        UserInfo[4] = UserRole;
                        UserInfo[5] = r.UserSubCategory;
                        UserInfo[6] = r.UserCategory;
                        if (string.IsNullOrWhiteSpace(r.UserStatus))
                        {
                            UserInfo[7] = "Waiting for previous user to Approve";
                        }
                        else
                        {
                            UserInfo[7] = r.UserStatus;
                        }
                        Logger.Info("Accessed DB, Checking UserMaster Details: User Details Found");
                        return Json(UserInfo);
                    }

                    return Json(UserInfo);
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetUserById' Action HTTP POST Main exception");
                string[] errors = { "error" };
                return Json(errors);
            }
            Logger.Info("Accessed DB, Checking UserMaster Details: User Details Not Found");
            string[] failures = { "failure" };
            return Json(failures);
        }

        [HttpPost]
        public JsonResult GetUserByName(string EmployeeName)
        {
            Logger.Info("Attempt Template GetUserByName");

            try
            {

                if (!string.IsNullOrWhiteSpace(EmployeeName))
                {
                    EmployeeName = EmployeeName.Trim();
                    //int EMPID = Convert.ToInt32(EmployeeID);
                    Logger.Info("Accessed DB, Checking UserMaster Details: EmployeeName match");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeName == EmployeeName) select tblUserMaster;
                    string[] UserInfo = new string[7];

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
                        UserInfo[1] = r.UserEmployeeID.ToString();
                        UserInfo[2] = r.UserEmployeeEmail;
                        UserInfo[3] = r.UserEmployeeDesignation;
                        UserInfo[4] = UserRole;
                        UserInfo[5] = r.UserSubCategory;
                        UserInfo[6] = r.UserCategory;
                        Logger.Info("Accessed DB, Checking UserMaster Details: User Details Found");
                        return Json(UserInfo);
                    }
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetUserByName' Action HTTP POST Main exception");
                string[] errors = { "error" };
                return Json(errors);
            }
            Logger.Info("Accessed DB, Checking UserMaster Details: User Details Not Found");
            string[] failures = { "failure" };
            return Json(failures);
        }

        [HttpPost]
        public JsonResult GetUserByEmail(string EmployeeEmail)
        {
            Logger.Info("Attempt Template GetUserByEmail");

            try
            {

                if (!string.IsNullOrWhiteSpace(EmployeeEmail))
                {
                    EmployeeEmail = EmployeeEmail.Trim();
                    //int EMPID = Convert.ToInt32(EmployeeID);
                    Logger.Info("Accessed DB, Checking UserMaster Details: EmployeeEmail match");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeEmail == EmployeeEmail) select tblUserMaster;
                    string[] UserInfo = new string[7];

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
                        UserInfo[1] = r.UserEmployeeID.ToString();
                        UserInfo[2] = r.UserEmployeeName;
                        UserInfo[3] = r.UserEmployeeDesignation;
                        UserInfo[4] = UserRole;
                        UserInfo[5] = r.UserSubCategory;
                        UserInfo[6] = r.UserCategory;
                        Logger.Info("Accessed DB, Checking UserMaster Details: User Details Found");
                        return Json(UserInfo);
                    }
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetUserByEmail' Action HTTP POST Main exception");
                string[] errors = { "error" };
                return Json(errors);
            }
            Logger.Info("Accessed DB, Checking UserMaster Details: User Details Not Found");
            string[] failures = { "failure" };
            return Json(failures);
        }


        [HttpPost]
        public ActionResult getLogDetail(int ID)
        {
            Logger.Info("Attempt Template getLogDetail");

            try
            {
                Logger.Info("Accessed DB, Checking Template Log Details: LogID match");

                var result = from tblTemplateLog in db.tblTemplateLogs.Where(x => x.LogTemplateUID == ID) select tblTemplateLog;
                Logger.Info("Accessed DB, Checking Template Log Details: LogDetails Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'getLogDetail' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult getTemplateAddendumLogDetail(int ID)
        {
            Logger.Info("Attempt Template getTemplateAddendumLogDetail");

            try
            {
                Logger.Info("Accessed DB, Checking Template Log Details: LogID match");

                var result = from tblTemplateAddendumLog in db.tblTemplateAddendumLogs.Where(x => x.LogTemplateUID == ID) select tblTemplateAddendumLog;
                Logger.Info("Accessed DB, Checking Template Log Details: LogDetails Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'getTemplateAddendumLogDetail' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult SaveLog(string details, int ID, string initialvalue, string UserID)
        {
            Logger.Info("Attempt Template SaveLog");

            try
            {
                Logger.Info("Accessing DB for Saving the Template Log Details");
                tblTemplateLog log = new tblTemplateLog
                {
                    LogTemplateUID = ID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Modified",
                    ChangedFrom = initialvalue,
                    ChangedTo = details,
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblTemplateLogs.Add(log);
                Logger.Info("Accessed DB, Template Log Record Saved");
                db.SaveChanges();

                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'SaveLog 'Action HTTP POST Main exception");
                return Json(Ex.Message);
            }
        }

        [HttpPost]
        public ActionResult SaveTemplateAddendumLog(string details, int ID, string initialvalue, string UserID)
        {
            Logger.Info("Attempt Template SaveTemplateAddendumLog");

            try
            {
                Logger.Info("Accessing DB for Saving the Template Log Details");
                tblTemplateAddendumLog log = new tblTemplateAddendumLog
                {
                    LogTemplateUID = ID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Modified",
                    ChangedFrom = initialvalue,
                    ChangedTo = details,
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblTemplateAddendumLogs.Add(log);
                Logger.Info("Accessed DB, Template Log Record Saved");
                db.SaveChanges();

                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'SaveTemplateAddendumLog 'Action HTTP POST Main exception");
                return Json(Ex.Message);
            }
        }

        [HttpPost]
        public ActionResult GetTemplateNameOnID(int id)
        {
            Logger.Info("Attempt Template GetTemplateNameOnID");
            try
            {
                Logger.Info("Accessing DB for Template Details: TemplateID match");
                var result = /*from tblTemplateMaster in*/ db.tblTemplateMasters.Where(x => x.TemplateID == id).Select(x => new { x.Template, x.Name, x.TemplateID }); //select tblTemplateMaster;
                Logger.Info("Accessed DB, Checking Template Details: Details Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetTemplateNameOnID' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveTemplate(string content, int ID, string[] arrVariableNames, int CurrentUser)
        // public ActionResult SaveTemplate(string content, int ID, object VariableName, object VariableValue)
        {
            int CurrentUserID = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUserID = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Template SaveTemplate");
            try
            {


                Logger.Info("Attempt Template Variables");
                try
                {
                    Logger.Info("Accessing DB for Contract Variable Details");
                    var Variable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == ID).Where(x => x.Type == "Template").Where(x => x.Version == "Template").Select(x => new { x.ID }); //select tblVariableData;


                    Logger.Info("Accessed DB, Checking Contract Variable Details: Details Found");
                    if (Variable.ToList().Count > 0)
                    {

                        foreach (var item in Variable)
                        {
                            var Id = item.ID;

                            tblVariableData eachvariable = db.tblVariableDatas.Find(Id);
                            db.tblVariableDatas.Remove(eachvariable);
                            //db.SaveChanges();


                        }
                        db.SaveChanges();
                    }

                }
                catch (Exception Ex)
                {
                    Logger.Error(Ex, "'Template' Controller , 'SaveTemplate' Action HTTP POST Main exception");
                    return Json("error");
                }

                for (int i = 0; i < arrVariableNames.Length; i++)
                {
                    Logger.Info("Attempt Contract Variables");
                    try
                    {
                        Logger.Info("Accessed DB, Checking Template Variable Details: Details Not Found");
                        Logger.Info("Accessing DB for Saving the Template Variables");
                        tblVariableData variable = new tblVariableData
                        {
                            Type = "Template",
                            TypeID = ID,
                            Variable = arrVariableNames[i],
                            Value = "",
                            Version = "Template",
                        };

                        db.tblVariableDatas.Add(variable);


                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Variables Saved");

                    }
                    catch { }
                }



                //ArrayList names = new ArrayList(arrVariableNames);
                //for (int i = 0; i < names.Count; ++i)
                //{
                //    Console.WriteLine(names[i]);
                //}

                //ArrayList values = new ArrayList(arrVariableValues);
                //for (int j = 0; j < values.Count; ++j)
                //{
                //    Console.WriteLine(values[j]);
                //}


                Logger.Info("Accessing DB for Saving the Template Draft Details");
                tblTemplateMaster template = db.tblTemplateMasters.Find(ID);

                string OldValues = "";
                string NewValues = "";

                if (template.Template != content)
                {
                    OldValues = OldValues + " From Old content " + " , ";
                    NewValues = NewValues + "New content : " + " , ";
                }

                template.Template = content;
                if (template.Status == "Draft")
                {
                    template.Initiator = CurrentUser;

                }
                Logger.Info("Accesed DB, Checking User Details: Modified");
                db.Entry(template).State = EntityState.Modified;

                tblTemplateLog logs = new tblTemplateLog();
                logs.LogTemplateUID = template.TemplateID;
                logs.ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName;
                logs.LogActivity = "Template Content Modified";
                logs.ChangedFrom = OldValues;
                logs.ChangedTo = NewValues;
                logs.DateandTime = DateTime.Now.ToString();
                db.tblTemplateLogs.Add(logs);


                db.SaveChanges();
                Logger.Info("Accessed DB, Contract Draft Saved");

                Logger.Info("Accessing DB for Saving the Template Log Details");
                tblTemplateLog log = new tblTemplateLog();
                log.LogTemplateUID = template.TemplateID;
                log.ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Template Saved";
                log.ChangedFrom = "-";
                log.ChangedTo = "-";
                log.DateandTime = DateTime.Now.ToString();
                db.tblTemplateLogs.Add(log);
                db.SaveChanges();
                Logger.Info("Accessed DB, Template Log Record Saved");

                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'SaveTemplate' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InitiateTemplate(string content, int ID, string[] arrVariableNames, string Comments, int CurrentUserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Template InitiateContract");
            try
            {
                Logger.Info("Accessing DB for Initiating the Template");
                tblTemplateMaster template = db.tblTemplateMasters.Find(ID);
                template.Initiator = CurrentUserID;

                template.InitiatorComments = Comments;

                template.Template = content;
                template.InitiatorTemplate = content;
                template.Status = "Pending Approval";
                template.InitiatorStatus = "Initiated";
                template.DateofInitiated = DateTime.Now;

                tblTemplateLog log = new tblTemplateLog
                {
                    LogTemplateUID = template.TemplateID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Initiated",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };
                db.tblTemplateLogs.Add(log);
                db.SaveChanges();
                Logger.Info("Accessed DB, Template Log Record Saved");

                Logger.Info("Accessed DB, Checking Template Details: Checking Approvers");
                if (!string.IsNullOrWhiteSpace(template.Approver1ID.ToString()))
                {
                    Logger.Info("Accessed DB, Checking Template Details: Approver1 Found");
                    template.NextApprover = template.Approver1ID.ToString();
                    template.Approver1Status = "Pending Approval";
                    template.Approver1ReceivedOn = DateTime.Now;
                    Logger.Info("Accessed DB, Checking Template Details: Approver1 Status Updated");
                }
                else if (!string.IsNullOrWhiteSpace(template.Approver2ID.ToString()))
                {
                    Logger.Info("Accessed DB, Checking Template Details: Approver2 Found");
                    template.NextApprover = template.Approver2ID.ToString();
                    template.Approver2Status = "Pending Approval";
                    template.Approver2ReceivedOn = DateTime.Now;
                    Logger.Info("Accessed DB, Checking Template Details: Approver2 Status Updated");

                }
                else if (!string.IsNullOrWhiteSpace(template.Approver3ID.ToString()))
                {
                    Logger.Info("Accessed DB, Checking Template Details: Approver3 Found");
                    template.NextApprover = template.Approver3ID.ToString();
                    template.Approver3Status = "Pending Approval";
                    template.Approver3ReceivedOn = DateTime.Now;
                    Logger.Info("Accessed DB, Checking Template Details: Approver3 Status Updated");
                }
                else if (!string.IsNullOrWhiteSpace(template.Approver4ID.ToString()))
                {
                    Logger.Info("Accessed DB, Checking Template Details: Approver4 Found");
                    template.NextApprover = template.Approver4ID.ToString();
                    template.Approver4Status = "Pending Approval";
                    template.Approver4ReceivedOn = DateTime.Now;
                    Logger.Info("Accessed DB, Checking Template Details: Approver4 Status Updated");
                }
                else if (!string.IsNullOrWhiteSpace(template.Approver5ID.ToString()))
                {
                    Logger.Info("Accessed DB, Checking Template Details: Approver5 Found");
                    template.NextApprover = template.Approver5ID.ToString();
                    template.Approver5Status = "Pending Approval";
                    template.Approver5ReceivedOn = DateTime.Now;
                    Logger.Info("Accessed DB, Checking Template Details: Approver5 Status Updated");
                }
                else if (!string.IsNullOrWhiteSpace(template.Approver6ID.ToString()))
                {
                    Logger.Info("Accessed DB, Checking Template Details: Approver6 Found");
                    template.NextApprover = template.Approver6ID.ToString();
                    template.Approver6Status = "Pending Approval";
                    template.Approver6ReceivedOn = DateTime.Now;
                    Logger.Info("Accessed DB, Checking Template Details: Approver6 Status Updated");
                }
                else if (!string.IsNullOrWhiteSpace(template.Approver7ID.ToString()))
                {
                    Logger.Info("Accessed DB, Checking Template Details: Approver7 Found");
                    template.NextApprover = template.Approver7ID.ToString();
                    template.Approver7Status = "Pending Approval";
                    template.Approver7ReceivedOn = DateTime.Now;
                    Logger.Info("Accessed DB, Checking Template Details: Approver7 Status Updated");
                }
                else if (!string.IsNullOrWhiteSpace(template.Approver8ID.ToString()))
                {
                    Logger.Info("Accessed DB, Checking Template Details: Approver8 Found");
                    template.NextApprover = template.Approver8ID.ToString();
                    template.Approver8Status = "Pending Approval";
                    template.Approver8ReceivedOn = DateTime.Now;
                    Logger.Info("Accessed DB, Checking Template Details: Approver8 Status Updated");
                }
                else if (!string.IsNullOrWhiteSpace(template.Approver9ID.ToString()))
                {
                    Logger.Info("Accessed DB, Checking Template Details: Approver9 Found");
                    template.NextApprover = template.Approver9ID.ToString();
                    template.Approver9Status = "Pending Approval";
                    template.Approver9ReceivedOn = DateTime.Now;
                    Logger.Info("Accessed DB, Checking Template Details: Approver9 Status Updated");
                }
                else if (!string.IsNullOrWhiteSpace(template.Approver10ID.ToString()))
                {
                    Logger.Info("Accessed DB, Checking Template Details: Approver10 Found");
                    template.NextApprover = template.Approver10ID.ToString();
                    template.Approver10Status = "Pending Approval";
                    template.Approver10ReceivedOn = DateTime.Now;
                    Logger.Info("Accessed DB, Checking Template Details: Approver10 Status Updated");
                }
                else
                {
                    Logger.Info("Accessed DB, Checking Template Details: Next Approver Not Found");
                    template.Status = "In Effect";
                    template.NextApprover = 0.ToString();
                    Logger.Info("Accessed DB, Template Approved");

                    Logger.Info("Accessing DB for Saving the Template Log Details");
                    tblTemplateLog log1 = new tblTemplateLog
                    {
                        LogTemplateUID = template.TemplateID,
                        ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                        LogActivity = "Template InEffect",
                        ChangedFrom = "-",
                        ChangedTo = "-",
                        DateandTime = DateTime.Now.ToString()
                    };
                    db.tblTemplateLogs.Add(log1);
                    db.SaveChanges();
                    Logger.Info("Accessed DB, Template Log Record Saved");

                    string approver = "";
                    approver = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                    string approveremail = "";
                    approveremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                    //var tableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                    //string[] temp = new string[5];

                    //foreach (var r in tableVariable)
                    //{
                    //    temp[0] = r.Variable;
                    //    temp[1] = r.Value;
                    //    if (temp[0] == "Vendor Name")
                    //    {
                    //        temp[3] = "Vendor Name";
                    //        temp[4] = temp[1];
                    //    }

                    //}

                    //string vendorName = temp[4];
                    string[] To = { approveremail };

                    string subject = template.Name + " Approved and ready to Use ";
                    string urL = ApplicationLink + "/Template/Details/" + ID;
                    string paragraph = "The Template details as mentioned below is approved and ready to use by other users.<br/><br/>";
                    string body = "Dear " + approver + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                    SMTP.Send(To, subject, body);
                }

                for (int i = 0; i < arrVariableNames.Length; i++)
                {
                    Logger.Info("Attempt Contract Variables");
                    try
                    {
                        Logger.Info("Accessed DB, Checking Contract Variable Details: Details Not Found");
                        Logger.Info("Accessing DB for Saving the Contract Variables");
                        tblVariableData newVariable = new tblVariableData
                        {
                            Type = "Template",
                            TypeID = ID,
                            Variable = arrVariableNames[i],
                            Value = "",
                            Version = "Initiator",
                        };

                        db.tblVariableDatas.Add(newVariable);

                        Logger.Info("Accessed DB, Contract Variables Saved");
                    }

                    catch { }
                }

                try
                {
                    string LogChangeFrom = "";
                    string LogChangeTo = "";
                    var Deligation = /*from tblDeligationMatrix in*/ db.tblDeligationMatrices.Select(x => new { x.DeligateFrom, x.DeligateTo, x.EndDate, x.StartDate, x.ID }); //select tblDeligationMatrix;
                    foreach (var r in Deligation)
                    {
                        int Found = 1;
                        DateTime startDate = DateTime.ParseExact(r.StartDate, "dd/MM/yyyy", null);
                        Found = DateTime.Compare(startDate, DateTime.Now);

                        if (Found <= 0)
                        {
                            Found = 1;
                            DateTime endDate = DateTime.ParseExact(r.EndDate, "dd/MM/yyyy", null);
                            Found = DateTime.Compare(endDate, DateTime.Now);
                            if (Found < 0)
                            {
                                var DID = r.ID;

                                tblDeligationMatrix Deligate = db.tblDeligationMatrices.Find(DID);
                                db.tblDeligationMatrices.Remove(Deligate);

                            }
                            else
                            {
                                try
                                {
                                    //DeligationMethod(Convert.ToInt32(eachItem.DeligateFrom), Convert.ToInt32(eachItem.DeligateTo));
                                    if ((r.DeligateFrom == template.Initiator) && (template.Status == "Pending Approval" || template.Status == "Rejected"))
                                    {
                                        template.Initiator = r.DeligateTo;
                                        tblDeligationLog logs = new tblDeligationLog();
                                        logs.LogDeligationUID = template.TemplateID;
                                        logs.ModifiedBy = "SYSTEM";
                                        logs.LogActivity = "Deligating Initiator in Template (" + template.TemplateID + ")";
                                        logs.ChangedFrom = r.DeligateFrom.ToString();
                                        logs.ChangedTo = r.DeligateTo.ToString();
                                        logs.DateandTime = DateTime.Now.ToString();
                                        db.tblDeligationLogs.Add(logs);
                                    }
                                    if ((r.DeligateFrom.ToString() == template.NextApprover) && (template.Status == "Pending Approval" || template.Status == "Rejected"))
                                    {
                                        template.NextApprover = r.DeligateTo.ToString();
                                        tblDeligationLog logs = new tblDeligationLog();
                                        logs.LogDeligationUID = template.TemplateID;
                                        logs.ModifiedBy = "SYSTEM";
                                        logs.LogActivity = "Deligating Next Approver in Template (" + template.TemplateID + ")";
                                        logs.ChangedFrom = r.DeligateFrom.ToString();
                                        logs.ChangedTo = r.DeligateTo.ToString();
                                        logs.DateandTime = DateTime.Now.ToString();
                                        db.tblDeligationLogs.Add(logs);
                                    }
                                    if ((r.DeligateFrom.ToString() == template.RejectedBy) && (template.Status == "Pending Approval" || template.Status == "Rejected"))
                                    {
                                        template.RejectedBy = r.DeligateTo.ToString();
                                        tblDeligationLog logs = new tblDeligationLog();
                                        logs.LogDeligationUID = template.TemplateID;
                                        logs.ModifiedBy = "SYSTEM";
                                        logs.LogActivity = "Deligating Rejected By in Template (" + template.TemplateID + ")";
                                        logs.ChangedFrom = r.DeligateFrom.ToString();
                                        logs.ChangedTo = r.DeligateTo.ToString();
                                        logs.DateandTime = DateTime.Now.ToString();
                                        db.tblDeligationLogs.Add(logs);
                                    }
                                    if ((r.DeligateFrom == template.Approver1ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver1Status == "Pending Approval" || template.Approver1Status == "Rejected" || template.Approver1Status == null))
                                    {
                                        template.Approver1ID = (int)r.DeligateTo;
                                        tblDeligationLog logs = new tblDeligationLog();
                                        logs.LogDeligationUID = template.TemplateID;
                                        logs.ModifiedBy = "SYSTEM";
                                        logs.LogActivity = "Deligating Approver 1 in Template (" + template.TemplateID + ")";
                                        logs.ChangedFrom = r.DeligateFrom.ToString();
                                        logs.ChangedTo = r.DeligateTo.ToString();
                                        logs.DateandTime = DateTime.Now.ToString();
                                        db.tblDeligationLogs.Add(logs);
                                    }
                                    if ((r.DeligateFrom == template.Approver2ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver2Status == "Pending Approval" || template.Approver2Status == "Rejected" || template.Approver2Status == null))
                                    {
                                        template.Approver2ID = r.DeligateTo;
                                        tblDeligationLog logs = new tblDeligationLog();
                                        logs.LogDeligationUID = template.TemplateID;
                                        logs.ModifiedBy = "SYSTEM";
                                        logs.LogActivity = "Deligating Approver 2 in Template (" + template.TemplateID + ")";
                                        logs.ChangedFrom = r.DeligateFrom.ToString();
                                        logs.ChangedTo = r.DeligateTo.ToString();
                                        logs.DateandTime = DateTime.Now.ToString();
                                        db.tblDeligationLogs.Add(logs);
                                    }
                                    if ((r.DeligateFrom == template.Approver3ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver3Status == "Pending Approval" || template.Approver3Status == "Rejected" || template.Approver3Status == null))
                                    {
                                        template.Approver3ID = r.DeligateTo;
                                        tblDeligationLog logs = new tblDeligationLog();
                                        logs.LogDeligationUID = template.TemplateID;
                                        logs.ModifiedBy = "SYSTEM";
                                        logs.LogActivity = "Deligating Approver 3 in Template (" + template.TemplateID + ")";
                                        logs.ChangedFrom = r.DeligateFrom.ToString();
                                        logs.ChangedTo = r.DeligateTo.ToString();
                                        logs.DateandTime = DateTime.Now.ToString();
                                        db.tblDeligationLogs.Add(logs);
                                    }
                                    if ((r.DeligateFrom == template.Approver4ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver4Status == "Pending Approval" || template.Approver4Status == "Rejected" || template.Approver4Status == null))
                                    {
                                        template.Approver4ID = r.DeligateTo;
                                        tblDeligationLog logs = new tblDeligationLog();
                                        logs.LogDeligationUID = template.TemplateID;
                                        logs.ModifiedBy = "SYSTEM";
                                        logs.LogActivity = "Deligating Approver 4 in Template (" + template.TemplateID + ")";
                                        logs.ChangedFrom = r.DeligateFrom.ToString();
                                        logs.ChangedTo = r.DeligateTo.ToString();
                                        logs.DateandTime = DateTime.Now.ToString();
                                        db.tblDeligationLogs.Add(logs);
                                    }
                                    if ((r.DeligateFrom == template.Approver5ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver5Status == "Pending Approval" || template.Approver5Status == "Rejected" || template.Approver5Status == null))
                                    {
                                        template.Approver5ID = r.DeligateTo;
                                        tblDeligationLog logs = new tblDeligationLog();
                                        logs.LogDeligationUID = template.TemplateID;
                                        logs.ModifiedBy = "SYSTEM";
                                        logs.LogActivity = "Deligating Approver 5 in Template (" + template.TemplateID + ")";
                                        logs.ChangedFrom = r.DeligateFrom.ToString();
                                        logs.ChangedTo = r.DeligateTo.ToString();
                                        logs.DateandTime = DateTime.Now.ToString();
                                        db.tblDeligationLogs.Add(logs);
                                    }
                                    if ((r.DeligateFrom == template.Approver6ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver6Status == "Pending Approval" || template.Approver6Status == "Rejected" || template.Approver6Status == null))
                                    {
                                        template.Approver6ID = r.DeligateTo;
                                        tblDeligationLog logs = new tblDeligationLog();
                                        logs.LogDeligationUID = template.TemplateID;
                                        logs.ModifiedBy = "SYSTEM";
                                        logs.LogActivity = "Deligating Approver 6 in Template (" + template.TemplateID + ")";
                                        logs.ChangedFrom = r.DeligateFrom.ToString();
                                        logs.ChangedTo = r.DeligateTo.ToString();
                                        logs.DateandTime = DateTime.Now.ToString();
                                        db.tblDeligationLogs.Add(logs);
                                    }
                                    if ((r.DeligateFrom == template.Approver7ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver7Status == "Pending Approval" || template.Approver7Status == "Rejected" || template.Approver7Status == null))
                                    {
                                        template.Approver7ID = r.DeligateTo;
                                        tblDeligationLog logs = new tblDeligationLog();
                                        logs.LogDeligationUID = template.TemplateID;
                                        logs.ModifiedBy = "SYSTEM";
                                        logs.LogActivity = "Deligating Approver 7 in Template (" + template.TemplateID + ")";
                                        logs.ChangedFrom = r.DeligateFrom.ToString();
                                        logs.ChangedTo = r.DeligateTo.ToString();
                                        logs.DateandTime = DateTime.Now.ToString();
                                        db.tblDeligationLogs.Add(logs);
                                    }
                                    if ((r.DeligateFrom == template.Approver8ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver8Status == "Pending Approval" || template.Approver8Status == "Rejected" || template.Approver8Status == null))
                                    {
                                        template.Approver8ID = r.DeligateTo;
                                        tblDeligationLog logs = new tblDeligationLog();
                                        logs.LogDeligationUID = template.TemplateID;
                                        logs.ModifiedBy = "SYSTEM";
                                        logs.LogActivity = "Deligating Approver 8 in Template (" + template.TemplateID + ")";
                                        logs.ChangedFrom = r.DeligateFrom.ToString();
                                        logs.ChangedTo = r.DeligateTo.ToString();
                                        logs.DateandTime = DateTime.Now.ToString();
                                        db.tblDeligationLogs.Add(logs);
                                    }
                                    if ((r.DeligateFrom == template.Approver9ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver9Status == "Pending Approval" || template.Approver9Status == "Rejected" || template.Approver9Status == null))
                                    {
                                        template.Approver9ID = r.DeligateTo;
                                        tblDeligationLog logs = new tblDeligationLog();
                                        logs.LogDeligationUID = template.TemplateID;
                                        logs.ModifiedBy = "SYSTEM";
                                        logs.LogActivity = "Deligating Approver 9 in Template (" + template.TemplateID + ")";
                                        logs.ChangedFrom = r.DeligateFrom.ToString();
                                        logs.ChangedTo = r.DeligateTo.ToString();
                                        logs.DateandTime = DateTime.Now.ToString();
                                        db.tblDeligationLogs.Add(logs);
                                    }
                                    if ((r.DeligateFrom == template.Approver10ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver10Status == "Pending Approval" || template.Approver10Status == "Rejected" || template.Approver10Status == null))
                                    {
                                        template.Approver10ID = r.DeligateTo;
                                        tblDeligationLog logs = new tblDeligationLog();
                                        logs.LogDeligationUID = template.TemplateID;
                                        logs.ModifiedBy = "SYSTEM";
                                        logs.LogActivity = "Deligating Approver 10 in Template (" + template.TemplateID + ")";
                                        logs.ChangedFrom = r.DeligateFrom.ToString();
                                        logs.ChangedTo = r.DeligateTo.ToString();
                                        logs.DateandTime = DateTime.Now.ToString();
                                        db.tblDeligationLogs.Add(logs);
                                    }
                                }
                                catch
                                { }
                            }

                        }


                    }


                    db.Entry(template).State = EntityState.Modified;

                   
                }
                catch { }
            db.SaveChanges();
                Logger.Info("Accessed DB, Template is Initiated");
                Logger.Info("Accessing DB for Saving the Template Log Details");


                string Initiator = "";
                Initiator = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                string Initiatoremail = "";
                Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                //var TableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                //string[] temp = new string[5];

                //foreach (var r in TableVariable)
                //{
                //    temp[0] = r.Variable;
                //    temp[1] = r.Value;
                //    if (temp[0] == "Vendor Name")
                //    {
                //        temp[3] = "Vendor Name";
                //        temp[4] = temp[1];
                //    }

                //}
                string employeename = Initiator;
                //string VendorName = temp[4];
                string[] TO = { Initiatoremail };

                string Subject = template.Name + " is initiated ";
                string UrL = ApplicationLink + "/Template/Details/" + ID;
                string Paragraph = "The Template details as mentioned below is initiated and requested for Approval.<br/>We will notify you once Template is approved and ready to use. <br/><br/>";
                string Body = "Dear " + Initiator + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                SMTP.Send(TO, Subject, Body);

                try
                {
                    string Approver1 = "";
                    Approver1 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeName).First();
                    string Approver1email = "";
                    Approver1email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeEmail).First();
                    var tableVariable01 = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == ID).Select(x => new { x.Value, x.Variable }); //select tblVariableData;
                    string[] contract01 = new string[5];

                    foreach (var r in tableVariable01)
                    {
                        contract01[0] = r.Variable;
                        contract01[1] = r.Value;
                        if (contract01[0] == "Vendor Name")
                        {
                            contract01[3] = "Vendor Name";
                            contract01[4] = contract01[1];
                        }

                    }

                    string vendorName01 = contract01[4];
                    string[] To = { Approver1email };

                    Subject = template.Name + "  is pending for review ";
                    UrL = ApplicationLink + "/Template/Details/" + ID;
                    Paragraph = "The Template details as mentioned below is initiated by " + Initiator + " and requested for your review.<br/><br/>";
                    Body = "Dear " + Approver1 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                    SMTP.Send(To, Subject, Body);
                }
                catch { }


                string[] Template = new string[2];
                Template[0] = "success";
                Template[1] = template.Status;
                return Json(Template);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'InitiateTemplate' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AcceptChangesLastInitiator(int ID, string finalcontent)
        {
            Logger.Info("Attempt Template AcceptChangesLastInitiator");
            try
            {
                string CurrentUser = User.Identity.Name.Split('|')[1];
                Logger.Info("Accessing DB for Saving the Template Details");
                tblTemplateMaster template = db.tblTemplateMasters.Find(ID);

                template.Template = finalcontent;
                template.Template = finalcontent;
                if (template.Initiator.ToString() == CurrentUser)
                {
                    template.InitiatorTemplate = finalcontent;
                }
                
                db.Entry(template).State = EntityState.Modified;
                db.SaveChanges();
                Logger.Info("Accessed DB, Details Saved to DB");
                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'AcceptChangesLastInitiator' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ApproveTemplate(string content, int ID, string[] arrVariableNames, string Comments)
        {
            int CurrentUserID = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUserID = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Template ApproveTemplate");
            try
            {
                string CurrentUser = User.Identity.Name.Split('|')[1];
                Logger.Info("Accessing DB for Approving the Template");
                tblTemplateMaster template = db.tblTemplateMasters.Find(ID);
                Logger.Info("Accessed DB, Checking Template Details: Checking Approver");
                if (template.NextApprover.ToString() == CurrentUser)
                {
                    Logger.Info("Accessed DB, Checking Template Details: Approver Found");

                    if (template.Approver1ID.ToString() == CurrentUser && template.Approver1Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver1 Found");
                        template.Approver1ApprovedOn = DateTime.Now;

                        template.Approver1Comments = Comments;

                        template.Approver1Status = "Approved";
                        template.Approver1Draft = content;
                        for (int i = 0; i < arrVariableNames.Length; i++)
                        {
                            Logger.Info("Attempt Contract Variables");
                            try
                            {
                                Logger.Info("Accessed DB, Checking Contract Variable Details: Details Not Found");
                                Logger.Info("Accessing DB for Saving the Contract Variables");
                                tblVariableData newVariable = new tblVariableData
                                {
                                    Type = "Template",
                                    TypeID = ID,
                                    Variable = arrVariableNames[i],
                                    Value = "",
                                    Version = "Approver 1",
                                };

                                db.tblVariableDatas.Add(newVariable);


                                Logger.Info("Accessed DB, Contract Variables Saved");
                            }

                            catch { }
                        }

                        template.Template = content;
                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Approved",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");

                        Logger.Info("Accessed DB, Checking Template Details: Checking Next Approvers");

                        if (template.Approver2ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver2 Found");
                            template.NextApprover = template.Approver2ID.ToString();
                            template.Approver2Status = "Pending Approval";
                            template.Approver2ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver2 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver2 = "";
                            Approver2 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver2email = "";
                            Approver2email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver2;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver2email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by " + Initiate + " and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver2 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver3ID>0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver3 Found");
                            template.NextApprover = template.Approver3ID.ToString();
                            template.Approver3Status = "Pending Approval";
                            template.Approver3ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver3 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver3 = "";
                            Approver3 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver3email = "";
                            Approver3email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver3;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver3email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver3 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver4ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver4 Found");
                            template.NextApprover = template.Approver4ID.ToString();
                            template.Approver4Status = "Pending Approval";
                            template.Approver4ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver4 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver4 = "";
                            Approver4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver4email = "";
                            Approver4email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver4;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver4email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver4 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver5ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver5 Found");
                            template.NextApprover = template.Approver5ID.ToString();
                            template.Approver5Status = "Pending Approval";
                            template.Approver5ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver5 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver5 = "";
                            Approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver5email = "";
                            Approver5email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver5;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver5email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver5 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver6ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver6 Found");
                            template.NextApprover = template.Approver6ID.ToString();
                            template.Approver6Status = "Pending Approval";
                            template.Approver6ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver6 Status");

                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver6 = "";
                            Approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver6email = "";
                            Approver6email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver6;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver6email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver6 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Contract Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver7ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver7 Found");
                            template.NextApprover = template.Approver7ID.ToString();
                            template.Approver7Status = "Pending Approval";
                            template.Approver7ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver7 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver7 = "";
                            Approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver7email = "";
                            Approver7email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver7;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver7email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver7 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver8ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver8 Found");
                            template.NextApprover = template.Approver8ID.ToString();
                            template.Approver8Status = "Pending Approval";
                            template.Approver8ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver8 Status");

                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver8 = "";
                            Approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver8email = "";
                            Approver8email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver8;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver8email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver8 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver9ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver9 Found");
                            template.NextApprover = template.Approver9ID.ToString();
                            template.Approver9Status = "Pending Approval";
                            template.Approver9ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver9 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver9 = "";
                            Approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver9email = "";
                            Approver9email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver9;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver9email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver9 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver10ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver10 Found");
                            template.NextApprover = template.Approver10ID.ToString();
                            template.Approver10Status = "Pending Approval";
                            template.Approver10ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver10 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver10 = "";
                            Approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver10email = "";
                            Approver10email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver10;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver10email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver10 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Next Approver Not Found");
                            template.Status = "In Effect";
                            template.NextApprover = 0.ToString();
                            Logger.Info("Accessed DB, Template Approved");

                            Logger.Info("Accessing DB for Saving the Template Log Details");
                            tblTemplateLog log1 = new tblTemplateLog
                            {
                                LogTemplateUID = template.TemplateID,
                                ModifiedBy = template.Approver1ID.ToString(),
                                LogActivity = "Template InEffect",
                                ChangedFrom = "-",
                                ChangedTo = "-",
                                DateandTime = DateTime.Now.ToString()
                            };
                            db.tblTemplateLogs.Add(log1);
                            db.SaveChanges();
                            Logger.Info("Accessed DB, Template Log Record Saved");

                            string approver = "";
                            approver = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string approveremail = "";
                            approveremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                            //var tableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp = new string[5];

                            //foreach (var r in tableVariable)
                            //{
                            //    temp[0] = r.Variable;
                            //    temp[1] = r.Value;
                            //    if (temp[0] == "Vendor Name")
                            //    {
                            //        temp[3] = "Vendor Name";
                            //        temp[4] = temp[1];
                            //    }

                            //}

                            //string vendorName = temp[4];
                            string[] To = { approveremail };

                            string Subject = template.Name + " Approved and ready to Use ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is approved and ready to use by other users.<br/><br/>";
                            string Body = "Dear " + approver + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(To, Subject, Body);
                        }

                    }
                    else if (template.Approver2ID.ToString() == CurrentUser && template.Approver2Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver2 Found");
                        template.Approver2ApprovedOn = DateTime.Now;
                        template.Approver2Comments = Comments;
                        template.Approver2Status = "Approved";
                        template.Approver2Draft = content;
                        for (int i = 0; i < arrVariableNames.Length; i++)
                        {
                            Logger.Info("Attempt Contract Variables");
                            try
                            {
                                Logger.Info("Accessed DB, Checking Contract Variable Details: Details Not Found");
                                Logger.Info("Accessing DB for Saving the Contract Variables");
                                tblVariableData newVariable = new tblVariableData
                                {
                                    Type = "Template",
                                    TypeID = ID,
                                    Variable = arrVariableNames[i],
                                    Value = "",
                                    Version = "Approver 2",
                                };

                                db.tblVariableDatas.Add(newVariable);


                                Logger.Info("Accessed DB, Contract Variables Saved");
                            }

                            catch { }
                        }

                        template.Template = content;
                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Approved",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Contract Log Record Saved");

                        Logger.Info("Accessed DB, Checking Template Details: Checking Next Approvers");
                        if (template.Approver3ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver3 Found");
                            template.NextApprover = template.Approver3ID.ToString();
                            template.Approver3Status = "Pending Approval";
                            template.Approver3ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver3 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver3 = "";
                            Approver3 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver3email = "";
                            Approver3email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver3;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver3email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver3 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver4ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver4 Found");
                            template.NextApprover = template.Approver4ID.ToString();
                            template.Approver4Status = "Pending Approval";
                            template.Approver4ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver4 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver4 = "";
                            Approver4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver4email = "";
                            Approver4email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver4;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver4email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver4 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver5ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver5 Found");
                            template.NextApprover = template.Approver5ID.ToString();
                            template.Approver5Status = "Pending Approval";
                            template.Approver5ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver5 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver5 = "";
                            Approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver5email = "";
                            Approver5email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver5;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver5email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver5 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver6ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver6 Found");
                            template.NextApprover = template.Approver6ID.ToString();
                            template.Approver6Status = "Pending Approval";
                            template.Approver6ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver6 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver6 = "";
                            Approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver6email = "";
                            Approver6email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver6;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver6email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver6 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Contract Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver7ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver7 Found");
                            template.NextApprover = template.Approver7ID.ToString();
                            template.Approver7Status = "Pending Approval";
                            template.Approver7ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver7 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver7 = "";
                            Approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver7email = "";
                            Approver7email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver7;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver7email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver7 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver8ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver8 Found");
                            template.NextApprover = template.Approver8ID.ToString();
                            template.Approver8Status = "Pending Approval";
                            template.Approver8ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver8 Status");

                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver8 = "";
                            Approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver8email = "";
                            Approver8email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver8;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver8email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver8 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver9ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver9 Found");
                            template.NextApprover = template.Approver9ID.ToString();
                            template.Approver9Status = "Pending Approval";
                            template.Approver9ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver9 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver9 = "";
                            Approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver9email = "";
                            Approver9email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver9;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver9email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver9 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver10ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver10 Found");
                            template.NextApprover = template.Approver10ID.ToString();
                            template.Approver10Status = "Pending Approval";
                            template.Approver10ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver10 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver10 = "";
                            Approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver10email = "";
                            Approver10email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver10;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver10email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver10 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Next Approver Not Found");
                            template.Status = "In Effect";
                            template.NextApprover = 0.ToString();
                            Logger.Info("Accessed DB, Template Approved");

                            tblTemplateLog log1 = new tblTemplateLog
                            {
                                LogTemplateUID = template.TemplateID,
                                ModifiedBy = template.Approver1ID.ToString(),
                                LogActivity = "Template InEffect",
                                ChangedFrom = "-",
                                ChangedTo = "-",
                                DateandTime = DateTime.Now.ToString()
                            };
                            db.tblTemplateLogs.Add(log1);
                            db.SaveChanges();
                            Logger.Info("Accessed DB, Template Log Record Saved");

                            string approver = "";
                            approver = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string approveremail = "";
                            approveremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                            //var tableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp = new string[5];

                            //foreach (var r in tableVariable)
                            //{
                            //    temp[0] = r.Variable;
                            //    temp[1] = r.Value;
                            //    if (temp[0] == "Vendor Name")
                            //    {
                            //        temp[3] = "Vendor Name";
                            //        temp[4] = temp[1];
                            //    }

                            //}

                            //string vendorName = temp[4];
                            string[] To = { approveremail };

                            string Subject = template.Name + " Approved and ready to Use ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is approved and ready to use by other users.<br/><br/>";
                            string Body = "Dear " + approver + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(To, Subject, Body);
                        }

                    }
                    else if (template.Approver3ID.ToString() == CurrentUser && template.Approver3Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver3 Found");
                        template.Approver3ApprovedOn = DateTime.Now;
                        template.Approver3Comments = Comments;
                        template.Approver3Status = "Approved";
                        template.Approver3Draft = content;
                        for (int i = 0; i < arrVariableNames.Length; i++)
                        {
                            Logger.Info("Attempt Contract Variables");
                            try
                            {
                                Logger.Info("Accessed DB, Checking Contract Variable Details: Details Not Found");
                                Logger.Info("Accessing DB for Saving the Contract Variables");
                                tblVariableData newVariable = new tblVariableData
                                {
                                    Type = "Template",
                                    TypeID = ID,
                                    Variable = arrVariableNames[i],
                                    Value = "",
                                    Version = "Approver 3",
                                };

                                db.tblVariableDatas.Add(newVariable);


                                Logger.Info("Accessed DB, Contract Variables Saved");
                            }

                            catch { }
                        }

                        template.Template = content;
                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Approved",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");

                        Logger.Info("Accessed DB, Checking Template Details: Checking Next Approvers");

                        if (template.Approver4ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver4 Found");
                            template.NextApprover = template.Approver4ID.ToString();
                            template.Approver4Status = "Pending Approval";
                            template.Approver4ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver4 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver4 = "";
                            Approver4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver4email = "";
                            Approver4email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver4;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver4email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver4 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver5ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver5 Found");
                            template.NextApprover = template.Approver5ID.ToString();
                            template.Approver5Status = "Pending Approval";
                            template.Approver5ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver5 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver5 = "";
                            Approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver5email = "";
                            Approver5email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver5;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver5email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver5 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver6ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver6 Found");
                            template.NextApprover = template.Approver6ID.ToString();
                            template.Approver6Status = "Pending Approval";
                            template.Approver6ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver6 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver6 = "";
                            Approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver6email = "";
                            Approver6email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver6;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver6email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver6 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Contract Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver7ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver7 Found");
                            template.NextApprover = template.Approver7ID.ToString();
                            template.Approver7Status = "Pending Approval";
                            template.Approver7ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver7 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver7 = "";
                            Approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver7email = "";
                            Approver7email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver7;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver7email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver7 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver8ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver8 Found");
                            template.NextApprover = template.Approver8ID.ToString();
                            template.Approver8Status = "Pending Approval";
                            template.Approver8ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver8 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver8 = "";
                            Approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver8email = "";
                            Approver8email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver8;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver8email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver8 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver9ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver9 Found");
                            template.NextApprover = template.Approver9ID.ToString();
                            template.Approver9Status = "Pending Approval";
                            template.Approver9ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver9 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver9 = "";
                            Approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver9email = "";
                            Approver9email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver9;
                            // string VendorName01 = temp01[4];
                            string[] TO = { Approver9email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver9 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver10ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver10 Found");
                            template.NextApprover = template.Approver10ID.ToString();
                            template.Approver10Status = "Pending Approval";
                            template.Approver10ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver10 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver10 = "";
                            Approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver10email = "";
                            Approver10email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver10;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver10email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver10 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Next Approver Not Found");
                            template.Status = "In Effect";
                            template.NextApprover = 0.ToString();
                            Logger.Info("Accessed DB, Template Approved");

                            tblTemplateLog log1 = new tblTemplateLog
                            {
                                LogTemplateUID = template.TemplateID,
                                ModifiedBy = template.Approver1ID.ToString(),
                                LogActivity = "Template InEffect",
                                ChangedFrom = "-",
                                ChangedTo = "-",
                                DateandTime = DateTime.Now.ToString()
                            };
                            db.tblTemplateLogs.Add(log1);
                            db.SaveChanges();
                            Logger.Info("Accessed DB, Template Log Record Saved");

                            string approver = "";
                            approver = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string approveremail = "";
                            approveremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                            //var tableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp = new string[5];

                            //foreach (var r in tableVariable)
                            //{
                            //    temp[0] = r.Variable;
                            //    temp[1] = r.Value;
                            //    if (temp[0] == "Vendor Name")
                            //    {
                            //        temp[3] = "Vendor Name";
                            //        temp[4] = temp[1];
                            //    }

                            //}

                            //string vendorName = temp[4];
                            string[] To = { approveremail };

                            string Subject = template.Name + " Approved and ready to Use ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is approved and ready to use by other users.<br/><br/>";
                            string Body = "Dear " + approver + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(To, Subject, Body);
                        }

                    }
                    else if (template.Approver4ID.ToString() == CurrentUser && template.Approver4Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver4 Found");
                        template.Approver4ApprovedOn = DateTime.Now;
                        template.Approver4Comments = Comments;
                        template.Approver4Status = "Approved";
                        template.Approver4Draft = content;
                        for (int i = 0; i < arrVariableNames.Length; i++)
                        {
                            Logger.Info("Attempt Contract Variables");
                            try
                            {
                                Logger.Info("Accessed DB, Checking Contract Variable Details: Details Not Found");
                                Logger.Info("Accessing DB for Saving the Contract Variables");
                                tblVariableData newVariable = new tblVariableData
                                {
                                    Type = "Template",
                                    TypeID = ID,
                                    Variable = arrVariableNames[i],
                                    Value = "",
                                    Version = "Approver 4",
                                };

                                db.tblVariableDatas.Add(newVariable);


                                Logger.Info("Accessed DB, Contract Variables Saved");
                            }

                            catch { }
                        }

                        template.Template = content;
                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Approved",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");

                        Logger.Info("Accessed DB, Checking Template Details: Checking Next Approvers");

                        if (template.Approver5ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver5 Found");
                            template.NextApprover = template.Approver5ID.ToString();
                            template.Approver5Status = "Pending Approval";
                            template.Approver5ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver5 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver5 = "";
                            Approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver5email = "";
                            Approver5email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver5;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver5email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver5 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver6ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver6 Found");
                            template.NextApprover = template.Approver6ID.ToString();
                            template.Approver6Status = "Pending Approval";
                            template.Approver6ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver6 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver6 = "";
                            Approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver6email = "";
                            Approver6email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver6;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver6email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver6 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Contract Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver7ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver7 Found");
                            template.NextApprover = template.Approver7ID.ToString();
                            template.Approver7Status = "Pending Approval";
                            template.Approver7ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver7 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver7 = "";
                            Approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver7email = "";
                            Approver7email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver7;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver7email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver7 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver8ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver8 Found");
                            template.NextApprover = template.Approver8ID.ToString();
                            template.Approver8Status = "Pending Approval";
                            template.Approver8ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver8 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver8 = "";
                            Approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver8email = "";
                            Approver8email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver8;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver8email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver8 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver9ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver9 Found");
                            template.NextApprover = template.Approver9ID.ToString();
                            template.Approver9Status = "Pending Approval";
                            template.Approver9ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver9 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver9 = "";
                            Approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver9email = "";
                            Approver9email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver9;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver9email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver9 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver10ID > 0)
                        {

                            Logger.Info("Accessed DB, Checking Template Details: Approver10 Found");
                            template.NextApprover = template.Approver10ID.ToString();
                            template.Approver10Status = "Pending Approval";
                            template.Approver10ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver10 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver10 = "";
                            Approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver10email = "";
                            Approver10email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver10;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver10email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver10 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Next Approver Not Found");
                            template.Status = "In Effect";
                            template.NextApprover = 0.ToString();
                            Logger.Info("Accessed DB, Template Approved");

                            tblTemplateLog log1 = new tblTemplateLog
                            {
                                LogTemplateUID = template.TemplateID,
                                ModifiedBy = template.Approver1ID.ToString(),
                                LogActivity = "Template InEffect",
                                ChangedFrom = "-",
                                ChangedTo = "-",
                                DateandTime = DateTime.Now.ToString()
                            };
                            db.tblTemplateLogs.Add(log1);
                            db.SaveChanges();
                            Logger.Info("Accessed DB, Template Log Record Saved");

                            string approver = "";
                            approver = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string approveremail = "";
                            approveremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                            //var tableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp = new string[5];

                            //foreach (var r in tableVariable)
                            //{
                            //    temp[0] = r.Variable;
                            //    temp[1] = r.Value;
                            //    if (temp[0] == "Vendor Name")
                            //    {
                            //        temp[3] = "Vendor Name";
                            //        temp[4] = temp[1];
                            //    }

                            //}

                            //string vendorName = temp[4];
                            string[] To = { approveremail };

                            string Subject = template.Name + " Approved and ready to Use ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is approved and ready to use by other users.<br/><br/>";
                            string Body = "Dear " + approver + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(To, Subject, Body);
                        }


                    }
                    else if (template.Approver5ID.ToString() == CurrentUser && template.Approver5Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver5 Found");
                        template.Approver5ApprovedOn = DateTime.Now;
                        template.Approver5Comments = Comments;
                        template.Approver5Status = "Approved";
                        template.Approver5Draft = content;
                        for (int i = 0; i < arrVariableNames.Length; i++)
                        {
                            Logger.Info("Attempt Contract Variables");
                            try
                            {
                                Logger.Info("Accessed DB, Checking Contract Variable Details: Details Not Found");
                                Logger.Info("Accessing DB for Saving the Contract Variables");
                                tblVariableData newVariable = new tblVariableData
                                {
                                    Type = "Template",
                                    TypeID = ID,
                                    Variable = arrVariableNames[i],
                                    Value = "",
                                    Version = "Approver 5",
                                };

                                db.tblVariableDatas.Add(newVariable);


                                Logger.Info("Accessed DB, Contract Variables Saved");
                            }

                            catch { }
                        }

                        template.Template = content;
                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Approved",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");

                        Logger.Info("Accessed DB, Checking Template Details: Checking Next Approvers");


                        if (template.Approver6ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver6 Found");
                            template.NextApprover = template.Approver6ID.ToString();
                            template.Approver6Status = "Pending Approval";
                            template.Approver6ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver6 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver6 = "";
                            Approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver6email = "";
                            Approver6email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver6;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver6email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver6 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Contract Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver7ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver7 Found");
                            template.NextApprover = template.Approver7ID.ToString();
                            template.Approver7Status = "Pending Approval";
                            template.Approver7ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver7 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver7 = "";
                            Approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver7email = "";
                            Approver7email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver7;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver7email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver7 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver8ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver8 Found");
                            template.NextApprover = template.Approver8ID.ToString();
                            template.Approver8Status = "Pending Approval";
                            template.Approver8ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver8 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver8 = "";
                            Approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver8email = "";
                            Approver8email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver8;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver8email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver8 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver9ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver9 Found");
                            template.NextApprover = template.Approver9ID.ToString();
                            template.Approver9Status = "Pending Approval";
                            template.Approver9ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver9 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver9 = "";
                            Approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver9email = "";
                            Approver9email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver9;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver9email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver9 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver10ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver10 Found");
                            template.NextApprover = template.Approver10ID.ToString();
                            template.Approver10Status = "Pending Approval";
                            template.Approver10ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver10 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver10 = "";
                            Approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver10email = "";
                            Approver10email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver10;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver10email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver10 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Next Approver Not Found");
                            template.Status = "In Effect";
                            template.NextApprover = 0.ToString();
                            Logger.Info("Accessed DB, Template Approved");

                            tblTemplateLog log1 = new tblTemplateLog
                            {
                                LogTemplateUID = template.TemplateID,
                                ModifiedBy = template.Approver1ID.ToString(),
                                LogActivity = "Template InEffect",
                                ChangedFrom = "-",
                                ChangedTo = "-",
                                DateandTime = DateTime.Now.ToString()
                            };
                            db.tblTemplateLogs.Add(log1);
                            db.SaveChanges();
                            Logger.Info("Accessed DB, Template Log Record Saved");

                            string approver = "";
                            approver = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string approveremail = "";
                            approveremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                            //var tableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp = new string[5];

                            //foreach (var r in tableVariable)
                            //{
                            //    temp[0] = r.Variable;
                            //    temp[1] = r.Value;
                            //    if (temp[0] == "Vendor Name")
                            //    {
                            //        temp[3] = "Vendor Name";
                            //        temp[4] = temp[1];
                            //    }

                            //}

                            //string vendorName = temp[4];
                            string[] To = { approveremail };

                            string Subject = template.Name + " Approved and ready to Use ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is approved and ready to use by other users.<br/><br/>";
                            string Body = "Dear " + approver + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(To, Subject, Body);
                        }

                    }

                    else if (template.Approver6ID.ToString() == CurrentUser && template.Approver6Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver6 Found");
                        template.Approver6ApprovedOn = DateTime.Now;
                        template.Approver6Comments = Comments;
                        template.Approver6Status = "Approved";
                        template.Approver6Draft = content;
                        for (int i = 0; i < arrVariableNames.Length; i++)
                        {
                            Logger.Info("Attempt Contract Variables");
                            try
                            {
                                Logger.Info("Accessed DB, Checking Contract Variable Details: Details Not Found");
                                Logger.Info("Accessing DB for Saving the Contract Variables");
                                tblVariableData newVariable = new tblVariableData
                                {
                                    Type = "Template",
                                    TypeID = ID,
                                    Variable = arrVariableNames[i],
                                    Value = "",
                                    Version = "Approver 6",
                                };

                                db.tblVariableDatas.Add(newVariable);


                                Logger.Info("Accessed DB, Contract Variables Saved");
                            }

                            catch { }
                        }

                        template.Template = content;
                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Approved",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");

                        Logger.Info("Accessed DB, Checking Template Details: Checking Next Approvers");

                        if (template.Approver7ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver7 Found");
                            template.NextApprover = template.Approver7ID.ToString();
                            template.Approver7Status = "Pending Approval";
                            template.Approver7ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver7 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver7 = "";
                            Approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver7email = "";
                            Approver7email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver7;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver7email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver7 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver8ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver8 Found");
                            template.NextApprover = template.Approver8ID.ToString();
                            template.Approver8Status = "Pending Approval";
                            template.Approver8ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver8 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver8 = "";
                            Approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver8email = "";
                            Approver8email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver8;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver8email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver8 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver9ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver9 Found");
                            template.NextApprover = template.Approver9ID.ToString();
                            template.Approver9Status = "Pending Approval";
                            template.Approver9ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver9 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver9 = "";
                            Approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver9email = "";
                            Approver9email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver9;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver9email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver9 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver10ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver10 Found");
                            template.NextApprover = template.Approver10ID.ToString();
                            template.Approver10Status = "Pending Approval";
                            template.Approver10ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver10 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver10 = "";
                            Approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver10email = "";
                            Approver10email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver10;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver10email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver10 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Next Approver Not Found");
                            template.Status = "In Effect";
                            template.NextApprover = 0.ToString();
                            Logger.Info("Accessed DB, Template Approved");

                            tblTemplateLog log1 = new tblTemplateLog
                            {
                                LogTemplateUID = template.TemplateID,
                                ModifiedBy = template.Approver1ID.ToString(),
                                LogActivity = "Template InEffect",
                                ChangedFrom = "-",
                                ChangedTo = "-",
                                DateandTime = DateTime.Now.ToString()
                            };
                            db.tblTemplateLogs.Add(log1);
                            db.SaveChanges();
                            Logger.Info("Accessed DB, Template Log Record Saved");

                            string approver = "";
                            approver = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string approveremail = "";
                            approveremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                            //var tableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp = new string[5];

                            //foreach (var r in tableVariable)
                            //{
                            //    temp[0] = r.Variable;
                            //    temp[1] = r.Value;
                            //    if (temp[0] == "Vendor Name")
                            //    {
                            //        temp[3] = "Vendor Name";
                            //        temp[4] = temp[1];
                            //    }

                            //}

                            //string vendorName = temp[4];
                            string[] To = { approveremail };

                            string Subject = template.Name + " Approved and ready to Use ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is approved and ready to use by other users.<br/><br/>";
                            string Body = "Dear " + approver + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(To, Subject, Body);
                        }

                    }

                    else if (template.Approver7ID.ToString() == CurrentUser && template.Approver7Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver7 Found");
                        template.Approver7ApprovedOn = DateTime.Now;
                        template.Approver7Comments = Comments;
                        template.Approver7Status = "Approved";
                        template.Approver7Draft = content;
                        for (int i = 0; i < arrVariableNames.Length; i++)
                        {
                            Logger.Info("Attempt Contract Variables");
                            try
                            {
                                Logger.Info("Accessed DB, Checking Contract Variable Details: Details Not Found");
                                Logger.Info("Accessing DB for Saving the Contract Variables");
                                tblVariableData newVariable = new tblVariableData
                                {
                                    Type = "Template",
                                    TypeID = ID,
                                    Variable = arrVariableNames[i],
                                    Value = "",
                                    Version = "Approver 7",
                                };

                                db.tblVariableDatas.Add(newVariable);


                                Logger.Info("Accessed DB, Contract Variables Saved");
                            }

                            catch { }
                        }

                        template.Template = content;
                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Approved",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");

                        Logger.Info("Accessed DB, Checking Template Details: Checking Next Approvers");

                        if (template.Approver8ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver8 Found");
                            template.NextApprover = template.Approver8ID.ToString();
                            template.Approver8Status = "Pending Approval";
                            template.Approver8ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver8 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver8 = "";
                            Approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver8email = "";
                            Approver8email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver8;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver8email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver8 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver9ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver9 Found");
                            template.NextApprover = template.Approver9ID.ToString();
                            template.Approver9Status = "Pending Approval";
                            template.Approver9ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver9 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver9 = "";
                            Approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver9email = "";
                            Approver9email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver9;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver9email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver9 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver10ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver10 Found");
                            template.NextApprover = template.Approver10ID.ToString();
                            template.Approver10Status = "Pending Approval";
                            template.Approver10ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver10 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver10 = "";
                            Approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver10email = "";
                            Approver10email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver10;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver10email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver10 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Next Approver Not Found");
                            template.Status = "In Effect";
                            template.NextApprover = 0.ToString();
                            Logger.Info("Accessed DB, Template Approved");

                            tblTemplateLog log1 = new tblTemplateLog
                            {
                                LogTemplateUID = template.TemplateID,
                                ModifiedBy = template.Approver1ID.ToString(),
                                LogActivity = "Template InEffect",
                                ChangedFrom = "-",
                                ChangedTo = "-",
                                DateandTime = DateTime.Now.ToString()
                            };
                            db.tblTemplateLogs.Add(log1);
                            db.SaveChanges();
                            Logger.Info("Accessed DB, Template Log Record Saved");

                            string approver = "";
                            approver = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string approveremail = "";
                            approveremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                            //var tableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp = new string[5];

                            //foreach (var r in tableVariable)
                            //{
                            //    temp[0] = r.Variable;
                            //    temp[1] = r.Value;
                            //    if (temp[0] == "Vendor Name")
                            //    {
                            //        temp[3] = "Vendor Name";
                            //        temp[4] = temp[1];
                            //    }

                            //}

                            //string vendorName = temp[4];
                            string[] To = { approveremail };

                            string Subject = template.Name + " Approved and ready to Use ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is approved and ready to use by other users.<br/><br/>";
                            string Body = "Dear " + approver + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(To, Subject, Body);
                        }


                    }

                    else if (template.Approver8ID.ToString() == CurrentUser && template.Approver8Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver8 Found");
                        template.Approver8ApprovedOn = DateTime.Now;
                        template.Approver8Comments = Comments;
                        template.Approver8Status = "Approved";
                        template.Approver8Draft = content;
                        for (int i = 0; i < arrVariableNames.Length; i++)
                        {
                            Logger.Info("Attempt Contract Variables");
                            try
                            {
                                Logger.Info("Accessed DB, Checking Contract Variable Details: Details Not Found");
                                Logger.Info("Accessing DB for Saving the Contract Variables");
                                tblVariableData newVariable = new tblVariableData
                                {
                                    Type = "Template",
                                    TypeID = ID,
                                    Variable = arrVariableNames[i],
                                    Value = "",
                                    Version = "Approver 8",
                                };

                                db.tblVariableDatas.Add(newVariable);


                                Logger.Info("Accessed DB, Contract Variables Saved");
                            }

                            catch { }
                        }

                        template.Template = content;
                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Approved",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");

                        Logger.Info("Accessed DB, Checking Template Details: Checking Next Approvers");

                        if (template.Approver9ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver9 Found");
                            template.NextApprover = template.Approver9ID.ToString();
                            template.Approver9Status = "Pending Approval";
                            template.Approver9ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver9 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver9 = "";
                            Approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver9email = "";
                            Approver9email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver9;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver9email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver9 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else if (template.Approver10ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver10 Found");
                            template.NextApprover = template.Approver10ID.ToString();
                            template.Approver10Status = "Pending Approval";
                            template.Approver10ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver10 Status");
                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver10 = "";
                            Approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver10email = "";
                            Approver10email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver10;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver10email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver10 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Next Approver Not Found");
                            template.Status = "In Effect";
                            template.NextApprover = 0.ToString();
                            Logger.Info("Accessed DB, Template Approved");

                            tblTemplateLog log1 = new tblTemplateLog
                            {
                                LogTemplateUID = template.TemplateID,
                                ModifiedBy = template.Approver1ID.ToString(),
                                LogActivity = "Template InEffect",
                                ChangedFrom = "-",
                                ChangedTo = "-",
                                DateandTime = DateTime.Now.ToString()
                            };
                            db.tblTemplateLogs.Add(log1);
                            db.SaveChanges();
                            Logger.Info("Accessed DB, Template Log Record Saved");

                            string approver = "";
                            approver = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string approveremail = "";
                            approveremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                            //var tableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp = new string[5];

                            //foreach (var r in tableVariable)
                            //{
                            //    temp[0] = r.Variable;
                            //    temp[1] = r.Value;
                            //    if (temp[0] == "Vendor Name")
                            //    {
                            //        temp[3] = "Vendor Name";
                            //        temp[4] = temp[1];
                            //    }

                            //}

                            //string vendorName = temp[4];
                            string[] To = { approveremail };

                            string Subject = template.Name + " Approved and ready to Use ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is approved and ready to use by other users.<br/><br/>";
                            string Body = "Dear " + approver + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(To, Subject, Body);
                        }


                    }

                    else if (template.Approver9ID.ToString() == CurrentUser && template.Approver9Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver9 Found");
                        template.Approver9ApprovedOn = DateTime.Now;
                        template.Approver9Comments = Comments;
                        template.Approver9Status = "Approved";
                        template.Approver9Draft = content;
                        for (int i = 0; i < arrVariableNames.Length; i++)
                        {
                            Logger.Info("Attempt Contract Variables");
                            try
                            {
                                Logger.Info("Accessed DB, Checking Contract Variable Details: Details Not Found");
                                Logger.Info("Accessing DB for Saving the Contract Variables");
                                tblVariableData newVariable = new tblVariableData
                                {
                                    Type = "Template",
                                    TypeID = ID,
                                    Variable = arrVariableNames[i],
                                    Value = "",
                                    Version = "Approver 9",
                                };

                                db.tblVariableDatas.Add(newVariable);


                                Logger.Info("Accessed DB, Contract Variables Saved");
                            }

                            catch { }
                        }

                        template.Template = content;
                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Approved",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");

                        Logger.Info("Accessed DB, Checking Template Details: Checking Next Approvers");

                        if (template.Approver10ID > 0)
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Approver10 Found");
                            template.NextApprover = template.Approver10ID.ToString();
                            template.Approver10Status = "Pending Approval";
                            template.Approver10ReceivedOn = DateTime.Now;
                            Logger.Info("Accessed DB, Checking Template Details: Update Approver10 Status");

                            string Initiate = "";
                            Initiate = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string Approver10 = "";
                            Approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver10email = "";
                            Approver10email = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeEmail).First();
                            //var TableVariable01 = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp01 = new string[5];

                            //foreach (var r in TableVariable01)
                            //{
                            //    temp01[0] = r.Variable;
                            //    temp01[1] = r.Value;
                            //    if (temp01[0] == "Vendor Name")
                            //    {
                            //        temp01[3] = "Vendor Name";
                            //        temp01[4] = temp01[1];
                            //    }

                            //}
                            string employeename = Approver10;
                            //string VendorName01 = temp01[4];
                            string[] TO = { Approver10email };

                            string Subject = template.Name + " is pending for review ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is initiated by" + Initiate + "and requested for your review.<br/><br/>";
                            string Body = "Dear " + Approver10 + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(TO, Subject, Body);
                        }
                        else
                        {
                            Logger.Info("Accessed DB, Checking Template Details: Next Approver Not Found");
                            template.Status = "In Effect";
                            template.NextApprover = 0.ToString();
                            Logger.Info("Accessed DB, Template Approved");

                            tblTemplateLog log1 = new tblTemplateLog
                            {
                                LogTemplateUID = template.TemplateID,
                                ModifiedBy = template.Approver1ID.ToString(),
                                LogActivity = "Template InEffect",
                                ChangedFrom = "-",
                                ChangedTo = "-",
                                DateandTime = DateTime.Now.ToString()
                            };
                            db.tblTemplateLogs.Add(log1);
                            db.SaveChanges();
                            Logger.Info("Accessed DB, Template Log Record Saved");

                            string approver = "";
                            approver = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                            string approveremail = "";
                            approveremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                            //var tableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                            //string[] temp = new string[5];

                            //foreach (var r in tableVariable)
                            //{
                            //    temp[0] = r.Variable;
                            //    temp[1] = r.Value;
                            //    if (temp[0] == "Vendor Name")
                            //    {
                            //        temp[3] = "Vendor Name";
                            //        temp[4] = temp[1];
                            //    }

                            //}

                            //string vendorName = temp[4];
                            string[] To = { approveremail };

                            string Subject = template.Name + " Approved and ready to Use ";
                            string UrL = ApplicationLink + "/Template/Details/" + ID;
                            string Paragraph = "The Template details as mentioned below is approved and ready to use by other users.<br/><br/>";
                            string Body = "Dear " + approver + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(To, Subject, Body);
                        }

                    }

                    else if (template.Approver10ID.ToString() == CurrentUser && template.Approver10Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver10 Found");
                        template.Approver10ApprovedOn = DateTime.Now;
                        template.Approver10Comments = Comments;
                        template.Approver10Status = "Approved";
                        template.Approver10Draft = content;
                        for (int i = 0; i < arrVariableNames.Length; i++)
                        {
                            Logger.Info("Attempt Contract Variables");
                            try
                            {
                                Logger.Info("Accessed DB, Checking Contract Variable Details: Details Not Found");
                                Logger.Info("Accessing DB for Saving the Contract Variables");
                                tblVariableData newVariable = new tblVariableData
                                {
                                    Type = "Template",
                                    TypeID = ID,
                                    Variable = arrVariableNames[i],
                                    Value = "",
                                    Version = "Approver 10",
                                };

                                db.tblVariableDatas.Add(newVariable);


                                Logger.Info("Accessed DB, Contract Variables Saved");
                            }

                            catch { }
                        }

                        template.Template = content;
                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Approved",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");

                        Logger.Info("Accessed DB, Checking Template Details:Further Approvers Not Found");
                        template.Status = "In Effect";
                        template.NextApprover = 0.ToString();
                        Logger.Info("Accessed DB, Template Approved");

                        tblTemplateLog log1 = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Template InEffect",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log1);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");

                        string approver = "";
                        approver = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                        string approveremail = "";
                        approveremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                        //var tableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == ID) select tblVariableData;
                        //string[] temp = new string[5];

                        //foreach (var r in tableVariable)
                        //{
                        //    temp[0] = r.Variable;
                        //    temp[1] = r.Value;
                        //    if (temp[0] == "Vendor Name")
                        //    {
                        //        temp[3] = "Vendor Name";
                        //        temp[4] = temp[1];
                        //    }

                        //}

                        //string vendorName = temp[4];
                        string[] To = { approveremail };

                        string Subject = template.Name + " Approved and ready to Use ";
                        string UrL = ApplicationLink + "/ Template/Details/" + ID;
                        string Paragraph = "The Template details as mentioned below is approved and ready to use by other users.<br/><br/>";
                        string Body = "Dear " + approver + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/>System Admin<br/> Contract Management System</body></html>";
                        SMTP.Send(To, Subject, Body);
                    }
                    try
                    {

                        string LogChangeFrom = "";
                        string LogChangeTo = "";
                        var Deligation = /*from tblDeligationMatrix in*/ db.tblDeligationMatrices.Select(x => new { x.DeligateFrom, x.DeligateTo, x.StartDate, x.EndDate, x.ID }); //select tblDeligationMatrix;
                        foreach (var r in Deligation)
                        {
                            int Found = 1;
                            DateTime startDate = DateTime.ParseExact(r.StartDate, "dd/MM/yyyy", null);
                            Found = DateTime.Compare(startDate, DateTime.Now);

                            if (Found <= 0)
                            {
                                Found = 1;
                                DateTime endDate = DateTime.ParseExact(r.EndDate, "dd/MM/yyyy", null);
                                Found = DateTime.Compare(endDate, DateTime.Now);
                                if (Found < 0)
                                {
                                    var DID = r.ID;

                                    tblDeligationMatrix Deligate = db.tblDeligationMatrices.Find(DID);
                                    db.tblDeligationMatrices.Remove(Deligate);

                                }
                                else
                                {
                                    try
                                    {
                                        //DeligationMethod(Convert.ToInt32(eachItem.DeligateFrom), Convert.ToInt32(eachItem.DeligateTo));
                                        if ((r.DeligateFrom == template.Initiator) && (template.Status == "Pending Approval" || template.Status == "Rejected"))
                                        {
                                            template.Initiator = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Initiator in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom.ToString() == template.NextApprover) && (template.Status == "Pending Approval" || template.Status == "Rejected"))
                                        {
                                            template.NextApprover = r.DeligateTo.ToString();
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Next Approver in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom.ToString() == template.RejectedBy) && (template.Status == "Pending Approval" || template.Status == "Rejected"))
                                        {
                                            template.RejectedBy = r.DeligateTo.ToString();
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Rejected By in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver1ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver1Status == "Pending Approval" || template.Approver1Status == "Rejected" || template.Approver1Status == null))
                                        {
                                            template.Approver1ID = (int)r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 1 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver2ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver2Status == "Pending Approval" || template.Approver2Status == "Rejected" || template.Approver2Status == null))
                                        {
                                            template.Approver2ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 2 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver3ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver3Status == "Pending Approval" || template.Approver3Status == "Rejected" || template.Approver3Status == null))
                                        {
                                            template.Approver3ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 3 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver4ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver4Status == "Pending Approval" || template.Approver4Status == "Rejected" || template.Approver4Status == null))
                                        {
                                            template.Approver4ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 4 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver5ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver5Status == "Pending Approval" || template.Approver5Status == "Rejected" || template.Approver5Status == null))
                                        {
                                            template.Approver5ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 5 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver6ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver6Status == "Pending Approval" || template.Approver6Status == "Rejected" || template.Approver6Status == null))
                                        {
                                            template.Approver6ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 6 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver7ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver7Status == "Pending Approval" || template.Approver7Status == "Rejected" || template.Approver7Status == null))
                                        {
                                            template.Approver7ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 7 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver8ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver8Status == "Pending Approval" || template.Approver8Status == "Rejected" || template.Approver8Status == null))
                                        {
                                            template.Approver8ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 8 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver9ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver9Status == "Pending Approval" || template.Approver9Status == "Rejected" || template.Approver9Status == null))
                                        {
                                            template.Approver9ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 9 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver10ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver10Status == "Pending Approval" || template.Approver10Status == "Rejected" || template.Approver10Status == null))
                                        {
                                            template.Approver10ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 10 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                    }
                                    catch
                                    { }
                                }

                            }


                        }

                        db.Entry(template).State = EntityState.Modified;

                    }
                    catch { }
                    db.SaveChanges();

                    Logger.Info("Accessed DB, Template Approved and Saved");

                    string[] Result = new string[2];
                    Result[0] = "success";
                    Result[1] = template.Status;
                    return Json(Result);

                }
                return Json("failure");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'ApproveTemplate' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AcceptChangesLastAprrover(int ID, string finalcontent)
        {
           
            Logger.Info("Attempt Template AcceptChangesLastAprrover");
            try
            {
                string CurrentUser = User.Identity.Name.Split('|')[1];
                Logger.Info("Accessing DB for Saving the Template Details");
                tblTemplateMaster template = db.tblTemplateMasters.Find(ID);

                template.Template = finalcontent;
                //template.Template = finalcontent;
                //if (template.Approver1ID.ToString() == CurrentUser)
                //{
                //    template.Approver1Draft = finalcontent;
                //}
                //else if (template.Approver2ID.ToString() == CurrentUser)
                //{
                //    template.Approver2Draft = finalcontent;
                //}
                //else if (template.Approver3ID.ToString() == CurrentUser)
                //{
                //    template.Approver3Draft = finalcontent;
                //}
                //else if (template.Approver4ID.ToString() == CurrentUser)
                //{
                //    template.Approver4Draft = finalcontent;
                //}
                //else if (template.Approver5ID.ToString() == CurrentUser)
                //{
                //    template.Approver5Draft = finalcontent;
                //}
                //else if (template.Approver6ID.ToString() == CurrentUser)
                //{
                //    template.Approver6Draft = finalcontent;
                //}
                //else if (template.Approver7ID.ToString() == CurrentUser)
                //{
                //    template.Approver7Draft = finalcontent;
                //}
                //else if (template.Approver8ID.ToString() == CurrentUser)
                //{
                //    template.Approver8Draft = finalcontent;
                //}
                //else if (template.Approver9ID.ToString() == CurrentUser)
                //{
                //    template.Approver9Draft = finalcontent;
                //}
                //else if (template.Approver10ID.ToString() == CurrentUser)
                //{
                //    template.Approver10Draft = finalcontent;
                //}
                db.Entry(template).State = EntityState.Modified;
                db.SaveChanges();
                Logger.Info("Accessed DB, Details Saved to DB");
                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'AcceptChangesLastAprrover' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]

        public ActionResult GetApproversForTemplate(int id)
        {
            Logger.Info("Attempt Template GetApproversForTemplate");

            try
            {
                string[] Users = new string[0];
                Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                var result = /*from tblTemplateMaster in*/ db.tblTemplateMasters.Where(x => x.TemplateID == id).Select(x => new
                {
                    x.Template,
                    x.InitiatorStatus,
                    x.Approver10Status,
                    x.Approver1Status,
                    x.Approver2Status,
                    x.Approver3Status,
                    x.Approver4Status,
                    x.Approver5Status,
                    x.Approver6Status,
                    x.Approver7Status,
                    x.Approver8Status,
                    x.Approver9Status,
                    x.Approver10ID,x.Approver1ID,x.Approver2ID,x.Approver3ID,x.Approver4ID,x.Approver5ID,x.Approver6ID,
                    x.Approver7ID,x.Approver8ID,x.Approver9ID
                }); //select tblTemplateMaster;

                foreach (var item in result)
                {
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Draft");
                    if (string.IsNullOrWhiteSpace(item.Template) == false)
                    {
                        Array.Resize(ref Users, Users.Length + 1);
                        Users[Users.Length - 1] = "Current Version";
                        Logger.Info("Accessed DB, Checking Template Details: Template Draft Found");
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Initiator Status");
                    if (item.InitiatorStatus == "Initiated")
                    {
                        Array.Resize(ref Users, Users.Length + 1);

                        Users[Users.Length - 1] = "Initiator";
                        Logger.Info("Accessed DB, Checking Template Details: Template Initiator Found");
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver1 Status");
                    if (item.Approver1Status == "Approved")
                    {
                        var AproverID = item.Approver1ID;

                        var ApproverDetail = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeID == AproverID).Select(x => new { x.UserRoleLegal }); //select tblUserMaster;
                        foreach(var r in ApproverDetail)
                        {
                            if(r.UserRoleLegal == true)
                            {
                                Array.Resize(ref Users, Users.Length + 1);
                                Users[Users.Length - 1] = "Approver 1" + " : " + item.Approver1ID;
                                Logger.Info("Accessed DB, Checking Template Details: Template Approver1 Found");
                            }
                        }
                        
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver2 Status");
                    if (item.Approver2Status == "Approved")
                    {
                        var AproverID = item.Approver2ID;

                        var ApproverDetail = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeID == AproverID).Select(x => new { x.UserRoleLegal }); //select tblUserMaster;
                        foreach (var r in ApproverDetail)
                        {
                            if (r.UserRoleLegal == true)
                            {
                                Array.Resize(ref Users, Users.Length + 1);
                                Users[Users.Length - 1] = "Approver 2" + " : " + item.Approver2ID;
                                Logger.Info("Accessed DB, Checking Template Details: Template Approver2 Found");
                            }
                        }
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver3 Status");
                    if (item.Approver3Status == "Approved")
                    {
                        var AproverID = item.Approver3ID;

                        var ApproverDetail = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeID == AproverID).Select(x => new { x.UserRoleLegal }); //select tblUserMaster;
                        foreach (var r in ApproverDetail)
                        {
                            if (r.UserRoleLegal == true)
                            {
                                Array.Resize(ref Users, Users.Length + 1);
                                Users[Users.Length - 1] = "Approver 3" + " : " + item.Approver3ID;
                                Logger.Info("Accessed DB, Checking Template Details: Template Approver3 Found");
                            }
                        }
                       
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver4 Status");
                    if (item.Approver4Status == "Approved")
                    {
                        var AproverID = item.Approver4ID;

                        var ApproverDetail = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeID == AproverID).Select(x => new { x.UserRoleLegal }); //select tblUserMaster;
                        foreach (var r in ApproverDetail)
                        {
                            if (r.UserRoleLegal == true)
                            {
                                Array.Resize(ref Users, Users.Length + 1);
                                Users[Users.Length - 1] = "Approver 4" + " : " + item.Approver4ID;
                                Logger.Info("Accessed DB, Checking Template Details: Template Approver4 Found");
                            }
                        }
                        
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver5 Status");
                    if (item.Approver5Status == "Approved")
                    {
                        var AproverID = item.Approver5ID;

                        var ApproverDetail = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeID == AproverID).Select(x => new { x.UserRoleLegal }); //select tblUserMaster;
                        foreach (var r in ApproverDetail)
                        {
                            if (r.UserRoleLegal == true)
                            {
                                Array.Resize(ref Users, Users.Length + 1);
                                Users[Users.Length - 1] = "Approver 5" + " : " + item.Approver5ID;
                                Logger.Info("Accessed DB, Checking Template Details: Template Approver5 Found");
                            }
                        }

                        
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver6 Status");
                    if (item.Approver6Status == "Approved")
                    {
                        var AproverID = item.Approver6ID;

                        var ApproverDetail = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeID == AproverID).Select(x => new { x.UserRoleLegal }); //select tblUserMaster;
                        foreach (var r in ApproverDetail)
                        {
                            if (r.UserRoleLegal == true)
                            {
                                Array.Resize(ref Users, Users.Length + 1);
                                Users[Users.Length - 1] = "Approver 6" + " : " + item.Approver6ID;
                                Logger.Info("Accessed DB, Checking Template Details: Template Approver6 Found");
                            }
                        }

                       
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver7 Status");
                    if (item.Approver7Status == "Approved")
                    {
                        var AproverID = item.Approver7ID;

                        var ApproverDetail = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeID == AproverID).Select(x => new { x.UserRoleLegal }); //select tblUserMaster;
                        foreach (var r in ApproverDetail)
                        {
                            if (r.UserRoleLegal == true)
                            {

                                Array.Resize(ref Users, Users.Length + 1);
                                Users[Users.Length - 1] = "Approver 7" + " : " + item.Approver7ID;
                                Logger.Info("Accessed DB, Checking Template Details: Template Approver7 Found");
                            }
                        }

                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver8 Status");
                    if (item.Approver8Status == "Approved")
                    {
                        var AproverID = item.Approver8ID;

                        var ApproverDetail = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeID == AproverID).Select(x => new { x.UserRoleLegal }); //select tblUserMaster;
                        foreach (var r in ApproverDetail)
                        {
                            if (r.UserRoleLegal == true)
                            {
                                Array.Resize(ref Users, Users.Length + 1);
                                Users[Users.Length - 1] = "Approver 8" + " : " + item.Approver8ID;
                                Logger.Info("Accessed DB, Checking Template Details: Template Approver8 Found");
                            }
                        }
                        
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver9 Status");
                    if (item.Approver9Status == "Approved")
                    {
                        var AproverID = item.Approver9ID;

                        var ApproverDetail = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeID == AproverID).Select(x => new { x.UserRoleLegal }); //select tblUserMaster;
                        foreach (var r in ApproverDetail)
                        {
                            if (r.UserRoleLegal == true)
                            {
                                Array.Resize(ref Users, Users.Length + 1);
                                Users[Users.Length - 1] = "Approver 9" + " : " + item.Approver9ID;
                                Logger.Info("Accessed DB, Checking Template Details: Template Approver9 Found");
                            }
                        }
                       
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver10 Status");
                    if (item.Approver10Status == "Approved")
                    {
                        var AproverID = item.Approver10ID;

                        var ApproverDetail = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeID == AproverID).Select(x => new { x.UserRoleLegal }); //select tblUserMaster;
                        foreach (var r in ApproverDetail)
                        {
                            if (r.UserRoleLegal == true)
                            {

                                Array.Resize(ref Users, Users.Length + 1);
                                Users[Users.Length - 1] = "Approver 10" + " : " + item.Approver10ID;
                                Logger.Info("Accessed DB, Checking Template Details: Template Approver10 Found");
                            }
                        }
                    }

                }
                Logger.Info("Accessed DB, Checking Template Details: Details Found");
                return Json(Users);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetApproversForTemplate' Action HTTP POST Main exception");

                return Json("errors");
            }
        }


        [HttpPost]
        public ActionResult GetVersionCompare(string selectvalue, int TemplateID)
        {
            Logger.Info("Attempt Template GetVersionCompare");
            try
            {
                Logger.Info("Accessing DB for Template Details");
                if (selectvalue.Contains("Current Version"))
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Template;
                    Logger.Info("Accessed DB, Checking Template Details: Template Draft Found");
                    return Json(result);
                }
                if (selectvalue.Contains("Initiator"))
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");

                    var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.InitiatorTemplate;
                    Logger.Info("Accessed DB, Checking Template Details: Initiator Draft Found");
                    return Json(result);
                }

                if (selectvalue.Contains("Approver 1"))
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver1Draft;
                    Logger.Info("Accessed DB, Checking Template Details: Approver1 Draft Found");
                    return Json(result);
                }

                if (selectvalue.Contains("Approver 2"))
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver2Draft;
                    Logger.Info("Accessed DB, Checking Template Details: Approver2 Draft Found");
                    return Json(result);
                }

                if (selectvalue.Contains("Approver 3"))
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver3Draft;
                    Logger.Info("Accessed DB, Checking Template Details: Approver3 Draft Found");
                    return Json(result);
                }

                if (selectvalue.Contains("Approver 4"))
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver4Draft;
                    Logger.Info("Accessed DB, Checking Template Details: Approver4 Draft Found");
                    return Json(result);
                }

                if (selectvalue.Contains("Approver 5"))
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver5Draft;
                    Logger.Info("Accessed DB, Checking Template Details: Approver5 Draft Found");
                    return Json(result);
                }

                if (selectvalue.Contains("Approver 6"))
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver6Draft;
                    Logger.Info("Accessed DB, Checking Template Details: Approver6 Draft Found");
                    return Json(result);
                }

                if (selectvalue.Contains("Approver 7"))
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver7Draft;
                    Logger.Info("Accessed DB, Checking Template Details: Approver7 Draft Found");
                    return Json(result);
                }

                if (selectvalue.Contains("Approver 8"))
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver8Draft;
                    Logger.Info("Accessed DB, Checking Template Details: Approver8 Draft Found");
                    return Json(result);
                }

                if (selectvalue.Contains("Approver 9"))
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver9Draft;
                    Logger.Info("Accessed DB, Checking Template Details: Approver9 Draft Found");
                    return Json(result);
                }

                if (selectvalue.Contains("Approver 10"))
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver10Draft;
                    Logger.Info("Accessed DB, Checking Template Details: Approver10 Draft Found");
                    return Json(result);
                }
                Logger.Info("Accessing DB for Template Details: Details Found");
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");

                return Json("errors");
            }
        }

        [HttpPost]
        public ActionResult GetVersionCompareForRestore(string selectvalue, int TemplateID)
        {
            Logger.Info("Attempt Template GetVersionCompare");
            try
            {
                Logger.Info("Accessing DB for Template Details");
                if (selectvalue.Contains("Current Version"))
                {
                    try
                    {
                        //Logger.Info("Accessing DB for Template Variable Details");
                        //var Variable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Template").Select(x => new { x.ID }); //select tblVariableData;
                        //if (Variable.ToList().Count > 0)
                        //{
                        //    foreach (var item in Variable)
                        //    {
                        //        var Id = item.ID;

                        //        tblVariableData eachvariable = db.tblVariableDatas.Find(Id);
                        //        db.tblVariableDatas.Remove(eachvariable);
                        //        //db.SaveChanges
                        //    }
                        //    db.SaveChanges();
                        //}


                        //var VariableforRestore = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Template") select tblVariableData;

                        //Logger.Info("Accessed DB, Checking Template Variable Details: Details Found");
                        //if (VariableforRestore.ToList().Count > 0)
                        //{

                        //    foreach (var item in VariableforRestore)
                        //    {
                        //        tblVariableData variable1 = new tblVariableData()
                        //        {
                        //            Type = "Template",
                        //            TypeID = TemplateID,
                        //            Variable = item.Variable,
                        //            Value = item.Value,
                        //            Version = "Template",
                        //        };
                        //        db.tblVariableDatas.Add(variable1);

                        //    }
                        //    db.SaveChanges();
                        //}
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");
                        return Json("error");
                    }

                    string result = "";
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result = (from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Template).First();
                    Logger.Info("Accessed DB, Checking Template Details: Template Draft Found");

                    tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
                    template.Template = result;
                    db.Entry(template).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(result);
                }
                else if (selectvalue.Contains("Initiator"))
                {
                    try
                    {
                        Logger.Info("Accessing DB for Template Variable Details");
                        var Variable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Template").Select(x => new { x.ID }); //select tblVariableData;
                        if (Variable.ToList().Count > 0)
                        {
                            foreach (var item in Variable)
                            {
                                var Id = item.ID;

                                tblVariableData eachvariable = db.tblVariableDatas.Find(Id);
                                db.tblVariableDatas.Remove(eachvariable);
                                //db.SaveChanges
                            }
                            db.SaveChanges();
                        }


                        var VariableforRestore = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Initiator") select tblVariableData;

                        Logger.Info("Accessed DB, Checking Template Variable Details: Details Found");
                        if (VariableforRestore.ToList().Count > 0)
                        {

                            foreach (var item in VariableforRestore)
                            {
                                tblVariableData variable1 = new tblVariableData()
                                {
                                    Type = "Template",
                                    TypeID = TemplateID,
                                    Variable = item.Variable,
                                    Value = item.Value,
                                    Version = "Template",
                                };
                                db.tblVariableDatas.Add(variable1);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");
                        return Json("error");
                    }

                    string result = "";

                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");

                    result = (from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.InitiatorTemplate).First();
                    Logger.Info("Accessed DB, Checking Template Details: Initiator Draft Found");

                    tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
                    template.Template = result;
                    db.Entry(template).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(result);
                }

                else if (selectvalue.Contains("Approver 1"))
                {
                    try
                    {
                        Logger.Info("Accessing DB for Template Variable Details");
                        var Variable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Template").Select(x => new { x.ID }); //select tblVariableData;
                        if (Variable.ToList().Count > 0)
                        {
                            foreach (var item in Variable)
                            {
                                var Id = item.ID;

                                tblVariableData eachvariable = db.tblVariableDatas.Find(Id);
                                db.tblVariableDatas.Remove(eachvariable);
                                //db.SaveChanges
                            }
                            db.SaveChanges();
                        }


                        var VariableforRestore = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Approver 1") select tblVariableData;

                        Logger.Info("Accessed DB, Checking Template Variable Details: Details Found");
                        if (VariableforRestore.ToList().Count > 0)
                        {

                            foreach (var item in VariableforRestore)
                            {
                                tblVariableData variable1 = new tblVariableData()
                                {
                                    Type = "Template",
                                    TypeID = TemplateID,
                                    Variable = item.Variable,
                                    Value = item.Value,
                                    Version = "Template",
                                };
                                db.tblVariableDatas.Add(variable1);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");
                        return Json("error");
                    }

                    string result = "";
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result = (from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver1Draft).First();
                    Logger.Info("Accessed DB, Checking Template Details: Approver1 Draft Found");

                    tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
                    template.Template = result;
                    db.Entry(template).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(result);
                }

                else if (selectvalue.Contains("Approver 2"))
                {
                    try
                    {
                        Logger.Info("Accessing DB for Template Variable Details");
                        var Variable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Template").Select(x => new { x.ID }); //select tblVariableData;
                        if (Variable.ToList().Count > 0)
                        {
                            foreach (var item in Variable)
                            {
                                var Id = item.ID;

                                tblVariableData eachvariable = db.tblVariableDatas.Find(Id);
                                db.tblVariableDatas.Remove(eachvariable);
                                //db.SaveChanges
                            }
                            db.SaveChanges();
                        }


                        var VariableforRestore = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Approver 2") select tblVariableData;

                        Logger.Info("Accessed DB, Checking Template Variable Details: Details Found");
                        if (VariableforRestore.ToList().Count > 0)
                        {

                            foreach (var item in VariableforRestore)
                            {
                                tblVariableData variable1 = new tblVariableData()
                                {
                                    Type = "Template",
                                    TypeID = TemplateID,
                                    Variable = item.Variable,
                                    Value = item.Value,
                                    Version = "Template",
                                };
                                db.tblVariableDatas.Add(variable1);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");
                        return Json("error");
                    }

                    string result = "";
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result = (from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver2Draft).First();
                    Logger.Info("Accessed DB, Checking Template Details: Approver2 Draft Found");

                    tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
                    template.Template = result;
                    db.Entry(template).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(result);
                }

                else if (selectvalue.Contains("Approver 3"))
                {
                    try
                    {
                        Logger.Info("Accessing DB for Template Variable Details");
                        var Variable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Template").Select(x => new { x.ID }); //select tblVariableData;
                        if (Variable.ToList().Count > 0)
                        {
                            foreach (var item in Variable)
                            {
                                var Id = item.ID;

                                tblVariableData eachvariable = db.tblVariableDatas.Find(Id);
                                db.tblVariableDatas.Remove(eachvariable);
                                //db.SaveChanges
                            }
                            db.SaveChanges();
                        }


                        var VariableforRestore = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Approver 3") select tblVariableData;

                        Logger.Info("Accessed DB, Checking Template Variable Details: Details Found");
                        if (VariableforRestore.ToList().Count > 0)
                        {

                            foreach (var item in VariableforRestore)
                            {
                                tblVariableData variable1 = new tblVariableData()
                                {
                                    Type = "Template",
                                    TypeID = TemplateID,
                                    Variable = item.Variable,
                                    Value = item.Value,
                                    Version = "Template",
                                };
                                db.tblVariableDatas.Add(variable1);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");
                        return Json("error");
                    }

                    string result = "";
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result = (from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver3Draft).First();
                    Logger.Info("Accessed DB, Checking Template Details: Approver3 Draft Found");

                    tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
                    template.Template = result;
                    db.Entry(template).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(result);
                }

                else if (selectvalue.Contains("Approver 4"))
                {
                    try
                    {
                        Logger.Info("Accessing DB for Template Variable Details");
                        var Variable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Template").Select(x => new { x.ID }); //select tblVariableData;
                        if (Variable.ToList().Count > 0)
                        {
                            foreach (var item in Variable)
                            {
                                var Id = item.ID;

                                tblVariableData eachvariable = db.tblVariableDatas.Find(Id);
                                db.tblVariableDatas.Remove(eachvariable);
                                //db.SaveChanges
                            }
                            db.SaveChanges();
                        }


                        var VariableforRestore = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Approver 4") select tblVariableData;

                        Logger.Info("Accessed DB, Checking Template Variable Details: Details Found");
                        if (VariableforRestore.ToList().Count > 0)
                        {

                            foreach (var item in VariableforRestore)
                            {
                                tblVariableData variable1 = new tblVariableData()
                                {
                                    Type = "Template",
                                    TypeID = TemplateID,
                                    Variable = item.Variable,
                                    Value = item.Value,
                                    Version = "Template",
                                };
                                db.tblVariableDatas.Add(variable1);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");
                        return Json("error");
                    }

                    string result = "";
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result = (from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver4Draft).First();
                    Logger.Info("Accessed DB, Checking Template Details: Approver4 Draft Found");

                    tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
                    template.Template = result;
                    db.Entry(template).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(result);
                }

                else if (selectvalue.Contains("Approver 5"))
                {
                    try
                    {
                        Logger.Info("Accessing DB for Template Variable Details");
                        var Variable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Template").Select(x => new { x.ID }); //select tblVariableData;
                        if (Variable.ToList().Count > 0)
                        {
                            foreach (var item in Variable)
                            {
                                var Id = item.ID;

                                tblVariableData eachvariable = db.tblVariableDatas.Find(Id);
                                db.tblVariableDatas.Remove(eachvariable);
                                //db.SaveChanges
                            }
                            db.SaveChanges();
                        }


                        var VariableforRestore = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Approver 5") select tblVariableData;

                        Logger.Info("Accessed DB, Checking Template Variable Details: Details Found");
                        if (VariableforRestore.ToList().Count > 0)
                        {

                            foreach (var item in VariableforRestore)
                            {
                                tblVariableData variable1 = new tblVariableData()
                                {
                                    Type = "Template",
                                    TypeID = TemplateID,
                                    Variable = item.Variable,
                                    Value = item.Value,
                                    Version = "Template",
                                };
                                db.tblVariableDatas.Add(variable1);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");
                        return Json("error");
                    }

                    string result = "";
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result = (from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver5Draft).First();
                    Logger.Info("Accessed DB, Checking Template Details: Approver5 Draft Found");

                    tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
                    template.Template = result;
                    db.Entry(template).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(result);
                }

                else if (selectvalue.Contains("Approver 6"))
                {
                    try
                    {
                        Logger.Info("Accessing DB for Template Variable Details");
                        var Variable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Template").Select(x => new { x.ID }); //select tblVariableData;
                        if (Variable.ToList().Count > 0)
                        {
                            foreach (var item in Variable)
                            {
                                var Id = item.ID;

                                tblVariableData eachvariable = db.tblVariableDatas.Find(Id);
                                db.tblVariableDatas.Remove(eachvariable);
                                //db.SaveChanges
                            }
                            db.SaveChanges();
                        }


                        var VariableforRestore = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Approver 6") select tblVariableData;

                        Logger.Info("Accessed DB, Checking Template Variable Details: Details Found");
                        if (VariableforRestore.ToList().Count > 0)
                        {

                            foreach (var item in VariableforRestore)
                            {
                                tblVariableData variable1 = new tblVariableData()
                                {
                                    Type = "Template",
                                    TypeID = TemplateID,
                                    Variable = item.Variable,
                                    Value = item.Value,
                                    Version = "Template",
                                };
                                db.tblVariableDatas.Add(variable1);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");
                        return Json("error");
                    }

                    string result = "";
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result = (from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver6Draft).First();
                    Logger.Info("Accessed DB, Checking Template Details: Approver6 Draft Found");

                    tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
                    template.Template = result;
                    db.Entry(template).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(result);
                }

                else if (selectvalue.Contains("Approver 7"))
                {
                    try
                    {
                        Logger.Info("Accessing DB for Template Variable Details");
                        var Variable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Template").Select(x => new { x.ID }); //select tblVariableData;
                        if (Variable.ToList().Count > 0)
                        {
                            foreach (var item in Variable)
                            {
                                var Id = item.ID;

                                tblVariableData eachvariable = db.tblVariableDatas.Find(Id);
                                db.tblVariableDatas.Remove(eachvariable);
                                //db.SaveChanges
                            }
                            db.SaveChanges();
                        }


                        var VariableforRestore = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Approver 7") select tblVariableData;

                        Logger.Info("Accessed DB, Checking Template Variable Details: Details Found");
                        if (VariableforRestore.ToList().Count > 0)
                        {

                            foreach (var item in VariableforRestore)
                            {
                                tblVariableData variable1 = new tblVariableData()
                                {
                                    Type = "Template",
                                    TypeID = TemplateID,
                                    Variable = item.Variable,
                                    Value = item.Value,
                                    Version = "Template",
                                };
                                db.tblVariableDatas.Add(variable1);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");
                        return Json("error");
                    }

                    string result = "";
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result = (from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver7Draft).First();
                    Logger.Info("Accessed DB, Checking Template Details: Approver7 Draft Found");

                    tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
                    template.Template = result;
                    db.Entry(template).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(result);
                }

                else if (selectvalue.Contains("Approver 8"))
                {
                    try
                    {
                        Logger.Info("Accessing DB for Template Variable Details");
                        var Variable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Template").Select(x => new { x.ID }); //select tblVariableData;
                        if (Variable.ToList().Count > 0)
                        {
                            foreach (var item in Variable)
                            {
                                var Id = item.ID;

                                tblVariableData eachvariable = db.tblVariableDatas.Find(Id);
                                db.tblVariableDatas.Remove(eachvariable);
                                //db.SaveChanges
                            }
                            db.SaveChanges();
                        }


                        var VariableforRestore = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Approver 8") select tblVariableData;

                        Logger.Info("Accessed DB, Checking Template Variable Details: Details Found");
                        if (VariableforRestore.ToList().Count > 0)
                        {

                            foreach (var item in VariableforRestore)
                            {
                                tblVariableData variable1 = new tblVariableData()
                                {
                                    Type = "Template",
                                    TypeID = TemplateID,
                                    Variable = item.Variable,
                                    Value = item.Value,
                                    Version = "Template",
                                };
                                db.tblVariableDatas.Add(variable1);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");
                        return Json("error");
                    }

                    string result = "";
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result = (from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver8Draft).First();
                    Logger.Info("Accessed DB, Checking Template Details: Approver8 Draft Found");

                    tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
                    template.Template = result;
                    db.Entry(template).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(result);
                }

                else if (selectvalue.Contains("Approver 9"))
                {
                    try
                    {
                        Logger.Info("Accessing DB for Template Variable Details");
                        var Variable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Template").Select(x => new { x.ID }); //select tblVariableData;
                        if (Variable.ToList().Count > 0)
                        {
                            foreach (var item in Variable)
                            {
                                var Id = item.ID;

                                tblVariableData eachvariable = db.tblVariableDatas.Find(Id);
                                db.tblVariableDatas.Remove(eachvariable);
                                //db.SaveChanges
                            }
                            db.SaveChanges();
                        }


                        var VariableforRestore = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Approver 9") select tblVariableData;

                        Logger.Info("Accessed DB, Checking Template Variable Details: Details Found");
                        if (VariableforRestore.ToList().Count > 0)
                        {

                            foreach (var item in VariableforRestore)
                            {
                                tblVariableData variable1 = new tblVariableData()
                                {
                                    Type = "Template",
                                    TypeID = TemplateID,
                                    Variable = item.Variable,
                                    Value = item.Value,
                                    Version = "Template",
                                };
                                db.tblVariableDatas.Add(variable1);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");
                        return Json("error");
                    }

                    string result = "";
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result = (from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver9Draft).First();
                    Logger.Info("Accessed DB, Checking Template Details: Approver9 Draft Found");

                    tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
                    template.Template = result;
                    db.Entry(template).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(result);
                }

                if (selectvalue.Contains("Approver 10"))
                {
                    try
                    {
                        Logger.Info("Accessing DB for Template Variable Details");
                        var Variable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Template").Select(x => new { x.ID }); //select tblVariableData;
                        if (Variable.ToList().Count > 0)
                        {
                            foreach (var item in Variable)
                            {
                                var Id = item.ID;

                                tblVariableData eachvariable = db.tblVariableDatas.Find(Id);
                                db.tblVariableDatas.Remove(eachvariable);
                                //db.SaveChanges
                            }
                            db.SaveChanges();
                        }


                        var VariableforRestore = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID).Where(x => x.Type == "Template").Where(x => x.Version == "Approver 10") select tblVariableData;

                        Logger.Info("Accessed DB, Checking Template Variable Details: Details Found");
                        if (VariableforRestore.ToList().Count > 0)
                        {

                            foreach (var item in VariableforRestore)
                            {
                                tblVariableData variable1 = new tblVariableData()
                                {
                                    Type = "Template",
                                    TypeID = TemplateID,
                                    Variable = item.Variable,
                                    Value = item.Value,
                                    Version = "Template",
                                };
                                db.tblVariableDatas.Add(variable1);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");
                        return Json("error");
                    }

                    string result = "";
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result = (from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID) select tblTemplateMaster.Approver10Draft).First();
                    Logger.Info("Accessed DB, Checking Template Details: Approver10 Draft Found");

                    tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
                    template.Template = result;
                    db.Entry(template).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(result);
                }
                Logger.Info("Accessing DB for Template Details: Details Found");
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetVersionCompare' Action HTTP POST Main exception");

                return Json("errors");
            }
        }


        [HttpPost]
        public ActionResult GetApproverComments(int TemplateID)
        {
            Logger.Info("Attempt Template GetApproverComments");
            try
            {
                Logger.Info("Accessing DB for Template Details");
                if (!string.IsNullOrWhiteSpace(TemplateID.ToString()))
                {
                    string[] Names = new string[12];
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    var result = /*from tblTemplateMaster in */db.tblTemplateMasters.Where(x => x.TemplateID == TemplateID).Select(x => new
                    {
                        x.Initiator,
                        x.Approver1ID,
                        x.Approver2ID,
                        x.Approver3ID,
                        x.Approver4ID,
                        x.Approver5ID,
                        x.Approver6ID,
                        x.Approver7ID,
                        x.Approver8ID,
                        x.Approver9ID,
                        x.Approver10ID
                    }); //select tblTemplateMaster;
                    foreach (var item in result)
                    {

                        var Initiator = item.Initiator;
                        var Approver1 = item.Approver1ID;
                        var Approver2 = item.Approver2ID;
                        var Approver3 = item.Approver3ID;
                        var Approver4 = item.Approver4ID;
                        var Approver5 = item.Approver5ID;
                        var Approver6 = item.Approver6ID;
                        var Approver7 = item.Approver7ID;
                        var Approver8 = item.Approver8ID;
                        var Approver9 = item.Approver9ID;
                        var Approver10 = item.Approver10ID;


                        string InitiatorName = "";
                        string Approver1Name = "";
                        string Approver2Name = "";
                        string Approver3Name = "";
                        string Approver4Name = "";
                        string Approver5Name = "";
                        string Approver6Name = "";
                        string Approver7Name = "";
                        string Approver8Name = "";
                        string Approver9Name = "";
                        string Approver10Name = "";
                        try
                        {
                            InitiatorName = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == Initiator) select tblUserMaster.UserEmployeeName).First();
                        }
                        catch { }

                        try
                        {
                            Approver1Name = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == Approver1) select tblUserMaster.UserEmployeeName).First();
                        }
                        catch { }

                        try
                        {
                            Approver2Name = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == Approver2) select tblUserMaster.UserEmployeeName).First();
                        }
                        catch { }

                        try
                        {
                            Approver3Name = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == Approver3) select tblUserMaster.UserEmployeeName).First();
                        }
                        catch { }

                        try
                        {
                            Approver4Name = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == Approver4) select tblUserMaster.UserEmployeeName).First();
                        }
                        catch { }

                        try
                        {
                            Approver5Name = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == Approver5) select tblUserMaster.UserEmployeeName).First();
                        }
                        catch { }

                        try
                        {
                            Approver6Name = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == Approver6) select tblUserMaster.UserEmployeeName).First();
                        }
                        catch { }

                        try
                        {
                            Approver7Name = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == Approver7) select tblUserMaster.UserEmployeeName).First();
                        }
                        catch { }

                        try
                        {
                            Approver8Name = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == Approver8) select tblUserMaster.UserEmployeeName).First();
                        }
                        catch { }

                        try
                        {
                            Approver9Name = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == Approver9) select tblUserMaster.UserEmployeeName).First();
                        }
                        catch { }

                        try
                        {
                            Approver10Name = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == Approver10) select tblUserMaster.UserEmployeeName).First();
                        }
                        catch { }

                        Names[0] = "success";
                        Names[1] = InitiatorName;
                        Names[2] = Approver1Name;
                        Names[3] = Approver2Name;
                        Names[4] = Approver3Name;
                        Names[5] = Approver4Name;
                        Names[6] = Approver5Name;
                        Names[7] = Approver6Name;
                        Names[8] = Approver7Name;
                        Names[9] = Approver8Name;
                        Names[10] = Approver9Name;
                        Names[11] = Approver10Name;
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Template Initiator and Approvers Comments Found");
                    return Json(Names);
                }
                Logger.Info("Accessed DB, Checking Template Details: Template Initiator and Approvers Comments Not Found");
                return Json("error");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetApproverComments' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult GetApproversBasedOnDeptAndSubDept(int LevelNumber)
        {
            Logger.Info("Attempt Contract GetApproversBasedOnClusterAndFunction");
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

                Logger.Info("Accessed DB, Checking ApprovalMaster Details: Department and SubDepartment match");
                var ApproverList = from tblApprovalMaster in db.tblApprovalMasters.Where(x => x.Plant == EmployeePlant).Where(x => x.WorkflowType == "Template").Where(x => x.ApprovalLevel == LevelNumber)
                                   select tblApprovalMaster.EMPID;
                Logger.Info("Accessed DB, Checking UserMaster Details: Department and SubDepartment Found");
                return Json(ApproverList);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetApproversBasedOnDeptAndSubDept' Action HTTP POST Main exception");
                return Json("error");
            }
        }

       



        [HttpPost]
        public ActionResult GetPreviousUsers(int id)
        {
            Logger.Info("Attempt Template GetPreviousUsers");
            try
            {
                Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                string[] Users = new string[0];
                var result = /*from tblTemplateMaster in*/ db.tblTemplateMasters.Where(x => x.TemplateID == id).Select(x => new
                {
                    x.InitiatorStatus,
                    x.Approver9Status,
                    x.Approver8Status,
                    x.Approver7Status,
                    x.Approver6Status,
                    x.Approver5Status,
                    x.Approver4Status,
                    x.Approver3Status,
                    x.Approver2Status,
                    x.Approver1Status,
                    x.Approver10Status
                }); //select tblTemplateMaster;

                foreach (var item in result)
                {
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Initiator Status");
                    if (item.InitiatorStatus == "Initiated")
                    {
                        Array.Resize(ref Users, Users.Length + 1);

                        Users[Users.Length - 1] = "Initiator";
                        Logger.Info("Accessed DB, Checking Template Details: Template Initiator Found");
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver1 Status");
                    if (item.Approver1Status == "Approved")
                    {
                        Array.Resize(ref Users, Users.Length + 1);
                        Users[Users.Length - 1] = "Approver 1";
                        Logger.Info("Accessed DB, Checking Template Details: Template Approver1 Found");
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver2 Status");
                    if (item.Approver2Status == "Approved")
                    {
                        Array.Resize(ref Users, Users.Length + 1);
                        Users[Users.Length - 1] = "Approver 2";
                        Logger.Info("Accessed DB, Checking Template Details: Template Approver2 Found");
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver3 Status");
                    if (item.Approver3Status == "Approved")
                    {
                        Array.Resize(ref Users, Users.Length + 1);
                        Users[Users.Length - 1] = "Approver 3";
                        Logger.Info("Accessed DB, Checking Template Details: Template Approver3 Found");
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver4 Status");
                    if (item.Approver4Status == "Approved")
                    {
                        Array.Resize(ref Users, Users.Length + 1);
                        Users[Users.Length - 1] = "Approver 4";
                        Logger.Info("Accessed DB, Checking Template Details: Template Approver4 Found");
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver5 Status");
                    if (item.Approver5Status == "Approved")
                    {
                        Array.Resize(ref Users, Users.Length + 1);
                        Users[Users.Length - 1] = "Approver 5";
                        Logger.Info("Accessed DB, Checking Template Details: Template Approver5 Found");
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver6 Status");
                    if (item.Approver6Status == "Approved")
                    {
                        Array.Resize(ref Users, Users.Length + 1);
                        Users[Users.Length - 1] = "Approver 6";
                        Logger.Info("Accessed DB, Checking Template Details: Template Approver6 Found");
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver7 Status");
                    if (item.Approver7Status == "Approved")
                    {
                        Array.Resize(ref Users, Users.Length + 1);
                        Users[Users.Length - 1] = "Approver 7";
                        Logger.Info("Accessed DB, Checking Template Details: Template Approver7 Found");
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver8 Status");
                    if (item.Approver8Status == "Approved")
                    {
                        Array.Resize(ref Users, Users.Length + 1);
                        Users[Users.Length - 1] = "Approver 8";
                        Logger.Info("Accessed DB, Checking Template Details: Template Approver8 Found");
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver9 Status");
                    if (item.Approver9Status == "Approved")
                    {
                        Array.Resize(ref Users, Users.Length + 1);
                        Users[Users.Length - 1] = "Approver 9";
                        Logger.Info("Accessed DB, Checking Template Details: Template Approver9 Found");
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Checking Template Approver10 Status");
                    if (item.Approver10Status == "Approved")
                    {
                        Array.Resize(ref Users, Users.Length + 1);
                        Users[Users.Length - 1] = "Approver 10";
                        Logger.Info("Accessed DB, Checking Template Details: Template Approver10 Found");
                    }

                }

                return Json(Users);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetPreviousUsers' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult AssignToPreviousUser(int TemplateID, string Comments)
        {
            int CurrentUserID = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUserID = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Template AssignToPreviousUser");
            try
            {
                string CurrentUser = User.Identity.Name.Split('|')[1];
                Logger.Info("Accessing DB for Assigning to Previous Users in Template");
                tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);

                Logger.Info("Accessed DB, Checking Template Details: Checking Approver");
                if (template.NextApprover.ToString() == CurrentUser)
                {
                    Logger.Info("Accessed DB, Checking Template Details: Approver Found");
                    if (template.Approver1ID.ToString() == CurrentUser && template.Approver1Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver1 Found");
                        template.Approver1Status = "Rejected";
                        template.Approver1Comments = Comments;

                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Rejected",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");
                        //initiator mailcode
                       

                            template.InitiatorStatus = "Rework";
                            template.NextApprover = template.Initiator.ToString();

                        string Initiator = "";
                        Initiator = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                        string approver1 = "";
                        approver1 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeName).First();
                        string Initiatoremail = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                        //var TableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID) select tblVariableData;
                        //string[] temp = new string[5];

                        //foreach (var r in TableVariable)
                        //{
                        //    temp[0] = r.Variable;
                        //    temp[1] = r.Value;
                        //    if (temp[0] == "Vendor Name")
                        //    {
                        //        temp[3] = "Vendor Name";
                        //        temp[4] = temp[1];
                        //    }

                        //}
                        string employeename = Initiator;
                        //string VendorName = temp[4];
                        string[] TO = { Initiatoremail };

                        string Subject = template.Name + " is Rejected ";
                        string UrL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string Paragraph = "The Template details as mentioned below is sent for revision by " + approver1 + " to " + Initiator + " .<br/><br/>";
                        string Body = "Dear " + Initiator + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(TO, Subject, Body);
                        Logger.Info("Accessed DB, Checking Template Details: Initiator is Assigned");
                            
                      
                    }
                    else if (template.Approver2ID.ToString() == CurrentUser && template.Approver2Status == "Pending Approval")
                    {
                        template.Approver2Status = "Rejected";
                        template.Approver2Comments = Comments;

                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Rejected",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");
                        
                       
                            template.InitiatorStatus = "Rework";
                            template.NextApprover = template.Initiator.ToString();
                        string Initiator = "";
                        Initiator = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                        string approver2 = "";
                        approver2 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeName).First();
                        string Initiatoremail = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                        
                        string employeename = Initiator;
                        //string VendorName = temp[4];
                        string[] TO = { Initiatoremail };

                        string Subject = template.Name + " is Rejected ";
                        string UrL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string Paragraph = "The Template details as mentioned below is sent for revision by " + approver2 + " to " + Initiator + " .<br/><br/>";
                        string Body = "Dear " + Initiator + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(TO, Subject, Body);

                        if (template.Approver1Status == "Approved")
                        {
                            string Approver1 = "";
                            Approver1 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeName).First();
                            approver2 = "";
                            approver2 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver1email = "";
                            Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeEmail).First();

                            //string employeename = Approver1;
                            //string VendorName = temp[4];
                            string[] To = { Approver1email };

                            string subject = template.Name + " is Rejected ";
                            string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                            string paragraph = "The Template details as mentioned below is sent for revision by " + approver2 + " to " + Initiator + " .<br/><br/>";
                            string body = "Dear " + Approver1 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(To, subject, body);
                        }
                         Logger.Info("Accessed DB, Checking Template Details: Initiator is Assigned");
                       
                    }
                    else if (template.Approver3ID.ToString() == CurrentUser && template.Approver3Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver3 Found");
                        template.Approver3Status = "Rejected";
                        template.Approver3Comments = Comments;

                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Rejected",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");
                        
                       
                            template.InitiatorStatus = "Rework";
                            template.NextApprover = template.Initiator.ToString();
                        string Initiator = "";
                        Initiator = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                        string approver3 = "";
                        approver3 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeName).First();
                        string Initiatoremail = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                        //var TableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID) select tblVariableData;
                        //string[] temp = new string[5];

                        //foreach (var r in TableVariable)
                        //{
                        //    temp[0] = r.Variable;
                        //    temp[1] = r.Value;
                        //    if (temp[0] == "Vendor Name")
                        //    {
                        //        temp[3] = "Vendor Name";
                        //        temp[4] = temp[1];
                        //    }

                        //}
                        string employeename = Initiator;
                        //string VendorName = temp[4];
                        string[] TO = { Initiatoremail };

                        string Subject = template.Name + " is Rejected ";
                        string UrL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string Paragraph = "The Template details as mentioned below is sent for revision by " + approver3 + " to " + Initiator + " .<br/><br/>";
                        string Body = "Dear " + Initiator + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(TO, Subject, Body);

                        if (template.Approver1Status == "Approved")
                        {
                            string Approver1 = "";
                            Approver1 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeName).First();
                            approver3 = "";
                            approver3 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver1email = "";
                            Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeEmail).First();

                            //string employeename = Approver1;
                            //string VendorName = temp[4];
                            string[] To = { Approver1email };

                            string subject = template.Name + " is Rejected ";
                            string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                            string paragraph = "The Template details as mentioned below is sent for revision by " + approver3 + " to " + Initiator + " .<br/><br/>";
                            string body = "Dear " + Approver1 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(To, subject, body);
                        }
                        if (template.Approver2Status == "Approved")
                        {
                            string Approver2 = "";
                            Approver2 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeName).First();
                            approver3 = "";
                            approver3 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeName).First();
                            string Approver1email = "";
                            Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeEmail).First();

                            //string employeename = Approver1;
                            //string VendorName = temp[4];
                            string[] To = { Approver1email };

                            string subject = template.Name + " is Rejected ";
                            string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                            string paragraph = "The Template details as mentioned below is sent for revision by " + approver3 + " to " + Initiator + " .<br/><br/>";
                            string body = "Dear " + Approver2 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                            SMTP.Send(To, subject, body);
                        }
                        Logger.Info("Accessed DB, Checking Template Details: Initiator is Assigned");
                        }
                    else if (template.Approver4ID.ToString() == CurrentUser && template.Approver4Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver4 Found");
                        template.Approver4Status = "Rejected";
                        template.Approver4Comments = Comments;

                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Rejected",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");
                       
                            template.InitiatorStatus = "Rework";
                            template.NextApprover = template.Initiator.ToString();

                    string Initiator = "";
                    Initiator = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                    string approver4 = "";
                    approver4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                    string Initiatoremail = "";
                    Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                    //var TableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID) select tblVariableData;
                    //string[] temp = new string[5];

                    //foreach (var r in TableVariable)
                    //{
                    //    temp[0] = r.Variable;
                    //    temp[1] = r.Value;
                    //    if (temp[0] == "Vendor Name")
                    //    {
                    //        temp[3] = "Vendor Name";
                    //        temp[4] = temp[1];
                    //    }

                    //}
                    string employeename = Initiator;
                    //string VendorName = temp[4];
                    string[] TO = { Initiatoremail };

                    string Subject = template.Name + " is Rejected ";
                    string UrL = ApplicationLink + "/Template/Details/" + TemplateID;
                    string Paragraph = "The Template details as mentioned below is sent for revision by " + approver4 + " to " + Initiator + " .<br/><br/>";
                        string Body = "Dear " + Initiator + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                    SMTP.Send(TO, Subject, Body);


                    if (template.Approver1Status == "Approved")
                    {
                        string Approver1 = "";
                        Approver1 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeName).First();
                        approver4 = "";
                        approver4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver4 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver1 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver2Status == "Approved")
                    {
                        string Approver2 = "";
                        Approver2 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeName).First();
                        approver4 = "";
                        approver4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver4 + " to " + Initiator + " .<br/><br/>";
                            string body = "Dear " + Approver2 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver3Status == "Approved")
                    {
                        string Approver3 = "";
                        Approver3 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeName).First();
                        approver4 = "";
                        approver4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver4 + " to " + Initiator + " .<br/><br/>";
                            string body = "Dear " + Approver3 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }

                    Logger.Info("Accessed DB, Checking Template Details: Initiator is Assigned");
                       
                    }
                    else if (template.Approver5ID.ToString() == CurrentUser && template.Approver5Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver5 Found");
                        template.Approver5Status = "Rejected";
                        template.Approver5Comments = Comments;

                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Rejected",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");
                        
                       
                            template.InitiatorStatus = "Rework";
                            template.NextApprover = template.Initiator.ToString();

                    string Initiator = "";
                    Initiator = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                    string approver5 = "";
                    approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                    string Initiatoremail = "";
                    Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                    //var TableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID) select tblVariableData;
                    //string[] temp = new string[5];

                    //foreach (var r in TableVariable)
                    //{
                    //    temp[0] = r.Variable;
                    //    temp[1] = r.Value;
                    //    if (temp[0] == "Vendor Name")
                    //    {
                    //        temp[3] = "Vendor Name";
                    //        temp[4] = temp[1];
                    //    }

                    //}
                    string employeename = Initiator;
                    //string VendorName = temp[4];
                    string[] TO = { Initiatoremail };

                    string Subject = template.Name + " is Rejected ";
                    string UrL = ApplicationLink + "/Template/Details/" + TemplateID;
                    string Paragraph = "The Template details as mentioned below is sent for revision by " + approver5 + " to " + Initiator + " .<br/><br/>";
                        string Body = "Dear " + Initiator + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                    SMTP.Send(TO, Subject, Body);

                    if (template.Approver1Status == "Approved")
                    {
                        string Approver1 = "";
                        Approver1 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeName).First();
                        approver5 = "";
                        approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver5 + " to " + Initiator + " .<br/><br/>";
                            string body = "Dear " + Approver1 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver2Status == "Approved")
                    {
                        string Approver2 = "";
                        Approver2 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeName).First();
                        approver5 = "";
                        approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver5 + " to " + Initiator + " .<br/><br/>";
                            string body = "Dear " + Approver2 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver3Status == "Approved")
                    {
                        string Approver3 = "";
                        Approver3 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeName).First();
                        approver5 = "";
                        approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver5 + " to " + Initiator + " .<br/><br/>";
                            string body = "Dear " + Approver3 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver4Status == "Approved")
                    {
                        string Approver4 = "";
                        Approver4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                        approver5 = "";
                        approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver5 + " to " + Initiator + " .<br/><br/>";
                            string body = "Dear " + Approver4 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Initiator is Assigned");
                        
                    }
                    else if (template.Approver6ID.ToString() == CurrentUser && template.Approver6Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver6 Found");
                        template.Approver6Status = "Rejected";
                        template.Approver6Comments = Comments;

                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Rejected",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");
                        
                        
                            template.InitiatorStatus = "Rework";
                            template.NextApprover = template.Initiator.ToString();

                    string Initiator = "";
                    Initiator = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                    string approver6 = "";
                    approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                    string Initiatoremail = "";
                    Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                    //var TableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID) select tblVariableData;
                    //string[] temp = new string[5];

                    //foreach (var r in TableVariable)
                    //{
                    //    temp[0] = r.Variable;
                    //    temp[1] = r.Value;
                    //    if (temp[0] == "Vendor Name")
                    //    {
                    //        temp[3] = "Vendor Name";
                    //        temp[4] = temp[1];
                    //    }

                    //}
                    string employeename = Initiator;
                    //string VendorName = temp[4];
                    string[] TO = { Initiatoremail };

                    string Subject = template.Name + " is Rejected ";
                    string UrL = ApplicationLink + "/Template/Details/" + TemplateID;
                    string Paragraph = "The Template details as mentioned below is sent for revision by " + approver6 + " to " + Initiator + " .<br/><br/>";
                        string Body = "Dear " + Initiator + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                    SMTP.Send(TO, Subject, Body);

                    if (template.Approver1Status == "Approved")
                    {
                        string Approver1 = "";
                        Approver1 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeName).First();
                        approver6 = "";
                        approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver6 + " to " + Initiator + " .<br/><br/>";
                            string body = "Dear " + Approver1 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver2Status == "Approved")
                    {
                        string Approver2 = "";
                        Approver2 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeName).First();
                        approver6 = "";
                        approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver6 + " to " + Initiator + " .<br/><br/>";
                            string body = "Dear " + Approver2 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver3Status == "Approved")
                    {
                        string Approver3 = "";
                        Approver3 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeName).First();
                        approver6 = "";
                        approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver6 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver3 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver4Status == "Approved")
                    {
                        string Approver4 = "";
                        Approver4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                        approver6 = "";
                        approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver6 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver4 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver5Status == "Approved")
                    {
                        string Approver5 = "";
                        Approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                        approver6 = "";
                        approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver6 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver5 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }

                    Logger.Info("Accessed DB, Checking Template Details: Initiator is Assigned");
                       
                    }
                    else if (template.Approver7ID.ToString() == CurrentUser && template.Approver7Status == "Pending Approval")
                    {
                        template.Approver7Status = "Rejected";
                        template.Approver7Comments = Comments;

                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Rejected",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");
                        
                         template.InitiatorStatus = "Rework";
                         template.NextApprover = template.Initiator.ToString();

                    string Initiator = "";
                    Initiator = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                    string approver7 = "";
                    approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                    string Initiatoremail = "";
                    Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                    //var TableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID) select tblVariableData;
                    //string[] temp = new string[5];

                    //foreach (var r in TableVariable)
                    //{
                    //    temp[0] = r.Variable;
                    //    temp[1] = r.Value;
                    //    if (temp[0] == "Vendor Name")
                    //    {
                    //        temp[3] = "Vendor Name";
                    //        temp[4] = temp[1];
                    //    }

                    //}
                    string employeename = Initiator;
                    //string VendorName = temp[4];
                    string[] TO = { Initiatoremail };

                    string Subject = template.Name + " is Rejected ";
                    string UrL = ApplicationLink + "/Template/Details/" + TemplateID;
                    string Paragraph = "The Template details as mentioned below is sent for revision by " + approver7 + " to " + Initiator + " .<br/><br/>";
                        string Body = "Dear " + Initiator + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                    SMTP.Send(TO, Subject, Body);

                    if (template.Approver1Status == "Approved")
                    {
                        string Approver1 = "";
                        Approver1 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeName).First();
                        approver7 = "";
                        approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver7 + " to " + Initiator + " .<br/><br/>";
                            string body = "Dear " + Approver1 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver2Status == "Approved")
                    {
                        string Approver2 = "";
                        Approver2 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeName).First();
                        approver7 = "";
                        approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver7 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver2 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver3Status == "Approved")
                    {
                        string Approver3 = "";
                        Approver3 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeName).First();
                        approver7 = "";
                        approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver7 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver3 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver4Status == "Approved")
                    {
                        string Approver4 = "";
                        Approver4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                        approver7 = "";
                        approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver7 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver4 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver5Status == "Approved")
                    {
                        string Approver5 = "";
                        Approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                        approver7 = "";
                        approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver7 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver5 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver6Status == "Approved")
                    {
                        string Approver6 = "";
                        Approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                        approver7 = "";
                        approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver7 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver6 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Initiator is Assigned");
                        
                    }
                    else if (template.Approver8ID.ToString() == CurrentUser && template.Approver8Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver8 Found");
                        template.Approver8Status = "Rejected";
                        template.Approver8Comments = Comments;

                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Rejected",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");
                       
                       
                            template.InitiatorStatus = "Rework";
                            template.NextApprover = template.Initiator.ToString();

                    string Initiator = "";
                    Initiator = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                    string approver8 = "";
                    approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                    string Initiatoremail = "";
                    Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                    //var TableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID) select tblVariableData;
                    //string[] temp = new string[5];

                    //foreach (var r in TableVariable)
                    //{
                    //    temp[0] = r.Variable;
                    //    temp[1] = r.Value;
                    //    if (temp[0] == "Vendor Name")
                    //    {
                    //        temp[3] = "Vendor Name";
                    //        temp[4] = temp[1];
                    //    }

                    //}
                    string employeename = Initiator;
                    //string VendorName = temp[4];
                    string[] TO = { Initiatoremail };

                    string Subject = template.Name + " is Rejected ";
                    string UrL = ApplicationLink + "/Template/Details/" + TemplateID;
                    string Paragraph = "The Template details as mentioned below is sent for revision by " + approver8 + " to " + Initiator + " .<br/><br/>";
                        string Body = "Dear " + Initiator + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                    SMTP.Send(TO, Subject, Body);

                    if (template.Approver1Status == "Approved")
                    {
                        string Approver1 = "";
                        Approver1 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeName).First();
                        approver8 = "";
                        approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver8 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver1 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver2Status == "Approved")
                    {
                        string Approver2 = "";
                        Approver2 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeName).First();
                        approver8 = "";
                        approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver8 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver2 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver3Status == "Approved")
                    {
                        string Approver3 = "";
                        Approver3 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeName).First();
                        approver8 = "";
                        approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver8 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver3 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver4Status == "Approved")
                    {
                        string Approver4 = "";
                        Approver4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                        approver8 = "";
                        approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver8 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver4 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver5Status == "Approved")
                    {
                        string Approver5 = "";
                        Approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                        approver8 = "";
                        approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver8 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver5 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver6Status == "Approved")
                    {
                        string Approver6 = "";
                        Approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                        approver8 = "";
                        approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver8 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver6 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver7Status == "Approved")
                    {
                        string Approver7 = "";
                        Approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                        approver8 = "";
                        approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver8 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver7 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Initiator is Assigned");
                       
                    }
                    else if (template.Approver9ID.ToString() == CurrentUser && template.Approver9Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver9 Found");
                        template.Approver9Status = "Rejected";
                        template.Approver9Comments = Comments;

                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Rejected",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");
                        string Initiator = "";
                        Initiator = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                        string approver9 = "";
                        approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                        string Initiatoremail = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                        //var TableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID) select tblVariableData;
                        //string[] temp = new string[5];

                        //foreach (var r in TableVariable)
                        //{
                        //    temp[0] = r.Variable;
                        //    temp[1] = r.Value;
                        //    if (temp[0] == "Vendor Name")
                        //    {
                        //        temp[3] = "Vendor Name";
                        //        temp[4] = temp[1];
                        //    }

                        //}
                        string employeename = Initiator;
                        //string VendorName = temp[4];
                        string[] TO = { Initiatoremail };

                        string Subject = template.Name + " is Rejected ";
                        string UrL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string Paragraph = "The Template details as mentioned below is sent for revision by " + approver9 + " to " + Initiator + " .<br/><br/>";
                        string Body = "Dear " + Initiator + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(TO, Subject, Body);
                       
                            template.InitiatorStatus = "Rework";
                            template.NextApprover = template.Initiator.ToString();
                    if (template.Approver1Status == "Approved")
                    {
                        string Approver1 = "";
                        Approver1 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeName).First();
                        approver9 = "";
                        approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver9 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver1 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver2Status == "Approved")
                    {
                        string Approver2 = "";
                        Approver2 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeName).First();
                        approver9 = "";
                        approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver9 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver2 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver3Status == "Approved")
                    {
                        string Approver3 = "";
                        Approver3 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeName).First();
                        approver9 = "";
                        approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver9 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver3 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver4Status == "Approved")
                    {
                        string Approver4 = "";
                        Approver4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                        approver9 = "";
                        approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver9 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver4 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver5Status == "Approved")
                    {
                        string Approver5 = "";
                        Approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                        approver9 = "";
                        approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver9 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver5 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver6Status == "Approved")
                    {
                        string Approver6 = "";
                        Approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                        approver9 = "";
                        approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver9 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver6 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver7Status == "Approved")
                    {
                        string Approver7 = "";
                        Approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                        approver9 = "";
                        approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver9 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver7 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver8Status == "Approved")
                    {
                        string Approver8 = "";
                        Approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                        approver9 = "";
                        approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver9 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver8 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Initiator is Assigned");
                        
                    }
                    else if (template.Approver10ID.ToString() == CurrentUser && template.Approver10Status == "Pending Approval")
                    {
                        Logger.Info("Accessed DB, Checking Template Details: Current User Approver10 Found");
                        template.Approver10Status = "Rejected";
                        template.Approver10Comments = Comments;

                        Logger.Info("Accessing DB for Saving the Template Log Details");
                        tblTemplateLog log = new tblTemplateLog
                        {
                            LogTemplateUID = template.TemplateID,
                            ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName,
                            LogActivity = "Rejected",
                            ChangedFrom = "-",
                            ChangedTo = "-",
                            DateandTime = DateTime.Now.ToString()
                        };
                        db.tblTemplateLogs.Add(log);
                        db.SaveChanges();
                        Logger.Info("Accessed DB, Template Log Record Saved");
                        string Initiator = "";
                        Initiator = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeName).First();
                        string approver10 = "";
                        approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                        string Initiatoremail = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                        //var TableVariable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == TemplateID) select tblVariableData;
                        //string[] temp = new string[5];

                        //foreach (var r in TableVariable)
                        //{
                        //    temp[0] = r.Variable;
                        //    temp[1] = r.Value;
                        //    if (temp[0] == "Vendor Name")
                        //    {
                        //        temp[3] = "Vendor Name";
                        //        temp[4] = temp[1];
                        //    }

                        //}
                        string employeename = Initiator;
                        //string VendorName = temp[4];
                        string[] TO = { Initiatoremail };

                        string Subject = template.Name + " is Rejected ";
                        string UrL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string Paragraph = "The Template details as mentioned below is sent for revision by " + approver10 + " to " + Initiator + " .<br/><br/>";
                        string Body = "Dear " + Initiator + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(TO, Subject, Body);
                      
                            template.InitiatorStatus = "Rework";
                            template.NextApprover = template.Initiator.ToString();
                    if (template.Approver1Status == "Approved")
                    {
                        string Approver1 = "";
                        Approver1 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeName).First();
                        approver10 = "";
                        approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver1ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver10 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver1 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver2Status == "Approved")
                    {
                        string Approver2 = "";
                        Approver2 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeName).First();
                        approver10 = "";
                        approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver2ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver10 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver2 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver3Status == "Approved")
                    {
                        string Approver3 = "";
                        Approver3 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeName).First();
                        approver10 = "";
                        approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver3ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver10 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver3 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver4Status == "Approved")
                    {
                        string Approver4 = "";
                        Approver4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                        approver10 = "";
                        approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver4ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver10 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver4 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver5Status == "Approved")
                    {
                        string Approver5 = "";
                        Approver5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                        approver10 = "";
                        approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver5ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver10 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver5 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver6Status == "Approved")
                    {
                        string Approver6 = "";
                        Approver6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                        approver10 = "";
                        approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver6ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver10 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver6 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver7Status == "Approved")
                    {
                        string Approver7 = "";
                        Approver7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                        approver10 = "";
                        approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver7ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver10 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver7 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver8Status == "Approved")
                    {
                        string Approver8 = "";
                        Approver8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                        approver10 = "";
                        approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver8ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver10 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver8 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    if (template.Approver9Status == "Approved")
                    {
                        string Approver9 = "";
                        Approver9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                        approver10 = "";
                        approver10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                        string Approver1email = "";
                        Initiatoremail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == template.Approver9ID) select tblUserMaster.UserEmployeeEmail).First();

                        //string employeename = Approver1;
                        //string VendorName = temp[4];
                        string[] To = { Approver1email };

                        string subject = template.Name + " is Rejected ";
                        string urL = ApplicationLink + "/Template/Details/" + TemplateID;
                        string paragraph = "The Template details as mentioned below is sent for revision by " + approver10 + " to " + Initiator + " .<br/><br/>";
                        string body = "Dear " + Approver9 + ",<br/><br/>" + paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + template.Name + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + template.TemplateID + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + urL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b> Contract Management System</b></body></html>";
                        SMTP.Send(To, subject, body);
                    }
                    Logger.Info("Accessed DB, Checking Template Details: Initiator is Assigned");
                       
                    }

                    template.Status = "Rejected";
                    template.RejectedBy = CurrentUser;
                    try
                    {
                        string LogChangeFrom = "";
                        string LogChangeTo = "";
                        var Deligation = from tblDeligationMatrix in db.tblDeligationMatrices select tblDeligationMatrix;
                        foreach (var r in Deligation)
                        {
                            int Found = 1;
                            DateTime startDate = DateTime.ParseExact(r.StartDate, "dd/MM/yyyy", null);
                            Found = DateTime.Compare(startDate, DateTime.Now);

                            if (Found <= 0)
                            {
                                Found = 1;
                                DateTime endDate = DateTime.ParseExact(r.EndDate, "dd/MM/yyyy", null);
                                Found = DateTime.Compare(endDate, DateTime.Now);
                                if (Found < 0)
                                {
                                    var DID = r.ID;

                                    tblDeligationMatrix Deligate = db.tblDeligationMatrices.Find(DID);
                                    db.tblDeligationMatrices.Remove(Deligate);

                                }
                                else
                                {
                                    try
                                    {
                                        //DeligationMethod(Convert.ToInt32(eachItem.DeligateFrom), Convert.ToInt32(eachItem.DeligateTo));
                                        if ((r.DeligateFrom == template.Initiator) && (template.Status == "Pending Approval" || template.Status == "Rejected"))
                                        {
                                            template.Initiator = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Initiator in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom.ToString() == template.NextApprover) && (template.Status == "Pending Approval" || template.Status == "Rejected"))
                                        {
                                            template.NextApprover = r.DeligateTo.ToString();
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Next Approver in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom.ToString() == template.RejectedBy) && (template.Status == "Pending Approval" || template.Status == "Rejected"))
                                        {
                                            template.RejectedBy = r.DeligateTo.ToString();
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Rejected By in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver1ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver1Status == "Pending Approval" || template.Approver1Status == "Rejected" || template.Approver1Status == null))
                                        {
                                            template.Approver1ID = (int)r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 1 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver2ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver2Status == "Pending Approval" || template.Approver2Status == "Rejected" || template.Approver2Status == null))
                                        {
                                            template.Approver2ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 2 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver3ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver3Status == "Pending Approval" || template.Approver3Status == "Rejected" || template.Approver3Status == null))
                                        {
                                            template.Approver3ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 3 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver4ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver4Status == "Pending Approval" || template.Approver4Status == "Rejected" || template.Approver4Status == null))
                                        {
                                            template.Approver4ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 4 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver5ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver5Status == "Pending Approval" || template.Approver5Status == "Rejected" || template.Approver5Status == null))
                                        {
                                            template.Approver5ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 5 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver6ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver6Status == "Pending Approval" || template.Approver6Status == "Rejected" || template.Approver6Status == null))
                                        {
                                            template.Approver6ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 6 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver7ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver7Status == "Pending Approval" || template.Approver7Status == "Rejected" || template.Approver7Status == null))
                                        {
                                            template.Approver7ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 7 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver8ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver8Status == "Pending Approval" || template.Approver8Status == "Rejected" || template.Approver8Status == null))
                                        {
                                            template.Approver8ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 8 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver9ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver9Status == "Pending Approval" || template.Approver9Status == "Rejected" || template.Approver9Status == null))
                                        {
                                            template.Approver9ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 9 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                        if ((r.DeligateFrom == template.Approver10ID) && (template.Status == "Pending Approval" || template.Status == "Rejected") && (template.Approver10Status == "Pending Approval" || template.Approver10Status == "Rejected" || template.Approver10Status == null))
                                        {
                                            template.Approver10ID = r.DeligateTo;
                                            tblDeligationLog logs = new tblDeligationLog();
                                            logs.LogDeligationUID = template.TemplateID;
                                            logs.ModifiedBy = "SYSTEM";
                                            logs.LogActivity = "Deligating Approver 10 in Template (" + template.TemplateID + ")";
                                            logs.ChangedFrom = r.DeligateFrom.ToString();
                                            logs.ChangedTo = r.DeligateTo.ToString();
                                            logs.DateandTime = DateTime.Now.ToString();
                                            db.tblDeligationLogs.Add(logs);
                                        }
                                    }
                                    catch
                                    { }
                                }

                            }


                        }

                        db.Entry(template).State = EntityState.Modified;

                    }
                    catch { }
                    db.SaveChanges();
                    Logger.Info("Accessed DB, Template Rejected");
                    return Json("success");
                }
                return Json("failure");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'AssignToPreviousUser' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult GetVendorName()
        {
            Logger.Info("Attempt Template GetVendorName");
            try
            {
                Logger.Info("Accessing DB for Vendor Details");
                var VendorName = from tblVendorMaster in db.tblVendorMasters select tblVendorMaster.VendorVendorName;
                Logger.Info("Accessing DB, Checking for Vendor Details: Vendor Name List Found");
                return Json(VendorName);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetVendorName' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult GetVendorCIN()
        {
            Logger.Info("Attempt Template GetVendorCIN");
            try
            {
                Logger.Info("Accessing DB for Vendor Details");
                var VendorCIN = from tblVendorMaster in db.tblVendorMasters select tblVendorMaster.VendorCorporateIdentificationNumber;
                Logger.Info("Accessing DB, Checking for Vendor Details: Vendor CIN List Found");
                return Json(VendorCIN);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetVendorCIN' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]

        public ActionResult GetVendorDetails(string VendorName)
        {
            Logger.Info("Attempt Template GetVendorDetails");
            try
            {

                Logger.Info("Accessing DB for Vendor Details : Vendor Name match");
                string[] Vendor = new string[10];
                var VendorDetails = from tblVendorMaster in db.tblVendorMasters.Where(x => x.VendorVendorName == VendorName) select tblVendorMaster;
                foreach (var item in VendorDetails)
                {
                    Vendor[0] = "success";
                    Vendor[1] = item.VendorVendorName;
                    Vendor[2] = item.VendorCorporateIdentificationNumber;
                    Vendor[3] = item.VendorAuthorisedSignatory;
                    Vendor[4] = item.VendorRegisteredAddress;
                    Vendor[5] = item.VendorBranchOffice1;
                    Vendor[6] = item.VendorBranchOffice2;
                    Vendor[7] = item.VendorBranchOffice3;
                    Vendor[8] = item.VendorBranchOffice4;
                    Vendor[9] = item.VendorBranchOffice5;
                }
                Logger.Info("Accessing DB for Vendor Details : Vendor Details Found");
                return Json(Vendor);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetVendorDetails' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]

        public ActionResult GetVendorDetailsOnCIN(string VendorCIN)
        {
            Logger.Info("Attempt Template GetVendorDetailsOnCIN");
            try
            {
                string[] Vendor = new string[10];
                Logger.Info("Accessing DB for Vendor Details : Vendor CIN match");
                var VendorDetails = from tblVendorMaster in db.tblVendorMasters.Where(x => x.VendorCorporateIdentificationNumber == VendorCIN) select tblVendorMaster;
                foreach (var item in VendorDetails)
                {
                    Vendor[0] = "success";
                    Vendor[1] = item.VendorVendorName;
                    Vendor[2] = item.VendorCorporateIdentificationNumber;
                    Vendor[3] = item.VendorAuthorisedSignatory;
                    Vendor[4] = item.VendorRegisteredAddress;
                    Vendor[5] = item.VendorBranchOffice1;
                    Vendor[6] = item.VendorBranchOffice2;
                    Vendor[7] = item.VendorBranchOffice3;
                    Vendor[8] = item.VendorBranchOffice4;
                    Vendor[9] = item.VendorBranchOffice5;
                }
                Logger.Info("Accessing DB for Vendor Details : Vendor Details Found");
                return Json(Vendor);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetVendorDetailsOnCIN' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        //[HttpPost]
        //public ActionResult UploadForSignOff(int TemplateID)
        //{
        //    Logger.Info("Attempt Template UploadForSignOff");
        //    try
        //    {
        //        Logger.Info("Accessing DB for Signing Off Contract");
        //        tblTemplateMaster template = db.tblTemplateMasters.Find(TemplateID);
        //        template.Status = "In Effect";

        //        db.Entry(template).State = EntityState.Modified;
        //        db.SaveChanges();
        //        Logger.Info("Accessed DB, Template is Signed Off and is in InEffect");
        //        return Json("success");
        //    }
        //    catch (Exception Ex)
        //    {
        //        Logger.Error(Ex, "'Template' Controller , 'UploadForSignOff' Action HTTP POST Main exception");
        //        return Json("error");
        //    }
        //}


        //-------------------------------Integrated-----------------------------//

        [HttpPost]
        public ActionResult Templatetabledetails(int templateID)
        {
            Logger.Info("Attempt Template Templatetabledetails");
            try
            {
                Logger.Info("Accessing DB for Template Details : TemplateID match");
                var TableTemplate = /*from tblTemplateMaster in*/ db.tblTemplateMasters.Where(x => x.TemplateID == templateID).Select(x => new
                {
                    x.Name,
                    x.Approver10ID,
                    x.Approver1ID,
                    x.Approver2ID,
                    x.Approver3ID,
                    x.Approver4ID,
                    x.Approver5ID,
                    x.Approver6ID,
                    x.Approver7ID,
                    x.Approver8ID,
                    x.Approver9ID,
                    x.Initiator,
                    x.DateofInitiated,
                    x.Approver10ApprovedOn,
                    x.Approver1ApprovedOn,
                    x.Approver2ApprovedOn,
                    x.Approver3ApprovedOn,
                    x.Approver4ApprovedOn,
                    x.Approver5ApprovedOn,
                    x.Approver6ApprovedOn,
                    x.Approver7ApprovedOn,
                    x.Approver8ApprovedOn,
                    x.Approver9ApprovedOn
                }); //select tblTemplateMaster;
                string[] temp = new string[24];
                foreach (var r in TableTemplate)
                {
                    temp[0] = "success";
                    temp[1] = r.Name;

                    var App1 = "";
                    try
                    {
                        App1 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == r.Approver1ID) select tblUserMaster.UserEmployeeName).First();
                    }
                    catch { }
                    var App2 = "";

                    try
                    {
                        App2 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == r.Approver2ID) select tblUserMaster.UserEmployeeName).First();
                    }
                    catch { }
                    var App3 = "";
                    try
                    {
                        App3 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == r.Approver3ID) select tblUserMaster.UserEmployeeName).First();

                    }
                    catch { }
                    var App4 = "";
                    try
                    {
                        App4 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == r.Approver4ID) select tblUserMaster.UserEmployeeName).First();
                    }
                    catch { }

                    var App5 = "";
                    try
                    {
                        App5 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == r.Approver5ID) select tblUserMaster.UserEmployeeName).First();
                    }
                    catch { }

                    var App6 = "";
                    try
                    {
                        App6 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == r.Approver6ID) select tblUserMaster.UserEmployeeName).First();
                    }
                    catch { }
                    var App7 = "";
                    try
                    {
                        App7 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == r.Approver7ID) select tblUserMaster.UserEmployeeName).First();
                    }
                    catch { }
                    var App8 = "";
                    try
                    {
                        App8 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == r.Approver8ID) select tblUserMaster.UserEmployeeName).First();
                    }
                    catch { }
                    var App9 = "";
                    try
                    {
                        App9 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == r.Approver9ID) select tblUserMaster.UserEmployeeName).First();
                    }
                    catch { }
                    var App10 = "";
                    try
                    {
                        App10 = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == r.Approver10ID) select tblUserMaster.UserEmployeeName).First();
                    }
                    catch { }
                    var Ini = "";
                    try
                    {
                        Ini = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == r.Initiator) select tblUserMaster.UserEmployeeName).First();
                    }
                    catch { }

                    temp[2] = r.Approver1ID.ToString() + " - " + App1; 
                    temp[3] = r.Approver2ID.ToString() + " - " + App2; 
                    temp[4] = r.Approver3ID.ToString() + " - " + App3; 
                    temp[5] = r.Approver4ID.ToString() + " - " + App4; 
                    temp[6] = r.Approver5ID.ToString() + " - " + App5; 
                    temp[7] = r.Approver6ID.ToString() + " - " + App6; 
                    temp[8] = r.Approver7ID.ToString() + " - " + App7; 
                    temp[9] = r.Approver8ID.ToString() + " - " + App8; 
                    temp[10] = r.Approver9ID.ToString() + " - " + App9; 
                    temp[11] = r.Approver10ID.ToString() + " - " + App10; 
                    temp[12] = r.Initiator.ToString() + " - " + Ini; 
                    temp[13] = r.DateofInitiated.ToString();
                    temp[14] = r.Approver1ApprovedOn.ToString();
                    temp[15] = r.Approver2ApprovedOn.ToString();
                    temp[16] = r.Approver3ApprovedOn.ToString();
                    temp[17] = r.Approver4ApprovedOn.ToString();
                    temp[18] = r.Approver5ApprovedOn.ToString();
                    temp[19] = r.Approver6ApprovedOn.ToString();
                    temp[20] = r.Approver7ApprovedOn.ToString();
                    temp[21] = r.Approver8ApprovedOn.ToString();
                    temp[22] = r.Approver9ApprovedOn.ToString();
                    temp[23] = r.Approver10ApprovedOn.ToString();

                }
                Logger.Info("Accessed DB, Checking Template Details : Details Found");
                return Json(temp);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'Templatetabledetails' Action HTTP POST Main exception");
                return Json("error");
            }
        }

      
        //---------------------------------------------------------------------//

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DownloadPDF(string editor4, int ID, string FileName, string Pagesize, float Margintop = 1, float Marginleft = 1, float Marginbottom = 1, float Marginright = 1)
        {
            try
            {
                //string file = @"d:\bseindia_2.xlsx";
                //string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //return File(file, contentType, Path.GetFileName(file));

                Marginleft = Marginleft * 30;
                Marginright = Marginright * 30;
                Margintop = Margintop * 30;
                Marginbottom = Marginbottom * 30;
                var result = /*from tblTemplateMaster in*/ db.tblTemplateMasters.Where(x => x.TemplateID == ID).Select(x => new { x.Status }); //select tblTemplateMaster;

                string[] record = new string[3];
                foreach (var eachRecord in result)
                {
                    // record[0] = eachRecord.SubCategory;
                    //record[1] = eachRecord.In;
                    record[2] = eachRecord.Status;
                }

                // string SubCategory = record[0].ToUpper();
                // string InEffectFrom = record[1];
                string ContractStatus = record[2];

                var doc = new Document();
                byte[] pdf; // result will be here
                using (var memoryStream = new MemoryStream())
                {
                    //var document = new Document(PageSize.A4, 50, 50, 60, 60);
                    var document = new Document();
                    document.SetMargins(Marginleft, Marginright, Margintop, Marginbottom + 50);
                    if (Pagesize == "A4")
                    {
                        document.SetPageSize(PageSize.A4);
                    }
                    if (Pagesize == "Legal")
                    {
                        document.SetPageSize(PageSize.LEGAL);
                    }
                    if (Pagesize == "Executive")
                    {
                        document.SetPageSize(PageSize.EXECUTIVE);
                    }
                    var writer = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();
                    using (var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(editor4)))
                    {
                        TextReader tr = new StreamReader(htmlMemoryStream);
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, tr);
                    }
                    document.Close();
                    doc = document;

                    pdf = memoryStream.ToArray();

                }
                //string InitaitedDate = DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "");
                //int contractID = 123456789;
                string WaterMark = "";
                string ContractID = $"{"Template"}{"_"}{ID}";
                //if (radio == "Draft")
                //{
                //    WaterMark = "Draft";
                //}
               
                //if (radio == "Custom")
                //{
                //    WaterMark = watermark;
                //}
                //if (radio == "No Watermark")
                //{
                //    WaterMark = "";
                //}

                // string WaterMark = "1234567890123450";


                byte[] bytes = pdf;
                Font blackFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
                Font GreyFont;
                if (WaterMark.Length > 10)
                {
                    GreyFont = FontFactory.GetFont("Arial", 40, Font.NORMAL, BaseColor.LIGHT_GRAY);
                }
                else
                {
                    GreyFont = FontFactory.GetFont("Arial", 100, Font.NORMAL, BaseColor.LIGHT_GRAY);
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    Image image;
                    image = Image.GetInstance(Server.MapPath("~/Content/.png"));
                    image.SetAbsolutePosition(doc.Right - 70, Marginbottom);
                    image.ScaleAbsolute(60f, 60f);

                    PdfReader reader = new PdfReader(bytes);
                    using (PdfStamper stamper = new PdfStamper(reader, stream))
                    {
                        
                        int pages = reader.NumberOfPages;
                        for (int i = 1; i <= pages; i++)
                        {
                            // stamper.GetUnderContent(i).AddImage(image);
                            if (WaterMark.Length > 0)
                            {

                                //ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase($"{"Page "}{i.ToString()}{" of "}{pages.ToString()}", blackFont), 568f, 15f, 0);
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, new Phrase($"{WaterMark}", GreyFont), doc.Right / 2, doc.Top / 2, 30);
                            }

                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase($"{"Page "}{i.ToString()}{" of "}{pages.ToString()}", blackFont), doc.Right / 2, doc.Left / 2, 0);
                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase($"{ContractID}", blackFont), Marginleft, Marginbottom + 10, 0);
                        }
                    }
                    bytes = stream.ToArray();
                }

                // return File(bytes, "application/pdf", "sampel.pdf");


                return File(bytes, "application/pdf", FileName + ".pdf");
            }
            catch
            {
                return RedirectToAction("DraftView", new { id = ID });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DownloadPDF2(string editor5, int ID, string FileName, string Pagesize, float Margintop, float Marginleft, float Marginbottom, float Marginright)
        {
            try
            {
                Marginleft = Marginleft * 96;
                Marginright = Marginright * 96;
                Margintop = Margintop * 96;
                Marginbottom = Marginbottom * 96;
                var result = /*from tblTemplateMaster in*/ db.tblTemplateMasters.Where(x => x.TemplateID == ID).Select(x => new { x.Status }); //select tblTemplateMaster;

                string[] record = new string[3];
                foreach (var eachRecord in result)
                {
                    //record[0] = eachRecord.SubCategory;
                    //record[1] = eachRecord.In;
                    record[2] = eachRecord.Status;
                }

                // string SubCategory = record[0].ToUpper();
                //string InEffectFrom = record[1];
                string ContractStatus = record[2];
                var doc = new Document();
                byte[] pdf; // result will be here
                using (var memoryStream = new MemoryStream())
                {
                    //var document = new Document(PageSize.A4, 50, 50, 60, 60);
                    var document = new Document();

                    document.SetMargins(Marginleft, Marginright, Margintop, Marginbottom + 50);
                    if (Pagesize == "A4")
                    {
                        document.SetPageSize(PageSize.A4);
                    }
                    if (Pagesize == "Legal")
                    {
                        document.SetPageSize(PageSize.LEGAL);
                    }
                    if (Pagesize == "Executive")
                    {
                        document.SetPageSize(PageSize.EXECUTIVE);
                    }
                    var writer = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();
                    using (var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(editor5)))
                    {
                        TextReader tr = new StreamReader(htmlMemoryStream);
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, tr);
                    }
                    document.Close();
                    doc = document;

                    pdf = memoryStream.ToArray();

                }
                //string InitaitedDate = DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "");
                //int contractID = 123456789;
                string WaterMark = "Draft";
                string ContractID = $"{"Template"}{"_"}{ID}";
                //if (radio1 == "Draft")
                //{
                //    WaterMark = "Draft";
                //}
               
                //if (radio1 == "Custom")
                //{
                //    WaterMark = watermark;
                //}
                //if (radio1 == "No Watermark")
                //{
                //    WaterMark = "";
                //}


                // string WaterMark = "1234567890123450";


                byte[] bytes = pdf;
                Font blackFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
                Font GreyFont;
                if (WaterMark.Length > 10)
                {
                    GreyFont = FontFactory.GetFont("Arial", 40, Font.NORMAL, BaseColor.LIGHT_GRAY);
                }
                else
                {
                    GreyFont = FontFactory.GetFont("Arial", 100, Font.NORMAL, BaseColor.LIGHT_GRAY);
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    PdfReader reader = new PdfReader(bytes);
                    using (PdfStamper stamper = new PdfStamper(reader, stream))
                    {
                        int pages = reader.NumberOfPages;
                        for (int i = 1; i <= pages; i++)
                        {
                            if (WaterMark.Length > 0)
                            {

                                //ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase($"{"Page "}{i.ToString()}{" of "}{pages.ToString()}", blackFont), 568f, 15f, 0);
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, new Phrase($"{WaterMark}", GreyFont), doc.Right / 2, doc.Top / 2, 30);
                            }

                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase($"{"Page "}{i.ToString()}{" of "}{pages.ToString()}", blackFont), doc.Right / 2, doc.Left / 2, 0);
                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase($"{ContractID}", blackFont), Marginleft, Marginbottom + 10, 0);
                        }
                    }
                    bytes = stream.ToArray();
                }

                // return File(bytes, "application/pdf", "sampel.pdf");


                return File(bytes, "application/pdf", FileName + ".pdf");
            }
            catch
            {
                return RedirectToAction("DraftView", new { id = ID });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DownloadPDF3(string editor6, int ID, string FileName, string Pagesize, float Margintop, float Marginleft, float Marginbottom, float Marginright)
        {
            try
            {
                Marginleft = Marginleft * 96;
                Marginright = Marginright * 96;
                Margintop = Margintop * 96;
                Marginbottom = Marginbottom * 96;
                var result = /*from tblTemplateMaster in*/ db.tblTemplateMasters.Where(x => x.TemplateID == ID).Select(x => new { x.Status }); //select tblTemplateMaster;

                string[] record = new string[3];
                foreach (var eachRecord in result)
                {
                    //record[0] = eachRecord.SubCategory;
                    //record[1] = eachRecord.In;
                    record[2] = eachRecord.Status;
                }

                //string SubCategory = record[0].ToUpper();
                //string InEffectFrom = record[1];
                string ContractStatus = record[2];

                var doc = new Document();
                byte[] pdf; // result will be here
                using (var memoryStream = new MemoryStream())
                {
                    //var document = new Document(PageSize.A4, 50, 50, 60, 60);
                    var document = new Document();
                    document.SetMargins(Marginleft, Marginright, Margintop, Marginbottom + 50);
                    if (Pagesize == "A4")
                    {
                        document.SetPageSize(PageSize.A4);
                    }
                    if (Pagesize == "Legal")
                    {
                        document.SetPageSize(PageSize.LEGAL);
                    }
                    if (Pagesize == "Executive")
                    {
                        document.SetPageSize(PageSize.EXECUTIVE);
                    }
                    var writer = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();
                    using (var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(editor6)))
                    {
                        TextReader tr = new StreamReader(htmlMemoryStream);
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, tr);
                    }
                    document.Close();
                    doc = document;

                    pdf = memoryStream.ToArray();

                }
                //string InitaitedDate = DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "");
                //int contractID = 123456789;
                string WaterMark = "Draft";
                string ContractID = $"{"Template"}{"_"}{ID}";
                //if (radio2 == "Draft")
                //{
                //    WaterMark = "Draft";
                //}
              
                //if (radio2 == "Custom")
                //{
                //    WaterMark = watermark;
                //}
                //if (radio2 == "No Watermark")
                //{
                //    WaterMark = "";
                //}


                // string WaterMark = "1234567890123450";


                byte[] bytes = pdf;
                Font blackFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
                Font GreyFont;
                if (WaterMark.Length > 10)
                {
                    GreyFont = FontFactory.GetFont("Arial", 40, Font.NORMAL, BaseColor.LIGHT_GRAY);
                }
                else
                {
                    GreyFont = FontFactory.GetFont("Arial", 100, Font.NORMAL, BaseColor.LIGHT_GRAY);
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    PdfReader reader = new PdfReader(bytes);
                    using (PdfStamper stamper = new PdfStamper(reader, stream))
                    {
                        int pages = reader.NumberOfPages;
                        for (int i = 1; i <= pages; i++)
                        {
                            if (WaterMark.Length > 0)
                            {

                                //ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase($"{"Page "}{i.ToString()}{" of "}{pages.ToString()}", blackFont), 568f, 15f, 0);
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, new Phrase($"{WaterMark}", GreyFont), doc.Right / 2, doc.Top / 2, 30);
                            }

                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase($"{"Page "}{i.ToString()}{" of "}{pages.ToString()}", blackFont), doc.Right / 2, doc.Left / 2, 0);
                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase($"{ContractID}", blackFont), Marginleft, Marginbottom + 10, 0);
                        }
                    }
                    bytes = stream.ToArray();
                }

                // return File(bytes, "application/pdf", "sampel.pdf");


                return File(bytes, "application/pdf", FileName + ".pdf");
            }
            catch
            {
                return RedirectToAction("DraftView", new { id = ID });
            }
            }
           
        [HttpPost]
        public ActionResult GetType_List()
        {
            Logger.Info("Attempt Template GetType_List");
            try
            {
                Logger.Info("Accessed DB, Checking Template Details");
                var Type = from tblTemplateMaster in db.tblTemplateMasters select tblTemplateMaster.Type;
                Logger.Info("Accessed DB, Checking Contract Details : Template Type Found");
                var TypeDistint = Type.Distinct();
                return Json(TypeDistint);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetType_List' Action HTTP POST Main exception");
                return Json("error");
            }
        }
        [HttpPost]
        public JsonResult getHodDetails(string EmployeeDetails, string OptionToSearch)
        {
            OptionToSearch = OptionToSearch.Trim();
            if (OptionToSearch == "Employee ID")
            {
                var result = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(EmployeeDetails))
                    .Select(x => new {
                        x.UserEmployeeID,
                        x.UserEmployeeName,
                        x.UserEmployeeDesignation,
                        x.UserCategory,
                        x.UserSubCategory,
                        x.UserRoleAdmin,
                        x.UserRoleApprover,
                        x.UserRoleFinance,
                        x.UserRoleInitiator,
                        x.UserRoleLegal,
                        x.UserRoleReviewer,
                        x.UserEmployeeEmail
                    }); //select tblUserMaster;
                return Json(result);
            }
            else if (OptionToSearch == "Employee Name")
            {
                var result = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeName.Contains(EmployeeDetails)).Select(x => new {
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserEmployeeDesignation,
                    x.UserCategory,
                    x.UserSubCategory,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserEmployeeEmail
                }); //select tblUserMaster;
                return Json(result);
            }
            else if (OptionToSearch == "Employee Email Address")
            {
                var result = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeEmail.Contains(EmployeeDetails)).Select(x => new {
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserEmployeeDesignation,
                    x.UserCategory,
                    x.UserSubCategory,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserEmployeeEmail
                }); //select tblUserMaster;
                return Json(result);
            }
            else
            {
                return Json("");
            }
        }


        [HttpPost]

        public ActionResult GetClauseForPreview(int ID)
        {
            Logger.Info("Attempt Template GetClauseForPreview");
            try
            {

                Logger.Info("Accessed DB, Checking Clause Details: ClauseID match");
                var result = from tblClauseMaster in db.tblClauseMasters.Where(x => x.ClauseClauseID == ID) select tblClauseMaster;
                string[] Template = new string[3];

                foreach (var r in result)
                {

                    Template[0] = r.ClauseClauseID.ToString();
                    Template[1] = r.ClauseClauseText;
                    Template[2] = r.ClauseClauseTitle;

                    Logger.Info("Accessed DB, Checking Clause Details: Clause Details Found");
                    return Json(Template);
                }
                Logger.Info("Accessed DB, Checking Clause Details: Clause Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetClauseForPreview' Action HTTP POST Main exception");
                return Json("error");
            }
        }
        [HttpPost]

        public ActionResult GetClauseText(int id)
        {
            Logger.Info("Attempt Template GetClauseText");
            try
            {

                Logger.Info("Accessed DB, Checking Clause Details: ClauseID match");
                var result = from tblClauseMaster in db.tblClauseMasters.Where(x => x.ClauseClauseID == id) select tblClauseMaster.ClauseClauseText;
                
                Logger.Info("Accessed DB, Checking Clause Details: Clause Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetClauseText' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult getclusterandfunction(int currentuserid)
        {
            Logger.Info("Attempt Template getclusterandfunction");
            try
            {
                if (!string.IsNullOrWhiteSpace(currentuserid.ToString()))
                {
                    Logger.Info("Accessing DB for Template Details : ID match");
                    var result = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == currentuserid.ToString()).Select(x => new { x.UserCategory, x.UserSubCategory }); //select tblUserMaster;
                    string[] UserInfo = new string[3];
                    foreach (var r in result)
                    {

                        UserInfo[0] = "success";
                        UserInfo[1] = r.UserSubCategory;
                        UserInfo[2] = r.UserCategory;
                        Logger.Info("Accessed DB, Checking TemplateMaster Details: Template Details Found");
                        return Json(UserInfo);
                    }
                }
            }

            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'getclusterandfunction' Action HTTP POST Main exception");
                return Json("error");
            }
            Logger.Info("Accessed DB, Checking Tempate Details: Template Details Not Found");
            string[] failures = { "failure" };
            return Json(failures);
        }

        [Authorize(Roles = "admin, legal")]
        public ActionResult Addendum()
        {
            //return View();
            Logger.Info("Attempt Template Addendum");

            Logger.Info("Accessing DB for saving  Template Amendments  Details");
            tblTemplateAmendment tblTemplate = db.tblTemplateAmendments.Find(1);


            if (tblTemplate == null)
            {
                Logger.Info("Accesed DB, Checking Template Amendments Details: Details not Found");
                return View(tblTemplate);
            }
            Logger.Info("Accesed DB, checking  Template Amendments Details: Details Found");

            Logger.Info("Redirecting to Template Addendum Details Page");
            return View(tblTemplate);
        }

        
        [HttpPost]
        public ActionResult getTemplateAmendments(string select)
        {
            Logger.Info("Attempt Template getTemplateAmendments");
            try
            {
                string[] result = new string[2];

                result[0] = (from tblTemplateAmendment in db.tblTemplateAmendments select tblTemplateAmendment.TempID.ToString()).First();
                Logger.Info("Accessing DB for Template Details");
                if (select == "Amendments")
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result[1] = (from tblTemplateAmendment in db.tblTemplateAmendments select tblTemplateAmendment.Amendments).First();
                    Logger.Info("Accessed DB, Checking Template Details: Template Found");
                    
                }
                if (select == "Termination")
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result[1] = (from tblTemplateAmendment in db.tblTemplateAmendments select tblTemplateAmendment.Termination).First();
                    Logger.Info("Accessed DB, Checking Template Details: Template Found");
                }

                if (select == "Extension")
                {
                    Logger.Info("Accessed DB, Checking Template Details: TemplateID match");
                    result[1] = (from tblTemplateAmendment in db.tblTemplateAmendments select tblTemplateAmendment.Extension).First();
                    Logger.Info("Accessed DB, Checking Template Details: Template Found");
                }
                Logger.Info("Accessing DB for Template Details: Details Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'getTemplateAmendments' Action HTTP POST Main exception");

                return Json("errors");
            }
        }

        [Authorize(Roles = "admin, legal")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveTemplateAmendments(string content, string Select, int ID = 0)
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
                Logger.Info("Accessing DB for TemplateAmendments Details");
                tblTemplateAmendment template = db.tblTemplateAmendments.Find(ID);


                if (template == null)
                {
                    tblTemplateAmendment NewTemplate = new tblTemplateAmendment();
                   
                    if (Select == "Amendments")
                    {
                        NewTemplate.Amendments = content;
                    }
                    if (Select == "Extension")
                    {
                        NewTemplate.Extension = content;
                    }
                    if (Select == "Termination")
                    {
                        NewTemplate.Termination = content;
                    }
                    db.tblTemplateAmendments.Add(NewTemplate);
                    db.SaveChanges();
                }
                else
                {
                    template.TempID = ID;
                    if (Select == "Amendments")
                    {
                        template.Amendments = content;
                        tblTemplateAddendumLog log = new tblTemplateAddendumLog();

                        log.LogTemplateUID = ID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified the template - " + Select +  " Content modified";
                        log.ChangedFrom = "";
                        log.ChangedTo = "";
                        log.DateandTime = DateTime.Now.ToString();
                   
                    db.tblTemplateAddendumLogs.Add(log);
                }
                    if (Select == "Extension")
                    {
                        template.Extension = content;
                        tblTemplateAddendumLog log = new tblTemplateAddendumLog();

                        log.LogTemplateUID = ID;
                            log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified the template - " + Select +  " Content modified";
                        log.ChangedFrom = "";
                        log.ChangedTo = "";
                        log.DateandTime = DateTime.Now.ToString();

                        db.tblTemplateAddendumLogs.Add(log);
                    }
                    if (Select == "Termination")
                    {
                        template.Termination = content;
                        tblTemplateAddendumLog log = new tblTemplateAddendumLog();

                        log.LogTemplateUID = ID;
                            log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified the template - " + Select +  " Content modified";
                        log.ChangedFrom = "";
                        log.ChangedTo = "";
                        log.DateandTime = DateTime.Now.ToString();

                        db.tblTemplateAddendumLogs.Add(log);
                    }

                    db.Entry(template).State = EntityState.Modified;
                    db.SaveChanges();
                    Logger.Info("Accessed DB, SaveTemplateAmendments Record Saved");

                    Logger.Info("Accessing DB for Saving the Template Log Details");
                    
                    //tblTemplateAddendumLog log = new tblTemplateAddendumLog();
                    //log.LogTemplateUID = ID;
                    //log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                    //log.LogActivity = "Modified the template - " + Select;
                    //log.ChangedFrom = "-";
                    //log.ChangedTo = "-";
                    //log.DateandTime = DateTime.Now.ToString();
                    //db.tblTemplateAddendumLogs.Add(log);
                    db.SaveChanges();
                    Logger.Info("Accessed DB, Template Log Record Saved");
                }


                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'SaveTemplateAmendments' Action HTTP POST Main exception");

                return Json("errors");
            }
        }

        [HttpPost]
        public JsonResult GetNextApproverName(int NextApproverID)
        {
            Logger.Info("Attempt Template GetNextApproverName");
            try
            {
                Logger.Info("Accessing DB for UserMaster Details: ID match");
                var Datas = db.tblUserMasters.Where(x => x.UserEmployeeID == NextApproverID).Select(x => new { x.UserEmployeeName });
                //result.se.LastOrDefault();
                var result = Datas.SingleOrDefault();
                Logger.Info("Accessed DB, Checking UserMaster Details : Status Found");
                return Json(result.UserEmployeeName);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetNextApproverName' Action HTTP POST Main exception");
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        

        [HttpPost]
        public JsonResult GetApprovalLevel()
        {
            Logger.Info("Attempt Template GetApprovalLevel");
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


                Logger.Info("Accessing DB for ApprovalMaster Details : Department and SubDepartment match");
                var result = /*from tblApprovalMaster in*/ db.tblApprovalMasters.Where(x => x.Plant == EmployeePlant).Where(x => x.WorkflowType == "Template").Select(x => new { x.ApprovalLevel });
                                 //select tblApprovalMaster;
                    string[] UserInfo = new string[12];
                    foreach (var r in result)
                    {
                        UserInfo[0] = r.ApprovalLevel.ToString();
                        
                            if(UserInfo[0] == "1")
                            {
                                UserInfo[1] = "Level1";
                                
                            }
                            else if (UserInfo[0] == "2")
                            {
                                UserInfo[2] = "Level2";
                                
                            }
                            else if (UserInfo[0] == "3")
                            {
                                UserInfo[3] = "Level3";
                                
                            }
                            else if (UserInfo[0] == "4")
                            {
                                UserInfo[4] = "Level4";
                                
                            }
                            else if (UserInfo[0] == "5")
                            {
                                UserInfo[5] = "Level5";
                                UserInfo[11] = "success";
                            }
                            else if (UserInfo[0] == "6")
                            {
                                UserInfo[6] = "Level6";
                                UserInfo[11] = "success";
                            }
                            else if (UserInfo[0] == "7")
                            {
                                UserInfo[7] = "Level7";
                                UserInfo[11] = "success";
                            }
                            else if (UserInfo[0] == "8")
                            {
                                UserInfo[8] = "Level8";
                                UserInfo[11] = "success";
                            }
                            else if (UserInfo[0] == "9")
                            {
                                UserInfo[9] = "Level9";
                                UserInfo[11] = "success";
                            }
                            else if (UserInfo[0] == "10")
                            {
                                UserInfo[10] = "Level10";
                                UserInfo[11] = "success";
                            }
                      
                    }
                Logger.Info("Accessed DB, Checking ApprovalMaster Details: ApprovalMaster Details Found");
                return Json(UserInfo);
            }

            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetApprovalLevel' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [HttpPost]
        public JsonResult GetPaperTypeDetails(string PaperType, string PaperSize)
        {
            Logger.Info("Attempt Template GetPaperTypeDetails");
            try
            {

                Logger.Info("Accessing DB for Configuration Details : Paper TYpe match");
                var TableConfig = db.tblConfigurations.Where(x => x.PaperType == PaperType).Where(x => x.PaperSize == PaperSize).Select(x => new { x.PaperType, x.PaperSize, x.MarginTop, x.MarginRight, x.MarginLeft, x.MarginBottom });

                string[] PaperInfo = new string[12];
                foreach (var r in TableConfig)
                {
                    PaperInfo[0] = r.PaperType;
                    PaperInfo[1] = r.PaperSize;
                    PaperInfo[2] = r.MarginTop.ToString();
                    PaperInfo[3] = r.MarginLeft.ToString();
                    PaperInfo[4] = r.MarginBottom.ToString();
                    PaperInfo[5] = r.MarginRight.ToString();
                    PaperInfo[6] = "success";
                }
                Logger.Info("Accessed DB, Checking Configuration Details : Details Found");
                return Json(PaperInfo);
            }

            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Template' Controller , 'GetPaperTypeDetails' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [HttpPost]
        public JsonResult UploadForSignOff(HttpPostedFileBase UploadSignOffContractFile)
        {

            if (UploadSignOffContractFile != null)
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"];
                string Folder = DateTime.Now.ToString("yyyyddM-HHmmss");
                path = Path.Combine(path, Folder);
                path = path.Replace(" ", "");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
               
                string FilePath = Path.Combine(path, Path.GetFileName(UploadSignOffContractFile.FileName));
                FilePath = FilePath.Replace(" ", "");
                UploadSignOffContractFile.SaveAs(FilePath);

                string HTMLContent = DOCtoHTML(FilePath);
                if (!string.IsNullOrWhiteSpace(HTMLContent))
                {
                    return Json(HTMLContent);
                }
            }
            return Json("");
           
        }

        string DOCtoHTML(string FilePath)
        {
            string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"];
            string EXEfile = Path.Combine(path, "Converter.exe");
            ProcessStartInfo info = new ProcessStartInfo(EXEfile);
            info.Arguments = @"HTML "+FilePath ;
            info.UseShellExecute = false;

            info.RedirectStandardInput = true;

            info.RedirectStandardError = true;

            info.RedirectStandardOutput = true;
            //info.WorkingDirectory = @"D:\Temp\WCFTEST";
            //info.UserName = @".\Elitebook1"; // see the link mentioned at the top

            //info.Password = new NetworkCredential("", "").SecurePassword;

            using (Process install = Process.Start(info))

            {

                string output = install.StandardOutput.ReadToEnd();

                install.WaitForExit();

                // Do something with you output data

                Console.WriteLine(output);

            }
            string fileContents = "";
            try
            {
                string OuputFile = Path.GetDirectoryName(FilePath);
                 OuputFile = Path.Combine(OuputFile,  Path.GetFileNameWithoutExtension(FilePath) + ".html");
                fileContents = System.IO.File.ReadAllText(OuputFile);
            }
            catch
            {

            }           
            return fileContents;
        }

    }
}
