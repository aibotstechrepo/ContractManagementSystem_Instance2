using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ContractManagementSystem.Models;
using NLog;

namespace ContractManagementSystem.Controllers
{
    public class ApprovalsController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        // GET: Approvals
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
        public ActionResult New()
        {
            Logger.Info("Accessing ApprovalsMaster New Page");
            return View(db.tblUserMasters.ToList());
        }

        public ActionResult Repository()
        {
            Logger.Info("Accessing ApprovalsMaster Repository Page");
            Logger.Info("Accessing DB for Repository");


            List<tblApprovalMaster> ApprovalMaster = db.tblApprovalMasters.ToList();
            ApprovalMaster.Reverse();
            return View(ApprovalMaster);

        }

        public ActionResult Index()
        {
            return RedirectToAction("Repository");
        }

        public ActionResult Details()
        {
            return RedirectToAction("Repository");
        }

        [Route("Approvals/Details/{id:int}")]
        public ActionResult Details(int id)
        {
            Logger.Info("Accessing DB for ApprovalMaster Details");
            tblApprovalMaster tblApprovalMaster = db.tblApprovalMasters.Find(id);

            Logger.Info("Accessed DB, Checking ApprovalMaster Details: Checking Status");
            if (tblApprovalMaster == null)
            {
                Logger.Info("Accessed DB, Checking ApprovalMaster Details: Details Not Found");
                return HttpNotFound();

            }
            Logger.Info("Accessed DB, Checking ApprovalMaster Details: Details Found");

            Logger.Info("Redirecting to ApprovalMaster Details Page");
            return View(tblApprovalMaster);
        }


        [HttpPost]
        public JsonResult getCategory(int CurrentUserID)
        {
            Logger.Info("Attempt ApproverMaster getCategory");
            try
            {
                var EmployeePlant = "";
                EmployeePlant = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == CurrentUserID) select tblUserMaster.UserPlant).First();

                Logger.Info("Accessing DB for Category List");
                var result = from tblDepartment in db.tblDepartments.Where(x => x.PlantName == EmployeePlant) select tblDepartment.DepartmentName;
                Logger.Info("Accessed DB, Checking Category List: Category Found");
                return Json(result);
            }
            catch(Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'getCategory' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [HttpPost]
        public JsonResult getSubCategory(string user_category_id)
        {
            Logger.Info("Attempt ApproverMaster getSubCategory");
            try
            {
               
                Logger.Info("Accessing DB for SubCategory List");
                
                var result = from tblSubDepartment in db.tblSubDepartments.Where(x => x.DepartmentName == user_category_id) select tblSubDepartment.SubDepartmentName;
                //foreach (var r in result)
                //{
                //    Console.WriteLine(r);
                //}
                Logger.Info("Accessed DB, Checking SubCategory List: SubCategory Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'getSubCategory' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        string UsersInFo(int[] UserList)
        {
            try
            {
                string Userinfo = "";

                foreach (var userID in UserList)
                {
                    string UserIDnName = userID + " - " + (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == userID) select tblUserMaster.UserEmployeeName).First();
                    Userinfo = Userinfo + UserIDnName + "<br />";
                }

                return Userinfo;
            }
            catch { }
            return null;
        }



        [HttpPost]
        public ActionResult SubmitApproversToDB(string WorkflowType, string ApproverCluster, string ApproverFunction, int[] UID1, int[] UID2, int[] UID3, int[] UID4, int[] UID5, int[] UID6, int[] UID7, int[] UID8, int[] UID9, int[] UID10, int CurrentUserID = 0)
        {
            string ChangedFrom = "";
            string ChangedTo = "";

            if(UID1 != null)
            {
                ChangedTo = "<b>Approver Level 1</b><br />" + UsersInFo(UID1);
            }
            if(UID2 != null)
            {
                ChangedTo += "<b>Approver Level 2</b><br />" + UsersInFo(UID2);
            }
            if (UID3 != null)
            {
                ChangedTo += "<b>Approver Level 3</b><br />" + UsersInFo(UID3);
            }
            if (UID4 != null)
            {
                ChangedTo += "<b>Approver Level 4</b><br />" + UsersInFo(UID4);
            }
            if (UID5 != null)
            {
                ChangedTo += "<b>Approver Level 5</b><br />" + UsersInFo(UID5);
            }
            if (UID6 != null)
            {
                ChangedTo += "<b>Approver Level 6</b><br />" + UsersInFo(UID6);
            }
            if (UID7 != null)
            {
                ChangedTo += "<b>Approver Level 7</b><br />" + UsersInFo(UID7);
            }
            if (UID8 != null)
            {
                ChangedTo += "<b>Approver Level 8</b><br />" + UsersInFo(UID8);
            }
            if (UID9 != null)
            {
                ChangedTo += "<b>Approver Level 9</b><br />" + UsersInFo(UID9);
            }
            if (UID10 != null)
            {
                ChangedTo += "<b>Approver Level 10</b><br />" + UsersInFo(UID10);
            }
           

            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }

            var EmployeePlant = "";
            EmployeePlant = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == CurrentUserID) select tblUserMaster.UserPlant).First();


            Logger.Info("Attempt ApproverMaster SubmitApproversToDB");
            try
            {
                string OldValues = "";
                string NewValues = "";

                try
                {
                   
                    if (WorkflowType == "Template")
                    {
                        Logger.Info("Accessing DB for ApproverMaster Details");
                        var Data = /*from tblApprovalMaster in*/ db.tblApprovalMasters.Where(x => x.Plant == EmployeePlant).Where(x => x.WorkflowType == WorkflowType).Select(x => new { x.ApprovalID, x.EMPID, x.ApprovalLevel }); //select tblApprovalMaster;

                        int[] TUID1 = new int[0]; int[] TUID2 = new int[0]; int[] TUID3 = new int[0]; int[] TUID4 = new int[0]; int[] TUID5 = new int[0]; int[] TUID6 = new int[0]; int[] TUID7 = new int[0]; int[] TUID8 = new int[0]; int[] TUID9 = new int[0]; int[] TUID10 = new int[0];

                        Logger.Info("Accessed DB, Checking ApproverMaster Details: Details Found");
                        if (Data.ToList().Count > 0)
                        {
                            foreach (var item in Data)
                            {
                                var Id = item.ApprovalID;


                                if (item.ApprovalLevel == 1)
                                {
                                    Array.Resize(ref TUID1, TUID1.Length + 1);
                                    TUID1[TUID1.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 2)
                                {
                                    Array.Resize(ref TUID2, TUID2.Length + 1);
                                    TUID2[TUID2.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 3)
                                {
                                    Array.Resize(ref TUID3, TUID3.Length + 1);
                                    TUID3[TUID3.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 4)
                                {
                                    Array.Resize(ref TUID4, TUID4.Length + 1);
                                    TUID4[TUID4.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 5)
                                {
                                    Array.Resize(ref TUID5, TUID5.Length + 1);
                                    TUID5[TUID5.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 6)
                                {
                                    Array.Resize(ref TUID6, TUID6.Length + 1);
                                    TUID6[TUID6.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 7)
                                {
                                    Array.Resize(ref TUID7, TUID7.Length + 1);
                                    TUID7[TUID7.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 8)
                                {
                                    Array.Resize(ref TUID8, TUID8.Length + 1);
                                    TUID8[TUID8.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 9)
                                {
                                    Array.Resize(ref TUID9, TUID9.Length + 1);
                                    TUID9[TUID9.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 10)
                                {
                                    Array.Resize(ref TUID10, TUID10.Length + 1);
                                    TUID10[TUID10.Length - 1] = Convert.ToInt32(item.EMPID);
                                }

                                if (TUID1.Length > 0)
                                {
                                    ChangedFrom = "<b>Approver Level 1</b><br />" + UsersInFo(TUID1);
                                }
                                if (TUID2.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 2</b><br />" + UsersInFo(TUID2);
                                }
                                if (TUID3.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 3</b><br />" + UsersInFo(TUID3);
                                }
                                if (TUID4.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 4</b><br />" + UsersInFo(TUID4);
                                }
                                if (TUID5.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 5</b><br />" + UsersInFo(TUID5);
                                }
                                if (TUID6.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 6</b><br />" + UsersInFo(TUID6);
                                }
                                if (TUID7.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 7</b><br />" + UsersInFo(TUID7);
                                }
                                if (TUID8.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 8</b><br />" + UsersInFo(TUID8);
                                }
                                if (TUID9.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 9</b><br />" + UsersInFo(TUID9);
                                }
                                if (TUID10.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 10</b><br />" + UsersInFo(TUID10);
                                }

                                tblApprovalMaster eachData = db.tblApprovalMasters.Find(Id);

                                db.tblApprovalMasters.Remove(eachData);
                                //db.SaveChanges();
                            }

                            tblApprovalLog log = new tblApprovalLog();
                            log.LogApprovalUID = 1;
                            log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                            log.LogActivity = "Template Approval Workflow Modified";
                            log.ChangedFrom = ChangedFrom;
                            log.ChangedTo = ChangedTo;
                            log.DateandTime = DateTime.Now.ToString();
                            db.tblApprovalLogs.Add(log);

                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        Logger.Info("Accessing DB for ApproverMaster Details");
                        var Data = /*from tblApprovalMaster in*/ db.tblApprovalMasters.Where(x => x.Plant == EmployeePlant).Where(x => x.WorkflowType == WorkflowType).Where(x => x.Department == ApproverCluster).Where(x => x.SubDepartment == ApproverFunction).Select(x => new { x.ApprovalID , x.EMPID, x.ApprovalLevel}); //select tblApprovalMaster;
                        int[] TUID1 = new int[0]; int[] TUID2 = new int[0]; int[] TUID3 = new int[0]; int[] TUID4 = new int[0]; int[] TUID5 = new int[0]; int[] TUID6 = new int[0]; int[] TUID7 = new int[0]; int[] TUID8 = new int[0]; int[] TUID9 = new int[0]; int[] TUID10 = new int[0];

                        Logger.Info("Accessed DB, Checking ApproverMaster Details: Details Found");
                        if (Data.ToList().Count > 0)
                        {

                            foreach (var item in Data)
                            {
                                var Id = item.ApprovalID;

                                if(item.ApprovalLevel == 1)
                                {
                                    Array.Resize(ref TUID1, TUID1.Length + 1);
                                    TUID1[TUID1.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 2)
                                {
                                    Array.Resize(ref TUID2, TUID2.Length + 1);
                                    TUID2[TUID2.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 3)
                                {
                                    Array.Resize(ref TUID3, TUID3.Length + 1);
                                    TUID3[TUID3.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 4)
                                {
                                    Array.Resize(ref TUID4, TUID4.Length + 1);
                                    TUID4[TUID4.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 5)
                                {
                                    Array.Resize(ref TUID5, TUID5.Length + 1);
                                    TUID5[TUID5.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 6)
                                {
                                    Array.Resize(ref TUID6, TUID6.Length + 1);
                                    TUID6[TUID6.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 7)
                                {
                                    Array.Resize(ref TUID7, TUID7.Length + 1);
                                    TUID7[TUID7.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 8)
                                {
                                    Array.Resize(ref TUID8, TUID8.Length + 1);
                                    TUID8[TUID8.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 9)
                                {
                                    Array.Resize(ref TUID9, TUID9.Length + 1);
                                    TUID9[TUID9.Length - 1] = Convert.ToInt32(item.EMPID);
                                }
                                if (item.ApprovalLevel == 10)
                                {
                                    Array.Resize(ref TUID10, TUID10.Length + 1);
                                    TUID10[TUID10.Length - 1] = Convert.ToInt32(item.EMPID);
                                }

                                if(TUID1.Length > 0)
                                {
                                    ChangedFrom = "<b>Approver Level 1</b><br />" + UsersInFo(TUID1);
                                }
                                if (TUID2.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 2</b><br />" + UsersInFo(TUID2);
                                }
                                if (TUID3.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 3</b><br />" + UsersInFo(TUID3);
                                }
                                if (TUID4.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 4</b><br />" + UsersInFo(TUID4);
                                }
                                if (TUID5.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 5</b><br />" + UsersInFo(TUID5);
                                }
                                if (TUID6.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 6</b><br />" + UsersInFo(TUID6);
                                }
                                if (TUID7.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 7</b><br />" + UsersInFo(TUID7);
                                }
                                if (TUID8.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 8</b><br />" + UsersInFo(TUID8);
                                }
                                if (TUID9.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 9</b><br />" + UsersInFo(TUID9);
                                }
                                if (TUID10.Length > 0)
                                {
                                    ChangedFrom += "<b>Approver Level 10</b><br />" + UsersInFo(TUID10);
                                }
                                
                                
                                tblApprovalMaster eachData = db.tblApprovalMasters.Find(Id);

                                db.tblApprovalMasters.Remove(eachData);


                                //db.SaveChanges();
                            }

                            tblApprovalLog log = new tblApprovalLog();
                            log.LogApprovalUID = 1;
                            log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                            log.LogActivity = "Contract Approval Workflow Modified";
                            log.ChangedFrom = ChangedFrom;
                            log.ChangedTo = ChangedTo;
                            log.DateandTime = DateTime.Now.ToString();
                            db.tblApprovalLogs.Add(log);

                            db.SaveChanges();
                        }
                    }

                    
                }
                catch (Exception Ex)
                {
                    Logger.Error(Ex, "'Approval' Controller , 'SubmitApproversToDB' Action HTTP POST Main exception");
                    return Json("error");
                }



                if (UID1 != null )
                {
                    for (int i = 0; i < UID1.Length; i++)
                    {
                        Logger.Info("Attempt ApproverMaster SubmitApproversToDB");
                        try
                        {
                            Logger.Info("Accessing DB for Saving the ApproverMaster Details");
                            tblApprovalMaster newData = new tblApprovalMaster
                            {
                                WorkflowType = WorkflowType,
                                Plant = EmployeePlant,
                                Department = ApproverCluster,
                                SubDepartment = ApproverFunction,
                                ApprovalLevel = 1,
                                EMPID = UID1[i],
                            };

                            db.tblApprovalMasters.Add(newData);

                           
                        }

                        catch { }

                        
                    }
                }

                if (UID2 != null)
                {
                    for (int i = 0; i < UID2.Length; i++)
                    {
                        Logger.Info("Attempt ApproverMaster SubmitApproversToDB");
                        try
                        {
                            Logger.Info("Accessing DB for Saving the ApproverMaster Details");
                            tblApprovalMaster newData = new tblApprovalMaster
                            {
                                WorkflowType = WorkflowType,
                                Plant = EmployeePlant,
                                Department = ApproverCluster,
                                SubDepartment = ApproverFunction,
                                ApprovalLevel = 2,
                                EMPID = UID2[i],
                            };

                            db.tblApprovalMasters.Add(newData);

                            
                        }

                        catch { }

                        
                    }
                }

                if (UID3 != null)
                {
                    for (int i = 0; i < UID3.Length; i++)
                    {
                        Logger.Info("Attempt ApproverMaster SubmitApproversToDB");
                        try
                        {
                            Logger.Info("Accessing DB for Saving the ApproverMaster Details");
                            tblApprovalMaster newData = new tblApprovalMaster
                            {
                                WorkflowType = WorkflowType,
                                Plant = EmployeePlant,
                                Department = ApproverCluster,
                                SubDepartment = ApproverFunction,
                                ApprovalLevel = 3,
                                EMPID = UID3[i],
                            };

                            db.tblApprovalMasters.Add(newData);

                           
                        }

                        catch { }

                        
                    }
                }

                if (UID4 != null)
                {
                    for (int i = 0; i < UID4.Length; i++)
                    {
                        Logger.Info("Attempt ApproverMaster SubmitApproversToDB");
                        try
                        {
                            Logger.Info("Accessing DB for Saving the ApproverMaster Details");
                            tblApprovalMaster newData = new tblApprovalMaster
                            {
                                WorkflowType = WorkflowType,
                                Plant = EmployeePlant,
                                Department = ApproverCluster,
                                SubDepartment = ApproverFunction,
                                ApprovalLevel = 4,
                                EMPID = UID4[i],
                            };

                            db.tblApprovalMasters.Add(newData);

                        }

                        catch { }

                        
                    }
                }

                if (UID5 != null)
                {
                    for (int i = 0; i < UID5.Length; i++)
                    {
                        Logger.Info("Attempt ApproverMaster SubmitApproversToDB");
                        try
                        {
                            Logger.Info("Accessing DB for Saving the ApproverMaster Details");
                            tblApprovalMaster newData = new tblApprovalMaster
                            {
                                WorkflowType = WorkflowType,
                                Plant = EmployeePlant,
                                Department = ApproverCluster,
                                SubDepartment = ApproverFunction,
                                ApprovalLevel = 5,
                                EMPID = UID5[i],
                            };

                            db.tblApprovalMasters.Add(newData);

                           
                        }

                        catch { }

                        
                    }
                }

                if (UID6 != null)
                {
                    for (int i = 0; i < UID6.Length; i++)
                    {
                        Logger.Info("Attempt ApproverMaster SubmitApproversToDB");
                        try
                        {
                            Logger.Info("Accessing DB for Saving the ApproverMaster Details");
                            tblApprovalMaster newData = new tblApprovalMaster
                            {
                                WorkflowType = WorkflowType,
                                Plant = EmployeePlant,
                                Department = ApproverCluster,
                                SubDepartment = ApproverFunction,
                                ApprovalLevel = 6,
                                EMPID = UID6[i],
                            };

                            db.tblApprovalMasters.Add(newData);

                        }

                        catch { }

                        
                    }
                }

                if (UID7 != null)
                {
                    for (int i = 0; i < UID7.Length; i++)
                    {
                        Logger.Info("Attempt ApproverMaster SubmitApproversToDB");
                        try
                        {
                            Logger.Info("Accessing DB for Saving the ApproverMaster Details");
                            tblApprovalMaster newData = new tblApprovalMaster
                            {
                                WorkflowType = WorkflowType,
                                Plant = EmployeePlant,
                                Department = ApproverCluster,
                                SubDepartment = ApproverFunction,
                                ApprovalLevel = 7,
                                EMPID = UID7[i],
                            };

                            db.tblApprovalMasters.Add(newData);

                          
                        }

                        catch { }

                        
                    }
                }

                if (UID8 != null)
                {
                    for (int i = 0; i < UID8.Length; i++)
                    {
                        Logger.Info("Attempt ApproverMaster SubmitApproversToDB");
                        try
                        {
                            Logger.Info("Accessing DB for Saving the ApproverMaster Details");
                            tblApprovalMaster newData = new tblApprovalMaster
                            {
                                WorkflowType = WorkflowType,
                                Plant = EmployeePlant,
                                Department = ApproverCluster,
                                SubDepartment = ApproverFunction,
                                ApprovalLevel = 8,
                                EMPID = UID8[i],
                            };

                            db.tblApprovalMasters.Add(newData);

                          
                        }

                        catch { }

                        
                    }
                }

                if (UID9 != null)
                {
                    for (int i = 0; i < UID9.Length; i++)
                    {
                        Logger.Info("Attempt ApproverMaster SubmitApproversToDB");
                        try
                        {
                            Logger.Info("Accessing DB for Saving the ApproverMaster Details");
                            tblApprovalMaster newData = new tblApprovalMaster
                            {
                                WorkflowType = WorkflowType,
                                Plant = EmployeePlant,
                                Department = ApproverCluster,
                                SubDepartment = ApproverFunction,
                                ApprovalLevel = 9,
                                EMPID = UID9[i],
                            };

                            db.tblApprovalMasters.Add(newData);

                          

                        }

                        catch { }

                        
                    }
                }

                if (UID10 != null)
                {
                    for (int i = 0; i < UID10.Length; i++)
                    {
                        Logger.Info("Attempt ApproverMaster SubmitApproversToDB");
                        try
                        {
                            Logger.Info("Accessing DB for Saving the ApproverMaster Details");
                            tblApprovalMaster newData = new tblApprovalMaster
                            {
                                WorkflowType = WorkflowType,
                                Plant = EmployeePlant,
                                Department = ApproverCluster,
                                SubDepartment = ApproverFunction,
                                ApprovalLevel = 10,
                                EMPID = UID10[i],
                            };

                            db.tblApprovalMasters.Add(newData);

                         

                        }

                        catch { }

                        
                    }
                }

                if (OldValues.Length > 0)
                {
                    tblApprovalLog logs = new tblApprovalLog();
                    logs.LogApprovalUID = 1;
                    logs.ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName;
                    logs.LogActivity = "Modified";
                    logs.ChangedFrom = OldValues;
                    logs.ChangedTo = NewValues;
                    logs.DateandTime = DateTime.Now.ToString();

                    db.tblApprovalLogs.Add(logs);
                }
                db.SaveChanges();

                Logger.Info("Accessed DB, ApprovalMaster Log Record Saved");
                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'SubmitApproversToDB' Action HTTP POST Main exception");
                return Json("error");
            }

        }



        //    [HttpPost]
        //public ActionResult UpdateApproversToDB(int ApprovalID, string WorkflowType, string ApproverCluster, string ApproverFunction, int[] UID, int CurrentUserID = 0)
        //{
        //    Logger.Info("Attempt ApproverMaster UpdateApproversToDB");
        //    try
        //    {
        //        Logger.Info("Accessing DB for Updating the ApprovalMaster Records");
        //        tblApprovalMaster approver = db.tblApprovalMasters.Find(ApprovalID);
        //        //approver.WorkflowType = WorkflowType;
        //        //approver.Category = ApproverCluster;
        //        //approver.SubCategory = ApproverFunction;

        //        approver.WorkflowType = HttpUtility.HtmlEncode(WorkflowType);
        //        approver.Category = HttpUtility.HtmlEncode(ApproverCluster);
        //        approver.SubCategory = HttpUtility.HtmlEncode(ApproverFunction);
        //        if (UID.Length > 0)
        //        {
        //            approver.Approver1ID = UID[0];
        //        }
        //        else
        //        {
        //            approver.Approver1ID = 0;
        //        }
        //        if (UID.Length > 1)
        //        {
        //            approver.Approver2ID = UID[1];
        //        }
        //        else
        //        {
        //            approver.Approver2ID = 0;
        //        }
        //        if (UID.Length > 2)
        //        {
        //            approver.Approver3ID = UID[2];
        //        }
        //        else
        //        {
        //            approver.Approver3ID = 0;
        //        }
        //        if (UID.Length > 3)
        //        {
        //            approver.Approver4ID = UID[3];
        //        }
        //        else
        //        {
        //            approver.Approver4ID = 0;
        //        }
        //        if (UID.Length > 4)
        //        {
        //            approver.Approver5ID = UID[4];
        //        }
        //        else
        //        {
        //            approver.Approver5ID = 0;
        //        }
        //        if (UID.Length > 5)
        //        {
        //            approver.Approver6ID = UID[5];
        //        }
        //        else
        //        {
        //            approver.Approver6ID = 0;
        //        }
        //        if (UID.Length > 6)
        //        {
        //            approver.Approver7ID = UID[6];
        //        }
        //        else
        //        {
        //            approver.Approver7ID = 0;
        //        }
        //        if (UID.Length > 7)
        //        {
        //            approver.Approver8ID = UID[7];
        //        }
        //        else
        //        {
        //            approver.Approver8ID = 0;
        //        }
        //        if (UID.Length > 8)
        //        {
        //            approver.Approver9ID = UID[8];
        //        }
        //        else
        //        {
        //            approver.Approver9ID = 0;
        //        }
        //        if (UID.Length > 9)
        //        {
        //            approver.Approver10ID = UID[9];
        //        }
        //        else
        //        {
        //            approver.Approver10ID = 0;
        //        }
        //        approver.LastModified = DateTime.Now;

        //        db.Entry(approver).State = EntityState.Modified;
        //        db.SaveChanges();
        //        Logger.Info("Accessed DB, ApprovalMaster Updated Record Saved");


        //        return Json("success");
        //    }
        //    catch (Exception Ex)
        //    {
        //        Logger.Error(Ex, "'Approvals' Controller , 'UpdateApproversToDB' Action HTTP POST Main exception");
        //        return Json("error");
        //    }
        //}


        [HttpPost]

        public ActionResult DeleteApproverFromDB(int ApprovalID, int CurrentUserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt ApproverMaster DeleteApproverFromDB");
            try
            {
                Logger.Info("Accessing DB for Deleting the ApprovalMaster Records");
                tblApprovalMaster approval = db.tblApprovalMasters.Find(ApprovalID);
                db.tblApprovalMasters.Remove(approval);
                db.SaveChanges();
                Logger.Info("Accessed DB, ApprovalMaster Record Deleted");

                Logger.Info("Accessing DB for Saving the ApprovalMaster Log Details");
                tblApprovalLog log = new tblApprovalLog
                {
                    LogApprovalUID = ApprovalID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Deleted",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblApprovalLogs.Add(log);
                db.SaveChanges();
                Logger.Info("Accessed DB, ApprovalMaster Log Record Saved");
                return Json("success");
            }
            catch(Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'DeleteApproverFromDB' Action HTTP POST Main exception");
                return Json("error");
            }
        }



    [HttpPost]
        public ActionResult getLogDetail(int ID = 0)
        {
            Logger.Info("Attempt ApprovalMaster getLogDetail");

            try
            {
                Logger.Info("Accessed DB, Checking ApprovalMaster Log Details: LogID match");
                var result = from tblApprovalLog in db.tblApprovalLogs select tblApprovalLog;
                Logger.Info("Accessed DB, Checking ApprovalMaster Log Details: LogDetails Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'getLogDetail' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult GetApproversBasedOnClusterAndFunction(string ApproverCluster, string ApproverFunction)
        {
            Logger.Info("Attempt ApprovalMaster GetApproversBasedOnClusterAndFunction");
            try
            {
                Logger.Info("Accessed DB, Checking UserMaster Details: Category and SubCategory match");
                var ApproverList = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserCategory.Contains(ApproverCluster)).
                            Where(x => x.UserSubCategory.Contains(ApproverFunction)).
                            Select(x => new
                            {
                                x.UserCategory,
                                x.UserEmployeeDesignation,
                                x.UserEmployeeEmail,
                                x.UserEmployeeID,
                                x.UserEmployeeName,
                                x.UserRoleAdmin,
                                x.UserRoleApprover,
                                x.UserRoleFinance,
                                x.UserRoleInitiator,
                                x.UserRoleLegal,
                                x.UserRoleReviewer,
                                x.UserSubCategory
                            });

                Logger.Info("Accessed DB, Checking UserMaster Details: Category and SubCategory Found");
                return Json(ApproverList);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'GetApproversBasedOnClusterAndFunction' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult SaveLog(string details, int ID, string initialvalue, string UserID)
        {
            Logger.Info("Attempt ApprovalMaster SaveLog");


            try
            {
                Logger.Info("Accessing DB for Saving the ApprovalMaster Log Details");
                tblApprovalLog log = new tblApprovalLog
                {
                    LogApprovalUID = ID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Modified",
                    ChangedFrom = initialvalue,
                    ChangedTo = details,
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblApprovalLogs.Add(log);
                db.SaveChanges();
                Logger.Info("Accessed DB, ApprovalMaster Log Record Saved");
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'SaveLog' Action HTTP POST Main exception");
                return Json(Ex.Message);
            }
        }
        [HttpPost]
        public JsonResult GetUserById(string EmployeeID)
        {
            Logger.Info("Attempt ApprovalMaster GetUserById");

            try
            {

                if (!string.IsNullOrWhiteSpace(EmployeeID))
                {
                    EmployeeID = EmployeeID.Trim();
                    int EMPID = Convert.ToInt32(EmployeeID);
                    Logger.Info("Accessed DB, Checking UserMaster Details: EmployeeID match");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == EMPID) select tblUserMaster;
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
                        UserInfo[1] = r.UserEmployeeName;
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
            catch(Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'GetUserById' Action HTTP POST Main exception");
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
            Logger.Info("Attempt ApprovalMaster GetUserByName");

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
                Logger.Error(Ex, "'Approvals' Controller , 'GetUserByName' Action HTTP POST Main exception");
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
            Logger.Info("Attempt ApprovalMaster GetUserByEmail");

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
                Logger.Error(Ex, "'Approvals' Controller , 'GetUserByEmail' Action HTTP POST Main exception");
                string[] errors = { "error" };
                return Json(errors);
            }
            Logger.Info("Accessed DB, Checking UserMaster Details: User Details Not Found");
            string[] failures = { "failure" };
            return Json(failures);
        }

        [HttpPost]
        public ActionResult CheckApprovalWorkflowExist(string ApprovalCluster,string ApprovalFunction,string workflowType)
        {
            Logger.Info("Attempt ApprovalMaster CheckApprovalWorkflowExist");
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

                if(workflowType == "Template")
                {
                    Logger.Info("Accessed DB, Checking ApprovalMaster Details: Checking Status");
                    var result = /*from tblApprovalMaster in*/ db.tblApprovalMasters.Where(x => x.Plant == EmployeePlant).Where(x => x.WorkflowType == workflowType).Select(x => new { x.ApprovalLevel, x.EMPID });
                    //select tblApprovalMaster;
                    return Json(result);
                }
                else
                {
                    Logger.Info("Accessed DB, Checking ApprovalMaster Details: Checking Status");
                    var result = /*from tblApprovalMaster in*/ db.tblApprovalMasters.Where(x => x.Plant == EmployeePlant).Where(x => x.Department == ApprovalCluster)
                                 .Where(x => x.SubDepartment == ApprovalFunction).Where(x => x.WorkflowType == workflowType).Select(x => new { x.ApprovalLevel, x.EMPID });
                    //select tblApprovalMaster;
                    return Json(result);
                }
                
            }
            catch(Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'CheckApprovalWorkflowExist' Action HTTP POST Main exception");
                return Json("Error");
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
                    x.UserStatus
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
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleFinance2,
                    x.UserRoleReviewer,
                    x.UserSubCategory,
                    x.UserStatus
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
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleFinance2,
                    x.UserRoleReviewer,
                    x.UserSubCategory,
                    x.UserStatus
                }); //select tblUserMaster; //select tblUserMaster;
                return Json(result);
            }
            else
            {
                return Json("");
            }
        }

        [HttpPost]
        public ActionResult DepartmentMappingCheck(int EmployeeID, string EmpDept, string EmpSubDept)
        {
            Logger.Info("Attempt ApprovalMaster DepartmentMappingCheck");
            try
            {
                Logger.Info("Accessed DB, Checking DepartmentMapping Details: Checking Status");
                var result = db.tblDepartmentMappings.Where(x => x.EmployeeID == EmployeeID)
                             .Where(x => x.Department == EmpDept).Where(x => x.SubDepartment == EmpSubDept).Count();
               
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'DepartmentMappingCheck' Action HTTP POST Main exception");
                return Json("Error");
            }
        }


    }
}