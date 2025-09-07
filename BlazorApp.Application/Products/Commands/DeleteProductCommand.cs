using MediatR;

namespace BlazorApp.Application.Products.Commands;

public class DeleteProductCommand : IRequest<bool>
{
    public int Id { get; set; }
}