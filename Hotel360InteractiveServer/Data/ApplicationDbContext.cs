using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Hotel360InteractiveServer.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        private readonly string? _connectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : this(options) => _connectionString = configuration.GetConnectionString("WINTOUCH")!;

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                if (dbConnection is DbConnection dbConn)
                {
                    await dbConn.OpenAsync();
                }
                else
                {
                    dbConnection.Open();
                }

                return await dbConnection.QueryAsync<T>(sql, parameters);
            }
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                if (dbConnection is DbConnection dbConn)
                {
                    await dbConn.OpenAsync();
                }
                else
                {
                    dbConnection.Open();
                }

                return await dbConnection.ExecuteAsync(sql, parameters);
            }
        }

        public static void ScriptEstadosTable(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var sql = @"
                IF NOT EXISTS (
                    SELECT 1
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'whotestadoquartos'
                      AND COLUMN_NAME = 'cdu_disponivelPortal'
                )
                BEGIN
                    ALTER TABLE dbo.whotestadoquartos
                    ADD cdu_disponivelPortal BIT NOT NULL DEFAULT 0;
                END";

            dbContext.Database.ExecuteSqlRaw(sql);
        }
    }
}
