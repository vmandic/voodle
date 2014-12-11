using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Voodle.BLL.StaticServices;
using Voodle.Utility;
using Voodle.Web.Controllers.Base;
using Voodle.Web.Models;
using Voodle.Web.Utility;

namespace Voodle.Web.Controllers.WebApp
{
    public partial class HomeController : BaseWebController
    {
        // GET: Home
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl = null)
        {
            if (AppAuthentication.IsAuthenticated)
                return RedirectToAction(Index());

            return View();
        }

        // GET: /Security/Login
        [HttpPost]
        [AllowAnonymous]
        public virtual ActionResult Login(UserLoginModel model, string returnUrl)
        {
            if (model == null)
            {
                model = new UserLoginModel();
                model.Username = Request.Form[1];
                model.Password = Request.Form[2];
                model.RememberMe = true;
            }

            UserLoginModel userLoginModel = UserService.LoginByUsernameAndPassword(model.Username, model.Password);

            if (!string.IsNullOrEmpty(returnUrl))
                returnUrl = HttpUtility.UrlDecode(returnUrl);

            switch (userLoginModel.LoginStatus)
            {
                case LoginStatus.SUCCESS:
                    AppAuthentication.SetAuthCookie(userLoginModel);

                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction(Index());

                case LoginStatus.ERROR:
                    ModelState.AddModelError("", "An error happend during the login, please try logging later.");
                    break;

                case LoginStatus.CREDENTIALS_FAIL:
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    break;
            }

            return View(new UserLoginModel());
        }

        /// <summary>
        /// Get: // Logoff
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult Logout()
        {
            AppAuthentication.SignOut();
            return RedirectToAction(Login());
        }
    }
}
