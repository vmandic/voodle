using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voodle.BLL.Models.WebApp
{
    public class PushNotificationModel
    {
        public int MobileDeviceID { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
    }
}
