using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Voodle.Utility;
using Voodle.Web.Utility;

namespace Voodle.Web.Filters
{
    public class WebAppAuthorizeRoles : AuthorizeAttribute
    {
        private readonly ICollection<AppRole> _allowedRoles;

        public WebAppAuthorizeRoles(params AppRole[] roles)
        {
            this._allowedRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return _allowedRoles.Contains(AppAuthentication.User.Role);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // toastr: error|info|success|warning
            filterContext.Result = new RedirectResult(VirtualPathUtility.ToAbsolute("~/Home?msg=Unauthorized access to the selected module!|error"), true);
        }
    }
}
