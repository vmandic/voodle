using System;
using System.Collections.Generic;
using System.Web.Http;
using Voodle.BLL.Models.Base;
using Voodle.BLL.Models.WebService.RequestModels;
using Voodle.BLL.StaticServices;
using Voodle.Utility;
using Voodle.Web.Controllers.Base;

namespace Voodle.Web.Controllers.WebService
{
    public class PushController : BaseWsController
    {
        private readonly ICollection<string> _availableSmartphones = new List<string>()
        { 
            SmartphonePlatform.WindowsPhone.GetDescription(),
            SmartphonePlatform.WindowsStoreApp.GetDescription()
        };

        [HttpPost]
        public BaseResponseModel<bool> Register(BaseRequestModel<RegisterMobileDeviceRequestModel> model)
        {
            var resp = new SaveResponseModel();

            if (String.Equals(model.Request.RegistrationID.Trim(), "") ||
                String.Equals(model.Request.Platform.Trim(), "") ||
                model.Request.ClientID == 0)
            {
                resp.Message = "Required parameters are invalid.";
                resp.Status = ResponseStatus.OK;
            }

            //YODA CONDITION is in da house, please excuse me for this crap
            if (!_availableSmartphones.Contains(model.Request.Platform))
            {
                resp.Message = "Invalid Smartphone OS requested.";
                resp.Status = ResponseStatus.OK;
            }

            resp.Response = PushService.RegisterMobileDevice(DbManager, model.Request);
            return resp;
        }
    }
}
