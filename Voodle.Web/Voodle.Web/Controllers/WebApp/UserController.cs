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

        [HttpGet]
        public virtual ActionResult CreateGet()
        {



            return null;
        }

        [HttpPost]
        public virtual ActionResult CreatePost() { return null; }

        [HttpGet]
        public virtual ActionResult UpdateGet() { return null; }

        [HttpPost]
        public virtual ActionResult UpdatePost() { return null; }

        [HttpGet]
        public virtual ActionResult DeleteGet() { return null; }

    }
}