using MediatR;
using BlazorApp.Domain.Entities;
using BlazorApp.Infrastructure;

namespace BlazorApp.Application.Products.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly AppDbContext _context;

    public CreateProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Product.Name,
            Color = request.Product.Color,
            Model = request.Product.Model
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}