using System.Collections.Generic;

namespace ClientCode.Shared
{
    public class PageResult<T>
    {
        public PageResult(IEnumerable<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }

        public IEnumerable<T> Items { get; }
        public int TotalCount { get; }
    }
}
