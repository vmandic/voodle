using System.Web.Http;
using Voodle.BLL;

namespace Voodle.Web.Controllers.Base
{
    public abstract class BaseWsController : ApiController
    {
        private DbContextManager _dbManager;

        public DbContextManager DbManager
        {
            get
            {
                // lazy accessor
                if (this._dbManager == null)
                    this._dbManager = new DbContextManager();

                return this._dbManager;
            }
        }
    }
}
