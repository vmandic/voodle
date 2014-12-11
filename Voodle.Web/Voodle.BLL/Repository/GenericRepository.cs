using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Voodle.Utility;
using Voodle.Web.Utility;

namespace Voodle.BLL.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public DbContext Context { get; set; }
        public IDbSet<TEntity> DbSet { get; set; }

        private bool shareContext;

        public GenericRepository(DbContext ctx)
        {
            this.Context = ctx;
            this.shareContext = true;
            this.DbSet = Context.Set<TEntity>();
        }

        public GenericRepository(DbContext ctx, bool shareContext = true)
        {
            this.Context = ctx;
            this.shareContext = shareContext;
            this.DbSet = Context.Set<TEntity>();
        }

        public bool SaveChanges()
        {
            try
            {
                this.Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // TODO: log error
                return false;
            }
        }

        public void Dispose()
        {
            if (shareContext && (Context != null))
                Context.Dispose();
        }

        public virtual IQueryable<TEntity> All()
        {
            return DbSet.AsQueryable();
        }

        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable<TEntity>();
        }

        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = DbSet.AsQueryable();

            if (includes != null)
                foreach (var i in includes)
                    query = query.Include(i);

            return query.Where(predicate).AsQueryable<TEntity>();
        }

        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }

        public virtual TEntity FindByKeys(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public virtual TEntity Create(TEntity TEntity)
        {
            var newEntry = DbSet.Add(TEntity);

            if (!shareContext)
                Context.SaveChanges();

            return newEntry;
        }

        public virtual int Count
        {
            get
            {
                return DbSet.Count();
            }
        }

        public int Delete(TEntity TEntity)
        {
            DbSet.Remove(TEntity);
            if (!shareContext)
                return Context.SaveChanges();

            return 0;
        }

        public virtual int Update(TEntity TEntity)
        {
            var entry = Context.Entry(TEntity);

            if (DbSet.Find(TEntity.GetInt32ByPropertyName("ID")) == null)
            {
                DbSet.Attach(TEntity);
                entry.State = EntityState.Modified;
            }

            if (!shareContext)
                return Context.SaveChanges();

            return 0;
        }

        public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var objects = Filter(predicate);

            foreach (var obj in objects)
                DbSet.Remove(obj);

            if (!shareContext)
                return Context.SaveChanges();

            return 0;
        }

        public IQueryable<TEntity> All(params string[] includes)
        {
            var q = Context.Set<TEntity>() as IQueryable<TEntity>;

            foreach (var include in includes)
                q = q.Include(include);

            return q;
        }

        public TEntity Create()
        {
            return Context.Set<TEntity>().Create();
        }

        public void InsertGraph(TEntity entity)
        {
            this.DbSet.Add(entity);
        }

        public TEntity FindById(object id, bool reload = false)
        {
            var entity = DbSet.Find(id);

            if (reload && entity != null)
            {
                var entry = Context.Entry(entity);
                entry.Reload();
            }

            return entity;
        }

        public TEntity FirstOrDefaultByIdAndInclude(int id, bool reload, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = this.DbSet.AsQueryable();

            if (includes != null)
                foreach (var i in includes)
                    query = query.Include(i);

            var entity = query.FirstOrDefault(x => x.GetInt32ByPropertyName("ID") == id);

            if (reload)
                Context.Entry(entity).Reload();

            return entity;
        }

        public TEntity FirstOrDefaultByIdAndInclude(Expression<Func<TEntity, bool>> predicate, bool reload, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = this.DbSet.AsQueryable();

            if (includes != null)
                foreach (var i in includes)
                    query = query.Include(i);

            var entity = query.FirstOrDefault(predicate);

            if (reload)
                Context.Entry(entity).Reload();

            return entity;
        }

        public bool HasAny(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbSet.Any(predicate);
        }

        public bool HasAll(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbSet.All(predicate);
        }

        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            var entry = Context.Entry(entity);
            entry.State = EntityState.Unchanged;

            //unwraps the entity properties using reflection to only update the sent properties
            foreach (var property in properties)
            {
                string propertyName = "";
                Expression bodyExpression = property.Body;
                if (bodyExpression.NodeType == ExpressionType.Convert && bodyExpression is UnaryExpression)
                {
                    Expression operand = ((UnaryExpression)property.Body).Operand;
                    propertyName = ((MemberExpression)operand).Member.Name;
                }
                else
                    propertyName = ExpressionHelper.GetPropertyName(property);

                entry.Property(propertyName).IsModified = true;
            }
        }

        public IOrderedQueryable<TEntity> FilterOrdered<OrderingType>(Expression<Func<TEntity, OrderingType>> orderBy, Expression<Func<TEntity, bool>> predicate = null, bool isOrderedDescending = false, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = this.DbSet.AsQueryable();

            if (includes != null)
                foreach (var i in includes)
                    query = query.Include(i);

            if (predicate != null)
                query = query.Where(predicate);

            //string propertyName = orderBy.Body.ToString().Split('.')[1];

            var orderedQuery = isOrderedDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

            return orderedQuery;
        }

        public IOrderedQueryable<TEntity> FilterOrdered(string orderBy, Expression<Func<TEntity, bool>> predicate = null, bool isOrderedDescending = false, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = this.DbSet.AsQueryable();

            if (orderBy == "")
                orderBy = "ID";

            if (includes != null)
                foreach (var i in includes)
                    query = query.Include(i);

            if (predicate != null)
                query = query.Where(predicate);

            var orderedQuery = isOrderedDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

            return orderedQuery;
        }

        public IQueryable<TEntity> FilterPage(IOrderedQueryable<TEntity> orderedQuery, out int total, int page = 1, int size = 24)
        {
            int skipCount = (page - 1) * size;
            var query = skipCount == 0 ? orderedQuery.Take(size) : orderedQuery.Skip(skipCount).Take(size);
            total = orderedQuery.Count();

            return query;
        }

        public IPagedList<TEntity> FilterPage(IOrderedQueryable<TEntity> orderedQuery, Expression<Func<TEntity, TEntity>> selectCaluse, int page = 1, int size = 24)
        {
            var pagedList = orderedQuery.Select(selectCaluse).ToPagedList(page, size);
            return pagedList;
        }

        public IQueryable<TEntity> FilterOrderedPage(dynamic orderBy, out int total, int page = 1, int size = 24, bool isOrderedDescending = false, Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes)
        {
            return FilterPage(FilterOrdered(orderBy, predicate, isOrderedDescending, includes), out total, page, size);
        }

        public IQueryable<TEntity> FilterOrderedPage<OrderingType>(Expression<Func<TEntity, OrderingType>> orderBy, out int total, int page = 1, int size = 24, bool isOrderedDescending = false, Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes)
        {
            return FilterPage(FilterOrdered(orderBy, predicate, isOrderedDescending, includes), out total, page, size);
        }
    }
}
