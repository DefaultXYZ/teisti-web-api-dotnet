using System.ComponentModel.DataAnnotations;

namespace Teisti.API.Models;

public record CreateProductRequest(
    [Required] string Name,
    [Required, Range(0.01, double.MaxValue)]
    decimal Price);
