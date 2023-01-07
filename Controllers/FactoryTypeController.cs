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
    [RoutePrefix("FactoryType")]
    public class FactoryTypeController : BaseController
    {
        [AuthorizeUser(UserPermission.FactoryType_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_FactoryTypes() { factoryType = new Ent_FactoryTypes() { factoryTypeID = -1, isActive = true} });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.FactoryType_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_FactoryTypes formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.factoryType.factoryTypeID != -1) ? true : false;

                if (ModelState.IsValid)
                {
                    formControl.factoryType.makerID = User.Identity.Name;
                    response = new Mdl_FactoryType().insert_UpdateFactoryType(formControl.factoryType, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = (isUpdateForm)? 
                            Localization.FactoryType.FactoryTypeUpdatedSuccessfully
                            : formControl.factoryType.name + ": " + Localization.FactoryType.FactoryTypeAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.factoryType.makerID, "FactoryType_Create"
                    , formControl.factoryType.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.FactoryType_Create)]
        [Route("IsValidfactoryTypeName")]
        public JsonResult IsValidfactoryTypeName(Vw_FactoryTypes formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.Factory_Type
                    , formControl.factoryType.factoryTypeID, formControl.factoryType.name);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.FactoryType_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View();
        }

        [AuthorizeUser(UserPermission.FactoryType_Search)]
        [Route("searchFactoryTypes")]
        public ActionResult searchFactoryTypes(string searchName, string searchDescription)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_FactoryTypes> searchResult = new List<Ent_FactoryTypes>();

            try
            {
                searchResult = new Mdl_FactoryType().searchFactoryType(searchName, searchDescription);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "FactoryType_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.FactoryType_View)]
        [Route("View")]
        public ActionResult View(int factoryTypeID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_FactoryTypes factoryType = new Ent_FactoryTypes();

            try
            {
                factoryType = new Mdl_FactoryType().viewFactoryType(factoryTypeID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", factoryType);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "FactoryType_View", "FT ID: " + factoryTypeID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.FactoryType_Edit)]
        [Route("Edit/{FTID?}")]
        public ActionResult Edit(int FTID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_FactoryTypes factoryType = new Ent_FactoryTypes();
            Vw_FactoryTypes vwFactoryType = null;
            try
            {
                factoryType = new Mdl_FactoryType().viewFactoryType(FTID);
                if(factoryType.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.FactoryType.FactoryTypeID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    vwFactoryType = new Vw_FactoryTypes() { factoryType = factoryType };
                    return View("Create", vwFactoryType);
                }

                return View("Create", vwFactoryType);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "FactoryType_View", "FT ID: " + FTID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}