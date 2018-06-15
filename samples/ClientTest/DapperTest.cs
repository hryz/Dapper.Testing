using System.Threading.Tasks;
using ClientCode.Products.QueryHandlers;
using Dapper.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientTest
{
    [TestClass]
    public class DapperTest
    {
        private const string ConnectionString = "Server=localhost;Database=AdventureWorks2017;Trusted_Connection=True;";

        [ClassInitialize]
        public static async Task ClassInit(TestContext context)
        {
            await DryRun.EnableSafeMode(ConnectionString);
        }

        [DataTestMethod]
        [DapperDataSource(typeof(GetProductListHandler))]
        public async Task DapperQueriesWork(QueryContext ctx)
        {
            await DryRun.ExecuteQuery(ctx);
        }

        [ClassCleanup]
        public static async Task ClassCleanup()
        {
            await DryRun.DisableSafeMode();
        }
    }
}
