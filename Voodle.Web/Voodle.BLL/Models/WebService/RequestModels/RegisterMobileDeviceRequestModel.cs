namespace Voodle.BLL.Models.WebService.RequestModels
{
    public class RegisterMobileDeviceRequestModel
    {
        public int ClientID { get; set; }

        public string RegistrationID { get; set; }

        public string Platform { get; set; }

        public string DeviceID { get; set; }
    }
}
