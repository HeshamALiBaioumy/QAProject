using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using QA.Models;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QA.Controllers
{
    [RoutePrefix("Login")]
    public class LoginController : BaseController
    {
        [HttpGet]
        [Route("login")]
        public ActionResult Login(string returnURL)
        {
            Ent_Login loginErrorMessage = new Ent_Login() { errorMessage = "" };
            return View(loginErrorMessage);
        }

        [HttpPost]
        [Route("login")]
        public ActionResult login(Ent_Login loginCredentials, string ReturnUrl)
        {
            ResponseLogin response = null;
            Exception logException = null;
            try
            {
                if (ModelState.IsValid)
                {
                    response = new Mdl_Login().validateLogin(loginCredentials);

                    if (response.responseStatus && response.userID != -1 && response.userRoles != null)
                    {
                        if (false/* response.isInitialPassword*/)
                        {
                            Session["currentUser"] = loginCredentials.userName;
                            Session["SideMenuRoles"] = response.userRoles;
                            IList<UserPermission> theApprovedRoles = response.userRoles.convertUserRolesToRolesList();
                            Session["UserRoles"] = theApprovedRoles;

                            FormsAuthentication.SetAuthCookie(response.userID.ToString(), false);
                            return RedirectToAction("ResetPassword");
                        }
                        else
                        {
                            // Save User ID
                            FormsAuthentication.SetAuthCookie(response.userID.ToString(), false);
                            Session["currentUser"] = loginCredentials.userName;

                            // Save SideBar Menu Roles
                            Session["SideMenuRoles"] = response.userRoles;

                            // Save User Access Roles for Controller
                            IList<UserPermission> theApprovedRoles = response.userRoles.convertUserRolesToRolesList();
                            Session["UserRoles"] = theApprovedRoles;

                            if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1
                                && ReturnUrl.StartsWith("/") && !(ReturnUrl.StartsWith("//"))
                                && !(ReturnUrl.StartsWith("/\\")))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return RedirectToAction(response.userRoles.initialScreenName, "Dashboard");
                            }
                        }
                    }
                    else
                    {
                        Ent_Login loginErrorMessage = new Ent_Login() { errorMessage = "Invalid User ID" };
                        ModelState.AddModelError("", Localization.Login.AccessDenied);
                        return View(loginErrorMessage);
                    }
                }
                else
                {
                    Ent_Login loginErrorMessage = new Ent_Login() { errorMessage = "Invalid Object" };
                    ModelState.AddModelError("", Localization.Login.AccessDenied);
                    return View(loginErrorMessage);
                }
            }
           
            catch (Exception ex)
            {
                logException = ex;

                Ent_Login loginErrorMessage = new Ent_Login() { errorMessage = ex.Message };
                ModelState.AddModelError("", Localization.Global.UnhandledErrorOccured);
                return View(loginErrorMessage);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(loginCredentials.userName, "login"
                    , loginCredentials.ToString(), (response != null) ? response.ToString() : ""
                    , logException, (response != null) ? response.responseStatus : false);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [Authorize]
        [Route("ResetPassword")]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [Route("ResetPassword")]
        public ActionResult ResetPassword(Ent_Login_ResetPassword formControl)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                if (ModelState.IsValid)
                {
                    formControl.userID = int.Parse(User.Identity.Name);
                    response = new Mdl_Login().changePassword(formControl);

                    if (response.responseStatus)
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage = Localization.Login.ResetPassword_PasswordChangedSuccess;
                    }
                    else
                    {
                        Session["Status"] = response.responseStatus;
                        Session["EndMessage"] = response.endUserMessage;
                    }

                    return RedirectToAction("logout");
                }
                else
                {
                    Session["Status"] = response.responseStatus = false;
                    Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                    response.comments = null;
                    response.errorMessage = "Invalid Model Object (Parameters)";

                    ModelState.AddModelError("", "Invalid Model Object (Parameters)");

                    return View();
                }
            }
            catch (Exception ex)
            {
                Session["Status"] = response.responseStatus = false;
                Session["EndMessage"] = response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                response.comments = ex.StackTrace;
                response.errorMessage = ex.Message;

                ModelState.AddModelError("", "Unhandled Error Occured !!");

                return View();
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(formControl.userID.ToString(), "ResetPassword"
                    , formControl.ToString(), response.ToString(), response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [Route("logout")]
        [Authorize]
        [AllowAnonymous]
        public ActionResult logout()
        {
            Session["currentUser"] = null;
            Session["Status"] = null;
            Session["EndMessage"] = null;
            Session["varprojectMilestones"] = null;
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();

            FormsAuthentication.SignOut();
            return RedirectToAction("login", controllerName: "Login");
        }

        [Route("unAuthorizedAccess")]
        public ActionResult UnAuthorizedAccess()
        {
            return View();
        }

        [Route("Error")]
        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }

        [Route("SetCulture")]
        [AllowAnonymous]
        public ActionResult SetCulture(string culture, string returnURL)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);

            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }

            Response.Cookies.Add(cookie);
            return Redirect(returnURL);
        }
    }
}