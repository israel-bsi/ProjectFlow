using System.Collections;
using System.Linq.Dynamic.Core;

namespace ProjectFlow.Core.Extension;

public static class IQueryableExtension
{
    public static IQueryable<T> FilterByProperty<T>(this IQueryable<T> query, string searchTerm, string filterBy)
    {
        var parts = filterBy.Split('.');
        if (parts.Length > 1)
        {
            var parentPropertyName = parts[0];
            var childPropertyName = parts[1];

            var parentProperty = typeof(T).GetProperty(parentPropertyName);
            if (parentProperty == null)
                return query;

            if (typeof(IEnumerable).IsAssignableFrom(parentProperty.PropertyType) &&
                parentProperty.PropertyType != typeof(string))
            {
                var predicate = $"{parentPropertyName}.Any({childPropertyName}.ToLower().Contains(@0.ToLower()))";
                query = query.Where(predicate, searchTerm);
            }
            else
            {
                var predicate = $"{parentPropertyName}.{childPropertyName}.ToLower().Contains(@0.ToLower())";
                query = query.Where(predicate, searchTerm);
            }

            return query;
        }

        var propertyInfo = typeof(T).GetProperty(filterBy);
        if (propertyInfo == null) return query;

        if (propertyInfo.PropertyType == typeof(string))
            return query.Where($"{filterBy}.ToLower().Contains(@0.ToLower())", searchTerm);

        if (propertyInfo.PropertyType == typeof(bool))
        {
            var boolValue = bool.TryParse(searchTerm, out var parsedBool) && parsedBool;
            return query.Where($"{filterBy} == @0", !boolValue);
        }

        if (propertyInfo.PropertyType == typeof(int))
        {
            searchTerm = string.IsNullOrEmpty(searchTerm) ? "0" : searchTerm;
            var searchValue = Convert.ChangeType(searchTerm, propertyInfo.PropertyType);
            return query.Where($"{filterBy} == @0", searchValue);
        }

        if (propertyInfo.PropertyType.IsEnum)
        {
            var searchValue = Enum.Parse(propertyInfo.PropertyType, searchTerm);
            return query.Where($"{filterBy} == @0", searchValue);
        }

        return query;
    }
}