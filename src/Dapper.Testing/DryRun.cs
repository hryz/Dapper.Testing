using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Dapper.Testing
{
    public class DryRun
    {
        private static IDbConnection _connection;
        private static bool _safeModeEnabled;

        public static async Task EnableSafeMode(string connectionString)
        {
            var con = new SqlConnection(connectionString);
            await con.OpenAsync();
            _connection = con;

            await _connection.ExecuteScalarAsync("SET FMTONLY ON");
            _safeModeEnabled = true;
        }

        public static async Task ExecuteQuery(QueryContext ctx)
        {
            if (!_safeModeEnabled)
            {
                throw new DryRunException("Safe mode is not enabled. It's dangerous to execute queries without it");
            }

            await _connection.ExecuteScalarAsync(ctx.QueryText, ctx.Parameters);
        }

        public static async Task DisableSafeMode()
        {
            await _connection.ExecuteScalarAsync("SET FMTONLY OFF");
            _safeModeEnabled = false;

            _connection.Close();
        }
    }
}
