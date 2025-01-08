using GenerateDatabaseObjects.Data;
using GenerateDatabaseObjects.Models;
using GenerateDatabaseObjects.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace GenerateDatabaseObjects.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> GetProductById(int id, string tenantId)
    {
        return await _context.Products
            .FromSqlRaw($"SELECT * FROM {Functions.Gets.ProductById}(@p1, @p2)",
                new NpgsqlParameter("p1", id),
                new NpgsqlParameter("p2", tenantId))
            .FirstOrDefaultAsync();
    }

    public async Task<List<Product>> GetProductsByTenant(string tenantId)
    {
        return await _context.Products
            .FromSqlRaw($"SELECT * FROM {Functions.Gets.ProductsByTenant}(@p1)",
                new NpgsqlParameter("p1", tenantId))
            .ToListAsync();
    }

    public async Task<List<Product>> SearchProducts(string searchTerm, string tenantId)
    {
        return await _context.Products
            .FromSqlRaw($"SELECT * FROM {Functions.Searchs.Products}(@p1, @p2)",
                new NpgsqlParameter("p1", searchTerm),
                new NpgsqlParameter("p2", tenantId))
            .ToListAsync();
    }

    public async Task<int> AddProduct(Product product)
    {
        var productIdParam = new NpgsqlParameter
        {
            ParameterName = "p_product_id",
            NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer,
            Direction = ParameterDirection.InputOutput,
            Value = DBNull.Value
        };

        await _context.Database.ExecuteSqlRawAsync(
            $"CALL {StoredProcedures.Adds.Product}(@p_name, @p_description, @p_rate, @p_tenant_id, @p_product_id)",
            new NpgsqlParameter("p_name", product.Name),
            new NpgsqlParameter("p_description", product.Description),
            new NpgsqlParameter("p_rate", product.Rate),
            new NpgsqlParameter("p_tenant_id", product.TenantId),
            productIdParam
        );

        return (int)productIdParam.Value;
    }

    public async Task UpdateProduct(Product product)
    {
        await _context.Database.ExecuteSqlRawAsync(
            $"CALL {StoredProcedures.Updates.Product}(@p_id, @p_name, @p_description, @p_rate, @p_tenant_id)",
            new NpgsqlParameter("p_id", product.Id),
            new NpgsqlParameter("p_name", product.Name),
            new NpgsqlParameter("p_description", product.Description),
            new NpgsqlParameter("p_rate", product.Rate),
            new NpgsqlParameter("p_tenant_id", product.TenantId)
        );
    }

    public async Task DeleteProduct(int id, string tenantId)
    {
        await _context.Database.ExecuteSqlRawAsync(
            $"CALL {StoredProcedures.Deletes.Product}(@p_id, @p_tenant_id)",
            new NpgsqlParameter("p_id", id),
            new NpgsqlParameter("p_tenant_id", tenantId)
        );
    }
}