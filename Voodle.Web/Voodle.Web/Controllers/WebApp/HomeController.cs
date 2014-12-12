using System.Web;
using System.Web.Mvc;
using Voodle.BLL.Models.Base;
using Voodle.BLL.StaticServices;
using Voodle.Utility;
using Voodle.Web.Controllers.Base;
using Voodle.Web.Utility;

namespace Voodle.Web.Controllers.WebApp
{
    public partial class HomeController : BaseWebController
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}
