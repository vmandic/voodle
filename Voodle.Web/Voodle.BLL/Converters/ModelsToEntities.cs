using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voodle.BLL.Models.Base;
using Voodle.BLL.Models.WebService.RequestModels;
using Voodle.Entities;

namespace Voodle.BLL.Converters
{
    public static class ModelsToEntities
    {
        public static MobileDevice ToMobileDeviceEntity(this RegisterMobileDeviceRequestModel m, MobileDevice entity = null)
        {
            RegisterMobileDeviceRequestModel model = m;
            var mobileDeviceEntity = entity ?? new MobileDevice();
            var now = DateTime.Now;

            mobileDeviceEntity.Active = true;
            mobileDeviceEntity.CreatedAt =
            mobileDeviceEntity.ModifiedAt = now;
            mobileDeviceEntity.PushNotificationsRegistrationID = model.RegistrationID;
            mobileDeviceEntity.SmartphonePlatform = model.Platform;
            mobileDeviceEntity.UserID = model.ClientID;
            mobileDeviceEntity.DeviceID = model.DeviceID;

            return mobileDeviceEntity;
        }

        public static User ToUserEntity(this UserModel _model, User _entity = null)
        {
            var model = _model;
            var entity = _entity ?? new User();
            var now = DateTime.Now;

            if (entity.ID == 0)
            {
                entity.Active = true;
                entity.CreatedAt = now;
                entity.Username = model.Username;
            }

            entity.Email = model.Email;
            entity.FirstName = model.Firstname;
            entity.LastLoggedAt = now;
            entity.LastName = model.Lastname;
            entity.ModifiedAt = now;
            entity.Password = model.Password;
            entity.RoleID = model.RoleID;

            return entity;
        }
    }
}
