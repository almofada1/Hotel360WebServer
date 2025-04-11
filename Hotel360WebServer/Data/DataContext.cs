using Dapper;
using Hotel360WebServer.Controller;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Hotel360WebServer.Data
{
    public class DataService
    {
        private readonly string _connectionString;

        public DataService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }



        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            string sql = "SELECT Password FROM Users WHERE Email = @Email";

            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                string storedHash = await dbConnection.QueryFirstOrDefaultAsync<string>(sql, new { Email = email });

                if (storedHash != null && VerifyPassword(password, storedHash))
                {
                    return true; // User is valid
                }
                return false; // Invalid credentials
            }
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(inputPassword);
                byte[] inputHashBytes = sha256.ComputeHash(inputBytes);

                string inputHash = Convert.ToBase64String(inputHashBytes);
                return inputHash == storedHash;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public async Task<bool> RegisterUserAsync(string email, string password)
        {
            string checkSql = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                int count = await dbConnection.ExecuteScalarAsync<int>(checkSql, new { Email = email });
                if (count > 0)
                {
                    return false;
                }

                string hashedPassword = HashPassword(password);

                string sql = "INSERT INTO Users (Email, Password) VALUES (@Email, @Password)";
                int rowsAffected = await dbConnection.ExecuteAsync(sql, new { Email = email, Password = hashedPassword });
                return rowsAffected > 0;
            }
        }
    }
}
