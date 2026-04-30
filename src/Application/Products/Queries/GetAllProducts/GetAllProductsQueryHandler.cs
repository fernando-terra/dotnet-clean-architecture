using Application.Products.Queries.GetProductById;
using Domain.Repositories;
using MediatR;

namespace Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsQueryHandler(IProductRepository repository)
    : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        return products.Select(p => new ProductDto(
            p.Id, p.Name, p.Description,
            p.Price.Amount, p.Price.Currency,
            p.StockQuantity, p.IsActive,
            p.CreatedAt, p.UpdatedAt));
    }
}
