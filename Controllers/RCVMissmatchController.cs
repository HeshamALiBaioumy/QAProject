using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using QA.Entities.View_Entities;
using QA.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace QA.Controllers
{
    [Authorize]
    [RoutePrefix("RCVMissmatch")]
    public class RCVMissmatchController : BaseController
    {
        [AuthorizeUser(UserPermission.RCVMM_Create)]
        [Route("createRCVMissmatch")]
        public ActionResult createRCVMissmatch(int RCVID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            Vw_RCV_Missmatch view = null;
            try
            {
                if (RCVID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    view = new Vw_RCV_Missmatch()
                    {
                        RCVMissmatch = new Mdl_RCVMissmatch().viewRCVMissmatch(RCVID, int.Parse(User.Identity.Name)),
                        lOVSampleUnits = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Generic_Units)
                    };

                    view.RCVMissmatch.ID = -1;
                    view.RCVMissmatch.mapProject.exportJEOJSON = view.RCVMissmatch.mapProject.getProjectShape;
                    view.RCVMissmatch.mapCR.exportJEOJSON = view.RCVMissmatch.mapCR.getProjectShape;
                    view.RCVMissmatch.mapCR.turfCoordinates = view.RCVMissmatch.mapCR.getTurfCoordinates;

                    if (view.RCVMissmatch != null)
                    {
                        Session["Status"] = response.responseStatus = true;
                        Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                        return View(view);
                    }
                    else
                    {
                        view = new Vw_RCV_Missmatch();
                        Session["Status"] = response.responseStatus = false;
                        Session["EndMessage"] = response.endUserMessage = Localization.RCV_Missmatch.RCVNotValidMissmatch;

                        return RedirectToAction("SearchPending", "RandomCRVerification");
                    }
                }
                else
                {
                    view = new Vw_RCV_Missmatch();
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;

                    return RedirectToAction("SearchPending", "RandomCRVerification");
                }
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;

                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return RedirectToAction("SearchPending", "RandomCRVerification");
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "createRCVMissmatch", "RCV ID: " + RCVID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.RCVMM_Create)]
        [Route("createRCVMissmatchFromProject")]
        public ActionResult createRCVMissmatchFromProject(int projectID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            Vw_RCV_Missmatch view = null;
            try
            {
                if (projectID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    view = new Vw_RCV_Missmatch()
                    {
                        RCVMissmatch = new Mdl_RCVMissmatch().viewProjectMissmatch(projectID),
                        lOVSampleUnits = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Generic_Units)
                    };

                    view.RCVMissmatch.ID = -1;
                    view.RCVMissmatch.RCVID = -1;
                    view.RCVMissmatch.mapProject.exportJEOJSON = view.RCVMissmatch.mapProject.getProjectShape;
                    view.RCVMissmatch.mapProject.turfCoordinates = view.RCVMissmatch.mapProject.getTurfCoordinates;

                    if (view.RCVMissmatch != null)
                    {
                        Session["Status"] = response.responseStatus = true;
                        Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                        return View("createRCVMissmatch", view);
                    }
                    else
                    {
                        view = new Vw_RCV_Missmatch();
                        Session["Status"] = response.responseStatus = false;
                        Session["EndMessage"] = response.endUserMessage = Localization.RCV_Missmatch.RCVNotValidMissmatch;

                        return RedirectToAction("Search", "Project");
                    }
                }
                else
                {
                    view = new Vw_RCV_Missmatch();
                    Session["Status"] = false;
                    Session["EndMessage"] = Localization.ErrorMessages.ModifiedURLException;

                    return RedirectToAction("Search", "Project");
                }
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;

                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return RedirectToAction("Search", "Project");
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "createRCVMissmatchFromProject", "Project ID: " + projectID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.RCVMM_Create)]
        [Route("createRCVMissmatch")]
        public ActionResult createRCVMissmatch(Vw_RCV_Missmatch formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.RCVMissmatch.ID != -1) ? true : false;

                if (ModelState.ContainsKey("RCVMissmatch.caseDescription"))
                    ModelState["RCVMissmatch.caseDescription"].Errors.Clear();

                // Handle Model state Errors - Sample 1 Details
                if (formControl.RCVMissmatch.sample1.sampleMaker == null
                    && formControl.RCVMissmatch.sample1.sampleSize == null
                    && formControl.RCVMissmatch.sample1.sampleLength == null
                    && formControl.RCVMissmatch.sample1.sampleUnitID == -1
                    )
                {
                    if (ModelState.ContainsKey("RCVMissmatch.sample1.sampleMaker"))
                        ModelState["RCVMissmatch.sample1.sampleMaker"].Errors.Clear();

                    if (ModelState.ContainsKey("RCVMissmatch.sample1.sampleSize"))
                        ModelState["RCVMissmatch.sample1.sampleSize"].Errors.Clear();

                    if (ModelState.ContainsKey("RCVMissmatch.sample1.sampleLength"))
                        ModelState["RCVMissmatch.sample1.sampleLength"].Errors.Clear();

                    if (ModelState.ContainsKey("RCVMissmatch.sample1.sampleUnitID"))
                        ModelState["RCVMissmatch.sample1.sampleUnitID"].Errors.Clear();
                }

                // Handle Model state Errors - Sample 2 Details
                if (formControl.RCVMissmatch.sample2.sampleMaker == null
                    && formControl.RCVMissmatch.sample2.sampleSize == null
                    && formControl.RCVMissmatch.sample2.sampleLength == null
                    && formControl.RCVMissmatch.sample2.sampleUnitID == -1
                    )
                {
                    if (ModelState.ContainsKey("RCVMissmatch.sample2.sampleMaker"))
                        ModelState["RCVMissmatch.sample2.sampleMaker"].Errors.Clear();

                    if (ModelState.ContainsKey("RCVMissmatch.sample2.sampleSize"))
                        ModelState["RCVMissmatch.sample2.sampleSize"].Errors.Clear();

                    if (ModelState.ContainsKey("RCVMissmatch.sample2.sampleLength"))
                        ModelState["RCVMissmatch.sample2.sampleLength"].Errors.Clear();

                    if (ModelState.ContainsKey("RCVMissmatch.sample2.sampleUnitID"))
                        ModelState["RCVMissmatch.sample2.sampleUnitID"].Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    formControl.RCVMissmatch.makerID = User.Identity.Name;

                    if (formControl.RCVMissmatch.mapSample1.exportJEOJSON != null)
                    {
                        MapGeoJSON mapJSON = new JavaScriptSerializer().Deserialize<MapGeoJSON>(
                            formControl.RCVMissmatch.mapSample1.exportJEOJSON);
                        formControl.RCVMissmatch.mapSample1.mapPoints =
                            Ent_MapPoint.convertToPushpinMapPoint(mapJSON.features[0].geometry.coordinates.ToArray() as object[]);
                    }

                    if (formControl.RCVMissmatch.mapSample2.exportJEOJSON != null)
                    {
                        MapGeoJSON mapJSON = new JavaScriptSerializer().Deserialize<MapGeoJSON>(
                            formControl.RCVMissmatch.mapSample2.exportJEOJSON);
                        formControl.RCVMissmatch.mapSample2.mapPoints =
                            Ent_MapPoint.convertToPushpinMapPoint(mapJSON.features[0].geometry.coordinates.ToArray() as object[]);
                    }

                    bool isRcvCase = true;
                    if (formControl.RCVMissmatch.RCVID == -1)
                    {
                        isRcvCase = false;
                    }
                    response = new Mdl_RCVMissmatch().insert_updateRCVMissmatch(formControl.RCVMissmatch, isRcvCase
                        , isUpdateForm);

                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        if (isUpdateForm)
                        {
                            Session["EndMessage"] = response.endUserMessage = (Localization.RCV_Missmatch.RCV_Missmatch_Case
                                + response.UDF + ", " + Localization.RCV_Missmatch.RCVMM_Updated_Success);
                        }
                        else
                        {
                            Session["EndMessage"] = response.endUserMessage = (Localization.RCV_Missmatch.RCV_Missmatch_Case
                                + response.UDF + ", " + Localization.RCV_Missmatch.RCVMM_Added_Success);
                        }
                    }
                    else
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage;
                    }

                    if (isUpdateForm)
                    {
                        return RedirectToAction("Search", "RCVMissmatch");
                    }
                    else
                    {
                        return RedirectToAction("SearchPending", "RandomCRVerification");
                    }
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

                return View(formControl);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(formControl.RCVMissmatch.makerID, "createRCVMissmatch"
                    , formControl.RCVMissmatch.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.RCVMM_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_RCV_Missmatch()
            {
                searchPendingOn = -1,
                lOVRCVs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.RCVs),
                lOVProjects = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Projects),
                lOVCRs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.CRs),
                lOVUserProfiles = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile),
                lOVRCVMissmatchStatus = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.RCVMM_Status),
                lOVPendingOn = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.RCVMM_PendingOn)
            });
        }

        [AuthorizeUser(UserPermission.RCVMM_Search)]
        [Route("searchMissmatchCases")]
        public ActionResult searchMissmatchCases(int searchRCVID, int searchCRID, int searchProjectID
            , int searchProfileID, int searchRCVMMStatus, int searchPendingOn)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_RCV_Missmatch> searchResult = new List<Ent_RCV_Missmatch>();

            try
            {
                searchResult = new Mdl_RCVMissmatch().searchMissmatchCases(searchRCVID, searchCRID, searchProjectID
                    , searchProfileID, searchRCVMMStatus, searchPendingOn, int.Parse(User.Identity.Name));

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "searchMissmatchCases"
                    , "RCV ID: " + searchRCVID + "~ CR ID: " + searchCRID + "Project ID: " + searchProjectID
                    + "~ Profile ID: " + searchProfileID + "Status ID: " + searchRCVMMStatus
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.RCVMM_View)]
        [Route("View")]
        public ActionResult View(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_RCV_Missmatch view = new Ent_RCV_Missmatch();

            try
            {
                view = new Mdl_RCVMissmatch().viewRCVMMCase(ID, int.Parse(User.Identity.Name));
                view.mapProject.exportJEOJSON = view.mapProject.getProjectShape;
                view.mapCR.exportJEOJSON = view.mapCR.getProjectShape;

                MapPointsMethods MPM = new MapPointsMethods();
                view.mapSample1.exportJEOJSON = (view.mapSample1.mapID == -1) ? "" 
                    : MPM.getProjectShape(view.mapSample1.mapPoints);
                view.mapSample2.exportJEOJSON = (view.mapSample2.mapID == -1) ? ""
                    : MPM.getProjectShape(view.mapSample2.mapPoints);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", view);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "RCV Missmatch View", "RCV MM Case ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.RCVMM_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Vw_RCV_Missmatch view = null;
            try
            {
                view = new Vw_RCV_Missmatch()
                {
                    RCVMissmatch = new Mdl_RCVMissmatch().viewRCVMMCase(ID, int.Parse(User.Identity.Name)),
                    lOVSampleUnits = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Generic_Units)
                };

                if (view.RCVMissmatch.ID != -1)
                {
                    view.RCVMissmatch.mapProject.exportJEOJSON = view.RCVMissmatch.mapProject.getProjectShape;
                    view.RCVMissmatch.mapCR.exportJEOJSON = view.RCVMissmatch.mapCR.getProjectShape;
                    view.RCVMissmatch.mapCR.turfCoordinates = view.RCVMissmatch.mapCR.getTurfCoordinates;
                    MapPointsMethods MPM = new MapPointsMethods();
                    view.RCVMissmatch.mapSample1.exportJEOJSON = (view.RCVMissmatch.mapSample1.mapID == -1) ? ""
                        : MPM.getProjectShape(view.RCVMissmatch.mapSample1.mapPoints);
                    view.RCVMissmatch.mapSample1.turfCoordinates = (view.RCVMissmatch.mapSample1.mapID == -1) ? ""
                        : MPM.getTurfCoordinates(view.RCVMissmatch.mapSample1.mapPoints);

                    view.RCVMissmatch.mapSample2.exportJEOJSON = (view.RCVMissmatch.mapSample2.mapID == -1) ? ""
                        : MPM.getProjectShape(view.RCVMissmatch.mapSample2.mapPoints);
                    view.RCVMissmatch.mapSample2.turfCoordinates = (view.RCVMissmatch.mapSample2.mapID == -1) ? ""
                        : MPM.getTurfCoordinates(view.RCVMissmatch.mapSample2.mapPoints);

                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;
                }
                else
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.RCV_Missmatch.Edit_ID_Not_Available_inDB;
                }

                return View("createRCVMissmatch", view);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "RCV MM Case Edit", "ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}