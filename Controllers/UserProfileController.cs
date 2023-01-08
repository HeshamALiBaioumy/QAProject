using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using QA.Entities.View_Entities;
using QA.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Configuration;
using System.Web.Mvc;

namespace QA.Controllers
{
    [Authorize]
    [RoutePrefix("UserProfile")]
    public class UserProfileController : BaseController
    {
        [AuthorizeUser(UserPermission.UserProfile_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            List<LOV> lstNationalities = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Nationalities);
            if (lstNationalities.Count > 1)
            {
                lstNationalities.RemoveAt(0);
            }

            ViewData["UserDefaultPassword"] = WebConfigurationManager.AppSettings["UserProfile_DefaultPassword"];

            return View(new Vw_UserProfile()
            {
                userProfile = new Ent_UserProfile()
                {
                    UserProfileID = -1,
                    contactDetails = new Ent_ContactDetails(),
                    nationalityTypeID = -1,
                    nationalityID = "SA",
                    isAssistantUser = true,
                    isActive = true,
                    expiryDate = DateTime.Now.AddYears(1)
                },
                lOVUserTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_UserTypes),
                lOVNationalityTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_NationalityTypes),
                lOVNationalities = lstNationalities,
                lOVSuperUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_SupervisorEngs),
                lOVRoles = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Roles),
                lOVProjectOwners = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Project_Owner)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.UserProfile_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_UserProfile formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.userProfile.UserProfileID != -1) ? true : false;
                if (ModelState.IsValid)
                {
                    formControl.userProfile.makerID = User.Identity.Name;
                    formControl.userProfile.password = WebConfigurationManager.AppSettings["UserProfile_DefaultPassword"];
                    formControl.userProfile.superUserID = (formControl.userProfile.isAssistantUser) ?
                        formControl.userProfile.superUserID : -1;
                    formControl.userProfile.isProjectOwnerMember = (formControl.userProfile.projectOwnerID == -1)
                        ? false : true;
                    response = new Mdl_UserProfile().insert_updateUserProfile(formControl.userProfile, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.userProfile.employeeName
                            + ": " + Localization.UserProfile.UserProfileAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.userProfile.makerID, "UserProfile_Create"
                    , formControl.userProfile.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.UserProfile_Create, UserPermission.UserProfile_Edit)]
        [Route("IsValidUserProfile")]
        public JsonResult IsValidUserProfile(Vw_UserProfile formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.UserProfile
                  , formControl.userProfile.UserProfileID, formControl.userProfile.userName);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.UserProfile_Create, UserPermission.UserProfile_Edit)]
        [Route("IsUniqueNID")]
        public JsonResult IsUniqueNID(Vw_UserProfile formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.UserProfile_UniqueNID
                  , formControl.userProfile.UserProfileID, formControl.userProfile.nationalId);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.UserProfile_Create, UserPermission.UserProfile_Edit)]
        [Route("IsUniqueEmplyeeID")]
        public JsonResult IsUniqueEmplyeeID(Vw_UserProfile formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.UserProfile_UniqueEmpID
                  , formControl.userProfile.UserProfileID, formControl.userProfile.employeeId);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.UserProfile_Create, UserPermission.UserProfile_Edit)]
        [Route("getSuperUsersDetails")]
        public JsonResult getSuperUsersDetails(int userTypeID)
        {
            try
            {
                List<LOV> lstLovSuperUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_UserType_SuperUsers
                        , parentID: userTypeID);
                return Json(new Vw_UserProfile()
                {
                    lOVSuperUsers = lstLovSuperUsers,
                    isAssistantUser = (lstLovSuperUsers.Count > 1) ? true : false
                });
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.UserProfile_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_UserProfile()
            {
                lOVUserTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_UserTypes),
                lOVNationalityTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Nationalities),
                lOVNationalities = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Nationalities),
                lOVSuperUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.EmptyList),
                lOVRoles = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Roles),
                lOVProjectOwners = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Project_Owner)
            });
        }

        [AuthorizeUser(UserPermission.UserProfile_Search)]
        [Route("searchUserProfile")]
        public ActionResult searchUserProfile(string searchEmployeeName, string searchUserName
            , string searchEmployeeId, string searchNationalId, string searchNationalityTypeID
            , string searchNationalityID, int searchSuperUserID, int searchUserTypeID
            , int searchIsActive, int searchIsLocked, int searchIsAssistant)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_UserProfile> searchResult = new List<Ent_UserProfile>();

            try
            {
                searchResult = new Mdl_UserProfile().searchUserProfile(searchEmployeeName, searchUserName
                    , searchEmployeeId, searchNationalId, searchNationalityTypeID, searchNationalityID
                    , searchSuperUserID, searchUserTypeID, searchIsActive, searchIsLocked, searchIsAssistant);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "UserProfile_Search", "EmployeeName: " + searchEmployeeName
                    + "~ UserName: " + searchUserName + "~ EmployeeId: " + searchEmployeeId
                    + "~ NationalId: " + searchNationalId + "~ NationalityTypeID: " + searchNationalityTypeID
                    + "~ NationalityID: " + searchNationalityID + "~ SuperUserID: " + searchSuperUserID
                    + "~ UserTypeID: " + searchUserTypeID + "~ IsActive: " + searchIsActive
                    + "~ IsLocked: " + searchIsLocked + "~ IsAssistant: " + searchIsAssistant, response.ToString()
                    , response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        // GET: UserProfile/Edit/5
        [AuthorizeUser(UserPermission.UserProfile_View)]
        [Route("View")]
        public ActionResult View(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_UserProfile UserProfile = new Ent_UserProfile();

            try
            {
                UserProfile = new Mdl_UserProfile().viewUserProfile(ID);

                response.responseStatus = true;
                response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                return PartialView("_viewSearchResult", UserProfile);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "UserProfile_View", "ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        // POST: UserProfile/Edit/5
        [AuthorizeUser(UserPermission.UserProfile_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_UserProfile UserProfile = new Ent_UserProfile();
            Vw_UserProfile vw_UserProfile = null;
            try
            {
                UserProfile = new Mdl_UserProfile().viewUserProfile(ID);
                if (UserProfile.userName == null)
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.UserProfile.UserProfileID_Not_Available_inDB;
                }
                else
                {
                    Session["Status"] = response.responseStatus = true;
                    Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                    List<LOV> lstNationalities = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Nationalities);
                    lstNationalities.RemoveAt(0);
                    return View("Create", new Vw_UserProfile()
                    {
                        userProfile = UserProfile,
                        lOVUserTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_UserTypes),
                        lOVNationalityTypes = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_NationalityTypes),
                        lOVNationalities = lstNationalities,
                        lOVSuperUsers = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_UserType_SuperUsers
                            , UserProfile.userTypeID),
                        lOVRoles = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserProfile_Roles),
                        lOVProjectOwners = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.Project_Owner)
                    });
                }

                return View("Create", vw_UserProfile);
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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "UserProfile_View", "UP ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}