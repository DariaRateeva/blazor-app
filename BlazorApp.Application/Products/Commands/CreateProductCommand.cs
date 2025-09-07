using MediatR;
using BlazorApp.Shared.Dtos;

namespace BlazorApp.Application.Products.Commands;
public class CreateProductCommand : IRequest<int>
{
    public ProductDto Product { get; set; } = new();
    
    public CreateProductCommand(ProductDto product)
    {
        Product = product;
    }
    
    public CreateProductCommand() { }
}
