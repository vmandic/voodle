using Voodle.Entities;

namespace Voodle.BLL
{
    public class DbContextManager
    {
        //private Hashtable _services;
        private AppEntities _context;

        public AppEntities Context
        {

            get
            {
                // lazy accessor
                if (this._context == null)
                    this._context = new AppEntities();

                return this._context;
            }
        }

        public DbContextManager()
        {

        }

        /// <summary>
        /// Resolves the repository dependency
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <returns>Concrete service accessor.</returns>
        /// <remarks>Should be replaced with the interface version</remarks>
        //public T Get<T>() where T : class
        //{
        //    string typeName = typeof(T).Name;

        //    // lazy accessor
        //    if (this._services == null)
        //        this._services = new Hashtable();

        //    // this will make the service transient over the request period, no reinstantiation of the context will be invoked
        //    if (!this._services.ContainsKey(typeName))
        //        this._services.Add(typeName, Activator.CreateInstance(typeof(T), this.Context));

        //    // resolve the service
        //    return (T)this._services[typeName];
        //}
    }
}
