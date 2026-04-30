using Application.Products.Queries.GetProductById;
using MediatR;

namespace Application.Products.Queries.GetAllProducts;

public sealed record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>;
