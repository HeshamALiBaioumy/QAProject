using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using QA.Entities.View_Entities;
using QA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace QA.Controllers
{
    [Authorize]
    [RoutePrefix("CheckListsWFlow")]
    public class CheckListsWFlowController : BaseController
    {
        [AuthorizeUser(UserPermission.CLWF_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_CheckListFlow_Master()
            {
                CheckListFlow_Master = new Ent_CheckListFlow_Master()
                {
                    ID = -1,
                    CLFlowStatus = 0,
                    registrationDate = DateTime.Now,
                    technician_maxDays = 0,
                    superEng_maxDays = 0,
                    qALab_maxDays = 0,
                    repSuper_maxDays = 0
                },
                lOVCheckLists = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Checklists),
                lOVCheckListSequences = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Checklist_Flow_Sequences),
                lOVCLFlowStatuses = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Checklist_Flow_Statuses),
                lOVTechnicianUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_QATech),
                lOVSupervisorEngUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_SupervisorEngs),
                lOVQALabUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_QALAB),
                lOVRepresentitiveSuperUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_RepSuper)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CLWF_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_CheckListFlow_Master formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.CheckListFlow_Master.ID != -1) ? true : false;

                if (ModelState.IsValid)
                {
                    formControl.CheckListFlow_Master.makerID = User.Identity.Name;
                    response = new Mdl_CheckListFlow().insert_updatCheckListFlow(formControl.CheckListFlow_Master, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = Localization.CheckListFlow_Master.CLF_AddedSuccessfully;
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

                    return View(formControl);
                }
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return View(formControl);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(formControl.CheckListFlow_Master.makerID, "CLWF_Create"
                    , formControl.CheckListFlow_Master.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CLWF_Maker, UserPermission.CLWF_Checker)]
        [Route("viewPending")]
        public ActionResult viewPending()
        {
            List<Ent_CheckListFlow_Master> lstView = new List<Ent_CheckListFlow_Master>();
            try
            {
                lstView = new Mdl_CheckListFlow().searchPendingCheckLists(int.Parse(User.Identity.Name));

                Session["Status"] = true;
                Session["EndMessage"] = Localization.CR_Workflow.PendingReceieved;
            }
            catch (Exception ex)
            {
                Session["Status"] = false;
                Session["EndMessage"] = Localization.Global.UnhandledErrorOccured;
            }

            return View(lstView);
        }

        [AuthorizeUser(UserPermission.CLWF_Maker)]
        [Route("checkListMaker")]
        public ActionResult checkListMaker(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            Vw_CheckList_Flow view = null;

            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    view = new Mdl_CheckListFlow().viewCLMaker(ID, int.Parse(User.Identity.Name));
                    if (view != null)
                    {
                        Session["Status"] = response.responseStatus = true;
                        Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;
                    }
                    else
                    {
                        view = new Vw_CheckList_Flow();
                        Session["Status"] = response.responseStatus = false;
                        Session["EndMessage"] = response.endUserMessage = Localization.CheckListFlow_Master.checkListDataRetrieveError;
                    }

                    view.lOVCLItemsAvailable = Ent_CheckList_Flow.fillCheckListMakerAvailableItems();
                    return View(view);
                }
                else
                {
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;

                    return RedirectToAction("viewPending");
                }
            }
            catch (Exception ex)
            {
                response.responseStatus = false;
                response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                Session["Status"] = response.responseStatus;
                Session["EndMessage"] = response.endUserMessage;

                return RedirectToAction("viewPending");
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "checkListMaker", "CL ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CLWF_Maker)]
        [Route("checkListMaker")]
        public ActionResult checkListMaker(Vw_CheckList_Flow formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                if (ModelState.IsValid)
                {
                    formControl.makerID = User.Identity.Name;

                    foreach (Ent_CheckList_Flow attachment in formControl.lstClItems)
                    {
                        if (attachment.attachFile != null)
                        {
                            string FileName = Path.GetFileNameWithoutExtension(attachment.attachFile.FileName);
                            string FileExtension = Path.GetExtension(attachment.attachFile.FileName);
                            string UploadPath = ConfigurationManager.AppSettings["Attach_Files_Path"].ToString();
                            FileName = String.Concat("CLFlow~", formControl.cLFlowID, "~", attachment.cLGID
                                , "~", attachment.cLItemID, "~", DateTime.Now.ToString("ddMMyyyyHHmmss")
                                , "~", formControl.makerID, "~", FileName.Trim(), FileExtension);
                            attachment.attachmentName = attachment.attachFile.FileName;
                            attachment.attachmentPath = FileName;
                            attachment.hasAttachment = true;
                            attachment.attachFile.SaveAs(UploadPath + FileName);
                        }
                        else
                        {
                            attachment.hasAttachment = false;
                        }
                    }

                    response = new Mdl_CheckListFlow().checkListMaker(formControl);

                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = Localization.CheckListFlow_Master
                            .CLF_Maker_AddedSuccessfully;

                        return RedirectToAction("viewPending");
                    }
                    else
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage;

                        string UploadPath = ConfigurationManager.AppSettings["Attach_Files_Path"].ToString();
                        foreach (Ent_CheckList_Flow attachment in formControl.lstClItems)
                        {
                            if (attachment.attachFile != null)
                            {
                                string fullPath = UploadPath + attachment.attachmentPath;
                                if (System.IO.File.Exists(fullPath))
                                {
                                    System.IO.File.Delete(fullPath);
                                }
                            }
                        }

                        formControl.lOVCLItemsAvailable = Ent_CheckList_Flow.fillCheckListMakerAvailableItems();
                        formControl.lstClItems = new List<Ent_CheckList_Flow>();
                        return View(formControl);
                    }
                }
                else
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                    response.comments = null;
                    response.errorMessage = ModelState.ToString();

                    formControl.lOVCLItemsAvailable = Ent_CheckList_Flow.fillCheckListMakerAvailableItems();
                    formControl.lstClItems = new List<Ent_CheckList_Flow>();
                    return View(formControl);
                }
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                string UploadPath = ConfigurationManager.AppSettings["Attach_Files_Path"].ToString();
                foreach (Ent_CheckList_Flow attachment in formControl.lstClItems)
                {
                    if (attachment.attachFile != null)
                    {
                        string fullPath = UploadPath + attachment.attachmentPath;
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                }

                formControl.lOVCLItemsAvailable = Ent_CheckList_Flow.fillCheckListMakerAvailableItems();
                formControl.lstClItems = new List<Ent_CheckList_Flow>();
                return View(formControl);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "checkListMaker-Post", "Input: " + formControl.ToString()
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CLWF_Checker)]
        [Route("checkListChecker")]
        public ActionResult checkListChecker(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            Vw_CheckList_Flow view = null;

            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    view = new Mdl_CheckListFlow().viewCLCheker(ID, int.Parse(User.Identity.Name));
                    if (view != null)
                    {
                        Session["Status"] = response.responseStatus = true;
                        Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;
                    }
                    else
                    {
                        view = new Vw_CheckList_Flow();
                        Session["Status"] = response.responseStatus = false;
                        Session["EndMessage"] = response.endUserMessage = Localization.CheckListFlow_Master.checkListDataRetrieveError;
                    }

                    return View(view);
                }
                else
                {
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;

                    return RedirectToAction("viewPending");
                }
            }
            catch (Exception ex)
            {
                response.responseStatus = false;
                response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                Session["Status"] = response.responseStatus;
                Session["EndMessage"] = response.endUserMessage;

                return RedirectToAction("viewPending");
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "checkListChecker", "CL ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CLWF_Checker)]
        [Route("DownloadAttachment")]
        public FileResult DownloadAttachment(string fileName, string downloadName)
        {
            string UploadPath = ConfigurationManager.AppSettings["Attach_Files_Path"].ToString();
            string fullPath = UploadPath + fileName;

            if (System.IO.File.Exists(fullPath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadName);
            }
            else
            {
                throw new FileNotFoundException("Unhandled Error Occured, Kindly contact system Admin");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CLWF_Maker)]
        [Route("checkListChecker")]
        public ActionResult checkListChecker(Vw_CheckList_Flow formControl, string CheckListFlowAction)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                formControl.makerID = User.Identity.Name;
                bool isAcceptCL = (CheckListFlowAction == Localization.CheckListFlow_Master.Accept_CLF) ? true : false;
                response = new Mdl_CheckListFlow().checkListCheker(formControl.cLFlowID, int.Parse(formControl.makerID)
                    , isAcceptCL);

                if (response.responseStatus)
                {
                    Session["Status"] = response.responseStatus;
                    Session["EndMessage"] = response.endUserMessage = Localization.CheckListFlow_Master
                        .CLF_Maker_AddedSuccessfully;

                    return RedirectToAction("viewPending");
                }
                else
                {
                    Session["Status"] = response.responseStatus;
                    Session["EndMessage"] = response.endUserMessage;
                    return View(formControl);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "checkListChecker-Post"
                    , ", Input: " + formControl.ToString() + ", Action" + CheckListFlowAction
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}