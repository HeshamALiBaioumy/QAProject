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
    [RoutePrefix("SampleTestCategory")]
    public class SampleTestCategoryController : BaseController
    {
        [AuthorizeUser(UserPermission.SampleTestCategory_Create)]
        [Route("Create")]

        public ActionResult Create()
        {
            return View(new Vw_SampleType() { sampleType = new Ent_SampleType() { ID = -1, isActive = true } });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.SampleTestCategory_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_SampleType formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.sampleType.ID != -1) ? true : false;
                if (ModelState.IsValid)
                {
                    formControl.sampleType.makerID = User.Identity.Name;
                    response = new Mdl_SampleType().insert_updateSampleType(formControl.sampleType, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.sampleType.name
                            + ": " + Localization.SampleType.SampleTypeAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.sampleType.makerID, "SampleType_Create"
                    , formControl.sampleType.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.SampleTestCategory_Create)]
        [Route("IsValidSampleTypeName")]
        public JsonResult IsValidSampleTypeName(Vw_SampleType formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.SAMPLE_TYPE
                    , formControl.sampleType.ID, formControl.sampleType.name);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.SampleTestCategory_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View();
        }

        [AuthorizeUser(UserPermission.SampleTestCategory_Search)]
        [Route("searchsampleType")]
        public ActionResult searchsampleType(string searchName, string searchDescription, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_SampleType> searchResult = new List<Ent_SampleType>();

            try
            {
                searchResult = new Mdl_SampleType().searchSampleType(searchName, searchDescription, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "sampleType_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.SampleTestCategory_View)]
        [Route("View")]
        public ActionResult View(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_SampleType sampleType = new Ent_SampleType();

            try
            {
                sampleType = new Mdl_SampleType().viewSampleType(ID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", sampleType);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "sampleType_View", "FT ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.SampleTestCategory_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_SampleType sampleType = new Ent_SampleType();
            Vw_SampleType vw_sampleType = null;
            try
            {
                sampleType = new Mdl_SampleType().viewSampleType(ID);
                if (sampleType.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.SampleType.sampleTypeID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    vw_sampleType = new Vw_SampleType() { sampleType = sampleType };
                    return View("Create", vw_sampleType);
                }

                return View("Create", vw_sampleType);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "sampleType_View", "MT ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}