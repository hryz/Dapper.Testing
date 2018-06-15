using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ClientCode.Products.Queries;
using ClientCode.Products.ReadModels;
using ClientCode.Shared;
using Dapper;
using Lib;
using Microsoft.Extensions.Configuration;

namespace ClientCode.Products.QueryHandlers
{
    [DapperQuery(nameof(PageQuery), nameof(DummyParameters), nameof(SearchClause), nameof(Upcoming))]
    [DapperQuery(nameof(PageQuery), nameof(DummyParameters), nameof(SearchClause), nameof(Current))]
    [DapperQuery(nameof(PageQuery), nameof(DummyParameters), nameof(SearchClause), nameof(Past))]
    [DapperQuery(nameof(PageQuery), nameof(DummyParameters), nameof(SearchClause), nameof(All))]

    [DapperQuery(nameof(CountQuery), nameof(DummyParameters), nameof(SearchClause), nameof(Upcoming))]
    [DapperQuery(nameof(CountQuery), nameof(DummyParameters), nameof(SearchClause), nameof(Current))]
    [DapperQuery(nameof(CountQuery), nameof(DummyParameters), nameof(SearchClause), nameof(Past))]
    [DapperQuery(nameof(CountQuery), nameof(DummyParameters), nameof(SearchClause), nameof(All))]
    public class GetProductListHandler
    {
        private const string CountQuery = @"
        SELECT COUNT(*)
        FROM Production.Product p
        WHERE p.ListPrice > 0
        {0}
        {1}";

        private const string PageQuery = @"
        SELECT
          ProductID       AS Id,
          ProductNumber   AS PN,
          p.Name          AS Name,
          p.ListPrice     AS Price,
          ProductLine     AS Line,
          pc.Name         AS Category,
          psc.Name        AS SubCategory,
          p.SellStartDate AS StartDate,
          p.SellEndDate   AS EndDate

        FROM Production.Product p
          INNER JOIN Production.ProductSubcategory psc
            ON p.ProductSubcategoryID = psc.ProductSubcategoryID
          INNER JOIN Production.ProductCategory pc
            ON psc.ProductCategoryID = pc.ProductCategoryID
        WHERE p.ListPrice > 0
          {0}
          {1}
        ORDER BY p.Name
          OFFSET @skip ROWS
          FETCH NEXT @take ROWS ONLY";

        private const string SearchClause = "AND p.Name LIKE @searchText";

        private static readonly object DummyParameters = new
        {
            searchText = "foo",
            skip = 10,
            take = 11
        };

        private readonly string _connectionString;

        public GetProductListHandler(IConfiguration cfg)
        {
            _connectionString = cfg.GetConnectionString("default");
        }

        private static string Upcoming => GetDateFilter(PeriodFilter.Upcoming);
        private static string Current => GetDateFilter(PeriodFilter.Current);
        private static string Past => GetDateFilter(PeriodFilter.Past);
        private static string All => GetDateFilter(PeriodFilter.All);

        public async Task<PageResult<ProductListItem>> Handle(GetProductList request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                searchText = $"%{request.SearchText}%",
                skip = (request.PageNo - 1) * request.PageSize,
                take = request.PageSize
            };

            var addSearch = !string.IsNullOrEmpty(request.SearchText)
                ? SearchClause
                : string.Empty;

            var addDates = GetDateFilter(request.TimeFilter);

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var countQuery = string.Format(CountQuery, addSearch, addDates);
                var pageQuery = string.Format(PageQuery, addSearch, addDates);

                var total = await db.ExecuteScalarAsync<int>(countQuery, parameters);
                var page = await db.QueryAsync<ProductListItem>(pageQuery, parameters);

                return new PageResult<ProductListItem>(page, total);
            }
        }

        private static string GetDateFilter(PeriodFilter filter)
        {
            switch (filter)
            {
                case PeriodFilter.Upcoming: return "AND (getutcdate() < SellStartDate)\r\n";
                case PeriodFilter.Current: return "AND (getutcdate() > SellStartDate AND getutcdate() < SellEndDate)\r\n";
                case PeriodFilter.Past: return "AND (getutcdate() > SellEndDate)\r\n";
                case PeriodFilter.All: return string.Empty;
                default: throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
        }
    }
}
