using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Voodle.BLL.Models.Base;
using Voodle.BLL.StaticServices;
using Voodle.Utility;
using Voodle.Web.Controllers.Base;

namespace Voodle.Web.Controllers.WebApp
{
    public partial class UserController : BaseWebController
    {
        [NonAction]
        private ActionResult CreateOrUpdateGet(ViewMode viewMode, int userId = 0)
        {
            var userModel = new UserModel();
            ViewBag.ViewMode = viewMode;

            switch (viewMode)
            {
                case ViewMode.Update:
                    userModel = UserService.GetSingleById(DbManager, userId);
                    break;
            }

            ViewBag.Roles = UserService.GetRoles(DbManager);
            return View(MVC.User.Views.ViewNames.AddDetailsUser, userModel);
        }

        [NonAction]
        private ActionResult CreateOrUpdatePost(ViewMode viewMode, UserModel userModel = null)
        {
            bool saved = false;
            ViewBag.ViewMode = viewMode;
            var resp = new JsonResult();

            try
            {
                switch (viewMode)
                {
                    // TODO: here we be... @zoki
                    case ViewMode.Create:
                        break;
                    case ViewMode.Update:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex, Request);
                resp.Data = new { saved = saved, responseStatus = ResponseStatus.OK, responseMessage = "A server error occured!", errorMessage = ex.Message };
            }

            return resp;
        }

        [HttpGet]
        [Route("/List")]
        [Route("/Index")]
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public virtual ActionResult CreateGet()
        {
            return CreateOrUpdateGet(ViewMode.Create);
        }

        [HttpPost]
        public virtual ActionResult CreatePost(UserModel userModel)
        {
            return CreateOrUpdatePost(ViewMode.Create, userModel);
        }

        [HttpGet]
        public virtual ActionResult UpdateGet(string id)
        {
            return CreateOrUpdateGet(ViewMode.Update, AppEncryption.DecryptToInt(id));
        }

        [HttpPost]
        public virtual ActionResult UpdatePost(UserModel userModel)
        {
            return CreateOrUpdatePost(ViewMode.Update, userModel);
        }

        [HttpGet]
        public virtual ActionResult DeleteGet(string id)
        {
            return new JsonResult() { Data = new { removed = UserService.DeleteById(DbManager, AppEncryption.DecryptToInt(id)), responseStatus = ResponseStatus.OK }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}