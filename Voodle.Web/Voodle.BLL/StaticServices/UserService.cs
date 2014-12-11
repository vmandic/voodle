using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voodle.BLL.Repository;
using Voodle.Entities;
using Voodle.Utility;
using Voodle.Web.Models;

namespace Voodle.BLL.StaticServices
{
    // no hard feelings for this crap

    public static class UserService
    {
        public static UserLoginModel LoginByUsernameAndPassword(string username, string password)
        {
            var userLoginModel = new UserLoginModel();
            username = username.ToLower();
            bool userExists = false;

            using (var ctx = new AppEntities())
            {
                IGenericRepository<User> userRepo = new GenericRepository<User>(ctx);

                // login with username or email, both are anyways unique, at least should be... :-)
                if (username.Contains('@'))
                    userExists = userRepo.HasAny(x => x.Email.ToLower() == username && x.Password == password && x.RoleID != (int)AppRole.RegularUser);
                else
                    userExists = userRepo.HasAny(x => x.Username.ToLower() == username && x.Password == password && x.RoleID != (int)AppRole.RegularUser);

                if (userExists)
                {
                    IQueryable<User> query = userRepo.Filter(x => x.Username.ToLower() == username && x.Password == password);

                    if (username.Contains('@'))
                        query = userRepo.Filter(x => x.Email.ToLower() == username && x.Password == password);

                    var usr = query.Select(x => new { x.ID, x.Username, x.RoleID, x.FirstName, x.LastName }).First();
                    var user = new User() { ID = usr.ID, LastLoggedAt = DateTime.Now };
                    userRepo.Update(user, x => x.LastLoggedAt);

                    if (userRepo.SaveChanges())
                    {
                        userLoginModel.LoginStatus = LoginStatus.SUCCESS;
                        userLoginModel.UserID = usr.ID;
                        userLoginModel.RoleID = usr.RoleID;
                        userLoginModel.Username = usr.Username;
                        userLoginModel.Firstname = usr.FirstName;
                        userLoginModel.Lastname = usr.LastName;
                    }
                    else
                        userLoginModel.LoginStatus = LoginStatus.ERROR;
                }
                else
                    userLoginModel.LoginStatus = LoginStatus.CREDENTIALS_FAIL;

            }
            return userLoginModel;
        }
    }
}
