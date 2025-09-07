using MediatR;
using BlazorApp.Infrastructure;
using BlazorApp.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Application.Products.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly AppDbContext _context;

    public GetProductByIdQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Where(p => p.Id == request.Id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Color = p.Color,
                Model = p.Model
            })
            .FirstOrDefaultAsync(cancellationToken);

        return product;
    }
}