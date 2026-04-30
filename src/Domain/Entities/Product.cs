using Domain.Common;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Money Price { get; private set; } = Money.Zero();
    public int StockQuantity { get; private set; }
    public bool IsActive { get; private set; }

    private Product() { }

    public static Product Create(string name, string description, decimal price, int stockQuantity)
    {
        var product = new Product
        {
            Name = name,
            Description = description,
            Price = Money.Of(price),
            StockQuantity = stockQuantity,
            IsActive = true
        };
        product.AddDomainEvent(new ProductCreatedEvent(product.Id, name, price));
        return product;
    }

    public void Update(string name, string description, decimal price, int stockQuantity)
    {
        Name = name;
        Description = description;
        Price = Money.Of(price);
        StockQuantity = stockQuantity;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }

    public bool HasStock(int quantity) => StockQuantity >= quantity;
}
