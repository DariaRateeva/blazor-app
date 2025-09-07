using  BlazorApp.Application.Products.Queries;
using  BlazorApp.Application.Products.Commands;
using BlazorApp.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var result = await _mediator.Send(new GetProductsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> PostProduct(ProductDto productDto)
    {
        var command = new CreateProductCommand(productDto);
        var productId = await _mediator.Send(command);
        
        var createdProduct = await _mediator.Send(new GetProductByIdQuery { Id = productId });
        return CreatedAtAction(nameof(GetProduct), new { id = productId }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, ProductDto productDto)
    {
        if (id != productDto.Id)
            return BadRequest();

        var command = new UpdateProductCommand(productDto);
        var result = await _mediator.Send(command);
        
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var command = new DeleteProductCommand { Id = id };
        var result = await _mediator.Send(command);
        
        if (!result)
            return NotFound();

        return NoContent();
    }
}
