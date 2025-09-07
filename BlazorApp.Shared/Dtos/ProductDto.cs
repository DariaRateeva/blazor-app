using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Shared.Dtos;

public class ProductDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    public string Name { get; set; } = string.Empty;

    public string? Color { get; set; }
    
    public string? Model { get; set; }
}