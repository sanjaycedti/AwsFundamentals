using System;
using Consumers.Api.Contracts.Data;
using Consumers.Api.Database;
using Dapper;

namespace Consumers.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

	public CustomerRepository(IDbConnectionFactory dbConnectionFactory)
	{
        _dbConnectionFactory = dbConnectionFactory;
	}

    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        using var connection = await _dbConnectionFactory.CreateConenctionAsync();

        var result = await connection.ExecuteAsync(
            @"INSERT INTO Customers (Id, GitHubUsername, FullName, Email, DateOfBirth)
            VALUES (@Id, @GitHubUsername, @FullName, @Email, @DateOfBirth)",
        customer);

        return result > 0;
    }

    public async Task<CustomerDto> GetAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConenctionAsync();

        return await connection.QuerySingleOrDefaultAsync<CustomerDto>(
            "SELECT * FROM Customers WHERE Id=@Id LIMIT 1", new { Id = id });
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConenctionAsync();

        return await connection.QueryAsync<CustomerDto>("SELECT * FROM Customers");
    }

    public async Task<bool> UpdateAsync(CustomerDto customer)
    {
        using var connection = await _dbConnectionFactory.CreateConenctionAsync();

        var result = await connection.ExecuteAsync(
            @"
            UPDATE Customers SET GitHubUsername = @GitHubUsername, FullName = @FullName, Email = @Email, DateOfBirth = @DateOfBirth
            WHERE Id=@Id", customer);

        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConenctionAsync();

        var result = await connection.ExecuteAsync(@"DELETE FROM Customers WHERE Id = @Id",
            new { Id = id });

        return result > 0;
    }
}

