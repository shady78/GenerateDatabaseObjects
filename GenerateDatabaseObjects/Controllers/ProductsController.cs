using GenerateDatabaseObjects.Models;
using GenerateDatabaseObjects.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenerateDatabaseObjects.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id, [FromHeader(Name = "TenantId")] string tenantId)
    {
        try
        {
            var product = await _productService.GetProductById(id, tenantId);
            if (product == null)
                return NotFound($"Product with ID {id} not found for tenant {tenantId}");

            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving the product");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetByTenant([FromHeader(Name = "TenantId")] string tenantId)
    {
        try
        {
            var products = await _productService.GetProductsByTenant(tenantId);
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving products");
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Product>>> Search(
        [FromQuery] string searchTerm,
        [FromHeader(Name = "TenantId")] string tenantId)
    {
        try
        {
            var products = await _productService.SearchProducts(searchTerm, tenantId);
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while searching for products");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(
        [FromBody] Product product,
        [FromHeader(Name = "TenantId")] string tenantId)
    {
        try
        {
            product.TenantId = tenantId;
            var productId = await _productService.AddProduct(product);
            product.Id = productId;

            return CreatedAtAction(
                nameof(GetById),
                new { id = productId, tenantId },
                product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while creating the product");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] Product product,
        [FromHeader(Name = "TenantId")] string tenantId)
    {
        try
        {
            if (id != product.Id)
                return BadRequest("ID mismatch");

            product.TenantId = tenantId;
            await _productService.UpdateProduct(product);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the product");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, [FromHeader(Name = "TenantId")] string tenantId)
    {
        try
        {
            await _productService.DeleteProduct(id, tenantId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while deleting the product");
        }
    }
}