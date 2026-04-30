using Application.Common.Interfaces;
using Application.Products.Commands.CreateProduct;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Moq;

namespace Application.Tests.Products;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock = new();
    private readonly Mock<IApplicationDbContext> _contextMock = new();

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnGuid()
    {
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _contextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new CreateProductCommandHandler(_repositoryMock.Object, _contextMock.Object);
        var command = new CreateProductCommand("Product A", "Description A", 29.99m, 100);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeEmpty();
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCallAddAndSave()
    {
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>(), default)).Returns(Task.CompletedTask);
        _contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        var handler = new CreateProductCommandHandler(_repositoryMock.Object, _contextMock.Object);
        var command = new CreateProductCommand("Product B", "Desc B", 49.99m, 50);

        await handler.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(r => r.AddAsync(It.Is<Product>(p => p.Name == "Product B"), default), Times.Once);
    }
}
