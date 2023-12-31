﻿using Microsoft.EntityFrameworkCore;
using Online_Store_ASP.NET_Core_MVC.Models;

public interface IProductRepository
{
    void Add(Product product);
    Task SaveChangesAsync();
    Task<Product> GetByIdAsync(int id);
}

public class ProductsController : IProductRepository
{
    private readonly DbContextProject _dbContext;

    public ProductsController(DbContextProject dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Product product)
    {
        _dbContext.Product.Add(product);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _dbContext.Product.FindAsync(id);
    }
}