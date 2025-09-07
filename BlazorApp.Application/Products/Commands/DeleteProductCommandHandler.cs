using MediatR;
using BlazorApp.Infrastructure;
using  BlazorApp.Application.Products.Commands;

namespace BlazorApp.Application.Products.Commands;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly AppDbContext _context;

    public DeleteProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.Id);
        
        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}