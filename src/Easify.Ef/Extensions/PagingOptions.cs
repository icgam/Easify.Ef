using System;

namespace Easify.Ef.Extensions
{
    public class PagingOptions
    {
        public const int DefaultPageSize = 20;

        public PagingOptions(int pageIndex, int pageSize)
        {
            if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (pageIndex <= 0) throw new ArgumentOutOfRangeException(nameof(pageIndex));

            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public PagingOptions() : this(0, DefaultPageSize)
        {
        }

        public PagingOptions(int pageIndex) : this(pageIndex, DefaultPageSize)
        {
        }

        public int PageIndex { get; }
        public int PageSize { get; }
    }
}