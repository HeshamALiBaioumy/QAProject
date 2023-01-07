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
    [RoutePrefix("projectOwnerType")]
    public class ProjectOwnerTypeController : BaseController
    {
        [AuthorizeUser(UserPermission.projectOwnerType_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_ProjectOwnerType() { projectOwnerType = new Ent_ProjectOwnerType()
            { ProjectOwnerTypeID = -1, isActive = true, isVendor = true } });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.projectOwnerType_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_ProjectOwnerType formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.projectOwnerType.ProjectOwnerTypeID == -1) ? true : false;
                if (ModelState.IsValid)
                {
                    formControl.projectOwnerType.makerID = User.Identity.Name;
                    response = new Mdl_ProjectOwnerType().insert_updateProjectOwnerType(formControl.projectOwnerType,isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.projectOwnerType.name
                            + ": " + Localization.PROJECT_OWNER_TYPE.ProjectOwnerTypeAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.projectOwnerType.makerID, "projectOwnerType_Create"
                    , formControl.projectOwnerType.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
        
        [HttpPost]
        [AuthorizeUser(UserPermission.projectOwnerType_Create, UserPermission.projectOwnerType_Edit)]
        [Route("IsValidProjectOwnerTypeName")]
        public JsonResult IsValidProjectOwnerTypeName(Vw_ProjectOwnerType formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.PROJECT_OWNER_TYPE
                    , formControl.projectOwnerType.ProjectOwnerTypeID, formControl.projectOwnerType.name);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.projectOwnerType_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View();
        }

        [AuthorizeUser(UserPermission.projectOwnerType_Search)]
        [Route("searchprojectOwnerType")]
        public ActionResult searchprojectOwnerType(string searchName, string searchDescription
            , int searchIsVendor, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_ProjectOwnerType> searchResult = new List<Ent_ProjectOwnerType>();

            try
            {
                searchResult = new Mdl_ProjectOwnerType().searchProjectOwnerType(searchName, searchDescription
                    , searchIsVendor, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "projectOwnerType_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.projectOwnerType_View)]
        [Route("View")]
        public ActionResult View(int projectOwnerTypeID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_ProjectOwnerType projectOwnerType = new Ent_ProjectOwnerType();

            try
            {
                projectOwnerType = new Mdl_ProjectOwnerType().viewProjectOwnerType(projectOwnerTypeID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", projectOwnerType);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "projectOwnerType_View", "FT ID: " + projectOwnerTypeID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.projectOwnerType_Edit)]
        [Route("Edit/{projectOwnerTypeID?}")]
        public ActionResult Edit(int projectOwnerTypeID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_ProjectOwnerType projectOwnerType = new Ent_ProjectOwnerType();
            Vw_ProjectOwnerType vw_projectOwnerType = null;
            try
            {
                projectOwnerType = new Mdl_ProjectOwnerType().viewProjectOwnerType(projectOwnerTypeID);
                if (projectOwnerType.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.PROJECT_OWNER_TYPE.projectOwnerTypeID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    vw_projectOwnerType = new Vw_ProjectOwnerType() { projectOwnerType = projectOwnerType };
                    return View("Create", vw_projectOwnerType);
                }

                return View("Create", vw_projectOwnerType);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "projectOwnerType_View", "MT ID: " + projectOwnerTypeID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}