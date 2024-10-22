namespace Contracts.Events;

public interface IEventHandler<T>
{
    public Task HandleAsync(T @event);
}