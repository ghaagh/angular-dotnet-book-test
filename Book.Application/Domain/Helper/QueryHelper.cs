using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Book.Application.Domain.Helper
{
    public class QueryHelper<T> : IQueryHelper<T> where T : class
    {
        public IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString)
        {
            if (!entities.Any() || string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return entities;
            }

            var orderQueryBuilder = new StringBuilder();
            var orderParams = orderByQueryString.Trim().Split(',');

            foreach (var param in orderParams)
            {
                var propertyFromQueryName = param.Trim(' ').Split(" ")[0];
                var propertyExistResult = IsPropertyExist(propertyFromQueryName.Trim(' '));

                if (string.IsNullOrWhiteSpace(param) || propertyExistResult == null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{propertyExistResult.PropertyName} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return string.IsNullOrEmpty(orderQuery) ? entities : entities.OrderBy(orderQuery);
        }

        public IQueryable<T> ApplySearch(IQueryable<T> entities, string searchValue, string searchFields)
        {
            if (!entities.Any() || string.IsNullOrWhiteSpace(searchValue) || string.IsNullOrEmpty(searchFields))
            {
                return entities;
            }

            var orderQueryBuilder = new StringBuilder();
            var orderParams = searchFields.Trim().Split(',');

            var count = orderParams.Length;
            foreach (var param in orderParams)
            {
                var propertyFromQueryName = param.Trim(' ').Split(" ")[0];
                var propertyExistResult = IsPropertyExist(propertyFromQueryName.Trim(' '));

                if (string.IsNullOrWhiteSpace(param) || propertyExistResult == null)
                    continue;

                var orOperator = count == 1 ? "" : "|| ";
                var propertyName = propertyExistResult.PropertyName;
                if (propertyExistResult.Type != typeof(string))
                {
                    propertyName = $"{propertyName}.ToString()";
                }

                orderQueryBuilder.Append($"{propertyName} != null && {propertyName}.ToLower().Contains(\"{searchValue.ToLower()}\") {orOperator}");
                count--;
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ', '|');

            return string.IsNullOrEmpty(orderQuery) ? entities : entities.Where(orderQuery);
        }

        public async Task<Paged<T>> ApplyPaginationAsync(IQueryable<T> entities, int currentPage, int pageSize)
        {
            var totalSize = await entities.CountAsync();
            var pagedData = await entities.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
            return new Paged<T>(pagedData, totalSize, currentPage, pageSize);
        }

        private static IsPropertyExistInfo IsPropertyExist(string path)
        {
            var type = typeof(T);
            var propertyPath = path;

            foreach (var propertyName in path.Split('.'))
            {
                var property = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                {
                    return null;
                }

                propertyPath = propertyPath.Replace(propertyName, property.Name);
                type = property.PropertyType;
            }

            return new IsPropertyExistInfo
            {
                PropertyName = propertyPath,
                Type = type
            };
        }

        private class IsPropertyExistInfo
        {
            public string PropertyName { get; set; }
            public Type Type { get; set; }
        }
    }
}
