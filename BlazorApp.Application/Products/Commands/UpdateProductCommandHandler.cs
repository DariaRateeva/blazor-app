using MediatR;
using BlazorApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Application.Products.Commands;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly AppDbContext _context;

    public UpdateProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.Product.Id);
        
        if (product == null)
            return false;

        product.Name = request.Product.Name;
        product.Color = request.Product.Color;
        product.Model = request.Product.Model;

        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}