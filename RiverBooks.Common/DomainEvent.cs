using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

namespace RiverBooks.Common;

public abstract class DomainEvent : INotification
{
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
}
