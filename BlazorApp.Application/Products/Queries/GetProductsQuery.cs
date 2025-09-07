using BlazorApp.Shared.Dtos;
using MediatR;

namespace BlazorApp.Application.Products.Queries;

public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
{
}