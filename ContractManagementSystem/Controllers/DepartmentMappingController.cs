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
    public class DepartmentMappingController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        // GET: Mapping
        public ActionResult Index()
        {
            Logger.Info("Accessing Mapping ApprovalMatrix Page");
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
        public JsonResult GetDepartment()
        {
            Logger.Info("Attempt Mapping GetDepartment");
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
                Logger.Error(Ex, "'Mapping' Controller , 'GetDepartment' Action HTTP POST Main exception");
                return Json("error");
            }
        }


        [HttpPost]
        public JsonResult GetSubDepartment(string department)
        {
            Logger.Info("Attempt Mapping GetSubDepartment");
            try
            {
                Logger.Info("Accessing DB for SubDepartment List");

                var result = from tblSubDepartment in db.tblSubDepartments.Where(x => x.DepartmentName == department) select tblSubDepartment.SubDepartmentName;
                //foreach (var r in result)
                //{
                //    Console.WriteLine(r);
                //}
                Logger.Info("Accessed DB, Checking SubDepartment List: SubDepartment Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Mapping' Controller , 'GetSubDepartment' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult SaveDepartmentMappingList(int EmployeeID, string[] arrDepartment, string[] arrSubDepartment)

        {
            //int CurrentUserID = 0;
            //string CurrentUserName = "";
            //try
            //{
            //    CurrentUserID = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
            //    CurrentUserName = User.Identity.Name.Split('|')[0];
            //}
            //catch { }
            //string LogChangeFrom = "";
            //string LogChangeTo = "";
            Logger.Info("Attempt Mapping SaveDepartmentMappingList");
            try
            {
                try
                {
                    Logger.Info("Accessing DB for DepartmentMapping Details");
                    var Data = /*from tblDepartmentMapping in*/ db.tblDepartmentMappings.Where(x => x.EmployeeID == EmployeeID).Select(x => new { x.ID }); //select tblDepartmentMapping;


                    Logger.Info("Accessed DB, Checking Contract Variable Details: Details Found");
                    if (Data.ToList().Count > 0)
                    {

                        foreach (var item in Data)
                        {
                            var Id = item.ID;

                            tblDepartmentMapping eachData = db.tblDepartmentMappings.Find(Id);
                            db.tblDepartmentMappings.Remove(eachData);
                            //db.SaveChanges();


                        }
                        db.SaveChanges();
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Error(Ex, "'Mapping' Controller , 'SaveDepartmentMappingList' Action HTTP POST Main exception");
                    return Json("error");
                }

                for (int i = 0; i < arrDepartment.Length; i++)
                {
                    Logger.Info("Attempt Mapping DepartmentMapping");
                    try
                    {
                       Logger.Info("Accessing DB for Saving the DepartmentMapping Details");
                        tblDepartmentMapping newData = new tblDepartmentMapping
                        {
                           EmployeeID = EmployeeID,
                           Department = arrDepartment[i],
                           SubDepartment = arrSubDepartment[i],
                        };

                        db.tblDepartmentMappings.Add(newData);
                       
                    }

                    catch { }
                    db.SaveChanges();
                }
                

                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Mapping' Controller , 'SaveDepartmentMappingList' Action HTTP POST Main exception");
                return Json("error");
            }
        }

        [HttpPost] 
        public JsonResult DepartmentMappingExistCheck(int EmployeeID)
        {
            Logger.Info("Attempt Mapping DepartmentMappingExistCheck");
            try
            {
                Logger.Info("Accessing DB for Mapping Details");

                var result = /*from tblDepartmentMapping in*/ db.tblDepartmentMappings.Where(x => x.EmployeeID == EmployeeID).Select(x => new { x.Department, x.SubDepartment }); //select tblDepartmentMapping;
               
                Logger.Info("Accessed DB, Checking Mapping List: Mapping Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Mapping' Controller , 'DepartmentMappingExistCheck' Action HTTP POST Main exception");
                return Json("error");
            }
        }


    }
}