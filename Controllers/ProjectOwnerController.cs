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
    [RoutePrefix("ProjectOwner")]
    public class ProjectOwnerController : BaseController
    {
        [AuthorizeUser(UserPermission.ProjectOwner_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_ProjectOwner()
            {
                projectOwner = new Ent_ProjectOwner()
                {
                    projectOwnerID = -1,
                    isActive = true,
                    isOwner = true,
                    contactDetails = new Ent_ContactDetails()
                }
                ,
                lOVProjectOwnerType = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.ProjectOwnerType)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.ProjectOwner_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_ProjectOwner formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.projectOwner.projectOwnerID != -1) ? true : false;
                if (ModelState.IsValid)
                {
                    formControl.projectOwner.makerID = User.Identity.Name;
                    response = new Mdl_ProjectOwner().insert_updateProjectOwner(formControl.projectOwner, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.projectOwner.name
                            + ": " + Localization.ProjectOwner.ProjectOwnerAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.projectOwner.makerID, "ProjectOwner_Create"
                    , formControl.projectOwner.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.ProjectOwner_Create)]
        [Route("IsValidProjectOwner")]
        public JsonResult IsValidProjectOwner(Vw_ProjectOwner formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.Project_Owner
                    , formControl.projectOwner.projectOwnerID, formControl.projectOwner.name
                    , formControl.projectOwner.pOTID);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.ProjectOwner_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_ProjectOwner()
            {
                lOVProjectOwnerType = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.ProjectOwnerType)

            });
        }

        [AuthorizeUser(UserPermission.ProjectOwner_Search)]
        [Route("searchProjectOwner")]
        public ActionResult searchProjectOwner(string searchName, string searchDescription
            , int searchPOTID, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_ProjectOwner> searchResult = new List<Ent_ProjectOwner>();

            try
            {
                searchResult = new Mdl_ProjectOwner().searchProjectOwner(searchName, searchDescription
                    , searchPOTID, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "ProjectOwner_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.ProjectOwner_View)]
        [Route("View")]
        public ActionResult View(int ProjectOwnerID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_ProjectOwner ProjectOwner = new Ent_ProjectOwner();

            try
            {
                ProjectOwner = new Mdl_ProjectOwner().viewProjectOwner(ProjectOwnerID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", ProjectOwner);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "ProjectOwner_View", "ProjectOwnerID: " + ProjectOwnerID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.ProjectOwner_Edit)]
        [Route("Edit/{POID?}")]
        public ActionResult Edit(int POID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_ProjectOwner ProjectOwner = new Ent_ProjectOwner();
            Vw_ProjectOwner vw_ProjectOwner = null;
            try
            {
                ProjectOwner = new Mdl_ProjectOwner().viewProjectOwner(POID);
                if (ProjectOwner.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.ProjectOwner.projectOwnerID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    vw_ProjectOwner = new Vw_ProjectOwner()
                    {
                        projectOwner = ProjectOwner,
                        lOVProjectOwnerType = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.ProjectOwnerType)
                    };
                    return View("Create", vw_ProjectOwner);
                }

                return View("Create", vw_ProjectOwner);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "ProjectOwner_View", "POID: " + POID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}