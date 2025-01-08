using GenerateDatabaseObjects.Models;

namespace GenerateDatabaseObjects.Services;

// Services/Interfaces/IProductService.cs
public interface IProductService
{
    /// <summary>
    /// Retrieves a product by its ID and tenant ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Product if found, null otherwise</returns>
    Task<Product> GetProductById(int id, string tenantId);

    /// <summary>
    /// Retrieves all products for a specific tenant
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>List of products</returns>
    Task<List<Product>> GetProductsByTenant(string tenantId);

    /// <summary>
    /// Searches products based on search term within a tenant
    /// </summary>
    /// <param name="searchTerm">Search term to match against name and description</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>List of matching products</returns>
    Task<List<Product>> SearchProducts(string searchTerm, string tenantId);

    /// <summary>
    /// Adds a new product
    /// </summary>
    /// <param name="product">Product to add</param>
    /// <returns>ID of the newly created product</returns>
    Task<int> AddProduct(Product product);

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="product">Product with updated information</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task UpdateProduct(Product product);

    /// <summary>
    /// Deletes a product
    /// </summary>
    /// <param name="id">Product ID to delete</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task DeleteProduct(int id, string tenantId);
}