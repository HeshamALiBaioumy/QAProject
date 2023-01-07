using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using QA.Entities.View_Entities;
using QA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace QA.Controllers
{
    [Authorize]
    [RoutePrefix("RCV")]
    public class RandomCRVerificationController : BaseController
    {
        [AuthorizeUser(UserPermission.RCV_RandomSearch)]
        [Route("SearchRandom")]
        public ActionResult SearchRandom()
        {
            ResponseMessage response = new ResponseMessage();
            Vw_CR_Workflow view = null;

            try
            {
                view = new Vw_CR_Workflow()
                {
                    lstCRs = new Mdl_RCV().searchRandom(int.Parse(User.Identity.Name))
                };

                Session["Status"] = response.responseStatus = true;
                Session["EndMessage"] = response.endUserMessage = Localization.Global.searchResultsRetrievedSucessfully;

                return View(view);
            }
            catch (Exception ex)
            {
                view = new Vw_CR_Workflow()
                {
                    lstCRs = null
                };

                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return View(view);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "RandomSearch", "User ID: " + int.Parse(User.Identity.Name)
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.RCV_Action)]
        [Route("AssignRCV")]
        public ActionResult AssignRCV(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    response = new Mdl_RCV().AssignCR(ID, int.Parse(User.Identity.Name));

                    Session["Status"] = response.responseStatus;
                    Session["EndMessage"] = String.Concat(Localization.RCV.Random_CR, ID
                        , Localization.RCV.Assigned_Success);
                }
                else
                {
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;
                }

                return RedirectToAction("SearchRandom");
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return RedirectToAction("SearchRandom");
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "AssignRCV", "CR ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.RCV_RandomSearch)]
        [Route("SearchPending")]
        public ActionResult SearchPending()
        {
            ResponseMessage response = new ResponseMessage();
            Vw_RCV view = null;

            try
            {
                view = new Vw_RCV()
                {
                    lstRCVs = new Mdl_RCV().searchPending(int.Parse(User.Identity.Name))
                };

                //Session["Status"] = response.responseStatus = true;
                //Session["EndMessage"] = response.endUserMessage = Localization.Global.searchResultsRetrievedSucessfully;

                return View(view);
            }
            catch (Exception ex)
            {
                view = new Vw_RCV()
                {
                    lstRCVs = null
                };

                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return View(view);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "SearchPending", "User ID: " + int.Parse(User.Identity.Name)
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.RCV_Action)]
        [Route("FeedbackRCV")]
        public ActionResult FeedbackRCV(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            Vw_RCV view = null;
            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    view = new Vw_RCV()
                    {
                        RCV = new Mdl_RCV().viewPendingRCV(ID, int.Parse(User.Identity.Name))
                    };

                    if (view != null)
                    {
                        Session["Status"] = response.responseStatus = true;
                        Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                        return View(view);
                    }
                    else
                    {
                        view = new Vw_RCV();
                        Session["Status"] = response.responseStatus = false;
                        Session["EndMessage"] = response.endUserMessage = Localization.CR_Workflow.NotAssignedCR;

                        return RedirectToAction("SearchPending");
                    }
                }
                else
                {
                    view = new Vw_RCV();
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;

                    return RedirectToAction("SearchPending");
                }
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;

                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return RedirectToAction("SearchPending");
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "FeedbackRCV", "RCV ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.RCV_Action)]
        [Route("FeedbackRCV")]
        public ActionResult FeedbackRCV(Vw_RCV formControl, string RCVAction)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                int action = -1;
                string redirectAction = "SearchPending";

                if (RCVAction == @Localization.RCV.FeedBack_AcceptRCV)
                {
                    action = 1; // Accept
                    redirectAction = "SearchPending";
                }
                else if (RCVAction == @Localization.RCV.FeedBack_RejectRCV)
                {
                    action = 2; // Reject
                    redirectAction = "createRCVMissmatch";
                }
                else if (RCVAction == @Localization.RCV.FeedBack_Pending)
                {
                    action = 4; // Pending Lab Results
                    redirectAction = "SearchPending";
                }

                formControl.RCV.lstAttachmentNames = new List<string>();
                formControl.RCV.lstAttachmentPaths = new List<string>();
                for (int i = 0; i < formControl.RCV.attachFiles.Count; i++)
                {
                    if (formControl.RCV.attachFiles[i] != null)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(formControl.RCV.attachFiles[i].FileName);
                        string FileExtension = Path.GetExtension(formControl.RCV.attachFiles[i].FileName);
                        string UploadPath = ConfigurationManager.AppSettings["Attach_Files_Path"].ToString();
                        FileName = String.Concat("RCV~", formControl.RCV.RCVID, "~", DateTime.Now.ToString("ddMMyyyyHHmmss")
                            , "~", User.Identity.Name, "~", i, "~", FileName.Trim(), FileExtension);
                        formControl.RCV.attachFiles[i].SaveAs(UploadPath + FileName);

                        formControl.RCV.lstAttachmentNames.Add(formControl.RCV.attachFiles[i].FileName);
                        formControl.RCV.lstAttachmentPaths.Add(FileName);
                    }
                }

                response = new Mdl_RCV().feedbackRCV(formControl.RCV.RCVID, int.Parse(User.Identity.Name)
                    , action, formControl.RCV.comments, formControl.RCV.lstAttachmentNames
                    , formControl.RCV.lstAttachmentPaths);

                if (response.responseStatus)
                {
                    Session["Status"] = response.responseStatus;
                    Session["EndMessage"] = response.endUserMessage = Localization.RCV.FeedBack_Action_Success;

                    if (action == 2)
                    {
                        return RedirectToAction(redirectAction, "RCVMissmatch", new
                        {
                            RCVID = formControl.RCV.RCVID,
                            token = URLParametersValidator.Encrypt(formControl.RCV.RCVID.ToString())
                        });
                    }
                    else
                    {
                        return RedirectToAction(redirectAction, "RandomCRVerification");
                    }
                }
                else
                {
                    Session["Status"] = response.responseStatus;
                    Session["EndMessage"] = response.endUserMessage;

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "FeedbackRCV", "RCVAction" + RCVAction
                    + ", RCV ID: " + formControl.RCV.RCVID + ", Comments: " + formControl.RCV.comments
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.RCV_RandomSearch, UserPermission.RCV_RandomSearch, UserPermission.RCV_Action
            , UserPermission.RCV_Search, UserPermission.RCV_Edit)]
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

        [AuthorizeUser(UserPermission.RCV_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_RCV()
            {
                lOVProjects = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Projects),
                lOVCRs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CRs),
                lOVUserProfiles = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_QATech),
                lOVRCVStatus = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.RCV_Status),
                searchStatus = -1
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.RCV_Search)]
        [Route("Search")]
        public ActionResult Search(int searchProjectID, int searchCRID, int searchProfileID
            , int searchStatus, string searchIsLabRequired)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_RCV> searchResult = new List<Ent_RCV>();

            try
            {
                searchResult = new Mdl_RCV().searchRCV(searchProjectID, searchCRID, searchProfileID
                    , searchStatus, searchIsLabRequired, int.Parse(User.Identity.Name));

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.searchResultsRetrievedSucessfully;

                return PartialView("_searchRCVResults", searchResult);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "RCV Search", "searchProjectID: " + searchProjectID
                    + "~ searchCRID: " + searchCRID + "~ searchProfileID: " + searchProfileID
                    + "~ searchStatus: " + searchStatus + "~ searchIsLabRequired: " + searchIsLabRequired
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.RCV_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            Vw_RCV view = null;
            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    view = new Vw_RCV()
                    {
                        RCV = new Mdl_RCV().viewEditRCV(ID, int.Parse(User.Identity.Name))
                    };

                    if (view.RCV == null)
                    {
                        Session["Status"] = response.responseStatus = false;
                        Session["EndMessage"] = response.endUserMessage = Localization.RCV.Edit_RCV_NotAvailable;
                    }
                    else
                    {
                        Session["Status"] = response.responseStatus = true;
                        Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;
                    }
                }
                else
                {
                    view = new Vw_RCV();
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;

                    return RedirectToAction("Search");
                }

                return View("FeedbackRCV", view);
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return RedirectToAction("Search");
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "RCV Edit", "RCV ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.RCV_RandomSearch, UserPermission.RCV_RandomSearch, UserPermission.RCV_Action
            , UserPermission.RCV_Search, UserPermission.RCV_Edit)]
        [Route("View")]
        public ActionResult View(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            Vw_RCV view = null;

            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    view = new Vw_RCV()
                    {
                        RCV = new Mdl_RCV().viewRCV(ID)
                    };

                    if (view.RCV == null)
                    {
                        Session["Status"] = response.responseStatus = false;
                        Session["EndMessage"] = response.endUserMessage = Localization.RCV.Edit_RCV_NotAvailable;
                    }
                    else
                    {
                        Session["Status"] = response.responseStatus = true;
                        Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;
                    }
                }
                else
                {
                    view = new Vw_RCV();
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;

                    return RedirectToAction("Search");
                }

                return PartialView("_viewRCV", view.RCV);
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return Json(new { Status = false, Message = Localization.ErrorMessages.UnhandledErrorOccured });
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "RCV View", "RCV ID: " + ID, response.ToString()
                    , response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}