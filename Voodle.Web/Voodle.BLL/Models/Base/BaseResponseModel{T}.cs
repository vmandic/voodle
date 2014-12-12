using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voodle.Utility;

namespace Voodle.BLL.Models.Base
{
    public class BaseResponseModel<ResponseType> : IResponseModel<ResponseType>, IResponseSaved
    {
        public BaseResponseModel()
        {
            this.Response = default(ResponseType);
            this.Status = ResponseStatus.ERROR;
            this.Message = "No additional messages.";
        }

        public ResponseStatus Status
        {
            get;
            set;
        }

        public string Message { get; set; }

        public ResponseType Response
        {
            get;
            set;
        }

        public bool Saved
        {
            get;
            set;
        }
    }
}
