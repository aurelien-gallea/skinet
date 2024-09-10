using System;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StoreContext Context;

    public ProductsController(StoreContext storeContext)
    {
        this.Context = storeContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await Context.Products.ToListAsync();
    }

    [HttpGet("{id:int}")] //api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await Context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product) {
        Context.Products.Add(product);

        await Context.SaveChangesAsync();

        
        return product;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> updateProduct(int id, Product product) {
        if (product.Id != id || !ProductExist(id))
        {
            return BadRequest("Cannot update this product.");
        }

        Context.Entry(product).State = EntityState.Modified;

        await Context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
     {
        var product = await Context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        Context.Products.Remove(product);
        await Context.SaveChangesAsync();

        return NoContent();
     }

    private bool ProductExist(int id) {
        return Context.Products.Any(x => x.Id == id);
    }
}
