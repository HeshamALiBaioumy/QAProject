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
    [RoutePrefix("CRType")]
    public class CRTypeController : BaseController
    {
        [AuthorizeUser(UserPermission.CRType_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_CRType()
            {
                CRType = new Ent_CRType() { ID = -1, isActive = true, CRcategory = -1 },
                lOVCRMC = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Type_MainCategories),
                lOVCRGroups = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Group_Types),
                lOVCategories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CRType_Categories)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CRType_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_CRType formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.CRType.ID != -1) ? true : false;

                if (ModelState.IsValid)
                {
                    formControl.CRType.makerID = User.Identity.Name;
                    response = new Mdl_CRType().insert_updateCRType(formControl.CRType, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.CRType.name
                            + ": " + Localization.CRType.CRTypeAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.CRType.makerID, "CRType_Create"
                    , formControl.CRType.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CRType_Create, UserPermission.CRType_Edit)]
        [Route("IsValidCRType")]
        public JsonResult IsValidCRType(Vw_CRType formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.CRType
                    , formControl.CRType.ID, formControl.CRType.name, formControl.CRType.groupID);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CRType_Create, UserPermission.CRType_Edit)]
        [Route("getCRMCGroups")]
        public JsonResult getCRMCGroups(int CRMCID)
        {
            try
            {
                List<LOV> lovCRMCGroups = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CRType_MC_Groups, parentID: CRMCID);

                return Json(lovCRMCGroups);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.CRType_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_CRType()
            {
                searchCategoryID = -1,
                lOVCRMC = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Type_MainCategories),
                lOVCRGroups = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CRType_Groups),
                lOVCategories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CRType_Categories)
            });
        }

        [AuthorizeUser(UserPermission.CRType_Search)]
        [Route("searchCRType")]
        public ActionResult searchCRType(string searchName, string searchDescription
            , int searchCRTMCID, int searchgroupID, int searchCategoryID, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_CRType> searchResult = new List<Ent_CRType>();

            try
            {
                searchResult = new Mdl_CRType().searchCRType(searchName, searchDescription, searchCRTMCID
                    , searchgroupID, searchCategoryID, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CRType_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CRType_View)]
        [Route("View")]
        public ActionResult View(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CRType CRType = new Ent_CRType();

            try
            {
                CRType = new Mdl_CRType().viewCRType(ID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", CRType);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CRType_View", "FT ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CRType_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CRType CRType = new Ent_CRType();
            Vw_CRType vw_CRType = null;
            try
            {
                CRType = new Mdl_CRType().viewCRType(ID);
                if (CRType.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.CRType.CRTypeID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    vw_CRType = new Vw_CRType()
                    {
                        CRType = CRType,
                        lOVCRMC = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Type_MainCategories),
                        lOVCRGroups = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CRType_Groups, parentID: CRType.CRTMCID),
                        lOVCategories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CRType_Categories)
                    };

                    return View("Create", vw_CRType);
                }

                return View("Create", vw_CRType);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CRType_View", "MT ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}