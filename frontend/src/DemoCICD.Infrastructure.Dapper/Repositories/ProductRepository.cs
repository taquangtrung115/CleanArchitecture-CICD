

using Dapper;
using DemoCICD.Domain.Abstractions.Dappers.Repositories.Product;
using DemoCICD.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DemoCICD.Infrastructure.Dapper.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<int> AddAsync(Product entity)
    {
        var sql = "Insert Into Product (Id,Name,Price,Description) VALUES (@Id,@Name,@Price,@Description)";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.ExecuteAsync(sql, entity);
            return result;
        }
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM Product WHERE Id = @Id";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result;
        }
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        var sql = "SELECT Id, Name, Price, Description FROM Product";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.QueryAsync<Product>(sql);
            return result.ToList();
        }
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        var sql = "SELECT Id, Name, Price, Description FROM Product WHERE Id = @Id";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
            return result;
        }
    }

    public async Task<int> UpdateAsync(Product entity)
    {
        var sql = "UPDATE Product SET Name = @Name, Price = @Price, Description = @Description WHERE Id = @Id";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.ExecuteAsync(sql, entity);
            return result;
        }
    }
}
