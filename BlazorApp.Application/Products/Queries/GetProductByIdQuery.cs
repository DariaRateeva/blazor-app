using MediatR;
using BlazorApp.Shared.Dtos;

namespace BlazorApp.Application.Products.Queries;

public class GetProductByIdQuery : IRequest<ProductDto?>
{
    public int Id { get; set; }
}