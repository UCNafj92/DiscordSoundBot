using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Abstractions
{
    public interface IEventBus
    {
        void Subscribe<T>(IEventHandler<T> handler)
            where T : IEvent;

        Task PublishAsync<T>(
            T @event,
            CancellationToken cancellationToken = default)
            where T : IEvent;
    }
}
