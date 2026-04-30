namespace Application.Products.Queries.GetProductById;

public sealed record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    int StockQuantity,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
