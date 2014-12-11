using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Voodle.BLL.Repository
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        bool SaveChanges();

        DbContext Context { get; set; }

        IDbSet<TEntity> DbSet { get; set; }

        bool HasAny(Expression<Func<TEntity, bool>> predicate);

        bool HasAll(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Adds the entity to the current graph context.
        /// </summary>
        void InsertGraph(TEntity entity);

        /// <summary>
        /// Gets all objects from database
        /// </summary>
        IQueryable<TEntity> All();

        /// <summary>
        /// Gets all objects from database with includes
        /// </summary>
        IQueryable<TEntity> All(params string[] includes);

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets objects from database by filter with related tables.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <param name="includes">Specified related entites</param>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Gets objects ordered by a sort selector with desired entites included.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <param name="orederBy">Specified a lambda sort property</param>
        /// <param name="total">Returns the total records count of the filter</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        IOrderedQueryable<TEntity> FilterOrdered<OrderingType>(Expression<Func<TEntity, OrderingType>> orderBy, Expression<Func<TEntity, bool>> predicate = null, bool isOrderedDescending = false, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Gets objects ordered by a sort selector with desired entites included.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <param name="orederBy">Specified a sort property string name</param>
        /// <param name="total">Returns the total records count of the filter</param>
        /// <param name="size">Specified the page size</param>
        IOrderedQueryable<TEntity> FilterOrdered(string orderBy, Expression<Func<TEntity, bool>> predicate = null, bool isOrderedDescending = false, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Gets an ordered set of objects for a targeted page with desired entites included.
        /// </summary>
        /// <param name="isOrderedDescending">Specify if the query will result a descending order by clause</param>
        /// <param name="orderBy">Specified a lambda sort property</param>
        /// <param name="total">Returns the total records count of the filter</param>
        /// <param name="page">The number of page to get entites for</param>
        /// <param name="total">Returns the total records count of the filter</param>
        /// <param name="size">Specified the page size</param>
        /// <param name="includes">Specify entities desired to be include</param>
        /// <param name="predicate">Specified a filter</param>
        IQueryable<TEntity> FilterOrderedPage(dynamic orderBy, out int total, int page = 1, int size = 24, bool isOrderedDescending = false, Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes);

        IQueryable<TEntity> FilterOrderedPage<OrderingType>(Expression<Func<TEntity, OrderingType>> orderBy, out int total, int page = 1, int size = 24, bool isOrderedDescending = false, Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Gets objects ordered by a sort selector and selects a targeted number of entites for a targeted page.
        /// </summary>
        /// <param name="orderedQuery">An ordered query to query upon</param>
        /// <param name="page">The number of page to get entites for</param>
        /// <param name="total">Returns the total records count of the filter</param>
        /// <param name="size">Specified the page size</param>
        IQueryable<TEntity> FilterPage(IOrderedQueryable<TEntity> orderedQuery, out int total, int page = 1, int size = 24);

        IPagedList<TEntity> FilterPage(IOrderedQueryable<TEntity> orderedQuery, Expression<Func<TEntity, TEntity>> selectCaluse, int page = 1, int size = 24);

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        bool Contains(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        TEntity FindByKeys(params object[] keys);

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Find object by specified id.
        /// </summary>
        /// <param name="id">The primary key value of the current entity.</param>
        TEntity FindById(object id, bool reload = false);

        /// <summary>
        /// Finds an entity in the current DbSet with the included related entites.
        /// </summary>
        /// <param name="id">The int Primary Key ID column property value</param>
        /// <param name="includes">Lambda expression references to related entites to be included.</param>
        /// <returns>A found entity from the DB or a null.</returns>
        /// <remarks>To enable searching by ID, the Repository implementation will expect to have Entity objects implemnting the IBaseEntity with the int ID property.</remarks>
        TEntity FirstOrDefaultByIdAndInclude(int id, bool reload, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Finds an entity in the current DbSet with the included related entites.
        /// </summary>
        /// <param name="predicate">A lambda expression predicate which defines the lookup query, i.e. where clause.</param>
        /// <param name="includes">Lambda expression references to related entites to be included.</param>
        /// <returns>A found entity from the DB or a null.</returns>
        TEntity FirstOrDefaultByIdAndInclude(Expression<Func<TEntity, bool>> predicate, bool reload, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        TEntity Create(TEntity t);

        /// <summary>
        /// Creates an empty instance of the given entity in the graph.
        /// </summary>
        TEntity Create();

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="t">Specified a existing object to delete.</param>        
        int Delete(TEntity t);

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        int Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="t">Specified the object to save.</param>
        int Update(TEntity t);

        /// <summary>
        /// Get the total objects count.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Generates a single update clause for the database instead of the standard fetch and update method of EntityFramework.
        /// </summary>
        /// <param name="entity">A in memory entity object with the ID value set up.</param>
        /// <param name="properties">Lambda expression params array of properties to be updated.</param>
        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
    }
}
