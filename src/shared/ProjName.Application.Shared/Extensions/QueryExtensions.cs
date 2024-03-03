using ProjName.Application.Shared.CQRS;
using AutoMapper.Internal;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ProjName.Application.Shared.Extensions;

public static class QueryExtensions
{
    public static IQueryable<TEntity> BuildFilterExpression<TEntity, TFilter>(this IQueryable<TEntity> query, TFilter filterObj)
        where TFilter : IBaseGetListQuery
    {
        var filterProps = filterObj.GetType().GetProperties().Where(p =>
        p.Name != "Options" &&
        p.GetValue(filterObj) != null);

        ParameterExpression p = Expression.Parameter(typeof(TEntity), "p");
        Expression finalExpression = null;

        foreach (var filterProp in filterProps.Where(x => x.PropertyType.IsGenericType))
        {
            var prop = typeof(TEntity).GetProperty(filterProp.Name);
            Expression innerExp = null;
            if (filterProp.PropertyType.GetGenericTypeDefinition() == typeof(FilterExp<>))
            {
                dynamic fieldFilters = filterProp.GetValue(filterObj);
                LogicalOperator op = fieldFilters.CombineWith;

                if (fieldFilters.Filters == null)
                {
                    continue;
                }

                foreach (var filter in fieldFilters.Filters)
                {
                    Expression exp = BuildComparisonExp(p, prop, filter.Value, (ComparisonOperator)filter.ComparisonOperator);
                    innerExp = innerExp.BuildCombineExp(exp, op);
                }
            }
            finalExpression = finalExpression.BuildCombineExp(innerExp, filterObj.Options?.CombineWith ?? LogicalOperator.And);
        }

        if (finalExpression != null)
        {
            var lmbda = Expression.Lambda<Func<TEntity, bool>>(finalExpression, p);
            query = query.Where(lmbda);
        }
        return query;
    }
    public static Expression<Func<T, bool>> GetFilterExpression<T>(this Dictionary<string, FilterExp<dynamic>> query, LogicalOperator combineWith = LogicalOperator.And)
    {
        ParameterExpression p = Expression.Parameter(typeof(T), "p");

        Expression finalExpression = null;
        foreach (var filterProp in query.Keys)
        {
            var prop = typeof(T).GetProperty(filterProp);
            Expression innerExp = null;
            dynamic fieldFilters = query[filterProp];
            LogicalOperator op = fieldFilters.CombineWith;

            if (fieldFilters.Filters == null)
            {
                continue;
            }

            foreach (var filter in fieldFilters.Filters)
            {
                Expression exp = BuildComparisonExp(p, prop, filter.Value, (ComparisonOperator)filter.ComparisonOperator);
                innerExp = innerExp.BuildCombineExp(exp, op);
            }
            finalExpression = finalExpression.BuildCombineExp(innerExp, combineWith);
        }

        if (finalExpression != null)
        {
            return Expression.Lambda<Func<T, bool>>(finalExpression, p);
        }
        return null;
    }
    public static IQueryable<TEntity> BuildIncludeExp<TEntity>(this IQueryable<TEntity> query, Type includes) where TEntity : class
    {
        var propToInclude = includes.GetProperties().Where(p => p.GetGetMethod().IsVirtual).ToList();

        ParameterExpression p = Expression.Parameter(typeof(TEntity), "p");

        foreach (var prop in propToInclude)
        {
            query = query.Include(prop.Name);
        }
        return query;
    }
    public static IQueryable<TEntity> BuildSortingExp<TEntity>(this IQueryable<TEntity> query, string sortBy, bool isDescending = false)
    {
        var propInfo = typeof(TEntity).GetProperties().FirstOrDefault(p => p.Name.Equals(sortBy, StringComparison.InvariantCultureIgnoreCase));
        if (propInfo == null)
        {
            return query;
        }

        ParameterExpression p = Expression.Parameter(typeof(TEntity), "p");
        var memberAccess = Expression.Property(p, propInfo);
        var lmbda = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(memberAccess, typeof(object)), p);

        if (isDescending)
        {
            query = query.OrderByDescending(lmbda);
        }
        else
        {
            query = query.OrderBy(lmbda);
        }
        return query;
    }
    public static (IQueryable<TEntity> Query, int TotalCount) BuildPagingExp<TEntity>(this IQueryable<TEntity> query, int pageNum, int pageSize)
    {
        if (pageNum <= 0)
        {
            return (query, query.Count());
        }
        return (query.Skip((pageNum - 1) * pageSize).Take(pageSize), query.Count());
    }
    private static object GetDefaultValue(Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }
        return null;
    }        
    private static Expression BuildCombineExp(this Expression existingExp, Expression newExp, LogicalOperator op)
    {
        if (existingExp == null)
        {
            existingExp = newExp;
        }
        else if (newExp != null)
        {
            existingExp = op switch
            {
                LogicalOperator.Or => Expression.OrElse(existingExp, newExp),
                _ => Expression.AndAlso(existingExp, newExp)
            };
        }
        return existingExp;
    }
    private static Expression BuildComparisonExp(ParameterExpression p, PropertyInfo prop, dynamic value, ComparisonOperator op)
    {
        var memberAccess = Expression.MakeMemberAccess(p, prop);

        if (prop.PropertyType.IsNullableType() && value != null)
        {
            memberAccess = Expression.MakeMemberAccess(memberAccess, prop.PropertyType.GetProperty("Value"));
        }
        var propertyValue = Expression.Constant(value?.GetType() == prop.PropertyType ? value : TypeDescriptor.GetConverter(prop.PropertyType).ConvertFrom(value));

        if (op != ComparisonOperator.Equals && op != ComparisonOperator.NotEquals)
        {
            if (value == null || (value is string && value == string.Empty))
            {
                return null;
            }
        }

        return op switch
        {
            ComparisonOperator.Contains => Expression.Call(memberAccess, prop.PropertyType.GetMethod("Contains", new[] { prop.PropertyType }), propertyValue),
            ComparisonOperator.GreaterThan => Expression.GreaterThan(memberAccess, propertyValue),
            ComparisonOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(memberAccess, propertyValue),
            ComparisonOperator.LessThan => Expression.LessThan(memberAccess, propertyValue),
            ComparisonOperator.LessThanOrEqual => Expression.LessThanOrEqual(memberAccess, propertyValue),
            ComparisonOperator.NotEquals => Expression.NotEqual(memberAccess, propertyValue),
            _ => Expression.Equal(memberAccess, propertyValue)
        };
    }
}
