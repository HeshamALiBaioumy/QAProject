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
    [RoutePrefix("MixerType")]
    public class MixerTypeController : BaseController
    {
        [AuthorizeUser(UserPermission.MixerType_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_MixerTypes() { mixerType = new Ent_MixerType() { mixerTypeID = -1, isActive = true } });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.MixerType_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_MixerTypes formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.mixerType.mixerTypeID == -1) ? true : false;

                if (ModelState.IsValid)
                {
                    formControl.mixerType.makerID = User.Identity.Name;
                    response = new mdl_MixerType().insert_updateMixerType(formControl.mixerType, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = Localization.MixerType.MixerTypeAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.mixerType.makerID, "MixerType_Create"
                    , formControl.mixerType.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.MixerType_Create)]
        [Route("IsValidMixerTypeName")]
        public JsonResult IsValidMixerTypeName(Vw_MixerTypes formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.Mixer_Type
                    , formControl.mixerType.mixerTypeID, formControl.mixerType.name);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.MixerType_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View();
        }

        [AuthorizeUser(UserPermission.MixerType_Search)]
        [Route("searchMixerTypes")]
        public ActionResult searchMixerTypes(string searchName, string searchDescription, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_MixerType> searchResult = new List<Ent_MixerType>();

            try
            {
                searchResult = new mdl_MixerType().searchMixerType(searchName, searchDescription, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "MixerType_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.MixerType_View)]
        [Route("View")]
        public ActionResult View(int mixerTypeID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_MixerType mixerType = new Ent_MixerType();

            try
            {
                mixerType = new mdl_MixerType().viewMixerType(mixerTypeID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", mixerType);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "MixerType_View", "FT ID: " + mixerTypeID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.MixerType_Edit)]
        [Route("Edit/{MTID?}")]
        public ActionResult Edit(int MTID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_MixerType mixerType = new Ent_MixerType();
            Vw_MixerTypes vw_MixerTypes = null;
            try
            {
                mixerType = new mdl_MixerType().viewMixerType(MTID);
                if (mixerType.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.MixerType.MixerTypeID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;
                    Session["updateID"] = MTID;

                    vw_MixerTypes = new Vw_MixerTypes() { mixerType = mixerType };
                    return View("Create", vw_MixerTypes);
                }

                return View("Create", vw_MixerTypes);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "MixerType_View", "MT ID: " + MTID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}