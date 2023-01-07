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
    [RoutePrefix("CheckLists")]
    public class CheckListsController : BaseController
    {
        [AuthorizeUser(UserPermission.CheckLists_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            List<LOV> lstCheckListGroups = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Checklist_Groups);
            if (lstCheckListGroups.Count > 0)
            {
                lstCheckListGroups.RemoveAt(0);

            }

            return View(new Vw_CheckLists()
            {
                checkLists = new Ent_CheckLists() { ID = -1, isActive = true },
                lOVCheckListGroups = lstCheckListGroups
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CheckLists_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_CheckLists formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.checkLists.ID != -1) ? true : false;

                if (ModelState.IsValid)
                {
                    formControl.checkLists.makerID = User.Identity.Name;
                    response = new Mdl_CheckLists().insert_updatCheckLists(formControl.checkLists, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.checkLists.name
                            + ": " + Localization.CheckLists.CheckListsAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.checkLists.makerID, "CheckLists_Create"
                    , formControl.checkLists.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CheckLists_Create)]
        [Route("IsValidCheckLists")]
        public JsonResult IsValidCheckLists(Vw_CheckLists formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.CheckList
                  , formControl.checkLists.ID, formControl.checkLists.name);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.CheckLists_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            List<LOV> lstCheckListGroups = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Checklist_Groups);
            if (lstCheckListGroups.Count > 0)
            {
                lstCheckListGroups.RemoveAt(0);
            }


            return View(new Vw_CheckLists()
            {
                lOVCheckListGroups = lstCheckListGroups
            });
        }

        [AuthorizeUser(UserPermission.CheckLists_Search)]
        [Route("searchCheckLists")]
        public ActionResult searchCheckLists(string searchName, string searchDescription
            , List<int> searchLstCLGroupIDs, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_CheckLists> searchResult = new List<Ent_CheckLists>();

            try
            {
                searchResult = new Mdl_CheckLists().searchCheckListt(searchName, searchDescription
                    , searchLstCLGroupIDs, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CheckLists_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CheckLists_View)]
        [Route("View")]
        public ActionResult View(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CheckLists CheckLists = new Ent_CheckLists();

            try
            {
                CheckLists = new Mdl_CheckLists().viewCheckList(ID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", CheckLists);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CheckLists_View", "Checklist ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CheckLists_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CheckLists CheckLists = new Ent_CheckLists();
            Vw_CheckLists vw_CheckLists = null;
            try
            {
                CheckLists = new Mdl_CheckLists().viewCheckList(ID);
                if (CheckLists.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.CheckLists.CheckListsID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    List<LOV> lstCheckListGroups = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Checklist_Groups);
                    lstCheckListGroups.RemoveAt(0);
                    vw_CheckLists = new Vw_CheckLists()
                    {
                        checkLists = CheckLists,
                        lOVCheckListGroups = lstCheckListGroups
                    };
                    return View("Create", vw_CheckLists);
                }

                return View("Create", vw_CheckLists);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CheckLists_View", "MT ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}