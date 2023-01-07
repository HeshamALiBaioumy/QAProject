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
    [RoutePrefix("DepartmentSection")]
    public class DepartmentSectionController : BaseController
    {
        [AuthorizeUser(UserPermission.DepartmentSection_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_DepartmentSection() {section = new Ent_DepartmentSection() { sectionID = -1, isActive = true }
                , lOVDepartments = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Department)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.DepartmentSection_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_DepartmentSection formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.section.sectionID != -1) ? true : false;
                if (ModelState.IsValid)
                {
                    formControl.section.makerID = User.Identity.Name;
                    response = new Mdl_DepartmentSection().insert_updateDepartmentSection(formControl.section,isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = (isUpdateForm) ?
                            Localization.DepartmentSection.DepartmentSectionAddedSuccessfull
                            : formControl.section.name + ": " + Localization.DepartmentSection.DepartmentSectionAddedSuccessfull;
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
                Mdl_Log log = new Mdl_Log(formControl.section.makerID, "DepartmentSection_Create"
                    , formControl.section.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.DepartmentSection_Create)]
        [Route("IsValidDepartmentSectionName")]
        public JsonResult IsValidDepartmentSectionName(Vw_DepartmentSection formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.Department_Section
                    , formControl.section.sectionID, formControl.section.name, formControl.section.departmentID);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.DepartmentSection_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_DepartmentSection()
            {
                lOVDepartments = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Department)
            });
        }

        [AuthorizeUser(UserPermission.DepartmentSection_Search)]
        [Route("searchDepartmentSection")]
        public ActionResult searchDepartmentSection(string searchName, string searchDescription
            , int searchDepartmentID, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_DepartmentSection> searchResult = new List<Ent_DepartmentSection>();

            try
            {
                searchResult = new Mdl_DepartmentSection().searchDepartmentSection(searchName, searchDescription
                    , searchDepartmentID, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "DepartmnetSection_Search", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.DepartmentSection_View)]
        [Route("View")]
        public ActionResult View(int DepartmentSectionID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_DepartmentSection DepartmentSection = new Ent_DepartmentSection();

            try
            {
                DepartmentSection = new Mdl_DepartmentSection().viewDepartmentSection(DepartmentSectionID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", DepartmentSection);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "DepartmentSection_View", "DepartmentSec ID: " + DepartmentSectionID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.DepartmentSection_Edit)]
        [Route("Edit/{depSecID?}")]
        public ActionResult Edit(int depSecID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_DepartmentSection DepartmentSection = new Ent_DepartmentSection();
            Vw_DepartmentSection vw_DepartmentSection = null;
            try
            {
                DepartmentSection = new Mdl_DepartmentSection().viewDepartmentSection(depSecID);
                if (DepartmentSection.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.DepartmentSection.DepartmentSectionID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    vw_DepartmentSection = new Vw_DepartmentSection() { section = DepartmentSection
                        , lOVDepartments = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Department)
                    };
                    return View("Create", vw_DepartmentSection);
                }

                return View("Create", vw_DepartmentSection);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "DepartmentSection_View", "MT ID: " + depSecID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}