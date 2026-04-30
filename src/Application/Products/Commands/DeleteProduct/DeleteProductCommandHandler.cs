using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Repositories;
using MediatR;

namespace Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler(
    IProductRepository repository,
    IApplicationDbContext context) : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);

        repository.Delete(product);
        await context.SaveChangesAsync(cancellationToken);
    }
}
