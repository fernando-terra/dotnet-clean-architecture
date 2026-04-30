using Domain.Common;

namespace Domain.Events;

public sealed record ProductCreatedEvent(Guid ProductId, string Name, decimal Price) : IDomainEvent;
