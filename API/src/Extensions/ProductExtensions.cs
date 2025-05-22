using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.Models;

namespace API.Src.Extensions
{
    public static class ProductExtensions
    {
        public static IQueryable<Product> Filter(
         this IQueryable<Product> query,
         int? id,
         IEnumerable<string>? brands,
         IEnumerable<string>? categories,
         int? minPrice,
         int? maxPrice,
         string? condition)
        {
            if (id.HasValue && id.Value > 0)
            {
                query = query.Where(p => p.Id == id.Value);
            }

            if (brands != null && brands.Any())
            {
                var brandSet = brands.Select(b => b.Trim().ToLower()).ToHashSet();
                query = query.Where(p => p.Brand != null && brandSet.Contains(p.Brand.Name.ToLower()));
            }

            if (categories != null && categories.Any())
            {
                var categorySet = categories.Select(c => c.Trim().ToLower()).ToHashSet();
                query = query.Where(p => p.Categories.Any(c => categorySet.Contains(c.Name.ToLower())));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(condition))
            {
                var normalizedCondition = condition.Trim().ToLower();
                query = query.Where(p => p.Condition != null && p.Condition.Name.ToLower() == normalizedCondition);
            }

            return query;
        }
        public static IQueryable<Product> Search(this IQueryable<Product> query, string? search)
        {
            if (string.IsNullOrWhiteSpace(search)) return query;

            var lowerCaseSearch = search.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearch));
        }


        public static IQueryable<Product> Sort(this IQueryable<Product> query, string? orderBy)
        {
            query = orderBy switch
            {
                "price" => query.OrderBy(p => (double)p.Price),
                "priceDesc" => query.OrderByDescending(p => (double)p.Price),
                _ => query.OrderBy(p => p.Name)
            };
            return query;
        }

    }
}