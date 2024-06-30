using FF_Fastfood.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
            // Lấy thông tin người dùng từ cookie
            var userCookie = httpContext.Request.Cookies["UserCookie"];
            var userId = userCookie.Values["UserId"];
            if (userCookie == null)
            {
                return false;
            }


            using (FF_FastFoodEntities db = new FF_FastFoodEntities())
            {
                // Lấy các vai trò của người dùng từ cơ sở dữ liệu
                var userRoles = from ur in db.UserRoles
                                join r in db.Roles on ur.role_id equals r.RoleId
                                where ur.account_id.ToString() == userId
                                select r.RoleName;

                // Kiểm tra vai trò của người dùng
                foreach (var role in allowedRoles)
                {
                    if (userRoles.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new { controller = "Account", action = "Login" })
            );
        }
      
    }
}
