using System.Web;
using System.Web.Mvc;
using Voodle.Web.Utility;

namespace Voodle.Web.Filters
{
    public class WebAppAuthorizeUser : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var cookie = httpContext.Request.Cookies[AppAuthentication.AuthCookieName];
            return httpContext.User.Identity.IsAuthenticated && cookie != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //the only proper way to redirect immediatelly
            filterContext.Result = new RedirectResult(VirtualPathUtility.ToAbsolute("~/Home/Login?returnUrl=" + HttpUtility.UrlEncode(filterContext.HttpContext.Request.RawUrl)), true);
        }
    }
}
