using QA.Entities.Business_Entities;
using QA.Entities.Reports_Entities;
using QA.Entities.Session_Entities;
using QA.Entities.View_Entities;
using QA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace QA.Controllers
{
    [Authorize]
    [RoutePrefix("CR")]
    public class CRController : BaseController
    {
        [AuthorizeUser(UserPermission.CR_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_CR()
            {
                CR = new Ent_CR() { CRID = -1, CRStatus = 0 },
                lOVProjects = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.ContractorAssistant_projects
                    , int.Parse(User.Identity.Name)),
                lOVCRTypeMCs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Type_MainCategories),
                lOVCRGroups = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Group_Types),
                lOVCRTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Types),
                lOVCRStatuses = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Statuses),
                lOVProjectItems = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Project_Items_All),
                lOVSampleUnits = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Generic_Units)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CR_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_CR formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.CR.CRID != -1) ? true : false;

                if (ModelState.IsValid)
                {
                    formControl.CR.makerID = User.Identity.Name;

                    MapGeoJSON mapJSON = new JavaScriptSerializer().Deserialize<MapGeoJSON>(
                        formControl.CR.mapSelection.exportJEOJSON);
                    formControl.CR.mapSelection.projectMapSelectionType = 
                        Ent_MapSelection.getDrawType(mapJSON.features[0].geometry.type);

                    if (formControl.CR.mapSelection.projectMapSelectionType == Ent_MapSelection.mapSelectionType.Polygon)
                    {
                        formControl.CR.mapSelection.mapPoints =
                        Ent_MapPoint.convertToMapPoints(mapJSON.features[0].geometry.coordinates[0] as object[]);
                    }
                    else if (formControl.CR.mapSelection.projectMapSelectionType == Ent_MapSelection.mapSelectionType.polyline)
                    {
                        formControl.CR.mapSelection.mapPoints =
                        Ent_MapPoint.convertToMapPoints(mapJSON.features[0].geometry.coordinates.ToArray() as object[]);
                    }

                    response = new Mdl_CR().insert_updateCR(formControl.CR, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        if (isUpdateForm)
                        {
                            Session["EndMessage"] = response.endUserMessage = (Localization.CR.CRID + formControl.CR.CRID
                                + ", " + Localization.CR.CRAddedSuccessfully);
                        }
                        else
                        {
                            Session["EndMessage"] = response.endUserMessage = (Localization.CR.CRID + response.UDF
                                + ", " + Localization.CR.CRAddedSuccessfully);
                        }
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
                Mdl_Log log = new Mdl_Log(formControl.CR.makerID, "CR_Create"
                    , formControl.CR.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CR_Create, UserPermission.CR_Edit)]
        [Route("getProjectItems")]
        public JsonResult getProjectItems(int projectID)
        {
            try
            {
                Vw_CR_Project view = null;
                if (projectID != -1)
                {
                    Ent_MapSelection mapSelection = new Mdl_Project().getProjectMapDetails(projectID);
                    mapSelection.exportJEOJSON = mapSelection.getProjectShape;
                    mapSelection.turfCoordinates = mapSelection.getTurfCoordinates;

                    view = new Vw_CR_Project()
                    {
                        lOVProjectItems = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Project_Items, parentID: projectID),
                        mapSelection = mapSelection
                    };
                }
                else
                {
                    view = new Vw_CR_Project()
                    {
                        lOVProjectItems = new List<LOV>(),
                        mapSelection = null
                    };
                }


                return Json(view);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CR_Create, UserPermission.CR_Edit)]
        [Route("getCRMCGroups")]
        public JsonResult getCRMCGroups(int MCID)
        {
            try
            {
                List<LOV> lovCRMCGroups = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CRType_MC_Groups, parentID: MCID);

                return Json(lovCRMCGroups);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CR_Create, UserPermission.CR_Edit)]
        [Route("checkCRTypeSampleRequired")]
        public JsonResult checkCRTypeSampleRequired(int CRTypeID)
        {
            try
            {
                bool isSampleRequired = new Mdl_CRType().viewCRType_SampleRequired(CRTypeID);

                return Json(isSampleRequired);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CR_Create, UserPermission.CR_Edit)]
        [Route("getCRTypes")]
        public JsonResult getCRTypes(int CRTypeID)
        {
            try
            {
                List<LOV> lovCRTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Group_Types, parentID: CRTypeID);

                return Json(lovCRTypes);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.CR_Action)]
        [Route("SearchPendingCrs")]
        public ActionResult SearchPendingCrs()
        {
            Vw_CR_Workflow view = null;
            try
            {
                view = new Vw_CR_Workflow()
                {
                    lstCRs = new Mdl_CR().searchPendingCRs(int.Parse(User.Identity.Name))
                };

                Session["Status"] = true;
                Session["EndMessage"] = Localization.CR_Workflow.PendingReceieved;
            }
            catch (Exception ex)
            {
                view = new Vw_CR_Workflow()
                {
                    lstCRs = null
                };

                Session["Status"] = false;
                Session["EndMessage"] = Localization.Global.UnhandledErrorOccured;
            }

            return View(view);
        }

        [AuthorizeUser(UserPermission.CR_Action)]
        [Route("SearchAssignedCrs")]
        public ActionResult SearchAssignedCrs()
        {
            Vw_CR_Workflow view = null;
            try
            {
                view = new Vw_CR_Workflow()
                {
                    lstCRs = new Mdl_CR().searchAssignedCRs(int.Parse(User.Identity.Name))
                };

                Session["Status"] = true;
                Session["EndMessage"] = Localization.CR_Workflow.PendingReceieved;
            }
            catch (Exception ex)
            {
                view = new Vw_CR_Workflow()
                {
                    lstCRs = null
                };

                Session["Status"] = false;
                Session["EndMessage"] = Localization.Global.UnhandledErrorOccured;
            }

            return View(view);
        }

        [AuthorizeUser(UserPermission.CR_Action)]
        [Route("recieveCR")]
        public ActionResult recieveCR(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    response = new Mdl_CR().recieveCR(ID, int.Parse(User.Identity.Name));

                    Session["Status"] = response.responseStatus;
                    Session["EndMessage"] = String.Concat(Localization.CR_Workflow.CR, ID
                        , Localization.CR_Workflow.Assigned_To_Success);
                }
                else
                {
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;
                }

                return RedirectToAction("SearchPendingCrs");
            }
            catch (Exception ex)
            {
                response.responseStatus = false;
                response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                Session["Status"] = response.responseStatus;
                Session["EndMessage"] = response.endUserMessage;

                return RedirectToAction("SearchPendingCrs");
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "recieveCR", "CR ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CR_Action)]
        [Route("ExecuteCR")]
        public ActionResult ExecuteCR(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            VW_CR_Execute view = null;
            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    view = new Mdl_CR().viewCR(ID, int.Parse(User.Identity.Name));
                    if (view != null)
                    {
                        view.projectMapSelection.exportJEOJSON = view.projectMapSelection.getProjectShape;
                        view.CR.mapSelection.exportJEOJSON = view.CR.mapSelection.getProjectShape;
                        view.CR.mapSelection.turfCoordinates = view.CR.mapSelection.getTurfCoordinates;
                        view.CR.sample.mapSelection.exportJEOJSON = view.CR.sample.mapSelection.getProjectShape;
                        view.CR.sample.mapSelection.turfCoordinates = view.CR.sample.mapSelection.getTurfCoordinates;

                        Session["Status"] = response.responseStatus = true;
                        Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;
                    }
                    else
                    {
                        view = new VW_CR_Execute();
                        Session["Status"] = response.responseStatus = false;
                        Session["EndMessage"] = response.endUserMessage = Localization.CR_Workflow.NotAssignedCR;
                    }
                }
                else
                {
                    view = new VW_CR_Execute();
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "ExecuteCR", "CR ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CR_Action)]
        [Route("ExecuteCR")]
        public ActionResult ExecuteCR(VW_CR_Execute view, string CRAction)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                bool isAcceptCR = (CRAction == @Localization.CR_Workflow.Accept_CR) ? true : false;

                view.CR.makerID = User.Identity.Name;

                // Handle MArker location if required From the user Step
                if (view.CR.isLabRequired && isAcceptCR)
                {
                    MapGeoJSON mapJSON = new JavaScriptSerializer().Deserialize<MapGeoJSON>(
                    view.CR.sample.mapSelection.exportJEOJSON);
                    view.CR.sample.mapSelection.mapPoints =
                        Ent_MapPoint.convertToPushpinMapPoint(mapJSON.features[0].geometry.coordinates.ToArray() as object[]);
                }

                response = new Mdl_CR().executeCR(view.CR.CRID, view.CR.isLabRequired
                    , view.CR.sample.mapSelection.mapPoints, isAcceptCR, view.CR.rejectReason);

                if (response.responseStatus)
                {
                    Session["Status"] = response.responseStatus;
                    Session["EndMessage"] = response.endUserMessage = Localization.CR.CRAddedSuccessfully;

                    return RedirectToAction("SearchAssignedCrs", "CR");
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
                Mdl_Log log = new Mdl_Log(view.CR.makerID, "ExecuteCR", "CRAction" + CRAction + "CRID: " + view.CR.CRID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CR_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_CR()
            {
                lOVProjects = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Projects),
                lOVProjectItems = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Project_Items_All),
                lOVCRTypeMCs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Type_MainCategories),
                lOVCRGroups = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CRType_Groups),
                lOVCRTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Types),
                lOVCRStatuses = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Statuses),
                searchStatus = -1,
                searchCRID = "%%"
            });
        }

        [AuthorizeUser(UserPermission.CR_Search)]
        [Route("SearchAllCrs")]
        public ActionResult SearchAllCrs(int searchProjectID, int searchProjectItemID, int searchCRMCID
            , int searchCRGroupID, int searchCRTypeID, int searchStatus, string searchCRID)
        {
            Vw_CR_Workflow view = null;
            try
            {
                view = new Vw_CR_Workflow()
                {
                    lstCRs = new Mdl_CR().searchAllCRs(int.Parse(User.Identity.Name), searchProjectID, searchProjectItemID
                        , searchCRMCID, searchCRGroupID, searchCRTypeID, searchStatus, searchCRID)
                };
            }
            catch (Exception ex)
            {
                view = new Vw_CR_Workflow()
                {
                    lstCRs = null
                };
            }

            return PartialView("_SearchResults", view);
        }

        [AuthorizeUser(UserPermission.CR_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            VW_CR_Execute respView = null;
            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    respView = new Mdl_CR().viewCR(ID, int.Parse(User.Identity.Name));
                    if (respView != null)
                    {
                        respView.CR.mapSelection.exportJEOJSON = respView.projectMapSelection.getProjectShape;
                        respView.CR.mapSelection.turfCoordinates = respView.CR.mapSelection.getTurfCoordinates;

                        Session["Status"] = response.responseStatus = true;
                        Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;
                    }
                    else
                    {
                        respView = new VW_CR_Execute();
                        Session["Status"] = response.responseStatus = false;
                        Session["EndMessage"] = response.endUserMessage = Localization.CR_Workflow.NotAssignedCR;
                    }

                    Vw_CR editView = new Vw_CR()
                    {
                        CR = respView.CR,
                        lOVProjects = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.ContractorAssistant_projects
                        , int.Parse(User.Identity.Name)),
                        lOVCRTypeMCs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Type_MainCategories),
                        lOVCRGroups = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CRType_MC_Groups, parentID: respView.CR.CRTypeMCID),
                        lOVCRTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Group_Types, parentID: respView.CR.CRTypeGroupID),
                        lOVCRStatuses = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_Statuses),
                        lOVProjectItems = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Project_Items, parentID: respView.CR.projectID),
                        lOVSampleUnits = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Generic_Units)
                    };

                    return View("Create", editView);
                }
                else
                {
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Edit CR", "CR ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CR_AddAttachments)]
        [Route("addAttachments/{ID?}")]
        public ActionResult addAttachments(int ID, string token)
        {
            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    Ent_Attachment attachment = new Ent_Attachment()
                    {
                        parentID = ID,
                        sampleResult = -1,
                        lOVSampleTestCategories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Sample_Categories),
                        lOVSampleTests = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.EmptyList),
                        lOVSampleTestResult = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_SampleTest_Result)
                    };

                    return View(attachment);
                }
                else
                {
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;
                    return RedirectToAction("Search");
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CR_AddAttachments)]
        [Route("getSampleCategoryTests")]
        public JsonResult getSampleCategoryTests(int categoryID)
        {
            try
            {
                List<LOV> lOVSampleTests = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Sample_Category_Tests
                    , parentID: categoryID);

                return Json(lOVSampleTests);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.CR_AddAttachments)]
        [Route("addAttachments")]
        public ActionResult addAttachments(Ent_Attachment formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                if (ModelState.IsValid)
                {
                    formControl.makerID = User.Identity.Name;

                    if (formControl.attachFile != null)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(formControl.attachFile.FileName);
                        string FileExtension = Path.GetExtension(formControl.attachFile.FileName);
                        string UploadPath = ConfigurationManager.AppSettings["Attach_Files_Path"].ToString();
                        FileName = String.Concat("CR~", formControl.parentID, "~", DateTime.Now.ToString("ddMMyyyyHHmmss")
                            , "~", formControl.makerID, "~", FileName.Trim(), FileExtension);
                        formControl.attachmentName = formControl.attachFile.FileName;
                        formControl.attachmentPath = FileName;
                        formControl.hasAttachment = true;
                        formControl.attachFile.SaveAs(UploadPath + FileName);

                        response = new Mdl_CR().addAttachments(formControl);
                        if (response.responseStatus)
                        {
                            Session["Status"] = response.responseStatus;
                            Session["EndMessage"] = response.endUserMessage = Localization.Attachment.AttachAddedSuccess;
                        }
                        else
                        {
                            Session["Status"] = response.responseStatus;
                            Session["EndMessage"] = response.endUserMessage;
                        }
                    }
                    else
                    {
                        Session["Status"] = response.responseStatus = false;
                        Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                        Session["attachFileNotExist"] = true;
                    }
                }
                else
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;

                    if (ModelState["attachmentFile"].Errors.Count > 0)
                    {
                        Session["attachFileNotExist"] = true;
                    }

                    response.comments = null;
                    response.errorMessage = ModelState.ToString();
                }

                formControl.lOVSampleTestCategories = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Sample_Categories);
                formControl.lOVSampleTests = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.EmptyList);
                formControl.lOVSampleTestResult = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CR_SampleTest_Result);
                return View(formControl);
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
                Mdl_Log log = new Mdl_Log(formControl.makerID, "addAttachments"
                    , formControl.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.CR_ViewAttachments)]
        [Route("viewAttachments/{ID?}")]
        public ActionResult viewAttachments(int ID, string token)
        {
            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    List<Ent_Attachment> lstAttachments = new Mdl_CR().viewAttachments(CRID: ID);
                    return View(lstAttachments);
                }
                else
                {
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;
                    return RedirectToAction("Search");
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [AuthorizeUser(UserPermission.CR_ViewAttachments)]
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

        [AuthorizeUser(UserPermission.Reports_CR_View)]
        [Route("ViewCR")]
        public ActionResult ViewCR(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            Rpt_CR view = null;
            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    view = new Mdl_CR().printCR(ID);
                    if (view != null)
                    {
                        MapPointsMethods MPM = new MapPointsMethods();
                        view.projectMapJEOJSON = MPM.getProjectShape(view.projectMapPoints);
                        view.CrMapJEOJSON = MPM.getProjectShape(view.crMapPoints);
                        view.SampleMapJEOJSON = (view.sampleMapPoints == null)
                            ? "" : MPM.getProjectShape(view.sampleMapPoints);

                        Session["Status"] = response.responseStatus = true;
                        Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;
                    }
                    else
                    {
                        view = new Rpt_CR();
                        Session["Status"] = response.responseStatus = false;
                        Session["EndMessage"] = response.endUserMessage = Localization.CR_Workflow.NotAssignedCR;
                    }
                }
                else
                {
                    view = new Rpt_CR();
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "ViewCR", "CR ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Reports_CR_Search)]
        [Route("SearchPrintCRs")]
        public ActionResult SearchPrintCRs()
        {
            return View(new Vw_CR_Report_ProjectCrs()
            {
                searchCRDateFrom = DateTime.Now,
                searchCRDateTo = DateTime.Now
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Reports_CR_Search)]
        [Route("SearchPrintCRs")]
        public ActionResult SearchPrintCRs(Vw_CR_Report_ProjectCrs formControl)
        {
            bool requestStatus = true;
            Vw_SearchCr_Report view = null;
            try
            {
                view = new Mdl_CR().printSearchCR(formControl.searchCRDateFrom, formControl.searchCRDateTo);
                return View("ViewPrintCRs", view);
            }
            catch (Exception ex)
            {
                view = new Vw_SearchCr_Report()
                {
                    searchDateFrom = formControl.searchCRDateFrom,
                    searchDateTo = formControl.searchCRDateTo,
                    lstSearchCRs = null
                };

                Session["Status"] = false;
                Session["EndMessage"] = Localization.ErrorMessages.UnhandledErrorOccured;
                requestStatus = false;
                return View(view);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "SearchPrintCRs", "From: " + formControl.searchCRDateFrom + "~ To: "
                    + formControl.searchCRDateTo, view.ToString(), requestStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Reports_CR_Projects_Search)]
        [Route("SearchPrintProjectCRs")]
        public ActionResult SearchPrintProjectCRs()
        {
            return View(new Vw_CR_Report_ProjectCrs()
            {
                lOVProjects = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Projects),
                searchCRDateFrom = DateTime.Now,
                searchCRDateTo = DateTime.Now
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Reports_CR_Projects_Search)]
        [Route("SearchPrintProjectCRs")]
        public ActionResult SearchPrintProjectCRs(Vw_CR_Report_ProjectCrs formControl)
        {
            bool requestStatus = true;
            Vw_SearchCr_Report view = null;
            try
            {
                view = new Mdl_CR().printSearchProjectCR(formControl.searchProjectID
                    , formControl.searchCRDateFrom, formControl.searchCRDateTo);
                return View("ViewPrintProjectCRs", view);
            }
            catch (Exception ex)
            {
                view = new Vw_SearchCr_Report()
                {
                    searchProjectID = formControl.searchProjectID,
                    searchDateFrom = formControl.searchCRDateFrom,
                    searchDateTo = formControl.searchCRDateTo,
                    lstSearchCRs = null
                };

                Session["Status"] = false;
                Session["EndMessage"] = Localization.ErrorMessages.UnhandledErrorOccured;
                requestStatus = false;
                return View(view);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "SearchPrintProjectCRs"
                    , "~ Project: " + formControl.searchProjectID + "From: " + formControl.searchCRDateFrom
                    + "~ To: " + formControl.searchCRDateTo, view.ToString(), requestStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Reports_CR_Samples_Search)]
        [Route("SearchPrintCRSamples")]
        public ActionResult SearchPrintCRSamples()
        {
            return View(new Vw_CR_Report_ProjectCrs()
            {
                searchCRDateFrom = DateTime.Now,
                searchCRDateTo = DateTime.Now
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Reports_CR_Samples_Search)]
        [Route("SearchPrintCRSamples")]
        public ActionResult SearchPrintCRSamples(Vw_CR_Report_ProjectCrs formControl)
        {
            bool requestStatus = true;
            Vw_SearchCr_Report view = null;
            try
            {
                view = new Mdl_CR().printCRSamples(formControl.searchCRDateFrom, formControl.searchCRDateTo);
                return View("ViewPrintCRSamples", view);
            }
            catch (Exception ex)
            {
                view = new Vw_SearchCr_Report()
                {
                    searchDateFrom = formControl.searchCRDateFrom,
                    searchDateTo = formControl.searchCRDateTo,
                    lstSearchCRs = null
                };

                Session["Status"] = false;
                Session["EndMessage"] = Localization.ErrorMessages.UnhandledErrorOccured;
                requestStatus = false;
                return View(view);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "SearchPrintCRSamples", "From: "
                    + formControl.searchCRDateFrom + "~ To: " + formControl.searchCRDateTo, view.ToString(), requestStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Reports_CR_Samples_SearchDetailed)]
        [Route("SearchPrintCRSamplesDetailed")]
        public ActionResult SearchPrintCRSamplesDetailed()
        {
            return View(new Vw_CR_Report_ProjectCrs()
            {
                lOVProjects = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Projects),
                searchCRDateFrom = DateTime.Now,
                searchCRDateTo = DateTime.Now
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Reports_CR_Samples_SearchDetailed)]
        [Route("SearchPrintCRSamplesDetailed")]
        public ActionResult SearchPrintCRSamplesDetailed(Vw_CR_Report_ProjectCrs formControl)
        {
            bool requestStatus = true;
            Vw_SearchCr_Report view = null;
            try
            {
                view = new Mdl_CR().printCRSamplesDetailed(formControl.searchProjectID
                  , formControl.searchCRDateFrom, formControl.searchCRDateTo);
                return View("ViewPrintCRSamplesDetailed", view);
            }
            catch (Exception ex)
            {
                view = new Vw_SearchCr_Report()
                {
                    searchProjectID = formControl.searchProjectID,
                    searchDateFrom = formControl.searchCRDateFrom,
                    searchDateTo = formControl.searchCRDateTo,
                    lstSearchCRs = null
                };

                Session["Status"] = false;
                Session["EndMessage"] = Localization.ErrorMessages.UnhandledErrorOccured;
                requestStatus = false;
                return View(view);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Reports_CR_Samples_SearchDetailed"
                    , "~ Project: " + formControl.searchProjectID + "From: " + formControl.searchCRDateFrom
                    + "~ To: " + formControl.searchCRDateTo, view.ToString(), requestStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        //[AuthorizeUser(UserPermission.CR_Create)]
        //[Route("TestMap")]
        //public ActionResult TestMap()
        //{
        //    Ent_MapSelection mapSelection = new Mdl_Project().getProjectMapDetails(102);
        //    mapSelection.exportJEOJSON = mapSelection.getProjectShape;
        //    mapSelection.turfCoordinates = mapSelection.getTurfCoordinates;

        //    return View(new Vw_CR()
        //    {
        //        CR = new Ent_CR()
        //        {
        //            CRID = -1,
        //            CRStatus = 0,
        //            mapSelection = mapSelection
        //        }
        //    });
        //}
    }
}