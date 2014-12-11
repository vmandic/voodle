using System.ComponentModel;
using Voodle.Utility;

namespace Voodle.Web.Models
{
    public class UserLoginModel
    {
        public UserLoginModel()
        {
            this.LoginStatus = LoginStatus.CREDENTIALS_FAIL;
        }

        public LoginStatus LoginStatus { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }
        [DisplayName("Remember me")]
        public bool RememberMe { get; set; }
        public int Active { get; set; }
        [DisplayName("Username")]
        public string Username { get; set; }
        [DisplayName("First name")]
        public string Firstname { get; set; }
        [DisplayName("Last name")]
        public string Lastname { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
    }
}
