using System.Collections.Generic;

namespace RiverBooks.Common;

public interface IHaveDomainEvents
{
    IEnumerable<DomainEvent> DomainEvents { get; }
    void AddDomainEvent(DomainEvent domainEvent);
    void ClearDomainEvents();
}