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
    [RoutePrefix("CRTypeGroup")]
    public class CRTypeGroupController : BaseController
    {
        [AuthorizeUser(UserPermission.CRTypeGroup_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_CRTypeGroup()
            {
                CRTypeGroup = new Ent_CRTypeGroup() { ID = -1, isActive = true }
            ,
                lOVCRMainCategories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Type_MainCategories)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CRTypeGroup_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_CRTypeGroup formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.CRTypeGroup.ID != -1) ? true : false;

                if (ModelState.IsValid)
                {
                    formControl.CRTypeGroup.makerID = User.Identity.Name;
                    response = new Mdl_CRTypeGroup().insert_updateCRTypeGroup(formControl.CRTypeGroup, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.CRTypeGroup.name
                            + ": " + Localization.CRTYPEGroup.CRTypeGroupAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.CRTypeGroup.makerID, "CRTypeGroup_Create"
                    , formControl.CRTypeGroup.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CRTypeGroup_Create)]
        [Route("IsValidCRTypeGroup")]
        public JsonResult IsValidCRTypeGroup(Vw_CRTypeGroup formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.CR_Type_Group
                  , formControl.CRTypeGroup.ID, formControl.CRTypeGroup.name, formControl.CRTypeGroup.CRTMCID);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.CRTypeGroup_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_CRTypeGroup()
            {
                lOVCRMainCategories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Type_MainCategories)
            });
        }

        [AuthorizeUser(UserPermission.CRTypeGroup_Search)]
        [Route("searchCRTypeGroup")]
        public ActionResult searchCRTypeGroup(string searchName, string searchDescription
            , int searchCRMCID, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_CRTypeGroup> searchResult = new List<Ent_CRTypeGroup>();

            try
            {
                searchResult = new Mdl_CRTypeGroup().searchCRTypeGroup(searchName, searchDescription
                    , searchCRMCID, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CRTypeGroup_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CRTypeGroup_View)]
        [Route("View")]
        public ActionResult View(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CRTypeGroup CRTypeGroup = new Ent_CRTypeGroup();

            try
            {
                CRTypeGroup = new Mdl_CRTypeGroup().viewCRTypeGroup(ID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", CRTypeGroup);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CRTypeGroup_View", "FT ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CRTypeGroup_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CRTypeGroup CRTypeGroup = new Ent_CRTypeGroup();
            Vw_CRTypeGroup vw_CRTypeGroup = null;
            try
            {
                CRTypeGroup = new Mdl_CRTypeGroup().viewCRTypeGroup(ID);
                if (CRTypeGroup.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.CRTYPEGroup.CRTypeGroupID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    vw_CRTypeGroup = new Vw_CRTypeGroup()
                    {
                        CRTypeGroup = CRTypeGroup
                    ,
                        lOVCRMainCategories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Type_MainCategories)
                    };
                    return View("Create", vw_CRTypeGroup);
                }

                return View("Create", vw_CRTypeGroup);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CRTypeGroup_View", "MT ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}