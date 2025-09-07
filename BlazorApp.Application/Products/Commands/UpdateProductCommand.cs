using MediatR;
using BlazorApp.Shared.Dtos;

namespace BlazorApp.Application.Products.Commands;

public class UpdateProductCommand : IRequest<bool>
{
    public ProductDto Product { get; set; } = new();
    
    public UpdateProductCommand(ProductDto product)
    {
        Product = product;
    }
    
    public UpdateProductCommand() { }
}
