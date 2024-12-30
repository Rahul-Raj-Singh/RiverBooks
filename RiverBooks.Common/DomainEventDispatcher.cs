using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace RiverBooks.Common;
public interface IDomainEventDispatcher
{
    Task DispatchAndClearDomainEvents(IEnumerable<IHaveDomainEvents> entitiesWithDomainEvents);
}

public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public async Task DispatchAndClearDomainEvents(IEnumerable<IHaveDomainEvents> entitiesWithDomainEvents)
    {
        foreach (var entity in entitiesWithDomainEvents)
        {
            foreach (var domainEvent in entity.DomainEvents.ToArray())
            {
                await mediator.Publish(domainEvent);
                entity.ClearDomainEvents();
            }
        }
    }
}