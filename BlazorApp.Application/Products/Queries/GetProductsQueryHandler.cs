using BlazorApp.Application.Products.Queries;
using MediatR;
using BlazorApp.Infrastructure;
using BlazorApp.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Application.Products.Queries;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly AppDbContext _context;

    public GetProductsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _context.Products
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Color = p.Color,
                Model = p.Model
            })
            .ToListAsync(cancellationToken);

        return products;
    }
}