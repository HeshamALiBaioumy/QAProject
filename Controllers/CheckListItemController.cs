using QA.Entities.Session_Entities;
using QA.Entities.View_Entities;
using QA.Models;
using System;
using System.Threading;
using System.Web.Mvc;
using System.Collections.Generic;
using QA.Entities.Business_Entities;

namespace QA.Controllers
{
    [Authorize]
    [RoutePrefix("CheckListItem")]
    public class CheckListItemController : BaseController
    {
        [AuthorizeUser(UserPermission.CheckListItem_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_CheckListItem() { checklistItem = new Ent_CheckListItem() { checkListItemID = -1, isActive = true } });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CheckListItem_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_CheckListItem formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {

                bool isUpdateForm = (formControl.checklistItem.checkListItemID != -1) ? true : false;

                if (ModelState.IsValid)
                {
                    formControl.checklistItem.makerID = User.Identity.Name;
                    response = new Mdl_CheckListItem().insert_updatCheckListItem(formControl.checklistItem, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.checklistItem.name
                            + ": " + Localization.CheckListItem.CheckListItemAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.checklistItem.makerID, "CheckListItem_Create"
                    , formControl.checklistItem.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        ////[HttpPost]
        ////[AuthorizeUser(UserPermission.CheckListItem_Create, UserPermission.CheckListItem_Edit)]
        ////[Route("IsValidCheckListItem")]
        ////public JsonResult IsValidCheckListItem(Vw_CheckListItem formControl)
        ////{
        ////    try
        ////    {
        ////        bool status = true;

        ////        status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.CheckListItem
        ////            , formControl.checklistItem.checkListItemID, formControl.checklistItem.name);

        ////        return Json(status);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return Json("Data Base Connection issue Occured !");
        ////    }
        ////}

        [AuthorizeUser(UserPermission.CheckListItem_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View();
        }

        [AuthorizeUser(UserPermission.CheckListItem_Search)]
        [Route("searchCheckListItem")]
        public ActionResult searchCheckListItem(string searchName, string searchDescription, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_CheckListItem> searchResult = new List<Ent_CheckListItem>();

            try
            {
                searchResult = new Mdl_CheckListItem().searchCheckListItem(searchName, searchDescription, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CheckListItem_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CheckListItem_View)]
        [Route("View")]
        public ActionResult View(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CheckListItem CheckListItem = new Ent_CheckListItem();

            try
            {
                CheckListItem = new Mdl_CheckListItem().viewCheckListItem(ID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", CheckListItem);
            }
            catch (Exception ex)
            {
                response.responseStatus = false;
                response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;
                return View();
            }
        }

        [AuthorizeUser(UserPermission.CheckListItem_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CheckListItem CheckListItem = new Ent_CheckListItem();
            Vw_CheckListItem vw_CheckListItem = null;
            try
            {
                CheckListItem = new Mdl_CheckListItem().viewCheckListItem(ID);
                if (CheckListItem.name == null)
                {

                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.CheckListItem.CheckListItemID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    vw_CheckListItem = new Vw_CheckListItem() { checklistItem = CheckListItem };
                    return View("Create", vw_CheckListItem);
                }

                return View("Create", vw_CheckListItem);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CheckListItem_View", "MT ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}