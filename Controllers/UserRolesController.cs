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
    [RoutePrefix("UserRoles")]
    public class UserRolesController : BaseController
    {
        [AuthorizeUser(UserPermission.UserRoles_Create)]
        [Route("Create")]
        public ActionResult Create()
        {
            return View(new Vw_UserRoles()
            {
                userRoles = new Ent_UserRoles()
                {
                    userRoleID = -1,
                    isActive = true,
                },
                lOVinitialScreens = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserRoles_InitialScreens)
            });
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.UserRoles_Create)]
        [Route("Create")]
        public ActionResult Create(Vw_UserRoles formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                bool isUpdateForm = (formControl.userRoles.userRoleID != -1) ? true : false;
                if (ModelState.IsValid)
                {
                    formControl.userRoles.makerID = User.Identity.Name;
                    response = new Mdl_UserRoles().insert_updateRole(formControl.userRoles, isUpdateForm);
                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = formControl.userRoles.roleName
                            + ": " + Localization.UserRoles.UserRoleAddedSuccessfully;
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
                Mdl_Log log = new Mdl_Log(formControl.userRoles.makerID, "UserRoles_Create"
                    , formControl.userRoles.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [HttpPost]
        [AuthorizeUser(UserPermission.UserRoles_Create, UserPermission.UserRoles_Edit)]
        [Route("IsValidUserRoleName")]
        public JsonResult IsValidUserRoleName(Vw_UserRoles formControl)
        {
            try
            {
                bool status = true;

                status = new Mdl_NameExist().validateNameExist(Mdl_NameExist.searchEntities.UserRoles
                  , formControl.userRoles.userRoleID, formControl.userRoles.roleName);

                return Json(status);
            }
            catch (Exception ex)
            {
                return Json("Data Base Connection issue Occured !");
            }
        }

        [AuthorizeUser(UserPermission.UserRoles_Search)]
        [Route("Search")]
        public ActionResult Search()
        {
            return View(new Vw_UserRoles());
        }

        [AuthorizeUser(UserPermission.UserRoles_Search)]
        [Route("searchUserProfile")]
        public ActionResult searchUserRoles(string searchName, int searchIsActive)
        {
            ResponseMessage response = new ResponseMessage();
            List<Ent_UserRoles> searchResult = new List<Ent_UserRoles>();

            try
            {
                searchResult = new Mdl_UserRoles().searchRoles(searchName, searchIsActive);

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
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "UserRoles_Search", "RoleName: " + searchName
                    + "~ IsActive: " + searchIsActive, response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.UserRoles_View)]
        [Route("View")]
        public ActionResult View(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_UserRoles userRoles = new Ent_UserRoles();

            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    userRoles = new Mdl_UserRoles().viewRole(ID);

                    response.responseStatus = true;
                    response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;

                    return PartialView("_viewSearchResult", userRoles);
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
                response.responseStatus = false;
                response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                return Json(new { Status = false, Message = Localization.ErrorMessages.UnhandledErrorOccured });
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "UserRoles_View", "ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.UserRoles_Edit)]
        [Route("Edit/{ID?}")]
        public ActionResult Edit(int ID, string token)
        {
            ResponseMessage response = new ResponseMessage();
            Ent_UserRoles userRoles = new Ent_UserRoles();
            Vw_UserRoles vw_UserRoles = null;
            try
            {
                if (ID.ToString().Equals(URLParametersValidator.Decrypt(token)))
                {
                    userRoles = new Mdl_UserRoles().viewRole(ID);
                    if (userRoles.roleName == null)
                    {
                        Session["Status"] = response.responseStatus = false;
                        Session["EndMessage"] = response.endUserMessage = Localization.UserRoles.RoleID_Not_Available_inDB;
                    }
                    else
                    {
                        Session["Status"] = response.responseStatus = true;
                        Session["EndMessage"] = response.endUserMessage = Localization.Global.ViewDataRetrievedSuccessfully;

                        vw_UserRoles = new Vw_UserRoles()
                        {
                            userRoles = userRoles,
                            lOVinitialScreens = new Mdl_LOV().fillLOV(Mdl_LOV.searchEntities.UserRoles_InitialScreens)
                        };
                    }

                    return View("Create", vw_UserRoles);
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

                return View("Create");
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "UserRoles_Edit", "ID: " + ID
                    , response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}