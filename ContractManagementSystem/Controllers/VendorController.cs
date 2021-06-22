using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using ContractManagementSystem.Models;
using NLog;

namespace ContractManagementSystem.Controllers
{

    //[Authorize(Roles = "admin")]
    public class VendorController : Controller
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

        // GET: Vendor
        [Authorize(Roles = "admin")]
        public ActionResult New()
        {
            Logger.Info("Accessing Vendor New Page");
            return View();
        }

        //*****************************Integrated (Pooja) on 14/3/20***************
        [HttpPost]
        public ActionResult New(tblVendorMaster model,int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor New");
            try
            {
                Logger.Info("Accessing DB for Saving VendorMaster Details");
                tblVendorMaster vendor = new tblVendorMaster();
                vendor.VendorVendorName = model.VendorVendorName;
                vendor.VendorTypeofEntity = model.VendorTypeofEntity;
                vendor.VendorCorporateIdentificationNumber = model.VendorCorporateIdentificationNumber;
                vendor.VendorRegisteredAddress = model.VendorRegisteredAddress;
                vendor.VendorAuthorisedSignatory = model.VendorAuthorisedSignatory;
                vendor.VendorBranchOffice1 = model.VendorBranchOffice1;
                vendor.VendorBranchOffice2 = model.VendorBranchOffice2;
                vendor.VendorBranchOffice3 = model.VendorBranchOffice3;
                vendor.VendorBranchOffice4 = model.VendorBranchOffice4;
                vendor.VendorBranchOffice5 = model.VendorBranchOffice5;
                db.tblVendorMasters.Add(vendor);
                db.SaveChanges();
                Logger.Info("Accessed DB, VendorMaster Details Saved");

                Logger.Info("Accessing DB for Saving VendorLog Details");
                tblVendorLog log = new tblVendorLog();
                log.LogVendorUID = vendor.VendorVendorID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Created";
                log.ChangedFrom = "-";
                log.ChangedTo = "-";
                log.DateandTime = DateTime.Now.ToString();

                db.tblVendorLogs.Add(log);
               
                db.SaveChanges();
                Logger.Info("Accesed DB, VendorLog Details Saved");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'New' Action HTTP POST Main exception");
                throw Ex;
            }
            return View(model);
        }
        //**************************************************************

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult VendorEdit(tblVendorMaster model,int VendorVendorID)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor VendorEdit");
            try
            {
                Logger.Info("Accessing DB for Updating the VendorMaster Details ");
              tblVendorMaster vendor = db.tblVendorMasters.Find(VendorVendorID);
                string OldValues = "";
                string NewValues = "";
                if (vendor.VendorTypeofEntity != model.VendorTypeofEntity)
                {
                    OldValues = OldValues + "Type Of Entity : " + vendor.VendorTypeofEntity + " , ";
                    NewValues = NewValues + "Type Of Entity : " + model.VendorTypeofEntity + " , ";
                }
                if (vendor.VendorVendorName != model.VendorVendorName)
                {
                    OldValues = OldValues + "Vendor Name : " + vendor.VendorVendorName + " , ";
                    NewValues = NewValues + "Vendor Name : " + model.VendorVendorName + " , ";
                }
                if (vendor.VendorCorporateIdentificationNumber != model.VendorCorporateIdentificationNumber)
                {
                    OldValues = OldValues + "GSTN : " + vendor.VendorCorporateIdentificationNumber + " , ";
                    NewValues = NewValues + "GSTN : " + model.VendorCorporateIdentificationNumber + " , ";
                }
                if (vendor.VendorAuthorisedSignatory != model.VendorAuthorisedSignatory)
                {
                    OldValues = OldValues + "Authorised Signatory : " + vendor.VendorAuthorisedSignatory + " , ";
                    NewValues = NewValues + "Authorised Signatory : " + model.VendorAuthorisedSignatory + " , ";
                }
                if (vendor.VendorRegisteredAddress != model.VendorRegisteredAddress)
                {
                    OldValues = OldValues + "Registered Address : " + vendor.VendorRegisteredAddress + " , ";
                    NewValues = NewValues + "Registered Address : " + model.VendorRegisteredAddress + " , ";
                }
                if (vendor.VendorBranchOffice1 != model.VendorBranchOffice1)
                {
                    OldValues = OldValues + "Branch 1 Address : " + vendor.VendorBranchOffice1 + " , ";
                    NewValues = NewValues + "Branch 1 Address : " + model.VendorBranchOffice1 + " , ";
                }
                if (vendor.VendorBranchOffice2 != model.VendorBranchOffice2)
                {
                    OldValues = OldValues + "Branch 2 Address : " + vendor.VendorBranchOffice2 + " , ";
                    NewValues = NewValues + "Branch 2 Address : " + model.VendorBranchOffice2 + " , ";
                }
                if (vendor.VendorBranchOffice3 != model.VendorBranchOffice3)
                {
                    OldValues = OldValues + "Branch 3 Address : " + vendor.VendorBranchOffice3 + " , ";
                    NewValues = NewValues + "Branch 3 Address : " + model.VendorBranchOffice3 + " , ";
                }
                if (vendor.VendorBranchOffice4 != model.VendorBranchOffice4)
                {
                    OldValues = OldValues + "Branch 4 Address : " + vendor.VendorBranchOffice4 + " , ";
                    NewValues = NewValues + "Branch 4 Address : " + model.VendorBranchOffice4 + " , ";
                }
                if (vendor.VendorBranchOffice5 != model.VendorBranchOffice5)
                {
                    OldValues = OldValues + "Branch 5 Address : " + vendor.VendorBranchOffice5 + " , ";
                    NewValues = NewValues + "Branch 5 Address : " + model.VendorBranchOffice5 + " , ";
                }


                //vendor.VendorVendorID = VendorVendorID;
                vendor.VendorVendorName = model.VendorVendorName;
            vendor.VendorTypeofEntity = model.VendorTypeofEntity;
            vendor.VendorCorporateIdentificationNumber = model.VendorCorporateIdentificationNumber;
            vendor.VendorRegisteredAddress = model.VendorRegisteredAddress;
            vendor.VendorAuthorisedSignatory = model.VendorAuthorisedSignatory;
            vendor.VendorBranchOffice1 = model.VendorBranchOffice1;
            vendor.VendorBranchOffice2 = model.VendorBranchOffice2;
            vendor.VendorBranchOffice3 = model.VendorBranchOffice3;
            vendor.VendorBranchOffice4 = model.VendorBranchOffice4;
            vendor.VendorBranchOffice5 = model.VendorBranchOffice5;
                Logger.Info("Checking model state validation");
                if (ModelState.IsValid)
            {
               
                db.Entry(vendor).State = EntityState.Modified;

                    if (OldValues.Length > 0)
                    {
                        Logger.Info("Accessing DB for Saving the VendorLog Details ");
                        tblVendorLog log = new tblVendorLog();
                        log.LogVendorUID = vendor.VendorVendorID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified";
                        log.ChangedFrom = OldValues;
                        log.ChangedTo = NewValues;
                        log.DateandTime = DateTime.Now.ToString();

                        db.tblVendorLogs.Add(log);
                    }

                    db.SaveChanges();
                    Logger.Info("Accesed DB, VendorMaster Details Updated");
                    Logger.Info("Redirecting to Repository");
                    TempData["status"] = "Success";
                    return RedirectToAction("Repository");
                    
            }
            
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'VendorEdit' Action HTTP POST Main exception");
               
            }
            return View();
        }


        // GET: tblVendorMasters/Details/5
        public ActionResult Details()
        {
            return RedirectToAction("Repository");
        }

        [Route("Vendor/Details/{id:int}")]
        public ActionResult Details(int id)
        {
            Logger.Info("Attempt Vendor Details");

            Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
            tblVendorMaster tblVendorMaster = db.tblVendorMasters.Find(id);
            if (tblVendorMaster == null)
            {
                Logger.Info("Accesed DB, Checking VendorMaster Details: Details not Found");
                return HttpNotFound();
            }
            Logger.Info("Accessed DB, checking  VendorMaster Details: Details Found");

            Logger.Info("Redirecting to VendorMaster Details Page");
            return View(tblVendorMaster);
        }


        [Authorize(Roles = "admin")]
        //[HttpPost, ActionName("Details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int VendorVendorID, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor DeleteConfirmed");
            try
            {
                Logger.Info("Accessing DB for Deleting the VendorMaster Details ");

                tblVendorMaster tblVendorMaster = db.tblVendorMasters.Find(VendorVendorID);
            db.tblVendorMasters.Remove(tblVendorMaster);
            Logger.Info("Accesed DB, Checking User Details: User Saved");
            db.SaveChanges();

                Logger.Info("Accesed DB, VendorMaster Record Deleted");

                Logger.Info("Accessing DB for Saving the VendorLog Details ");
                tblVendorLog log = new tblVendorLog();
            log.LogVendorUID = VendorVendorID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Deleted";
            log.ChangedFrom = "-";
            log.ChangedTo = "-";
            log.DateandTime = DateTime.Now.ToString();

            db.tblVendorLogs.Add(log);
           
            db.SaveChanges();
                Logger.Info("Accesed DB, VendorLog Records Saved");

                
                
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'DeleteConfirmed' Action HTTP POST Main exception");
                //throw Ex;
            }
            TempData["deletestatus"] = "DeleteSuccess";
            Logger.Info("Redirecting to Repository");
            return RedirectToAction("Repository");
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
            Logger.Info("Attempt Vendor Repository");
            Logger.Info("Accessing DB for Vendor Repository");

            List<tblVendorMaster> VendorList = db.tblVendorMasters.ToList();
            VendorList.Reverse();
            return View(VendorList);

        }

        public ActionResult Index()
        {
            return RedirectToAction("Repository");
        }

        [HttpPost]
        public JsonResult entity_list()
        {
            Logger.Info("Attempt Vendor entity_list");
            try
            {
                Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                var result = from tblVendorMaster in db.tblVendorMasters select tblVendorMaster.VendorTypeofEntity;
                result = result.Distinct();
                Logger.Info("Accessed DB, Checked VendorMaster Details:Type Details Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'entity_list' Action HTTP POST Main exception");
                throw Ex;
            }
        }

        [HttpPost]
        public ActionResult getLogDetail(int ID)
        {
            Logger.Info("Attempt Vendor getLogDetail");
            try
            {
                Logger.Info("Accessing DB for VendorLog Details: VendorLogID Match");
                var result = from tblVendorLog in db.tblVendorLogs.Where(x => x.LogVendorUID == ID) select tblVendorLog;
                Logger.Info("Accessed DB, Checked VendorLog Details : Details  Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'getLogDetail' Action HTTP POST Main exception");
                throw Ex;
            }
        }

        [HttpPost]
        public ActionResult SaveLog(string details, int ID, string initialvalue, string UserID)
        {
            Logger.Info("Attempt Vendor SaveLog");
            try
            {
                Logger.Info("Accessing DB for saving VendorLog Details ");
                tblVendorLog log = new tblVendorLog();

                log.LogVendorUID = ID;
                log.ModifiedBy = UserID.ToString();
                log.LogActivity = "Modified";
                log.ChangedFrom = initialvalue;
                log.ChangedTo = details;
                log.DateandTime = DateTime.Now.ToString();

                db.tblVendorLogs.Add(log);
                
                db.SaveChanges();
                Logger.Info("Accesed DB, VendorLog  Details Saved");

                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'SaveLog' Action HTTP POST Main exception");
                return Json(Ex.Message);
            }
        }

        [HttpPost]
        public JsonResult entitylist()
        {
            Logger.Info("Attempt Vendor entitylist");
            try
            {
                Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                var result = from tblVendorMaster in db.tblVendorMasters select tblVendorMaster.VendorTypeofEntity;
                result = result.Distinct();
                Logger.Info("Accessed DB, Checked VendorMaster Details:Vendor Type Details Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'entitylist' Action HTTP POST Main exception");
                throw Ex;
            }
        }
    }
}