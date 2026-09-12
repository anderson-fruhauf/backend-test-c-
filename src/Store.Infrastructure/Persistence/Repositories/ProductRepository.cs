namespace Store.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Application.Abstractions;
using Domain.Entities;
using Persistence;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) => _context = context;

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _context.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
}