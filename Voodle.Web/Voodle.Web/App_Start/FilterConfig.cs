using System.Web;
using System.Web.Mvc;
using Voodle.Web.Filters;

namespace Voodle.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new WebAppRequireHTTPS());
        }
    }
}
