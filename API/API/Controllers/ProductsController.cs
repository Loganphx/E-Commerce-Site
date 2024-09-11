using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
        [FromQuery] string? brand, 
        [FromQuery] string? type, 
        [FromQuery] string? sort)
    {
        IEnumerable<Product> products = await repository.GetProductsAsync(brand, type, sort);
        return Ok(products);
    }

    [HttpGet("{id:int}")] //api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repository.GetProductByIdAsync(id);

        if (product == null) return NotFound($"No product found for id {id}");
        
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        await repository.AddProductAsync(product);

        if (await repository.SaveChangesAsync())
        {
            return CreatedAtAction("GetProduct", new {id = product.Id});
        };

        return BadRequest("Failed to create product.");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !await ProductExists(id))
            return BadRequest("Cannot update this product");
        
        repository.UpdateProduct(product);

        if (await repository.SaveChangesAsync())
        {
            return NoContent();
        }
        
        return BadRequest("Problem updating the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repository.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        repository.DeleteProduct(product);

        if (await repository.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IEnumerable<string>>> GetBrands()
    {
        var brands = await repository.GetBrandsAsync();
        return Ok(brands);
    }
    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<string>>> GetTypes()
    {
        var types = await repository.GetTypesAsync();
        return Ok(types);
    }
    private async Task<bool> ProductExists(int id)
    {
        return await repository.ProductExists(id);
    }
}