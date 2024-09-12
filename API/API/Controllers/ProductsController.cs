using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
        [FromQuery] ProductSpecParams specParams)
    {
        var spec = new ProductSpecification(specParams);
        
        var products = await repository.ListAsync(spec);
        
        return Ok(products);
    }

    [HttpGet("{id:int}")] //api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repository.GetByIdAsync(id);

        if (product == null) return NotFound($"No product found for id {id}");
        
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        await repository.AddAsync(product);

        if (await repository.SaveAllAsync())
        {
            return CreatedAtAction("GetProduct", new {id = product.Id});
        }

        return BadRequest("Failed to create product.");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !await ProductExists(id))
            return BadRequest("Cannot update this product");
        
        repository.Update(product);

        if (await repository.SaveAllAsync())
        {
            return NoContent();
        }
        
        return BadRequest("Problem updating the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repository.GetByIdAsync(id);

        if (product == null) return NotFound();

        repository.Remove(product);

        if (await repository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IEnumerable<string>>> GetBrands()
    {
        var spec = new ProductBrandListSpecification();
        
        var brands = await repository.ListAsync(spec);

        return Ok(brands);
    }
    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<string>>> GetTypes()
    {
        var spec = new ProductTypeListSpecification();
        
        var types = await repository.ListAsync(spec);
        return Ok(types);
    }
    private async Task<bool> ProductExists(int id)
    {
        return await repository.Exists(id);
    }
}