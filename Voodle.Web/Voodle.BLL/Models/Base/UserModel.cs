using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voodle.BLL.Models.Base
{
    public class UserModel : UserLoginModel
    {
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime LastLoggedAt { get; set; }
        public string Email { get; set; }
        public bool Saved { get; set; }
    }
}
