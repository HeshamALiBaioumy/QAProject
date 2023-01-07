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
    [RoutePrefix("CRTypeMC")]
    public class CRTypeMCController : BaseController
    {
        [AuthorizeUser(UserPermission.CRTYPEMC_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_CRTypeMainCategories()
            {
                CRTypeMainCategories = new Ent_CRTypeMainCategories()
                {
                    ID = -1,
                    isActive = true
                }
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CRTYPEMC_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_CRTypeMainCategories formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.CRTypeMainCategories.ID != -1) ? true : false;

                if (ModelState.IsValid)
                {
                    formControl.CRTypeMainCategories.makerID = User.Identity.Name;
                    response = new Mdl_CRTypeMainCategories().insert_updateCRTypeMC(formControl.CRTypeMainCategories, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.CRTypeMainCategories.name
                            + ": " + Localization.CR_TYPES_MAIN_CATEGORIES.CRTypeMCAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.CRTypeMainCategories.makerID, "CRTYPEMC_Create"
                    , formControl.CRTypeMainCategories.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CRTYPEMC_Create)]
        [Route("IsValidCRTypeMCName")]
        public JsonResult IsValidCRTypeMCName(Vw_CRTypeMainCategories formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.CRTypeMC
                  , formControl.CRTypeMainCategories.ID, formControl.CRTypeMainCategories.name);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.CRTYPEMC_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View();
        }

        [AuthorizeUser(UserPermission.CRTYPEMC_Search)]
        [Route("searchCRTYPEMC")]
        public ActionResult searchCRTYPEMC(string searchName, string searchDescription, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_CRTypeMainCategories> searchResult = new List<Ent_CRTypeMainCategories>();

            try
            {
                searchResult = new Mdl_CRTypeMainCategories().searchCRTypeMainCategories(searchName, searchDescription
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CRTYPEMC_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CRTYPEMC_View)]
        [Route("View")]
        public ActionResult View(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CRTypeMainCategories CRTYPEMC = new Ent_CRTypeMainCategories();

            try
            {
                CRTYPEMC = new Mdl_CRTypeMainCategories().viewCRTypeMainCategories(ID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", CRTYPEMC);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CRTYPEMC_View", "FT ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CRTYPEMC_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CRTypeMainCategories CRTYPEMC = new Ent_CRTypeMainCategories();
            Vw_CRTypeMainCategories vw_CRTYPEMC = null;
            try
            {
                CRTYPEMC = new Mdl_CRTypeMainCategories().viewCRTypeMainCategories(ID);
                if (CRTYPEMC.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.CR_TYPES_MAIN_CATEGORIES.CRTYPEMCID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    vw_CRTYPEMC = new Vw_CRTypeMainCategories() { CRTypeMainCategories = CRTYPEMC };
                    return View("Create", vw_CRTYPEMC);
                }

                return View("Create", vw_CRTYPEMC);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CRTYPEMC_View", "MT ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}