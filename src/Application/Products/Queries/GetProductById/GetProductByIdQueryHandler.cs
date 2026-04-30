using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler(IProductRepository repository)
    : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);

        return new ProductDto(
            product.Id, product.Name, product.Description,
            product.Price.Amount, product.Price.Currency,
            product.StockQuantity, product.IsActive,
            product.CreatedAt, product.UpdatedAt);
    }
}
