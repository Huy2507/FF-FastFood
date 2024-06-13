using FF_Fastfood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FF_Fastfood.Attributes
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedRoles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            this.allowedRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            // Lấy thông tin người dùng từ session
            var user = httpContext.Session["User"] as Account;
            if (user == null)
            {
                return false;
            }

            // Kiểm tra vai trò của người dùng
            foreach (var role in allowedRoles)
            {
                if (user.role.Equals(role, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary(
                    new { controller = "Account", action = "Login" })
            );
        }
    }
}