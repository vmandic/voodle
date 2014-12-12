﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Voodle.Utility
{
    public static class AppExtensions
    {
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expresion '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo;
        }

        public static long ToUnixTimeStamp(this DateTime val)
        {
            DateTime unixStart = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
            return (long)Math.Floor((val.ToUniversalTime() - unixStart).TotalSeconds);
        }

        public static string GetDescription(this Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static object GetObjectByPropertyName(this object obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }

        public static int GetInt32ByPropertyName(this object obj, string propName)
        {
            return Convert.ToInt32(obj.GetObjectByPropertyName(propName));
        }

        public static decimal GetDecimalByPropertyName(this object obj, string propName)
        {
            return Convert.ToDecimal(obj.GetObjectByPropertyName(propName));
        }

        public static string GetStringByPropertyName(this object obj, string propName)
        {
            return Convert.ToString(obj.GetObjectByPropertyName(propName));
        }
    }

    public static class LinqExtensions
    {
        #region Pagination

        public static IQueryable<TEntity> GetPage<TEntity>(this IOrderedQueryable<TEntity> orderedQuery, out int total, int page = 1, int size = 24)
        {
            int skipCount = (page - 1) * size;
            var query = skipCount == 0 ? orderedQuery.Take(size) : orderedQuery.Skip(skipCount).Take(size);
            total = orderedQuery.Count();

            return query;
        }

        #endregion

        #region Ordering

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }

        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

        #endregion

        #region Filtering

        public static IQueryable<T> Where<T>(this IQueryable<T> source, string field, string value)
        {
            return source.Where(CreateFilteringExpression<T>(field, value));
        }

        private static Expression<Func<T, bool>> CreateFilteringExpression<T>(string propertyName, string propertyValue)
        {
            string[] props = propertyName.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            MethodInfo method = null;
            Expression exprValue = null;

            if (type == typeof(string))
            {
                method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                exprValue = Expression.Constant(propertyValue, typeof(string));
            }
            else if (type == typeof(int))
            {
                method = typeof(int).GetMethod("Equals", new[] { typeof(int) });
                exprValue = Expression.Constant(Convert.ToInt32(propertyValue), typeof(int));
            }
            else if (type == typeof(long))
            {
                method = typeof(long).GetMethod("Equals", new[] { typeof(long) });
                exprValue = Expression.Constant(Convert.ToInt64(propertyValue), typeof(long));
            }
            else if (type == typeof(decimal))
            {
                method = typeof(long).GetMethod("Equals", new[] { typeof(long) });
                exprValue = Expression.Constant(Convert.ToDecimal(propertyValue), typeof(decimal));
            }
            else if (type == typeof(DateTime))
            {
                DateTime result;
                DateTime.TryParse(propertyValue, out result);
                method = typeof(DateTime).GetMethod("Equals", new[] { typeof(DateTime) });
                exprValue = Expression.Constant(result, typeof(DateTime));
            }

            var methodExp = Expression.Call(expr, method, exprValue);

            return Expression.Lambda<Func<T, bool>>(methodExp, arg);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        #endregion
    }

    public static class DbContextExtensions
    {
        /// <summary>
        /// Adds an entity (if newly created) or update (if has non-default Id).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The db context.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks>
        /// Will not work for HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).
        /// Will not work for composite keys.
        /// </remarks>
        public static T AddOrUpdate<T>(this DbContext context, T entity)
            where T : class
        {
            if (context == null) throw new ArgumentNullException("context");
            if (entity == null) throw new ArgumentNullException("entity");

            if (IsTransient(context, entity))
            {
                context.Set<T>().Add(entity);
            }
            else
            {
                context.Set<T>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }
            return entity;
        }

        /// <summary>
        /// Determines whether the specified entity is newly created (Id not specified).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is transient; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Will not work for HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).
        /// Will not work for composite keys.
        /// </remarks>
        public static bool IsTransient<T>(this DbContext context, T entity)
            where T : class
        {
            if (context == null) throw new ArgumentNullException("context");
            if (entity == null) throw new ArgumentNullException("entity");

            var propertyInfo = FindPrimaryKeyProperty<T>(context);
            var propertyType = propertyInfo.PropertyType;
            //what's the default value for the type?
            var transientValue = propertyType.IsValueType ?
                Activator.CreateInstance(propertyType) : null;
            //is the pk the same as the default value (int == 0, string == null ...)
            return Equals(propertyInfo.GetValue(entity, null), transientValue);
        }

        /// <summary>
        /// Loads a stub entity (or actual entity if already loaded).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks>
        /// Will not work for composite keys.
        /// </remarks>
        public static T Load<T>(this DbContext context, object id)
             where T : class
        {
            if (context == null) throw new ArgumentNullException("context");
            if (id == null) throw new ArgumentNullException("id");

            var property = FindPrimaryKeyProperty<T>(context);
            //check to see if it's already loaded (slow if large numbers loaded)
            var entity = context.Set<T>().Local
                .FirstOrDefault(x => id.Equals(property.GetValue(x, null)));
            if (entity == null)
            {
                //it's not loaded, just create a stub with only primary key set
                entity = CreateEntity<T>(id, property);

                context.Set<T>().Attach(entity);
            }
            return entity;
        }

        /// <summary>
        /// Determines whether the specified entity is loaded from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="id">The id.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is loaded; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Will not work for composite keys.
        /// </remarks>
        public static bool IsLoaded<T>(this DbContext context, object id)
            where T : class
        {
            if (context == null) throw new ArgumentNullException("context");
            if (id == null) throw new ArgumentNullException("id");

            var property = FindPrimaryKeyProperty<T>(context);
            //check to see if it's already loaded (slow if large numbers loaded)
            var entity = context.Set<T>().Local
                .FirstOrDefault(x => id.Equals(property.GetValue(x, null)));
            return entity != null;
        }

        /// <summary>
        /// Marks the reference navigation properties unchanged. 
        /// Use when adding a new entity whose references are known to be unchanged.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="entity">The entity.</param>
        public static void MarkReferencesUnchanged<T>(DbContext context, T entity)
            where T : class
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            var objectSet = objectContext.CreateObjectSet<T>();
            var elementType = objectSet.EntitySet.ElementType;
            var navigationProperties = elementType.NavigationProperties;
            //the references
            var references = from navigationProperty in navigationProperties
                             let end = navigationProperty.ToEndMember
                             where end.RelationshipMultiplicity == RelationshipMultiplicity.ZeroOrOne ||
                             end.RelationshipMultiplicity == RelationshipMultiplicity.One
                             select navigationProperty.Name;
            //NB: We don't check Collections. EF wants to handle the object graph.

            var parentEntityState = context.Entry(entity).State;
            foreach (var navigationProperty in references)
            {
                //if it's modified but not loaded, don't need to touch it
                if (parentEntityState == EntityState.Modified &&
                    !context.Entry(entity).Reference(navigationProperty).IsLoaded)
                    continue;
                var propertyInfo = typeof(T).GetProperty(navigationProperty);
                var value = propertyInfo.GetValue(entity, null);
                context.Entry(value).State = EntityState.Unchanged;
            }
        }

        /// <summary>
        /// Merges a DTO into a new or existing entity attached/added to context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="dataTransferObject">The data transfer object. It must have a primary key property of the same name and type as the actual entity.</param>
        /// <returns></returns>
        /// <remarks>
        /// Will not work for composite keys.
        /// </remarks>
        public static T Merge<T>(this DbContext context, object dataTransferObject)
             where T : class
        {
            if (context == null) throw new ArgumentNullException("context");
            if (dataTransferObject == null) throw new ArgumentNullException("dataTransferObject");

            var property = FindPrimaryKeyProperty<T>(context);
            //find the id property of the dto
            var idProperty = dataTransferObject.GetType().GetProperty(property.Name);
            if (idProperty == null)
                throw new InvalidOperationException("Cannot find an id on the dataTransferObject");
            var id = idProperty.GetValue(dataTransferObject, null);
            //has the id been set (existing item) or not (transient)?
            var propertyType = property.PropertyType;
            var transientValue = propertyType.IsValueType ?
                Activator.CreateInstance(propertyType) : null;
            var isTransient = Equals(id, transientValue);
            T entity;
            if (isTransient)
            {
                //it's transient, just create a dummy
                entity = CreateEntity<T>(id, property);
                //if DatabaseGeneratedOption(DatabaseGeneratedOption.None) and no id, this errors
                context.Set<T>().Attach(entity);
            }
            else
            {
                //try to load from identity map or database
                entity = context.Set<T>().Find(id);
                if (entity == null)
                {
                    //could not find entity, assume assigned primary key
                    entity = CreateEntity<T>(id, property);
                    context.Set<T>().Add(entity);
                }
            }
            //copy the values from DTO onto the entry
            context.Entry(entity).CurrentValues.SetValues(dataTransferObject);
            return entity;
        }


        private static PropertyInfo FindPrimaryKeyProperty<T>(IObjectContextAdapter context)
            where T : class
        {
            //find the primary key
            var objectContext = context.ObjectContext;
            //this will error if it's not a mapped entity
            var objectSet = objectContext.CreateObjectSet<T>();
            var elementType = objectSet.EntitySet.ElementType;
            var pk = elementType.KeyMembers.First();
            //look it up on the entity
            var propertyInfo = typeof(T).GetProperty(pk.Name);
            return propertyInfo;
        }

        private static T CreateEntity<T>(object id, PropertyInfo property)
            where T : class
        {
            // consider IoC here
            var entity = (T)Activator.CreateInstance(typeof(T));
            //set the value of the primary key (may error if wrong type)
            property.SetValue(entity, id, null);
            return entity;
        }
    }

    public static class DbContextMetadata
    {
        private static MetadataWorkspace FindMetadataWorkspace(IObjectContextAdapter context)
        {
            var objectContext = context.ObjectContext;
            return objectContext.MetadataWorkspace;
        }

        private static ObjectSet<T> FindObjectSet<T>(IObjectContextAdapter context)
            where T : class
        {
            var objectContext = context.ObjectContext;
            //this can throw an InvalidOperationException if it's not mapped
            var objectSet = objectContext.CreateObjectSet<T>();
            return objectSet;
        }

        private static IEnumerable<NavigationProperty> FindNavigationPropertyCollection<T>(
            IObjectContextAdapter context)
            where T : class
        {
            var objectSet = FindObjectSet<T>(context);
            var elementType = objectSet.EntitySet.ElementType;
            var navigationProperties = elementType.NavigationProperties;
            return navigationProperties;
        }

        /// <summary>
        /// Finds the names of the entities in a DbContext.
        /// </summary>
        public static IEnumerable<string> FindEntities(DbContext context)
        {
            var metadataWorkspace = FindMetadataWorkspace(context);
            var items = metadataWorkspace.GetItems<EntityType>(DataSpace.CSpace);
            return items.Select(t => t.FullName);
        }

        /// <summary>
        /// Finds the underlying table names.
        /// </summary>
        public static IEnumerable<string> FindTableNames(DbContext context)
        {
            var metadataWorkspace = FindMetadataWorkspace(context);
            //we don't have to force a metadata load in Code First, apparently
            var items = metadataWorkspace.GetItems<EntityType>(DataSpace.SSpace);
            //namespace name is not significant (it's not schema name)
            return items.Select(t => t.Name);
        }

        /// <summary>
        /// Finds the primary key property names for an entity of specified type.
        /// </summary>
        public static IEnumerable<string> FindPrimaryKey<T>(DbContext context)
            where T : class
        {
            var objectSet = FindObjectSet<T>(context);
            var elementType = objectSet.EntitySet.ElementType;
            return elementType.KeyMembers.Select(p => p.Name);
        }

        /// <summary>
        /// Determines whether the specified entity is transient.
        /// </summary>
        public static bool IsTransient<T>(DbContext context, T entity)
            where T : class
        {
            var pk = FindPrimaryKey<T>(context).First();
            //look it up on the entity
            var propertyInfo = typeof(T).GetProperty(pk);
            var propertyType = propertyInfo.PropertyType;
            //what's the default value for the type?
            var transientValue = propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null;
            //is the pk the same as the default value (int == 0, string == null ...)
            return propertyInfo.GetValue(entity, null) == transientValue;
        }

        /// <summary>
        /// Finds the navigation properties (References and Collections)
        /// </summary>
        public static IEnumerable<string> FindNavigationProperties<T>(DbContext context)
            where T : class
        {
            var navigationProperties = FindNavigationPropertyCollection<T>(context);
            return navigationProperties.Select(p => p.Name);
        }

        /// <summary>
        /// Finds the navigation collection properties.
        /// </summary>
        public static IEnumerable<string> FindNavigationCollectionProperties<T>(DbContext context)
            where T : class
        {
            var navigationProperties = FindNavigationPropertyCollection<T>(context);

            return from navigationProperty in navigationProperties
                   where navigationProperty.ToEndMember.RelationshipMultiplicity ==
                        RelationshipMultiplicity.Many
                   select navigationProperty.Name;
        }

        /// <summary>
        /// Finds the navigation reference properties
        /// </summary>
        public static IEnumerable<string> FindNavigationReferenceProperties<T>(DbContext context)
            where T : class
        {
            var navigationProperties = FindNavigationPropertyCollection<T>(context);

            return from navigationProperty in navigationProperties
                   let end = navigationProperty.ToEndMember
                   where end.RelationshipMultiplicity == RelationshipMultiplicity.ZeroOrOne ||
                    end.RelationshipMultiplicity == RelationshipMultiplicity.One
                   select navigationProperty.Name;
        }
    }
}
