using QA.Entities.Business_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QA.Entities.Session_Entities
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        private readonly UserPermission[] allowedPermissionLevels;

        public AuthorizeUserAttribute(params UserPermission[] permissionLevels)
        {
            this.allowedPermissionLevels = permissionLevels;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;

            IList<UserPermission> theApprovedPermissionLevelList = GetApprovedUserPermissionlevels();

            foreach (UserPermission permissionLevel in allowedPermissionLevels)
            {
                if (theApprovedPermissionLevelList.Any(a => a == permissionLevel) == true)
                {
                    authorize = true;
                }
            }

            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Login", action = "UnAuthorizedAccess" }));
        }

        private IList<UserPermission> GetApprovedUserPermissionlevels()
        {
            try
            {
                IList<UserPermission> theApprovedRoles = (IList<UserPermission>)HttpContext.Current.Session["UserRoles"];
                return theApprovedRoles;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //If they are authorized, handle accordingly
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}