using Teisti.API.Models;
using Teisti.Domain.Models;

namespace Teisti.API.Extensions;

public static class ProductExtensions
{
    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse(product.Id, product.Name, product.Price);
    }
}
