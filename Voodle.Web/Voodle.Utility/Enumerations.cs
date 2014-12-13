using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voodle.Utility
{
    public enum ViewMode
    {
        Create,
        Update
    }

    public enum AppRole
    {
        [Description("System administrator")]
        SystemAdministrator = 1,
        [Description("Super user")]
        SuperUser = 2,
        [Description("Regular user")]
        RegularUser = 3
    }

    public interface IResponseModel<ResponseType>
    {
        ResponseType Response { get; set; }
        ResponseStatus Status { get; set; }
        string Message { get; set; }
    }

    public interface IResponseSaved
    {
        bool Saved { get; set; }
    }

    public enum ResponseStatus
    {
        OK = 200,
        SAVED = 201,
        ERROR = 500
    }

    public enum LoginStatus
    {
        SUCCESS = 200,
        NOT_CHECKED = 201,
        ERROR = 500,
        CREDENTIALS_FAIL = 404
    }

    public enum SmartphonePlatform
    {
        [Description("android")]
        Android,
        [Description("ios")]
        IOS,
        [Description("wp")]
        WindowsPhone,
        [Description("wsa")]
        WindowsStoreApp
    }

    public enum PushNotificationStatus
    {
        Unprocessed = 0,
        Processing = 100,
        Processed = 200,
        Error = 500
    }
}
