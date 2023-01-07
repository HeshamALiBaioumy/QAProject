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
    [RoutePrefix("Dashboard")]
    public class DashboardController : BaseController
    {
        [AuthorizeUser(UserPermission.Dashboard_SuperEng)]
        [Route("SuperEngDashboard")]
        public ActionResult SuperEngDashboard()
        {
            Vw_Dashboard dashboard = new Vw_Dashboard();

            try
            {
                dashboard.dashboard = new Ent_Dashboard() { makerID = User.Identity.Name };
                dashboard = new Mdl_Dashboard().SupervisorEngDashboard(int.Parse(dashboard.dashboard.makerID));

                if (dashboard.response.responseStatus)
                {
                    dashboard.response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;
                }
                else
                {
                    dashboard.dashboard = new Ent_Dashboard ();
                }

                return View(dashboard);
            }
            catch (Exception ex)
            {
                dashboard.response.responseStatus = false;
                dashboard.response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                dashboard.response.comments = ex.StackTrace;
                dashboard.response.errorMessage = ex.Message;
                dashboard.dashboard = null;

                return View(dashboard);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "SuperEng Dashboard", "User ID: " + User.Identity.Name
                    , dashboard.response.ToString(), dashboard.response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Dashboard_QualityEng)]
        [Route("QualityEngDashboard")]
        public ActionResult QualityEngDashboard()
        {
            Vw_Dashboard dashboard = new Vw_Dashboard();

            try
            {
                dashboard.dashboard = new Ent_Dashboard() { makerID = User.Identity.Name };
                dashboard = new Mdl_Dashboard().QualityEngDashboard(int.Parse(dashboard.dashboard.makerID));

                if (dashboard.response.responseStatus)
                {
                    dashboard.response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;
                }
                else
                {
                    dashboard.dashboard = null;
                }

                return View(dashboard);
            }
            catch (Exception ex)
            {
                dashboard.response.responseStatus = false;
                dashboard.response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                dashboard.response.comments = ex.StackTrace;
                dashboard.response.errorMessage = ex.Message;
                dashboard.dashboard = null;

                return View(dashboard);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Quality Eng Dashboard", "User ID: " + User.Identity.Name
                    , dashboard.response.ToString(), dashboard.response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Dashboard_ConsultantEng)]
        [Route("ConsultantEngDashboard")]
        public ActionResult ConsultantEngDashboard()
        {
            Vw_Dashboard dashboard = new Vw_Dashboard();

            try
            {
                dashboard.dashboard = new Ent_Dashboard() { makerID = User.Identity.Name };
                dashboard = new Mdl_Dashboard().ConsultantEngDashboard(int.Parse(dashboard.dashboard.makerID));

                if (dashboard.response.responseStatus)
                {
                    dashboard.response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;
                }
                else
                {
                    dashboard.dashboard = null;
                }

                return View(dashboard);
            }
            catch (Exception ex)
            {
                dashboard.response.responseStatus = false;
                dashboard.response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                dashboard.response.comments = ex.StackTrace;
                dashboard.response.errorMessage = ex.Message;
                dashboard.dashboard = null;

                return View(dashboard);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "ConsultantEng Dashboard", "User ID: " + User.Identity.Name
                    , dashboard.response.ToString(), dashboard.response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Dashboard_AuthLab)]
        [Route("AuthLabDashboard")]
        public ActionResult AuthLabDashboard()
        {
            Vw_Dashboard dashboard = new Vw_Dashboard();

            try
            {
                dashboard.dashboard = new Ent_Dashboard() { makerID = User.Identity.Name };
                dashboard = new Mdl_Dashboard().AuthLabDashboard(int.Parse(dashboard.dashboard.makerID));

                if (dashboard.response.responseStatus)
                {
                    dashboard.response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;
                }
                else
                {
                    dashboard.dashboard = null;
                }

                return View(dashboard);
            }
            catch (Exception ex)
            {
                dashboard.response.responseStatus = false;
                dashboard.response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                dashboard.response.comments = ex.StackTrace;
                dashboard.response.errorMessage = ex.Message;
                dashboard.dashboard = null;

                return View(dashboard);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "AuthLab Dashboard", "User ID: " + User.Identity.Name
                    , dashboard.response.ToString(), dashboard.response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Dashboard_Technician)]
        [Route("TechnicianDashboard")]
        public ActionResult TechnicianDashboard()
        {
            Vw_Dashboard dashboard = new Vw_Dashboard();

            try
            {
                dashboard.dashboard = new Ent_Dashboard() { makerID = User.Identity.Name };
                dashboard = new Mdl_Dashboard().TechnicianDashboard(int.Parse(dashboard.dashboard.makerID));

                if (dashboard.response.responseStatus)
                {
                    dashboard.response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;
                }
                else
                {
                    dashboard.dashboard = null;
                }

                return View(dashboard);
            }
            catch (Exception ex)
            {
                dashboard.response.responseStatus = false;
                dashboard.response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                dashboard.response.comments = ex.StackTrace;
                dashboard.response.errorMessage = ex.Message;
                dashboard.dashboard = null;

                return View(dashboard);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "Technician Dashboard", "User ID: " + User.Identity.Name
                    , dashboard.response.ToString(), dashboard.response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }

        [AuthorizeUser(UserPermission.Dashboard_CEO)]
        [Route("CEODashboard")]
        public ActionResult CEODashboard()
        {
            Vw_Dashboard dashboard = new Vw_Dashboard();

            try
            {
                dashboard.dashboard = new Ent_Dashboard() { makerID = User.Identity.Name };
                dashboard = new Mdl_Dashboard().CEODashboard(int.Parse(dashboard.dashboard.makerID));

                if (dashboard.response.responseStatus)
                {
                    dashboard.response.endUserMessage = Localization.Global.ViewResultsRetrievedSucessfully;
                }
                else
                {
                    dashboard.dashboard = null;
                }

                return View(dashboard);
            }
            catch (Exception ex)
            {
                dashboard.response.responseStatus = false;
                dashboard.response.endUserMessage = Localization.ErrorMessages.UnhandledErrorOccured;
                dashboard.response.comments = ex.StackTrace;
                dashboard.response.errorMessage = ex.Message;
                dashboard.dashboard = null;

                return View(dashboard);
            }
            finally
            {
                Mdl_Log log = new Mdl_Log(User.Identity.Name, "CEO Dashboard", "User ID: " + User.Identity.Name
                    , dashboard.response.ToString(), dashboard.response.responseStatus);

                Thread logThread = new Thread(() => log.insertLogMessge());
                logThread.Start();
            }
        }
    }
}