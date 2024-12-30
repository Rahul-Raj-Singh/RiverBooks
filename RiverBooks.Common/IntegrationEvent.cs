using System;
using MediatR;

namespace RiverBooks.Common;

public abstract class IntegrationEvent : INotification
{
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
}