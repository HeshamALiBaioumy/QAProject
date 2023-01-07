using QA.Entities.Business_Entities;
using QA.Entities.Reports_Entities;
using QA.Entities.Session_Entities;
using QA.Entities.View_Entities;
using QA.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace QA.Controllers
{
    [Authorize]
    [RoutePrefix("Project")]
    public class ProjectController : BaseController
    {
        [AuthorizeUser(UserPermission.Project_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_Project()
            {
                project = new Ent_Project()
                {
                    ID = -1,
                    isActive = true,
                    startDate = DateTime.Now,
                    endDate = DateTime.Now.AddMonths(3),
                    registerDate = DateTime.Now,
                    projectMileStones = new List<Ent_ProjectItem>(),
                    mapSelection = new Ent_MapSelection()
                    {
                        centerLatitude = WebConfigurationManager.AppSettings["map_Initial_Center_Lat"],
                        centerLongitude = WebConfigurationManager.AppSettings["map_Initial_Center_Lng"],
                        zoomLevel = int.Parse(WebConfigurationManager.AppSettings["map_Initial_Center_Zoom"]),
                        displayOnUserLocation = bool.Parse(WebConfigurationManager.AppSettings["map_Initial_CurrentLocation"])
                    }
                },
                lOVProjectOwners = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.projectOwner_IsOwner),
                lOVSupervisorEngineers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.SupervisorEngineers),
                lOVDepartments = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Department),
                lOVSections = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Department_Sections),
                lOVConsultants = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Consultants),
                lOVConsultantAssistant = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.EmptyList),
                lOVContractors = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Contractor),
                lOVContractorAssistant = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.EmptyList),
                lOVAuthorizedLabs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_AuthLab),
                lOVQATechnicians = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_QATech),
                lOVQualityAssuranceEngineers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_QualityEngineers),
                lOVMilestoneAmtUnits = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Generic_Units)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Project_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_Project formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.project.ID != -1) ? true : false;
                if (ModelState.IsValid)
                {
                    // Fill Model from JS controls
                    formControl.project.makerID = User.Identity.Name;
                    formControl.project.projectMileStones = Session["varprojectMilestones"] as List<Ent_ProjectItem>;
                    MapGeoJSON mapJSON = new JavaScriptSerializer().Deserialize<MapGeoJSON>(
                        formControl.project.mapSelection.exportJEOJSON);
                    formControl.project.mapSelection.projectMapSelectionType = Ent_MapSelection.mapSelectionType.Polygon;
                    formControl.project.mapSelection.mapPoints =
                        Ent_MapPoint.convertToMapPoints(mapJSON.features[0].geometry.coordinates[0] as object[]);

                    Session["varprojectMilestones"] = null;
                    response = new Mdl_Project().insert_updateProject(formControl.project, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.project.name
                            + ": " + Localization.Project.ProjectAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.project.makerID, "Project_Create"
                    , formControl.project.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Project_Create, UserPermission.Project_Edit)]
        [Route("setProjectMilestones")]
        public JsonResult setProjectMilestones(List<Ent_ProjectItem> projectMilestones)
        {
            try
            {
                Session["varprojectMilestones"] = projectMilestones;
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Project_Create, UserPermission.Project_Edit)]
        [Route("IsValidProject")]
        public JsonResult IsValidProject(Vw_Project formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.PROJECTS
                  , formControl.project.ID, formControl.project.name);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Project_Create, UserPermission.Project_Edit)]
        [Route("getProjectOwner_Related")]
        public JsonResult getProjectOwner_Related(int projectOwnerID)
        {
            try
            {
                return Json(new Vw_Project
                {
                    lOVSupervisorEngineers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.projectOwner_SupervisorEngineers
                        , parentID: projectOwnerID),
                    lOVDepartments = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.projectOwner_Departments
                        , parentID: projectOwnerID),
                    lOVSections = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.EmptyList)
                });
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Project_Create, UserPermission.Project_Edit)]
        [Route("getDepartmentSections")]
        public JsonResult getDepartmentSections(int departmentID)
        {
            try
            {
                return Json(new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Department_Sections
                        , parentID: departmentID));
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Project_Create, UserPermission.Project_Edit)]
        [Route("getConsultant_Assistants")]
        public JsonResult getConsultant_Assistants(int consultantID)
        {
            try
            {
                return Json(new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Consultant_Assistants
                        , parentID: consultantID));
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.Project_Create, UserPermission.Project_Edit)]
        [Route("getContractor_Assistants")]
        public JsonResult getContractor_Assistants(int contractorID)
        {
            try
            {
                return Json(new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Contractor_Assistants
                        , parentID: contractorID));
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.Project_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_Project()
            {
                lOVProjectOwners = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.projectOwner_IsOwner),
                lOVSupervisorEngineers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.SupervisorEngineers),
                lOVDepartments = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Department),
                lOVSections = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Sections),
                lOVConsultants = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Consultants),
                lOVConsultantAssistant = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_All_Consultant_Assistants),
                lOVContractors = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Contractor),
                lOVContractorAssistant = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_All_Contractor_Assistants),
                lOVAuthorizedLabs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_AuthLab),
                lOVQATechnicians = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_QATech),
                lOVQualityAssuranceEngineers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_QualityEngineers)
            });
        }

        [AuthorizeUser(UserPermission.Project_Search)]
        [Route("searchProject")]
        public ActionResult searchProject(string searchName, int searchProjectOwnerID, int searchSupervisorEngID
            , int searchDepartmentID, int searchDepartmentSectionID, int searchConsultantID
            , int searchConsultantAssistantID, int searchContractorID, int searchContractorAssistantID
            , int searchAuthorizedLabID, int searchQATechnicianID, int searchQualityEngineerID, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_Project> searchResult = new List<Ent_Project>();

            try
            {
                searchResult = new Mdl_Project().searchProject(searchName, searchProjectOwnerID, searchSupervisorEngID
                    , searchDepartmentID, searchDepartmentSectionID, searchConsultantID, searchConsultantAssistantID
                    , searchContractorID, searchContractorAssistantID, searchAuthorizedLabID, searchQATechnicianID
                    , searchQualityEngineerID, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Project_Search", "name: " + searchName
                    + " ~ ProjectOwner: " + searchProjectOwnerID + " ~ SupervisorEng: " + searchSupervisorEngID
                    + " ~ Department: " + searchDepartmentID + " ~ DepartmentSection: " + searchDepartmentSectionID
                    + " ~ Consultant: " + searchConsultantID + " ~ ConsultantAssis: " + searchConsultantAssistantID
                    + " ~ Contractor: " + searchContractorID + " ~ ContractorAssis: " + searchContractorAssistantID
                    + " ~ AuthorizedLab: " + searchAuthorizedLabID + " ~ QATechnician: " + searchQATechnicianID
                    + " ~ IsActive: " + searchIsActive, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Project_View)]
        [Route("View")]
        public ActionResult View(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_Project Project = new Ent_Project();

            try
            {
                Project = new Mdl_Project().viewProject(ID);
                Project.mapSelection.exportJEOJSON = Ent_MapPoint.prepareMapGeoJSON(Project.mapSelection.mapPoints);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", Project);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Project_View", "Project ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Project_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_Project Project = new Ent_Project();
            Vw_Project vw_Project = null;
            try
            {
                Project = new Mdl_Project().viewProject(ID);
                Project.mapSelection.exportJEOJSON = Ent_MapPoint.prepareMapGeoJSON(Project.mapSelection.mapPoints);
                if (Project.name == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.Project.projectID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    vw_Project = new Vw_Project()
                    {
                        project = Project,
                        lOVProjectOwners = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.projectOwner_IsOwner),
                        lOVSupervisorEngineers = new Mdl_LOV().fillLOV
                            (Mdl_LOV.searchEntities.SupervisorEngineers, Project.projectOwnerID),
                        lOVDepartments = new Mdl_LOV().fillLOV
                            (Mdl_LOV.searchEntities.Department, Project.projectOwnerID),
                        lOVSections = new Mdl_LOV().fillLOV
                            (Mdl_LOV.searchEntities.Department_Sections, Project.departmentID),
                        lOVConsultants = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Consultants),
                        lOVConsultantAssistant = new Mdl_LOV().fillLOV
                            (Mdl_LOV.searchEntities.UserProfile_Consultant_Assistants, Project.consultantID),
                        lOVContractors = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Contractor),
                        lOVContractorAssistant = new Mdl_LOV().fillLOV
                            (Mdl_LOV.searchEntities.UserProfile_Contractor_Assistants, Project.contractorID),
                        lOVAuthorizedLabs = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_AuthLab),
                        lOVQATechnicians = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_QATech),
                        lOVMilestoneAmtUnits = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Generic_Units),
                        lOVQualityAssuranceEngineers = new Mdl_LOV().fillLOV(
                            Mdl_LOV.searchEntities.UserProfile_QualityEngineers)
                    };
                }

                return View("Create", vw_Project);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Project_View", "Project ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Reports_Project_View)]
        [Route("ViewProject")]
        public ActionResult ViewProject(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            Rpt_Project view = null;
            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    view = new Mdl_Project().printProjectDetails(ID);
                    if (view != null)
                    {
                        MapPointsMethods MPM = new MapPointsMethods();
                        view.projectMapJEOJSON = MPM.getProjectShape(view.projectMapPoints);

                        Session["Status"] = response.responseStatus = true;
                        Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;
                    }
                    else
                    {
                        view = new Rpt_Project();
                        Session["Status"] = response.responseStatus = false;
                        Session["EndMessage"] = response.endUserMessage = Localization.CR_Workflow.NotAssignedCR;
                    }
                }
                else
                {
                    view = new Rpt_Project();
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "ViewProject", "Project ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}