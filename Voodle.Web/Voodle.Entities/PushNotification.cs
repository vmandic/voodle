//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Voodle.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class PushNotification
    {
        public int ID { get; set; }
        public int MobileDeviceID { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime ModifiedAt { get; set; }
        public string Description { get; set; }
    
        public virtual MobileDevice MobileDevice { get; set; }
    }
}
