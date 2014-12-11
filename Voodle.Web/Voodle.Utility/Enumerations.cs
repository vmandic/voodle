﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voodle.Utility
{
    public enum AppRole
    {
        SystemAdministrator = 1,
        SuperUser = 2,
        RegularUser = 3
    }

    public interface IResponseStatus
    {
        ResponseStatus ResponseStatus { get; set; }
        string ResponseInfo { get; set; }
    }

    public interface IResponseSaved
    {
        bool Saved { get; set; }
    }

    public enum ResponseStatus
    {
        OK = 200,
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
        WindowsPhone
    }

    public enum PushNotificationStatus
    {
        Unprocessed = 0,
        Processing = 100,
        Processed = 200,
        Error = 500
    }
}