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
using static QA.Entities.Business_Entities.Ent_MMC_Conversation;

namespace QA.Controllers
{
    [Authorize]
    [RoutePrefix("MMCConversation")]
    public class MMC_ConversationController : BaseController
    {
        [AuthorizeUser(UserPermission.RCV_Conversation_View)]
        [Route("ViewReplies")]
        public ActionResult ViewReplies(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            Vw_MMC_Conversation view = null;
            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    view = new Mdl_MMCConversation().viewMMCConversation(ID, int.Parse(User.Identity.Name));
                    if (view != null)
                    {
                        if(Session["Status"] == null)
                        {
                            Session["Status"] = response.responseStatus = true;
                            Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;
                        }
                    }
                    else
                    {
                        view = new Vw_MMC_Conversation();
                        if (Session["Status"] == null)
                        {
                            Session["Status"] = response.responseStatus = false;
                            Session["EndMessage"] = response.endUserMessage = Localization.MMC_Conversation.View_MMCID_NotAvailable;
                        }
                    }
                }
                else
                {
                    view = new Vw_MMC_Conversation();
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;
                }

                return View(view);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "MMC Conversation - ViewReplies", "ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.RCV_Conversation_View)]
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
        [AuthorizeUser(UserPermission.RCV_Conversation_Reply)]
        [Route("addReply")]
        public ActionResult addReply(Vw_MMC_Conversation view, string addReplyAction)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                view.conversation.replyAction = (addReplyAction == Localization.MMC_Conversation.AddReply_Accept) ?
                    feedbackActions.Reply_Accept : feedbackActions.Reply_Reject;
                view.conversation.replyUserID = int.Parse(User.Identity.Name);

                view.conversation.lstAttachmentNames = new List<string>();
                view.conversation.lstAttachmentPaths = new List<string>();
                for (int i = 0; i < view.conversation.replyAttachments.Count; i++)
                {
                    if (view.conversation.replyAttachments[i] != null)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(view.conversation.replyAttachments[i].FileName);
                        string FileExtension = Path.GetExtension(view.conversation.replyAttachments[i].FileName);
                        string UploadPath = ConfigurationManager.AppSettings["Attach_Files_Path"].ToString();
                        FileName = String.Concat("MMC_Conversation~", view.conversation.MMCID
                            , "~", DateTime.Now.ToString("ddMMyyyyHHmmss")
                            , "~", User.Identity.Name, "~", i, "~", FileName.Trim(), FileExtension);
                        view.conversation.replyAttachments[i].SaveAs(UploadPath + FileName);

                        view.conversation.lstAttachmentNames.Add(view.conversation.replyAttachments[i].FileName);
                        view.conversation.lstAttachmentPaths.Add(FileName);
                    }
                }

                response = new Mdl_MMCConversation().MMC_Reply(view.conversation.MMCID, view.conversation.replyUserID
                  , view.conversation.replyAction, view.conversation.replyMessage, view.conversation.lstAttachmentNames
                  , view.conversation.lstAttachmentPaths);

                if (response.responseStatus)
                {
                    Session["Status"] = response.responseStatus;
                    Session["EndMessage"] = response.endUserMessage = Localization.MMC_Conversation.AddReply_ReplyAddedSuccess;
                }
                else
                {
                    Session["Status"] = response.responseStatus;
                    Session["EndMessage"] = response.endUserMessage;
                }

                return RedirectToAction("ViewReplies", new RouteValueDictionary(new
                {
                    Controller = "MMC_Conversation",
                    Action = "ViewReplies",
                    ID = view.conversation.MMCID,
                    token = URLParametersValidator.Encrypt(view.conversation.MMCID.ToString())
                }));
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return RedirectToAction("ViewReplies", new RouteValueDictionary(new
                {
                    Controller = "MMC_Conversation",
                    Action = "ViewReplies",
                    ID = view.conversation.MMCID,
                    token = URLParametersValidator.Encrypt(view.conversation.MMCID.ToString())
                }));
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(view.conversation.replyUserID.ToString(), "MMCConversation - addReply"
                    , "view" + view.conversation.ToString() + "addReplyAction: " + addReplyAction
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.RCV_Conversation_Close)]
        [Route("closeCase")]
        public ActionResult closeCase(Vw_MMC_Conversation view, string closeAction)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                view.conversation.replyAction = (closeAction == Localization.MMC_Conversation.CloseCase_Fixed) ?
                    feedbackActions.Close_Fixed : feedbackActions.Close_Closed;
                view.conversation.replyUserID = int.Parse(User.Identity.Name);

                view.conversation.lstAttachmentNames = new List<string>();
                view.conversation.lstAttachmentPaths = new List<string>();
                for (int i = 0; i < view.conversation.replyAttachments.Count; i++)
                {
                    if (view.conversation.replyAttachments[i] != null)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(view.conversation.replyAttachments[i].FileName);
                        string FileExtension = Path.GetExtension(view.conversation.replyAttachments[i].FileName);
                        string UploadPath = ConfigurationManager.AppSettings["Attach_Files_Path"].ToString();
                        FileName = String.Concat("MMC_Conversation~", view.conversation.MMCID
                            , "~", DateTime.Now.ToString("ddMMyyyyHHmmss")
                            , "~", User.Identity.Name, "~", i, "~", FileName.Trim(), FileExtension);
                        view.conversation.replyAttachments[i].SaveAs(UploadPath + FileName);

                        view.conversation.lstAttachmentNames.Add(view.conversation.replyAttachments[i].FileName);
                        view.conversation.lstAttachmentPaths.Add(FileName);
                    }
                }

                response = new Mdl_MMCConversation().MMC_Reply(view.conversation.MMCID, view.conversation.replyUserID
                  , view.conversation.replyAction, view.conversation.replyMessage, view.conversation.lstAttachmentNames
                  , view.conversation.lstAttachmentPaths);

                if (response.responseStatus)
                {
                    Session["Status"] = response.responseStatus;
                    Session["EndMessage"] = response.endUserMessage = 
                        Localization.MMC_Conversation.CloseCase_ClosedSuccessfully;
                }
                else
                {
                    Session["Status"] = response.responseStatus;
                    Session["EndMessage"] = response.endUserMessage;
                }

                return RedirectToAction("ViewReplies", new RouteValueDictionary(new
                {
                    Controller = "MMC_Conversation",
                    Action = "ViewReplies",
                    ID = view.conversation.MMCID,
                    token = URLParametersValidator.Encrypt(view.conversation.MMCID.ToString())
                }));
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return RedirectToAction("ViewReplies", new RouteValueDictionary(new
                {
                    Controller = "MMC_Conversation",
                    Action = "ViewReplies",
                    ID = view.conversation.MMCID,
                    token = URLParametersValidator.Encrypt(view.conversation.MMCID.ToString())
                }));
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(view.conversation.replyUserID.ToString(), "MMCConversation - closeCase"
                    , "view" + view.conversation.ToString() + "closeAction: " + closeAction
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}