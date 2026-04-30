using Domain.Entities;
using Domain.Events;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests;

public class ProductTests
{
    [Fact]
    public void Create_ValidParameters_ShouldCreateProduct()
    {
        var product = Product.Create("Test Product", "Description", 99.99m, 10);

        product.Should().NotBeNull();
        product.Name.Should().Be("Test Product");
        product.Price.Amount.Should().Be(99.99m);
        product.Price.Currency.Should().Be("BRL");
        product.StockQuantity.Should().Be(10);
        product.IsActive.Should().BeTrue();
        product.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent()
    {
        var product = Product.Create("Test", "Desc", 10m, 5);

        product.DomainEvents.Should().ContainSingle();
        product.DomainEvents.First().Should().BeOfType<ProductCreatedEvent>();
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        var product = Product.Create("Test", "Desc", 10m, 5);
        product.Deactivate();

        product.IsActive.Should().BeFalse();
        product.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Money_WithNegativeAmount_ShouldThrow()
    {
        var act = () => Money.Of(-1m);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Money_Add_SameCurrency_ShouldSum()
    {
        var a = Money.Of(10m);
        var b = Money.Of(5m);
        var result = a.Add(b);

        result.Amount.Should().Be(15m);
        result.Currency.Should().Be("BRL");
    }

    [Fact]
    public void Money_Add_DifferentCurrency_ShouldThrow()
    {
        var brl = Money.Of(10m, "BRL");
        var usd = Money.Of(10m, "USD");

        var act = () => brl.Add(usd);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void HasStock_SufficientQuantity_ShouldReturnTrue()
    {
        var product = Product.Create("Test", "Desc", 10m, 5);
        product.HasStock(3).Should().BeTrue();
        product.HasStock(5).Should().BeTrue();
        product.HasStock(6).Should().BeFalse();
    }
}
