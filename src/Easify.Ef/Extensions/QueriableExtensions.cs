using System;
using System.Linq;

namespace Easify.Ef.Extensions
{
    public static class QueriableExtensions
    {
        public static IQueryable<T> PagedBy<T>(this IQueryable<T> quaryable, PagingOptions options)
        {
            if (quaryable == null) throw new ArgumentNullException(nameof(quaryable));
            if (options == null) throw new ArgumentNullException(nameof(options));

            return quaryable.PagedBy(options.PageIndex, options.PageSize);
        }

        public static IQueryable<T> PagedBy<T>(this IQueryable<T> quaryable, int pageIndex, int pageSize)
        {
            if (quaryable == null) throw new ArgumentNullException(nameof(quaryable));
            if (pageIndex <= 0) throw new ArgumentOutOfRangeException(nameof(pageIndex));
            if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize));

            return quaryable.Skip(pageIndex * pageSize).Take(pageSize);
        }        
        
        public static IQueryable<TResult> ProjectTo<T, TResult>(this IQueryable<T> quaryable, Func<T, TResult> projection)
        {
            if (quaryable == null) throw new ArgumentNullException(nameof(quaryable));
            if (projection == null) throw new ArgumentNullException(nameof(projection));

            return quaryable.Select(m => projection(m));
        }
    }
}