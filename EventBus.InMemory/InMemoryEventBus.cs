using EventBus.Abstractions;

namespace EventBus.InMemory
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly Dictionary<Type, List<object>> _handlers = new();

        public void Subscribe<T>(IEventHandler<T> handler) where T : IEvent

        {
            var type = typeof(T);

            if (!_handlers.ContainsKey(type))
            {
                _handlers[type] = new List<object>();
            }

            _handlers[type].Add(handler);
        }

        public async Task PublishAsync<T>(
            T @event,
            CancellationToken cancellationToken = default)
            where T : IEvent
        {
            var type = typeof(T);

            if (!_handlers.TryGetValue(type, out var handlers))
                return;

            await Task.WhenAll(
                handlers.Select(handler =>
                    ((IEventHandler<T>)handler).HandleAsync(@event, cancellationToken)));
        }
    }
}
