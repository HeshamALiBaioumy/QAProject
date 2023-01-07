using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using QA.Entities.View_Entities;
using QA.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;

namespace QA.Controllers
{
    [Authorize]
    [RoutePrefix("Department")]
    public class DepartmentController : BaseController
    {
        [AuthorizeUser(UserPermission.Department_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_Department()
            {
                department = new Ent_Department() { departmentID = -1, isActive = true},
                lOVprojectOwners = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.projectOwner_IsOwner)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Department_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_Department formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.department.departmentID != -1) ? true : false;
                if (ModelState.IsValid)
                {
                    formControl.department.makerID = User.Identity.Name;
                    response = new Mdl_Department().insert_updateDepartment(formControl.department, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = (isUpdateForm) ?
                            Localization.Department.DepartmentUpdatedSuccessfully
                            : formControl.department.name + ": " + Localization.Department.DepartmentAddedSuccessfully;
                    }
                    else
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage;
                    }

                    return RedirectToAction("Create");
                }
                else
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                    response.comments = null;
                    response.errorMessage = ModelState.ToString();

                    return View();
                }
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return View();
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(formControl.department.makerID, "Department_Create"
                    , formControl.department.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Department_Create)]
        [Route("IsValidDepartment")]
        public JsonResult IsValidDepartmentName(Vw_Department formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.Department
                    , formControl.department.departmentID, formControl.department.name, formControl.department.projectOwnerID);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.Department_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_Department() { lOVprojectOwners = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Project_Owner) });
        }

        [AuthorizeUser(UserPermission.Department_Search)]
        [Route("searchDepartments")]
        public ActionResult searchDepartments(string searchName, string searchDescription
            , int searchprojectOwnerID, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_Department> searchResult = new List<Ent_Department>();

            try
            {
                searchResult = new Mdl_Department().searchDepartment(searchName, searchDescription
                    , searchprojectOwnerID, searchIsActive);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.searchResultsRetrievedSucessfully;

                return PartialView("_searchResults", searchResult);
            }
            catch (Exception ex)
            {
                response.responseStatus = false;
                response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return Json(new { Status = false, Message = Localization.ErrorMessages.UnhandledErrorOccured });
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Department_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Department_View)]
        [Route("View")]
        public ActionResult View(int DepartmentID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_Department department = new Ent_Department();

            try
            {
                department = new Mdl_Department().viewDepartment(DepartmentID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", department);
            }
            catch (Exception ex)
            {
                response.responseStatus = false;
                response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return Json(new { Status = false, Message = Localization.ErrorMessages.UnhandledErrorOccured });
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Department_View", "Depart ID: " + DepartmentID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Department_Edit)]
        [Route("Edit/{depID?}")]
        public ActionResult Edit(int depID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_Department Department = new Ent_Department();
            Vw_Department vw_Department = null;
            try
            {
                Department = new Mdl_Department().viewDepartment(depID);
                if (Department.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.Department.DepartmentID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    vw_Department = new Vw_Department()
                    {
                        department = Department,
                        lOVprojectOwners = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Project_Owner)
                    };
                    return View("Create", vw_Department);
                }

                return View("Create", vw_Department);
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return View("Create");
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Department_View", "dep ID: " + depID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}