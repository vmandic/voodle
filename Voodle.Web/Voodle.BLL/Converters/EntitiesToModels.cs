using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voodle.BLL.Models.Base;
using Voodle.Entities;

namespace Voodle.BLL.Converters
{
    public static class EntitiesToModels
    {
        public static UserModel ToUserModel(this User _entity)
        {
            // debugger hack for inspection
            var entity = _entity;
            var model = new UserModel();

            return model;
        }
    }
}
