using ContractManagementSystem.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ContractManagementSystem.Controllers
{
    public class PlantMasterController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        // GET: Plant

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

        public ActionResult Plant()
        {
            if (TempData["newstatus"] != null)
            {
                ViewBag.Status = "NewSuccess";
                TempData.Remove("newstatus");
            }
            if (TempData["Plantstatus"] != null)
            {
                ViewBag.Status = "PlantSuccess";
                TempData.Remove("Plantstatus");
            }
            Logger.Info("Attempt PlantMaster Plant");
            Logger.Info("Accessing DB for Plant Page");
            return View(db.tblPlants.ToList());
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult SaveRecord1(tblPlant model, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt PlantMaster SaveRecord1");
            try
            {

                Logger.Info("Accessing DB for saving the Plant Records");
                //tblPlant Plant = new tblPlant();
                //Plant.PlantName = model.PlantName;
                model.PlantName = HttpUtility.HtmlEncode(model.PlantName);

                model.LastModified = DateTime.Now;
                db.tblPlants.Add(model);
                db.SaveChanges();
                Logger.Info("Accessed DB, Plant Details Saved");
                TempData["newstatus"] = "NewSuccess";

                Logger.Info("Accessing DB for Saving the PlantLog Details");

                tblPlantLog log = new tblPlantLog
                {
                    LogPlantUID = model.PlantID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Created",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblPlantLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, Plant Log Details Saved");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'PlantMaster' Controller , 'SaveRecord' Action HTTP POST Main exception");
            }

            Logger.Info("Setting Auth Cookie and Redirecting to Plant");
            return RedirectToAction("Plant");
        }


        [HttpPost]

        public ActionResult PlantNameVerification(string Plant)
        {
            Logger.Info("Attempt PlantMaster PlantNameVerification");
            Plant = Plant.Trim();
            try
            {
                Logger.Info("Accessed DB, Checking Plant Details: Plant match");
                var result = from tblPlant in db.tblPlants.Where(x => x.PlantName == Plant) select tblPlant.PlantName;
                Logger.Info("Accessed DB, Checking Plant Details: Plant Found");

                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'PlantMaster' Controller , 'PlantNameVerification' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult SaveRecordPlant(tblPlant model)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }

            Logger.Info("Attempt PlantMaster SaveRecordPlant");

            try
            {
                Logger.Info("Accessing DB for saving Plant Records");
                tblPlant Plant = db.tblPlants.Find(model.PlantID);

                string OldValues = "";
                string NewValues = "";
                if (Plant.PlantName != model.PlantName)
                {
                    OldValues = OldValues + "Plant Name : " + Plant.PlantName + " , ";
                    NewValues = NewValues + "Plant Name : " + model.PlantName + " , ";
                }
                //Category.CategoryName = model.CategoryName;
                Plant.PlantName = HttpUtility.HtmlEncode(model.PlantName);


                //PlantID = model.PlantID,
                Plant.LastModified = DateTime.Now;

                Logger.Info("Accessed DB: Checking ModelState Validation");
                if (ModelState.IsValid)
                {
                    db.Entry(Plant).State = EntityState.Modified;

                    if (OldValues.Length > 0)
                    {
                        tblPlantLog log = new tblPlantLog();
                        log.LogPlantUID = Plant.PlantID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified";
                        log.ChangedFrom = OldValues;
                        log.ChangedTo = NewValues;
                        log.DateandTime = DateTime.Now.ToString();
                        db.tblPlantLogs.Add(log);
                    }
                    db.SaveChanges();
                    Logger.Info("Accesed DB, Plant Details Saved");
                    TempData["Plantstatus"] = "PlantSuccess";
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'PlantMaster' Controller , 'SaveRecord' Action HTTP POST Main exception");
            }

            Logger.Info("Redirecting to Plant");
            return RedirectToAction("Plant");
        }

        [HttpPost]

        public ActionResult PlantNameVerification2(string Plant, int ID)
        {
            Logger.Info("Attempt PlantMaster PlantNameVerification");
            Plant = Plant.Trim();
            try
            {
                Logger.Info("Accessed DB, Checking Plant Details: Plant match");
                var result = from tblPlant in db.tblPlants.Where(x => x.PlantID != ID).Where(x => x.PlantName == Plant) select tblPlant.PlantName;
                Logger.Info("Accessed DB, Checking Plant Details: Plant Found");

                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'PlantMaster' Controller , 'PlantNameVerification' Action HTTP POST Main exception");
                return Json("error");
            }
        }



        [HttpPost]
        public JsonResult EditPlant(string PlantID)
        {
            Logger.Info("Attempt PlantMaster EditPlant");
            try
            {

                int IDName = Convert.ToInt32(PlantID);
                Logger.Info("Accessing DB for Updating the Plant List");
                var result = db.tblPlants.Where(x => x.PlantID == IDName).Select(x => new { x.PlantID, x.PlantName });

                /* select tblPlant;*/
                foreach (var r in result)
                {
                    Logger.Info("Accesed DB, Plant Updating List Saved");
                    return Json(r);
                }
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'PlantMaster' Controller , 'EditPlant' Action HTTP POST Main exception");
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult getLogDetail(int ID)
        {
            Logger.Info("Attempt PlantMaster getLogDetail");
            try
            {
                Logger.Info("Accessing DB for PlantLog Details : PlantLogID Match");
                var result = from tblPlantLog in db.tblPlantLogs.Where(x => x.LogPlantUID == ID) select tblPlantLog;
                Logger.Info("Accessed DB, Checking PlantLog Deatils : PlantLog Details Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'PlantMaster' Controller , 'getLogDetail' Action HTTP POST Main exception");
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult DeleteConfirmedPlant(int PlantID, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt PlantMaster DeleteConfirmedPlant");
            try
            {
                Logger.Info("Accessing DB for Deleting the Plant List ");
                tblPlant tblPlant = db.tblPlants.Find(PlantID);
                db.tblPlants.Remove(tblPlant);

                db.SaveChanges();
                Logger.Info("Accesed DB, Plant Details Deleted");

                Logger.Info("Accessing DB for Saving Plant Log Details");
                tblPlantLog log = new tblPlantLog
                {
                    LogPlantUID = PlantID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Deleted",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblPlantLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accesed DB, Plant Log Details Saved");

                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'PlantMaster' Controller , 'DeleteConfirmedPlant' Action HTTP POST Main exception");
                //MessageBox.Show(ex.ToString());
                return Json(Ex.Message);
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
                Logger.Error(Ex, "'PlantMaster' Controller , 'PlantList' Action HTTP POST Main exception");
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

    }
}