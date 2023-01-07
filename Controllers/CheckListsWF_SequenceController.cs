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
    [RoutePrefix("CheckListsWF_Sequence")]
    public class CheckListsWF_SequenceController : BaseController
    {
        [AuthorizeUser(UserPermission.CLWF_Sequence_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_CheckListFlow_Sequence()
            {
                CheckListFlow_Sequence = new Ent_CheckListFlow_Sequence()
                {
                    ID = -1,
                    technician_maxDays = 0,
                    superEng_maxDays = 0,
                    qALab_maxDays = 0,
                    repSuper_maxDays = 0,
                    isActive = true
                },
                lOVCheckLists = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Checklists),
                lOVCLFlowStatuses = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Checklist_Flow_Statuses),
                lOVTechnicianUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_QATech),
                lOVSupervisorEngUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_SupervisorEngs),
                lOVQALabUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_QALAB),
                lOVRepresentitiveSuperUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_RepSuper)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CLWF_Sequence_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_CheckListFlow_Sequence formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.CheckListFlow_Sequence.ID != -1) ? true : false;
                if (ModelState.IsValid)
                {
                    formControl.CheckListFlow_Sequence.makerID = User.Identity.Name;
                    response = new Mdl_CheckListsWF_Sequence().insert_updateSequence(formControl.CheckListFlow_Sequence
                        , isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = (isUpdateForm) ?
                            Localization.CheckListFlow_Sequence.SequenceUpdatedSuccessfully
                            : formControl.CheckListFlow_Sequence.name + ": "
                            + Localization.CheckListFlow_Sequence.SequenceAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.CheckListFlow_Sequence.makerID, "CLWF_Sequence_Create"
                    , formControl.CheckListFlow_Sequence.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CLWF_Sequence_Create)]
        [Route("IsValidSequenceName")]
        public JsonResult IsValidSequenceName(Vw_CheckListFlow_Sequence formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.CheckListFlowSequence
                    , formControl.CheckListFlow_Sequence.ID, formControl.CheckListFlow_Sequence.name);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.CLWF_Sequence_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_CheckListFlow_Sequence());
        }

        [AuthorizeUser(UserPermission.CLWF_Sequence_Search)]
        [Route("searchCLFSequences")]
        public ActionResult searchCLFSequences(string searchName, string searchDescription
            , int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_CheckListFlow_Sequence> searchResult = new List<Ent_CheckListFlow_Sequence>();

            try
            {
                searchResult = new Mdl_CheckListsWF_Sequence().searchSequences(searchName, searchDescription, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "searchCLFSequences", "Search name: " + searchName
                    + "~ Search Desc: " + searchDescription, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CLWF_Sequence_View)]
        [Route("View")]
        public ActionResult View(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CheckListFlow_Sequence checkListFlow_Sequence = new Ent_CheckListFlow_Sequence();

            try
            {
                checkListFlow_Sequence = new Mdl_CheckListsWF_Sequence().viewSequence(ID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", checkListFlow_Sequence);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CLWF_Sequence_View", "ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CLWF_Sequence_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_CheckListFlow_Sequence Sequence = new Ent_CheckListFlow_Sequence();
            Vw_CheckListFlow_Sequence Vw_CheckListFlow_Sequence = null;
            try
            {
                Sequence = new Mdl_CheckListsWF_Sequence().viewSequence(ID);
                if (Sequence.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.CheckListFlow_Sequence.SequenceID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    Vw_CheckListFlow_Sequence = new Vw_CheckListFlow_Sequence()
                    {
                        CheckListFlow_Sequence = Sequence,
                        lOVCheckLists = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Checklists),
                        lOVCLFlowStatuses = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Checklist_Flow_Statuses),
                        lOVTechnicianUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_QATech),
                        lOVSupervisorEngUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_SupervisorEngs),
                        lOVQALabUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_QALAB),
                        lOVRepresentitiveSuperUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_RepSuper)
                    };
                }

                return View("Create", Vw_CheckListFlow_Sequence);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CLWF_Sequence_Edit", "ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}