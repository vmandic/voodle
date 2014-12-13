using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voodle.BLL.Models.Base
{
    public class UserModel : UserLoginModel
    {
        [DisplayName("Created")]
        public DateTime CreatedAt { get; set; }
        [DisplayName("Modifed")]
        public DateTime ModifiedAt { get; set; }
        [DisplayName("Last login")]
        public DateTime LastLoggedAt { get; set; }
        public string Email { get; set; }
        public bool Saved { get; set; }
    }
}
