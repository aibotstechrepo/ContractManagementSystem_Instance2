using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using ContractManagementSystem.Models;
using NLog;

namespace ContractManagementSystem.Controllers
{
    //[Authorize(Roles = "admin")]
    public class ClauseController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
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
        // GET: Clause
        [Authorize(Roles = "admin")]
        public ActionResult New()
        {
            Logger.Info("Accessing Clause New Page");
            return RedirectToAction("Dashboard", "Home");
        }


        //*********************Integrated 13/3/20(Pooja)******************//


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(tblClauseMaster emp,string editor1, int UserID = 0)
        {
            Logger.Info("Attempt ClauseMaster New");
            string status = "";
            try
            {
                Logger.Info("Accessing DB for Saving the ClauseMaster Records");
                //tblClauseMaster emp = new tblClauseMaster();

                emp.ClauseClauseTitle = HttpUtility.HtmlEncode(emp.ClauseClauseTitle);
                emp.ClauseClauseDescription = HttpUtility.HtmlEncode(emp.ClauseClauseDescription);
                //emp.ClauseClauseTitle = model.ClauseClauseTitle;
                //emp.ClauseClauseDescription = model.ClauseClauseDescription;
                //emp.ClauseClauseType = model.ClauseClauseType;

                emp.ClauseClauseText = editor1;
                emp.ClauseLastModified = DateTime.Now;


                db.tblClauseMasters.Add(emp);
                db.SaveChanges();
                Logger.Info("Accessed DB, ClauseMaster Record Saved");

                Logger.Info("Accessing DB for Saving the ClauseMaster Log Details");
                tblClauseLog log = new tblClauseLog
                {
                    LogClauseUID = emp.ClauseClauseID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Creation",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblClauseLogs.Add(log);
               
                db.SaveChanges();
                Logger.Info("Accessed DB, ClauseMaster Log Details Saved");

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'New' Action HTTP POST Main exception");
                status = Ex.InnerException.Message;
                // MessageBox.Show(ex.ToString());
            }

            //TempData["Success"] = true;
            return View(emp);

        }
        //*******************************************************

        public ActionResult Details()
        {
            return RedirectToAction("Dashboard", "Home");
        }

        [Route("Clause/Details/{id:int}")]
        public ActionResult Details(int id)
        {
            Logger.Info("Accessing DB for ClauseMaster Details");
            tblClauseMaster tblClauseMaster = db.tblClauseMasters.Find(id);

            Logger.Info("Accessed DB, Checking ClauseMaster Details: Checking Status");
            if (tblClauseMaster == null)
            {
                Logger.Info("Accessed DB, Checking ClauseMaster Details: Details Not Found");
                return HttpNotFound();
            }

            Logger.Info("Accessed DB, Checking ClauseMaster Details: Details Found");

            Logger.Info("Redirecting to ClauseMaster Details Page");
            return RedirectToAction("Dashboard", "Home");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ClauseEdit(tblClauseMaster model, int ClauseClauseID, string editor1)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }

            Logger.Info("Attempt ClauseMaster ClauseEdit");
            try
            {
                Logger.Info("Accessing DB for Updating the ClauseMaster Records");
                tblClauseMaster tblClauseMaster = db.tblClauseMasters.Find(ClauseClauseID);
                string OldValues = "";
                string NewValues = "";
                if (tblClauseMaster.ClauseClauseTitle != model.ClauseClauseTitle)
                {
                    OldValues = OldValues + "Clause Title : " + tblClauseMaster.ClauseClauseTitle + " , ";
                    NewValues = NewValues + "Clause Title : " + model.ClauseClauseTitle + " , ";
                }
                if (tblClauseMaster.ClauseClauseDescription != model.ClauseClauseDescription)
                {
                    OldValues = OldValues + "Clause Description : " + tblClauseMaster.ClauseClauseDescription + " , ";
                    NewValues = NewValues + "Clause Description : " + model.ClauseClauseDescription + " , ";
                }
                if (tblClauseMaster.ClauseClauseText != editor1)
                {
                    OldValues = OldValues + "Clause Text : " + tblClauseMaster.ClauseClauseText + " , ";
                    NewValues = NewValues + "Clause Text : " + editor1 + " , ";
                }
                // tblClauseMaster tblClauseMaster = new tblClauseMaster();
                tblClauseMaster.ClauseClauseID = ClauseClauseID;

                tblClauseMaster.ClauseClauseTitle = HttpUtility.HtmlEncode(tblClauseMaster.ClauseClauseTitle);
                tblClauseMaster.ClauseClauseDescription = HttpUtility.HtmlEncode(tblClauseMaster.ClauseClauseDescription);
                //tblClauseMaster.ClauseClauseTitle = tblClauseMaster.ClauseClauseTitle;
                //tblClauseMaster.ClauseClauseDescription = tblClauseMaster.ClauseClauseDescription;
                //tblClauseMaster.ClauseClauseType = ClauseClauseType;

                tblClauseMaster.ClauseClauseText = editor1;
                tblClauseMaster.ClauseLastModified = DateTime.Now;

                Logger.Info("Checking for ClauseMaster ModelState Validation");
                if (ModelState.IsValid)
                {
                   
                    db.Entry(tblClauseMaster).State = EntityState.Modified;
                    Logger.Info("Accessing DB for Saving the ClauseLog Details ");
                    tblClauseLog log = new tblClauseLog();
                    log.LogClauseUID = tblClauseMaster.ClauseClauseID;
                    log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                    log.LogActivity = "Modification";
                    log.ChangedFrom = OldValues;
                    log.ChangedTo = NewValues;
                    log.DateandTime = DateTime.Now.ToString();
                    db.tblClauseLogs.Add(log);
                    db.SaveChanges();
                    Logger.Info("Accessed DB, ClauseMaster Updated Record Saved");

                    Logger.Info("Redirecting to ClauseMaster Repository Page");
                    TempData["status"] = "Success";
                    return RedirectToAction("Dashboard", "Home");
                }
               
                
            }
            catch(Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'ClauseEdit' Action HTTP POST Main exception");
                //status = Ex.InnerException.Message;
            }
            return RedirectToAction("Dashboard", "Home");
        }

        [Authorize(Roles = "admin")]
        // [HttpPost, ActionName("Details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed( int ClauseClauseID,int UserID)
        {
            Logger.Info("Attempt ClauseMaster DeleteConfirmed");
            try
            {
                Logger.Info("Accessing DB for Deleting the ClauseMaster Records");
                tblClauseMaster tblClauseMaster = db.tblClauseMasters.Find(ClauseClauseID);
                db.tblClauseMasters.Remove(tblClauseMaster);
                db.SaveChanges();
                Logger.Info("Accessed DB, ClauseMaster Record Deleted");

                Logger.Info("Accessing DB for Saving the ClauseMaster Log Details");
                tblClauseLog log = new tblClauseLog
                {
                    LogClauseUID = ClauseClauseID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Deletion",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblClauseLogs.Add(log);
               
                db.SaveChanges();
                Logger.Info("Accessed DB, ClauseMaster Log Record Saved");


            }
            catch(Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'DeleteConfirmed' Action HTTP POST Main exception");
            }
            TempData["deletestatus"] = "DeleteSuccess";
            Logger.Info("Redirecting to ClauseMaster Repository Page");
            return RedirectToAction("Dashboard", "Home");

        }
        public ActionResult Repository()
        {
            if (TempData["status"] != null)
            {
                ViewBag.Status = "Success";
                TempData.Remove("status");
            }
            if (TempData["deletestatus"] != null)
            {
                ViewBag.Status = "DeleteSuccess";
                TempData.Remove("deletestatus");
            }
            Logger.Info("Accessing ClauseMaster Repository Page");
            Logger.Info("Accessing DB for Repository");

            List<tblClauseMaster> ClauseMaster = db.tblClauseMasters.ToList();
            ClauseMaster.Reverse();
            return RedirectToAction("Dashboard", "Home");

        }

        public ActionResult Index()
        {
            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public JsonResult category_list()
        {
            Logger.Info("Attempt ClauseMaster category_list");
            try
            {

                Logger.Info("Accessing DB for Category List");
                var result = from tblCategory in db.tblCategories select tblCategory.CategoryName;
                Logger.Info("Accessed DB, Checking Category List: Category List Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'category_list' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult subcategory_list(string category_id)
        {
            Logger.Info("Attempt ClauseMaster subcategory_list");
            try
            {
                Logger.Info("Accessing DB for SubCategory List");
                var result = from tblSubCategory in db.tblSubCategories.Where(x => x.CategoryName == category_id) select tblSubCategory.SubCategoryName;
                foreach (var r in result)
                {
                    Console.WriteLine(r);
                }
                Logger.Info("Accessed DB, Checking SubCategory List: SubCategory List Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'subcategory_list' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveLog(string details, int ID, string initialvalue, int UserID = 0)
        {
            Logger.Info("Attempt ClauseMaster SaveLog");
            try
            {
                Logger.Info("Accessing DB for Saving the ClauseMaster Log Details");
                tblClauseLog log = new tblClauseLog
                {
                    LogClauseUID = ID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Modification",
                    ChangedFrom = initialvalue,
                    ChangedTo = details,
                    DateandTime = DateTime.Now.ToString()
                };
                db.tblClauseLogs.Add(log);
                db.SaveChanges();
                Logger.Info("Accessed DB, ClauseMaster Log Record Saved");


                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'SaveLog' Action HTTP POST Main exception");
                return Json(Ex.Message);
            }
        }

        [HttpPost]
        public ActionResult getLogDetail(int ID)
        {
            Logger.Info("Attempt ClauseMaster getLogDetail");

            try
            {
                Logger.Info("Accessed DB, Checking ClauseMaster Log Details: LogID match");
                var result = from tblClauseLog in db.tblClauseLogs.Where(x => x.LogClauseUID == ID) select tblClauseLog;
                Logger.Info("Accessed DB, Checking ClauseMaster Log Details: LogDetails Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'getLogDetail' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        

    }
}