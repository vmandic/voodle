using System.Web.Mvc;
using Voodle.BLL;

namespace Voodle.Web.Controllers.Base
{
    public abstract partial class BaseWebController : Controller
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