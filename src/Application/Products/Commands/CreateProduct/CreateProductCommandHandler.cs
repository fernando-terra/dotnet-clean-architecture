using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler(
    IProductRepository repository,
    IApplicationDbContext context) : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(request.Name, request.Description, request.Price, request.StockQuantity);
        await repository.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return product.Id;
    }
}
