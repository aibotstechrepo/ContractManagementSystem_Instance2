using ContractManagementSystem.Models;
using NLog;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContractManagementSystem.Controllers
{
    //[Authorize(Roles = "admin")]
    public class DepartmentMasterController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        // GET: Division

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

        public ActionResult Department()
        {
            if (TempData["newstatus"] != null)
            {
                ViewBag.Status = "NewSuccess";
                TempData.Remove("newstatus");
            }
            if (TempData["categorystatus"] != null)
            {
                ViewBag.Status = "CategorySuccess";
                TempData.Remove("categorystatus");
            }
            Logger.Info("Attempt DepartmentMaster Department");
            Logger.Info("Accessing DB for Department Page");
            return View(db.tblDepartments.ToList());
        }

        public ActionResult Index()
        {
            return RedirectToAction("Department");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult SaveRecord1(tblDepartment model, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt DepartmentMaster SaveRecord1");
            try
            {

                Logger.Info("Accessing DB for saving the Category Records");
                //tblDepartment category = new tblDepartment();
                //category.CategoryName = model.CategoryName;
                model.DepartmentName = HttpUtility.HtmlEncode(model.DepartmentName);

                model.LastModified = DateTime.Now;
                db.tblDepartments.Add(model);
                db.SaveChanges();
                Logger.Info("Accessed DB, Category Details Saved");
                TempData["newstatus"] = "NewSuccess";

                Logger.Info("Accessing DB for Saving the CategoryLog Details");

                tblCategoryLog log = new tblCategoryLog
                {
                    LogCategoryUID = model.DepartmentID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Created",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblCategoryLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, Category Log Details Saved");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'SaveRecord' Action HTTP POST Main exception");
            }
            
            Logger.Info("Setting Auth Cookie and Redirecting to Category");
            return RedirectToAction("Department");
        }
        public ActionResult SubDepartment()
        {
            if (TempData["newstatus"] != null)
            {
                ViewBag.Status = "NewSuccess";
                TempData.Remove("newstatus");
            }
            if (TempData["subcategorystatus"] != null)
            {
                ViewBag.Status = "SubCategorySuccess";
                TempData.Remove("subcategorystatus");
            }
            Logger.Info("Attempt DepartmentMaster SubDepartment");
            Logger.Info("Accessing DB for SubCategory Page");
            return View(db.tblSubDepartments.ToList());
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult SaveRecord3(tblSubDepartment model, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt DepartmentMaster SaveRecord3");
            try
            {
                Logger.Info("Accessing DB for saving the SubCategory Records");
                // tblSubDepartment subcategory = new tblSubDepartment();

                model.PlantName = HttpUtility.HtmlEncode(model.PlantName);
                model.DepartmentName = HttpUtility.HtmlEncode(model.DepartmentName);
                model.SubDepartmentName = HttpUtility.HtmlEncode(model.SubDepartmentName);
                //subcategory.CategoryName = model.CategoryName;
                //subcategory.SubCategoryName = model.SubCategoryName;
                model.LastModified = DateTime.Now;

                db.tblSubDepartments.Add(model);

                db.SaveChanges();
                Logger.Info("Accessed DB, SubCategory Details Saved");
                TempData["newstatus"] = "NewSuccess";

                Logger.Info("Accessing DB for Saving the SubCategoryLog Details");
                tblSubCategoryLog log = new tblSubCategoryLog
                {
                    LogSubCategoryUID = model.SubDepartmentID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Created",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblSubCategoryLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, SubCategoryLog Details Saved");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'SaveRecord3' Action HTTP POST Main exception");
            }
            
            return RedirectToAction("SubDepartment");
        }


        [HttpPost]
        public JsonResult CategoryList(string PlantName)
        {
            Logger.Info("Attempt DepartmentMaster CategoryList");
            try
            {
                Logger.Info("Accessing DB for Department List: Checking Status ");
                var result = from tblDepartment in db.tblDepartments.Where(x => x.PlantName == PlantName) select tblDepartment.DepartmentName;
                Logger.Info("Accesed DB, Checked Department List : Department List Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'CategoryList' Action HTTP POST Main exception");
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult PlantList()
        {
            Logger.Info("Attempt PlantMaster PlantList");
            try
            {
                Logger.Info("Accessing DB for Plant List: Checking Status ");
                var result = from tblPlant in db.tblPlants select tblPlant.PlantName;
                Logger.Info("Accesed DB, Checked Plant List : Plant List Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'PlantList' Action HTTP POST Main exception");
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult EditCategory(string CategoryID)
        {
            Logger.Info("Attempt DepartmentMaster EditCategory");
            try
            {

                int IDName = Convert.ToInt32(CategoryID);
                Logger.Info("Accessing DB for Updating the Category List");
                var result = from tblDepartment in db.tblDepartments.Where(x => x.DepartmentID == IDName) select tblDepartment;

                /* select tblDepartment;*/
                foreach (var r in result)
                {
                    Logger.Info("Accesed DB, Category Updating List Saved");
                    return Json(r);
                }
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'EditCategory' Action HTTP POST Main exception");
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult SaveRecordCategory(tblDepartment model)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }

            Logger.Info("Attempt DepartmentMaster SaveRecordCategory");

            try
            {
                Logger.Info("Accessing DB for saving Category Records");
                tblDepartment Department = db.tblDepartments.Find(model.DepartmentID);

                string OldValues = "";
                string NewValues = "";
                if (Department.PlantName != model.PlantName)
                {
                    OldValues = OldValues + "Plant Name : " + Department.PlantName + " , ";
                    NewValues = NewValues + "Plant Name : " + model.PlantName + " , ";
                }
                if (Department.DepartmentName != model.DepartmentName)
                {
                    OldValues = OldValues + "Department Name : " + Department.DepartmentName + " , ";
                    NewValues = NewValues + "Department Name : " + model.DepartmentName + " , ";
                }
                //Category.CategoryName = model.CategoryName;
                Department.PlantName = HttpUtility.HtmlEncode(model.PlantName);
                Department.DepartmentName = HttpUtility.HtmlEncode(model.DepartmentName);


                    //DepartmentID = model.DepartmentID,
                    Department.LastModified = DateTime.Now;
                
                Logger.Info("Accessed DB: Checking ModelState Validation");
                if (ModelState.IsValid)
                {
                    db.Entry(Department).State = EntityState.Modified;

                    if (OldValues.Length > 0)
                    {
                        tblDepartmentLog log = new tblDepartmentLog();
                        log.LogDepartmentUID = Department.DepartmentID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified";
                        log.ChangedFrom = OldValues;
                        log.ChangedTo = NewValues;
                        log.DateandTime = DateTime.Now.ToString();
                        db.tblDepartmentLogs.Add(log);
                    }
                    db.SaveChanges();
                    Logger.Info("Accesed DB, Category Details Saved");
                    TempData["categorystatus"] = "CategorySuccess";
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'SaveRecord' Action HTTP POST Main exception");
            }
            
            Logger.Info("Redirecting to Department");
            return RedirectToAction("Department");
        }

        [HttpPost]
        public JsonResult EditSubCategory(string SubCategoryID)
        {
            Logger.Info("Attempt DepartmentMaster EditCategory");
            try
            {
                int IDName = Convert.ToInt32(SubCategoryID);
                Logger.Info("Accessing DB for Udating the SubCategory Record ");
                var result = from tblSubDepartment in db.tblSubDepartments.Where(x => x.SubDepartmentID == IDName) select tblSubDepartment;

                foreach (var r in result)
                {

                    return Json(r);
                }
                Logger.Info("Accessed DB, SubCategory Record Updated");
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'EditSubCategory' Action HTTP POST Main exception");
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult SaveRecordSubCategory(tblSubDepartment model)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }

            Logger.Info("Attempt DepartmentMaster SaveRecordSubCategory");
            try
            {
                Logger.Info("Accessing DB for Saving SubCategory Records");
                //tblSubDepartment SubCategory = new tblSubDepartment();
                //SubCategory.SubCategoryName = model.SubCategoryName;
                //SubCategory.CategoryName = model.CategoryName;
                tblSubDepartment SubDepartment = db.tblSubDepartments.Find(model.SubDepartmentID);

                string OldValues = "";
                string NewValues = "";
                if (SubDepartment.PlantName != model.PlantName)
                {
                    OldValues = OldValues + "Plant Name : " + SubDepartment.PlantName + " , ";
                    NewValues = NewValues + "Plant Name : " + model.PlantName + " , ";
                }
                if (SubDepartment.DepartmentName != model.DepartmentName)
                {
                    OldValues = OldValues + "Department Name : " + SubDepartment.DepartmentName + " , ";
                    NewValues = NewValues + "Department Name : " + model.DepartmentName + " , ";
                }
                if (SubDepartment.SubDepartmentName != model.SubDepartmentName)
                {
                    OldValues = OldValues + "Sub Department Name : " + SubDepartment.SubDepartmentName + " , ";
                    NewValues = NewValues + "Sub Department Name : " + model.SubDepartmentName + " , ";
                }

                SubDepartment.PlantName = HttpUtility.HtmlEncode(model.PlantName);
                SubDepartment.DepartmentName = HttpUtility.HtmlEncode(model.DepartmentName);
                SubDepartment.SubDepartmentName = HttpUtility.HtmlEncode(model.SubDepartmentName);

                // model.SubDepartmentID = model.SubDepartmentID;
                SubDepartment.LastModified = DateTime.Now;
                Logger.Info("Checking model State validation");
                if (ModelState.IsValid)
                {
                    db.Entry(SubDepartment).State = EntityState.Modified;
                    if (OldValues.Length > 0)
                    {
                        Logger.Info("Accessing DB for Saving the SubDepartment Details ");
                        tblSubDepartmentLog log = new tblSubDepartmentLog();
                        log.LogSubDepartmentUID = SubDepartment.SubDepartmentID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified";
                        log.ChangedFrom = OldValues;
                        log.ChangedTo = NewValues;
                        log.DateandTime = DateTime.Now.ToString();
                        db.tblSubDepartmentLogs.Add(log);
                    }
                    db.SaveChanges();
                    Logger.Info("Accessed DB, Subdepartment Details Saved");
                    TempData["subcategorystatus"] = "SubCategorySuccess";
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'SaveRecordSubCategory' Action HTTP POST Main exception");
            }
           
            Logger.Info("Redirecting to SubDepartment");
            return RedirectToAction("SubDepartment");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult DeleteConfirmedSubCategory(int SubCategoryID, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt DepartmentMaster DeleteConfirmedSubCategory");
            try
            {
                Logger.Info("Accessing DB for Deleting the SubCategory List ");
                tblSubDepartment tblSubDepartment = db.tblSubDepartments.Find(SubCategoryID);
                db.tblSubDepartments.Remove(tblSubDepartment);
                db.SaveChanges();
                Logger.Info("Accesed DB, SubCategory List Deleted");

                Logger.Info("Accessing DB for Saving the SubCategory Log Details");
                tblSubCategoryLog log = new tblSubCategoryLog
                {
                    LogSubCategoryUID = SubCategoryID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Deleted",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblSubCategoryLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accesed DB, SubCategory Log Details Saved");

                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'DeleteConfirmedSubCategory' Action HTTP POST Main exception");
                //MessageBox.Show(ex.ToString());
                return Json(Ex.Message);
            }

        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult DeleteConfirmedCategory(int CategoryID, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt DepartmentMaster DeleteConfirmedCategory");
            try
            {
                Logger.Info("Accessing DB for Deleting the Category List ");
                tblDepartment tblDepartment = db.tblDepartments.Find(CategoryID);
                db.tblDepartments.Remove(tblDepartment);

                db.SaveChanges();
                Logger.Info("Accesed DB, Category Details Deleted");

                Logger.Info("Accessing DB for Saving Category Log Details");
                tblCategoryLog log = new tblCategoryLog
                {
                    LogCategoryUID = CategoryID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Deleted",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblCategoryLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accesed DB, Category Log Details Saved");

                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'DeleteConfirmedCategory' Action HTTP POST Main exception");
                //MessageBox.Show(ex.ToString());
                return Json(Ex.Message);
            }

        }

        [HttpPost]
        public ActionResult getLogDetail(int ID)
        {
            Logger.Info("Attempt DepartmentMaster getLogDetail");
            try
            {
                Logger.Info("Accessing DB for DepartmentLog Details : DepartmentLogID Match");
                var result = from tblDepartmentLog in db.tblDepartmentLogs.Where(x => x.LogDepartmentUID == ID) select tblDepartmentLog;
                Logger.Info("Accessed DB, Checking DepartmentLog Deatils : DepartmentLog Details Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'getLogDetail' Action HTTP POST Main exception");
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }


        [HttpPost]
        public ActionResult SaveLog(string details, int ID, string initialvalue, string UserID)
        {

            Logger.Info("Attempt DepartmentMaster SaveLog");
            try
            {
                Logger.Info("Accessing DB for saving CategoryLog Details");
                tblCategoryLog log = new tblCategoryLog
                {
                    LogCategoryUID = ID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Modified",
                    ChangedFrom = initialvalue,
                    ChangedTo = details,
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblCategoryLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, CategoryLog Details Saved");

                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'SaveLog' Action HTTP POST Main exception");
                return Json(Ex.Message);
            }
        }

        [HttpPost]
        public ActionResult getLog2Detail(int ID)
        {
            Logger.Info("Attempt DepartmentMaster getLog2Detail");
            try
            {
                Logger.Info("Accessing DB for SubDepartmentLog Details: SubDepartmentLogID Match");
                var result = from tblSubDepartmentLog in db.tblSubDepartmentLogs.Where(x => x.LogSubDepartmentUID == ID) select tblSubDepartmentLog;
                Logger.Info("Accessed DB, Checked SubDepartmentLog : SubDepartmentLog Details Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'getLog2Detail' Action HTTP POST Main exception");
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult SaveLog2(string details, int ID, string initialvalue, string UserID)
        {
            Logger.Info("Attempt DepartmentMaster SaveLog2");
            try
            {
                Logger.Info("Accessing DB for saving SubCategoryLog Details");
                tblSubCategoryLog log = new tblSubCategoryLog
                {
                    LogSubCategoryUID = ID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Modified",
                    ChangedFrom = initialvalue,
                    ChangedTo = details,
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblSubCategoryLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, SubCategory Log Details Saved");
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'SaveLog2' Action HTTP POST Main exception");
                return Json(Ex.Message);
            }
        }
        [HttpPost]

        public ActionResult DepartmentNameVerification(string Plant, string Department)
        {
            Logger.Info("Attempt DepartmentMaster DepartmentNameVerification");
            Department = Department.Trim();
            Plant = Plant.Trim();
            try
            {
                Logger.Info("Accessed DB, Checking Department Details: Department match");
                var result = from tblDepartment in db.tblDepartments.Where(x => x.PlantName == Plant).Where(x => x.DepartmentName == Department) select tblDepartment;
                string[] name = new string[2];
                foreach (var r in result)
                {
                    name[0] = r.PlantName;
                    name[1] = r.DepartmentName;
                }

                Logger.Info("Accessed DB, Checking Department Details: Department Found");
                
                return Json(name);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'DepartmentNameVerification' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [HttpPost]

        public ActionResult DepartmentNameVerification2(string Plant, string Department, int ID)
        {
            Logger.Info("Attempt DepartmentMaster DepartmentNameVerification");
            Department = Department.Trim();
            Plant = Plant.Trim();
            try
            {
                Logger.Info("Accessed DB, Checking Department Details: Department match");
                var result = from tblDepartment in db.tblDepartments.Where(x => x.DepartmentID != ID).Where(x => x.DepartmentName == Department).Where(x => x.PlantName == Plant) select tblDepartment;
                string[] name = new string[2];
                foreach(var r in result)
                {
                    name[0] = r.PlantName;
                    name[1] = r.DepartmentName;
                }
               
                Logger.Info("Accessed DB, Checking Department Details: Department Found");

                return Json(name);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'DepartmentNameVerification' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]

        public ActionResult SubDepartmentNameVerification(string Plant, string Department, string SubDepartment)
        {
            Logger.Info("Attempt DepartmentMaster SubDepartmentNameVerification");
            Department = Department.Trim();
            SubDepartment = SubDepartment.Trim();
            try
            {
                Logger.Info("Accessed DB, Checking SubDepartment Details: SubDepartment match");
                var result = from tblSubDepartment in db.tblSubDepartments.Where(x => x.PlantName == Plant).Where(x => x.DepartmentName == Department).Where(x => x.SubDepartmentName == SubDepartment) select tblSubDepartment;
                string[] name = new string[3];
                foreach (var r in result)
                {
                    name[0] = r.DepartmentName;
                    name[1] = r.SubDepartmentName;
                    name[2] = r.PlantName;
                }
                Logger.Info("Accessed DB, Checking SubDepartment Details: SubDepartment Found");
                return Json(name);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'SubDepartmentNameVerification' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [HttpPost]

        public ActionResult SubDepartmentNameVerification2(string Plant, string Department, string SubDepartment, int ID)
        {
            Logger.Info("Attempt DepartmentMaster SubDepartmentNameVerification");
            Department = Department.Trim();
            SubDepartment = SubDepartment.Trim();
            try
            {
                Logger.Info("Accessed DB, Checking SubDepartment Details: SubDepartment match");
                var result = from tblSubDepartment in db.tblSubDepartments.Where(x => x.SubDepartmentID != ID).Where(x => x.PlantName == Plant).Where(x => x.DepartmentName == Department).Where(x => x.SubDepartmentName == SubDepartment) select tblSubDepartment;
                string[] name = new string[3];
                foreach (var r in result)
                {
                    name[0] = r.DepartmentName;
                    name[1] = r.SubDepartmentName;
                    name[2] = r.PlantName;

                }
                Logger.Info("Accessed DB, Checking SubDepartment Details: SubDepartment Found");
                return Json(name);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DepartmentMaster' Controller , 'SubDepartmentNameVerification' Action HTTP POST Main exception");
                return Json("error");
            }
        }
    }
}


