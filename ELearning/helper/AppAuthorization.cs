using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using ELearning.Utility;

namespace ELearning.helper
{
    public class AppAuthorization : AuthorizeAttribute, IAuthorizationFilter
    {
        // public class ClaimRequirementFilter 
        //{
        readonly string _role;

        public AppAuthorization(string role)
        {
            _role = role;

        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.HttpContext.SignOutAsync();
                context.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                return;
            }

            var identity = (ClaimsIdentity)user.Identity;

            var name = identity.Claims.Where(c => c.Type == "role")
                               .Select(c => c.Value).SingleOrDefault();

            if (Convert.ToInt16(name) == Convert.ToInt16(RoleType.SuperAdmin)
                && _role== RoleType.SuperAdmin.ToString()
                )
            {
                return;
            }

            if (Convert.ToInt16(name) == Convert.ToInt16(RoleType.InstituteAdmin)
                && _role == RoleType.InstituteAdmin.ToString()
                )
            {
                return;
            }

            if (Convert.ToInt16(name) == Convert.ToInt16(RoleType.Student)
                && _role == RoleType.Student.ToString()
                )
            {
                return;
            }

            context.HttpContext.SignOutAsync();
            context.Result = new RedirectToRouteResult(
            new RouteValueDictionary(new { controller = "Home", action = "Index" }));
        }
    }
}
