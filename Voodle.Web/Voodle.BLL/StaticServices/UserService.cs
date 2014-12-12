using System;
using System.Linq;
using Voodle.BLL.Models.Base;
using Voodle.BLL.Repository;
using Voodle.Entities;
using Voodle.Utility;
using Voodle.BLL.Converters;

namespace Voodle.BLL.StaticServices
{
    // no hard feelings for this crap

    public static class UserService
    {
        public static UserLoginModel LoginByUsernameAndPassword(DbContextManager dbManager, string username, string password)
        {
            var userLoginModel = new UserLoginModel();
            username = username.ToLower();
            bool userExists = false;

            IGenericRepository<User> userRepo = new GenericRepository<User>(dbManager.Context);

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


            return userLoginModel;
        }

        public static UserModel GetSingleByUserLoginModel_Mobile(DbContextManager dbManager, UserLoginModel userLoginModel)
        {
            UserModel userModel = new GenericRepository<User>(dbManager.Context)
                .Filter(x => x.Active == true && x.ID == userLoginModel.UserID)
                .Select(x => new UserModel()
                {
                    CreatedAt = x.CreatedAt ?? new DateTime(1970, 1, 1),
                    ModifiedAt = x.ModifiedAt ?? new DateTime(1970, 1, 1),
                    LastLoggedAt = x.LastLoggedAt ?? new DateTime(1970, 1, 1)
                }).FirstOrDefault();

            userModel.Active = true;
            userModel.Firstname = userLoginModel.Firstname;
            userModel.Lastname = userLoginModel.Lastname;
            userModel.LoginStatus = LoginStatus.SUCCESS;
            userModel.Password = userLoginModel.Password;
            userModel.RememberMe = userLoginModel.RememberMe;
            userModel.RoleID = userLoginModel.RoleID;
            userModel.UserID = userLoginModel.UserID;
            userModel.Username = userLoginModel.Username;

            return userModel;
        }

        public static UserModel Update_Mobile(DbContextManager dbManager, UserModel userModel)
        {
            var repo = new GenericRepository<User>(dbManager.Context);

            User userEntity = repo.FindById(userModel.UserID);
            userEntity = userModel.ToUserEntity(userEntity);
            userModel.ModifiedAt = DateTime.Now;

            repo.Update(userEntity);
            userModel.Saved = repo.SaveChanges();

            return userModel;
        }

        public static UserModel Create_Mobile(DbContextManager dbManager, UserModel userModel)
        {
            var repo = new GenericRepository<User>(dbManager.Context);

            User userEntity = userModel.ToUserEntity();
            userModel.ModifiedAt = DateTime.Now;

            repo.Create(userEntity);
            userModel.Saved = repo.SaveChanges();

            return userModel;
        }
    }
}
