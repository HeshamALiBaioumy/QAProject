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
    [RoutePrefix("Factory")]
    public class FactoryController : BaseController
    {
        [AuthorizeUser(UserPermission.Factory_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_Factory() { factory = new Ent_Factory() { factoryID = -1, isActive = true }
            , lOVfactoryTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Factory_Type)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Factory_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_Factory formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.factory.factoryID == -1) ? true : false;

                if (ModelState.IsValid)
                {
                    formControl.factory.makerID = User.Identity.Name;
                    response = new Mdl_Factory().insert_updateFactory(formControl.factory, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = Localization.Factory.factoryAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.factory.makerID, "Factory_Create"
                    , formControl.factory.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Factory_Create)]
        [Route("IsValidFactoryName")]
        public JsonResult IsValidFactoryName(Vw_Factory formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.Factory
                    , formControl.factory.factoryID, formControl.factory.name, formControl.factory.factoryTypeID);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.Factory_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_Factory() { lOVfactoryTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Factory_Type) });
        }

        [AuthorizeUser(UserPermission.Factory_Search)]
        [Route("searchFactory")]
        public ActionResult searchFactory(string searchName, string searchDescription, string searchAddressLine
            , string searchFactoryPower, string searchFactoryCode, int searchfactoryTypeID, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_Factory> searchResult = new List<Ent_Factory>();

            try
            {
                searchResult = new Mdl_Factory().searchFactory(searchName, searchDescription, searchFactoryCode
                    , searchAddressLine, searchFactoryPower, searchfactoryTypeID, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Factory_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Factory_View)]
        [Route("View")]
        public ActionResult View(int FactoryID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_Factory factory = new Ent_Factory();

            try
            {
                factory = new Mdl_Factory().viewFactory(FactoryID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", factory);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Factory_View", "FT ID: " + FactoryID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Factory_Edit)]
        [Route("Edit/{FID?}")]
        public ActionResult Edit(int FID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_Factory factory = new Ent_Factory();
            Vw_Factory vw_Factory = null;
            try
            {
                factory = new Mdl_Factory().viewFactory(FID);
                if (factory.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.Factory.FactoryID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;
                    Session["updateID"] = FID;

                    vw_Factory = new Vw_Factory() { factory = factory
                        , lOVfactoryTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Factory_Type) };
                    return View("Create", vw_Factory);
                }

                return View("Create", vw_Factory);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Factory_View", "MT ID: " + FID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}