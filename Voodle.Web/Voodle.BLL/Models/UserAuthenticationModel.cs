using Voodle.Utility;

namespace Voodle.Web.ViewModels
{
    public class UserAuthenticationModel
    {
        public string UserID { get; set; }
        public string RoleID { get; set; }


        public AppRole Role
        {
            get
            {
                return (AppRole)AppEncryption.DecryptToInt(this.RoleID);
            }
        }
        public string Username { get; set; }
        public string FullName
        {
            get
            {
                return this.Firstname + " " + this.Lastname;
            }
        }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public int GetDecryptedUserID()
        {
            return AppEncryption.DecryptToInt(this.UserID);
        }

        public int SetEncryptedUserID
        {
            set
            {
                this.UserID = AppEncryption.Encrypt(value);
            }
        }

        public int SetEncryptedRoleID
        {
            set
            {
                this.RoleID = AppEncryption.Encrypt(value);
            }
        }
    }
}
