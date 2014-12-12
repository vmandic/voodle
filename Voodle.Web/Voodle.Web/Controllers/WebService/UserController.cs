using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Voodle.BLL.Models.Base;
using Voodle.BLL.Models.WebService.ResponseModels;
using Voodle.BLL.StaticServices;
using Voodle.Utility;
using Voodle.Web.Controllers.Base;

namespace Voodle.Web.Controllers.WebService
{
    public class UserController : BaseWsController
    {
        [HttpGet]
        public BaseResponseModel<UserModel> LoginAndSettings(string username, string password)
        {
            var resp = new UserLoginAndSettingsResponseModel();

            try
            {
                UserLoginModel userLoginModel = UserService.LoginByUsernameAndPassword(DbManager, username, password);

                switch (userLoginModel.LoginStatus)
                {
                    case LoginStatus.SUCCESS:
                        resp.Response = UserService.GetSingleByUserLoginModel_Mobile(DbManager, userLoginModel);
                        resp.Status = ResponseStatus.OK;
                        break;
                    case LoginStatus.ERROR:
                    case LoginStatus.CREDENTIALS_FAIL:
                        resp.Status = ResponseStatus.OK;
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex, Request);
                resp.Message = Logger.GetExceptionMessage(ex);
                resp.Status = ResponseStatus.ERROR;
            }

            return resp;
        }

        [HttpPost]
        public BaseResponseModel<UserModel> Update(BaseRequestModel<UserModel> model)
        {
            var resp = new BaseResponseModel<UserModel>();

            try
            {
                UserModel userModel = UserService.Update_Mobile(DbManager, model.Request);

                resp.Saved = userModel.Saved;
                resp.Response = userModel;
                resp.Status = ResponseStatus.OK;
            }
            catch (Exception ex)
            {
                Logger.Log(ex, Request);

                resp.Saved = false;
                resp.Message = Logger.GetExceptionMessage(ex);
                resp.Status = ResponseStatus.ERROR;
            }

            return resp;
        }

        [HttpPost]
        public BaseResponseModel<UserModel> Register(BaseRequestModel<UserModel> model)
        {
            var resp = new BaseResponseModel<UserModel>();

            try
            {
                UserModel userModel = UserService.Create_Mobile(DbManager, model.Request);

                resp.Saved = userModel.Saved;
                resp.Response = userModel;
                resp.Status = ResponseStatus.OK;
            }
            catch (Exception ex)
            {
                Logger.Log(ex, Request);

                resp.Saved = false;
                resp.Message = Logger.GetExceptionMessage(ex);
                resp.Status = ResponseStatus.ERROR;
            }

            return resp;
        }
    }
}
