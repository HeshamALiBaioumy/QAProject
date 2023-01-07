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
    [RoutePrefix("CheckListItemGroup")]
    public class CheckListItemGroupController : BaseController
    {
        [AuthorizeUser(UserPermission.CheckListItemGroup_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            List<LOV> lstCheckListItems = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Checklist_Groups);
            if (lstCheckListItems.Count >0)
            {
                lstCheckListItems.RemoveAt(0);
            }
           

            return View(new Vw_CheckListItemGroup()
            {
                checkListItemGroup = new Ent_CheckListItemGroup() { ID = -1, isActive = true },
                lOVCheckListItems = lstCheckListItems
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CheckListItemGroup_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_CheckListItemGroup formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.checkListItemGroup.ID != -1) ? true : false;

                if (ModelState.IsValid)
                {
                    formControl.checkListItemGroup.makerID = User.Identity.Name;
                    response = new Mdl_CheckListItemGroup().insert_updatCheckListItemGroup(formControl.checkListItemGroup
                        , isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.checkListItemGroup.name
                            + ": " + Localization.CheckListItemGroup.CheckListItemGroupAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.checkListItemGroup.makerID, "CheckListItemGroup_Create"
                    , formControl.checkListItemGroup.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CheckListItemGroup_Create, UserPermission.CheckListItemGroup_Edit)]
        [Route("IsValidCheckListItemGroup")]
        public JsonResult IsValidCheckListItemGroup(Vw_CheckListItemGroup formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.CheckListItemGroup
                    , formControl.checkListItemGroup.ID, formControl.checkListItemGroup.name);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.CheckListItemGroup_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            List<LOV> lstCheckListItems = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Checklist_Items);
            if (lstCheckListItems.Count >0)
            {
                lstCheckListItems.RemoveAt(0);

            }

            return View(new Vw_CheckListItemGroup()
            {
                lOVCheckListItems = lstCheckListItems
            });
        }

        [AuthorizeUser(UserPermission.CheckListItemGroup_Search)]
        [Route("searchCheckListItemGroup")]
        public ActionResult searchCheckListItemGroup(string searchName, string searchDescription
            , List<int> searchLstCLItemIDs, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_CheckListItemGroup> searchResult = new List<Ent_CheckListItemGroup>();

            try
            {
                searchResult = new Mdl_CheckListItemGroup().searchMCheckListItemGroup(searchName, searchDescription
                    , searchLstCLItemIDs, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CheckListItemGroup_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CheckListItemGroup_View)]
        [Route("View")]
        public ActionResult View(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CheckListItemGroup CheckListItemGroup = new Ent_CheckListItemGroup();

            try
            {
                CheckListItemGroup = new Mdl_CheckListItemGroup().viewCheckListITemGroup(ID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", CheckListItemGroup);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CheckListItemGroup_View", "ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CheckListItemGroup_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CheckListItemGroup CheckListItemGroup = new Ent_CheckListItemGroup();
            Vw_CheckListItemGroup vw_CheckListItemGroup = null;
            try
            {
                CheckListItemGroup = new Mdl_CheckListItemGroup().viewCheckListITemGroup(ID);
                if (CheckListItemGroup.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.CheckListItemGroup.CheckListItemGroupID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    List<LOV> lstCheckListItems = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Checklist_Items);
                    lstCheckListItems.RemoveAt(0);
                    vw_CheckListItemGroup = new Vw_CheckListItemGroup()
                    {
                        checkListItemGroup = CheckListItemGroup,
                        lOVCheckListItems = lstCheckListItems
                    };
                    return View("Create", vw_CheckListItemGroup);
                }

                return View("Create", vw_CheckListItemGroup);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CheckListItemGroup_View", "MT ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}