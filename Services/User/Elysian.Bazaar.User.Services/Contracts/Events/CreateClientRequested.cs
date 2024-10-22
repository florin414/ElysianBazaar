namespace Contracts.Events;

public record CreateClientRequested(Client Client, DateTimeOffset RequestedOn);