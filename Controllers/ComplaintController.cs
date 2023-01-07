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
namespace QA.Controllers
{
    [Authorize]
    [RoutePrefix("Complaint")]
    public class ComplaintController : BaseController
    {
        [AuthorizeUser(UserPermission.Complaint_Create)]
        [Route("Create/{CRID?}")]
        public ActionResult Create(int? CRID, int? projectID, string token)
        {
            if (CRID == null)
            {
                return View(new Vw_Complaint()
                {
                    complaint = new Ent_Complaint() { complaintID = -1, complaintStatus = 0 },
                    lOVProjects = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Projects),
                    lOVCRs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.EmptyList),
                    lOVComplaintStatus = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Complaint_Status)
                });
            }
            else
            {
                if (CRID != null && projectID != null && token != null
                    && (CRID + projectID).ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    return View(new Vw_Complaint()
                    {
                        complaint = new Ent_Complaint()
                        {
                            complaintID = -1,
                            complaintStatus = 0,
                            CRID = CRID.GetValueOrDefault(),
                            projectID = projectID.GetValueOrDefault()
                        },
                        lOVProjects = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Projects),
                        lOVCRs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.ProjectCRs
                            , parentID: projectID.GetValueOrDefault()),
                        lOVComplaintStatus = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Complaint_Status)
                    });
                }
                else
                {
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;

                    return View(new Vw_Complaint()
                    {
                        complaint = new Ent_Complaint() { complaintID = -1, complaintStatus = 0 },
                        lOVProjects = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Projects),
                        lOVCRs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.EmptyList),
                        lOVComplaintStatus = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Complaint_Status)
                    });
                }
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Complaint_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_Complaint formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.complaint.complaintID != -1) ? true : false;

                if (ModelState.IsValid)
                {
                    formControl.complaint.makerID = User.Identity.Name;

                    if (formControl.complaint.attachFile != null)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(formControl.complaint.attachFile.FileName);
                        string FileExtension = Path.GetExtension(formControl.complaint.attachFile.FileName);
                        string UploadPath = ConfigurationManager.AppSettings["Attach_Files_Path"].ToString();
                        FileName = String.Concat("Compl~", formControl.complaint.CRID, "~", DateTime.Now.ToString("ddMMyyyyHHmmss")
                            , "~", formControl.complaint.makerID, "~", FileName.Trim(), FileExtension);
                        formControl.complaint.attachmentName = formControl.complaint.attachFile.FileName;
                        formControl.complaint.attachmentPath = FileName;
                        formControl.complaint.hasAttachment = true;
                        formControl.complaint.attachFile.SaveAs(UploadPath + FileName);
                    }
                    else
                    {
                        formControl.complaint.hasAttachment = false;
                    }

                    response = new Mdl_Complaint().insert_updateComplaint(formControl.complaint, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = Localization.Complaint.ComplaintAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.complaint.makerID, "Complaint_Create"
                    , formControl.complaint.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Complaint_Create, UserPermission.Complaint_Edit)]
        [Route("IsValidComplaintName")]
        public JsonResult IsValidComplaintName(Vw_Complaint formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.Complaint
                    , formControl.complaint.complaintID, formControl.complaint.comments);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Complaint_Create, UserPermission.Complaint_Edit)]
        [Route("getProjectCRs")]
        public JsonResult getProjectCRs(int projectID)
        {
            try
            {
                List<LOV> lovProjectCrs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.ProjectCRs, parentID: projectID);

                return Json(lovProjectCrs);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.Complaint_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_Complaint()
            {
                searchcomplaintStatus = -1
                ,
                lOVProjects = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Projects)
                ,
                lOVCRs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.ProjectCRs)
                ,
                lOVComplaintStatus = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Complaint_Status)
            });
        }

        [AuthorizeUser(UserPermission.Complaint_Search)]
        [Route("searchComplaint")]
        public ActionResult searchComplaint(int searchProjectID, int searchCRID, string searchComments
            , string searchDescription, int searchcomplaintStatus)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_Complaint> searchResult = new List<Ent_Complaint>();

            try
            {
                searchResult = new Mdl_Complaint().searchComplaint(searchProjectID, searchCRID, searchComments
                    , searchDescription, searchcomplaintStatus);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Complaint_Search", "searchProjectID: " + searchProjectID
                    + "searchCRID: " + searchCRID + "searchComments: " + searchComments + "~ searchDescription: "
                    + searchDescription + "searchcomplaintStatus: " + searchcomplaintStatus, response.ToString()
                    , response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Complaint_View)]
        [Route("View")]
        public ActionResult View(int ComplaintID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_Complaint Complaint = new Ent_Complaint();

            try
            {
                Complaint = new Mdl_Complaint().viewComplaint(ComplaintID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", Complaint);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Complaint_View", "FT ID: " + ComplaintID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Complaint_Edit)]
        [Route("Edit/{ComplaintID?}")]
        public ActionResult Edit(int ComplaintID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_Complaint Complaint = new Ent_Complaint();
            Vw_Complaint vw_Complaint = null;
            try
            {
                Complaint = new Mdl_Complaint().viewComplaint(ComplaintID);
                if (Complaint.comments == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.Complaint.ComplaintID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;
                    Session["updateID"] = ComplaintID;

                    vw_Complaint = new Vw_Complaint()
                    {
                        complaint = Complaint
                        ,
                        lOVProjects = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Projects)
                        ,
                        lOVCRs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.ProjectCRs, Complaint.projectID)
                        ,
                        lOVComplaintStatus = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Complaint_Status)
                    };
                    return View("Create", vw_Complaint);
                }

                return View("Create", vw_Complaint);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Complaint_View", "MT ID: " + ComplaintID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Complaint_Create, UserPermission.Complaint_Edit, UserPermission.Complaint_View)]
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
    }
}