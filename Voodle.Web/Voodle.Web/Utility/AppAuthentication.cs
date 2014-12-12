using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Security;
using Voodle.BLL.Models.Base;
using Voodle.Utility;

namespace Voodle.Web.Utility
{
    public class AppAuthentication
    {
        public const string AuthCookieName = "voodleapp_auth";

        /// <summary>
        /// Wrapper around the FormsAuthentication.SetAuthCookie.
        /// </summary>
        /// <param name="userName">Username to save in the cookie.</param>
        /// <param name="persistentCookie">Make a persistant cookie.</param>
        public static void SetFormsAuthCookie(string userName, bool persistentCookie = true)
        {
            FormsAuthentication.SetAuthCookie(userName, persistentCookie);
        }

        public static void RenewAuthCookie(UserAuthenticationModel userModel, bool persistentCookie = true)
        {
            SetAppAuthCookie(userModel);
        }

        private static void SetAppAuthCookie(UserAuthenticationModel userModel)
        {
            var cookie = new HttpCookie(AuthCookieName);
            cookie.Expires = DateTime.Now.AddYears(3);
            cookie.Value = AppEncryption.Encrypt(JsonConvert.SerializeObject(userModel));

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void SetAuthCookie(UserAuthenticationModel userModel, bool persistentCookie = true)
        {
            SetFormsAuthCookie(userModel.Username, persistentCookie);
            SetAppAuthCookie(userModel);
        }

        internal static void SetAuthCookie(UserLoginModel userLoginModel, bool persistentCookie = true)
        {
            SetAuthCookie(userLoginModel.ToUserAuthenticationModel(), persistentCookie);
        }

        public static UserAuthenticationModel User
        {
            get
            {

                HttpCookie cookie = HttpContext.Current.Request.Cookies[AuthCookieName];

                if (cookie != null)
                    return JsonConvert.DeserializeObject<UserAuthenticationModel>(AppEncryption.DecryptToString(cookie.Value));
                else
                    return new UserAuthenticationModel();
            }
            set
            {
                SetAuthCookie(value);
            }
        }

        public static void SignOut()
        {
            FormsAuthentication.SignOut();

            if (HttpContext.Current.Request.Cookies[AuthCookieName] != null)
            {
                var cookie = new HttpCookie(AuthCookieName);
                cookie.Expires = DateTime.Now.AddDays(-1d);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }
    }

    public static class AppAuthenitcationExtensions
    {
        //public static UserAuthenticationModel ToUserAuthenticationModel(this UserProfileWebModel model)
        //{
        //    var uam = new UserAuthenticationModel()
        //    {
        //        GymID = model.GymID,
        //        RoleID = model.RoleID,
        //        UserID = model.UserID,
        //        Username = model.Username,
        //        Firstname = model.FirstName,
        //        Lastname = model.LastName
        //    };

        //    return uam;
        //}

        public static UserAuthenticationModel ToUserAuthenticationModel(this UserLoginModel model)
        {
            return new UserAuthenticationModel()
            {
                SetEncryptedRoleID = model.RoleID,
                SetEncryptedUserID = model.UserID,
                Username = model.Username,
                Firstname = model.Firstname,
                Lastname = model.Lastname
            };
        }
    }
}
