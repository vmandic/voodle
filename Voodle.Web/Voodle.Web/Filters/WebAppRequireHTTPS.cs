using System;
using System.Web.Mvc;

namespace Voodle.Web.Filters
{
    /// <summary>
    /// Redirects incoming request to secure HTTP using SSL.
    /// </summary>
    public class WebAppRequireHTTPS : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (filterContext.HttpContext.Request.IsLocal)
            {
                // when connection to the application is local, don't do any HTTPS stuff
                return;
            }

            base.OnAuthorization(filterContext);
        }
    }
}
