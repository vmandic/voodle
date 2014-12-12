using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voodle.BLL.Models.Base
{
    public class BaseRequestModel<RequestType>
    {
        public RequestType Request { get; set; }
    }
}
