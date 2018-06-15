using ClientCode.Shared;

namespace ClientCode.Products.Queries
{
    public class GetProductList
    {
        public GetProductList(PeriodFilter timeFilter, int pageNo, int pageSize, string searchText)
        {
            TimeFilter = timeFilter;
            PageNo = pageNo;
            PageSize = pageSize;
            SearchText = searchText;
        }

        public PeriodFilter TimeFilter { get; }
        public int PageNo { get; }
        public int PageSize { get; }
        public string SearchText { get; }
    }
}
