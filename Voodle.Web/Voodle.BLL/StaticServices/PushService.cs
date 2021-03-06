﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voodle.BLL.Models.WebService.RequestModels;
using Voodle.BLL.Repository;
using Voodle.Entities;
using Voodle.BLL.Converters;
using Voodle.BLL.Models.WebApp;

namespace Voodle.BLL.StaticServices
{
    public static class PushService
    {
        public static bool PushMessage(DbContextManager dbManager, PushNotificationModel model)
        {
            bool saved = false;
            var repo = new GenericRepository<PushNotification>(dbManager.Context);

            PushNotification pushNotificationEntity = model.ToPushNotificationEntity();
            repo.Create(pushNotificationEntity);
            saved = repo.SaveChanges();

            return saved;
        }

        public static bool RegisterMobileDevice(DbContextManager dbManager, RegisterMobileDeviceRequestModel model)
        {
            IGenericRepository<MobileDevice> repo = new GenericRepository<MobileDevice>(dbManager.Context);

            //check if the device for the user already exists in the database, if so, just do an update
            MobileDevice device = repo.Find(x => x.UserID == model.ClientID && String.Equals(x.DeviceID, model.DeviceID));
            MobileDevice entity = model.ToMobileDeviceEntity(device);

            if (device == null)
                repo.Create(entity);

            return repo.SaveChanges();
        }
    }
}
