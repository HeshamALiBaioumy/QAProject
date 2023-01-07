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
    [RoutePrefix("Mixer")]
    public class MixerController : BaseController
    {
        [AuthorizeUser(UserPermission.Mixer_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_Mixer()
            {
                mixer = new Ent_Mixer() { mixerID = -1, isActive = true }
                ,
                lOVMixerTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Mixer_Type)
                ,
                lOVFactories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Factory)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Mixer_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_Mixer formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.mixer.mixerID == -1) ? true : false;
                if (ModelState.IsValid)
                {
                    formControl.mixer.makerID = User.Identity.Name;
                    response = new Mdl_Mixer().insert_updateMixer(formControl.mixer, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.mixer.name
                            + ": " + Localization.Mixer.MixerAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.mixer.makerID, "Mixer_Create"
                    , formControl.mixer.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Mixer_Create)]
        [Route("IsValidMixer")]
        public JsonResult IsValidMixer(Vw_Mixer formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.Mixer
                    , formControl.mixer.mixerID, formControl.mixer.name, formControl.mixer.mixerTypeID);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.Mixer_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_Mixer()
            {
                lOVMixerTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Mixer_Type)
                ,
                lOVFactories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Factory)
            });
        }

        [AuthorizeUser(UserPermission.Mixer_Search)]
        [Route("searchMixer")]
        public ActionResult searchMixer(string searchName, string searchDescription, string searchMixerCode
            , string searchMixerPower, string searchAddressLine, int searchMixerTypeID, int searchIsActive
            , string searchMixerModel, int searchfactoryID)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_Mixer> searchResult = new List<Ent_Mixer>();

            try
            {
                searchResult = new Mdl_Mixer().searchMixer(searchName, searchDescription, searchMixerCode
                    , searchMixerModel, searchMixerPower, searchAddressLine, searchfactoryID, searchMixerTypeID
                    , searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Mixer_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Mixer_View)]
        [Route("View")]
        public ActionResult View(int MixerID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_Mixer Mixer = new Ent_Mixer();

            try
            {
                Mixer = new Mdl_Mixer().viewMixer(MixerID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", Mixer);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Mixer_View", "FT ID: " + MixerID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Mixer_Edit)]
        [Route("Edit/{MxID?}")]
        public ActionResult Edit(int MxID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_Mixer Mixer = new Ent_Mixer();
            Vw_Mixer vw_Mixer = null;
            try
            {
                Mixer = new Mdl_Mixer().viewMixer(MxID);
                if (Mixer.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.Mixer.MixerID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    vw_Mixer = new Vw_Mixer()
                    {
                        mixer = Mixer
                        ,
                        lOVMixerTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Mixer_Type)
                        ,
                        lOVFactories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Factory)
                    };
                    return View("Create", vw_Mixer);
                }

                return View("Create", vw_Mixer);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Mixer_View", "MT ID: " + MxID, response.ToString()
                    , response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}