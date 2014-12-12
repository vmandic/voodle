using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Voodle.Web.Controllers.Base;

namespace Voodle.Web.Controllers.WebApp
{
    public partial class UserController : BaseWebController
    {
        // GET: User
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult CreateGet() { return null; }

        public virtual ActionResult CreatePost() { return null; }

        public virtual ActionResult UpdateGet() { return null; }

        public virtual ActionResult UpdatePost() { return null; }

        public virtual ActionResult Delete() { return null; }

    }
}