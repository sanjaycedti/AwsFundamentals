using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace Customers.Api.Database;

public interface IDbConnectionFactory
{
	Task<IDbConnection> CreateConenctionAsync();
}

public class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConenctionAsync()
    {
        var connection = new SqliteConnection(_connectionString);

        await connection.OpenAsync();

        return connection;
    }
}

