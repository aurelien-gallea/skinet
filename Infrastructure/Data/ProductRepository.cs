using System;
using System.Security.Cryptography.X509Certificates;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{

    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await context.Products.Select(x => x.Brand).Distinct().ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams)
    {
        string brands = "";
        string types = "";


        if (specParams.Brands.Count > 0)
        {
            foreach (var b in specParams.Brands)
            {
                brands += b + ",";
            }
            brands = brands.Substring(0, brands.Length - 1);
        }

        if (specParams.Types.Count > 0)
        {
            foreach (var t in specParams.Types)
            {
                types += t + ",";
            }
            types = types.Substring(0, types.Length - 1);
        }

        
        var query = context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brands))
        {
            query = query.Where(x => brands.Contains(x.Brand));
        }
        if(!string.IsNullOrWhiteSpace(types))
        {
            query =query.Where(x => types.Contains(x.Type));
        }
        
        if (!string.IsNullOrWhiteSpace(specParams.Search))
        {
            query = query.Where(x => x.Name.Contains(specParams.Search));
        }
        query = specParams.Sort switch
        {
            "priceAsc" => query.OrderBy(x => x.Price),
            "priceDesc" => query.OrderByDescending(x => x.Price),
            "name" => query.OrderBy(x=> x.Name),
            _ => query.OrderBy(x =>x.Name)
        };
        

        return await query.ToListAsync();
    }


    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await context.Products.Select(x => x.Type).Distinct().ToListAsync();
    }

    public bool ProductExists(int id)
    {
        return context.Products.Any(x=> x.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
        context.Entry(product).State = EntityState.Modified;
    }
}
