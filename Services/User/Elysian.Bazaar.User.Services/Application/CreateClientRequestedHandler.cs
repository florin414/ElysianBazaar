using Contracts.Events;
using Infrastructure.Npg.Contexts;
using Infrastructure.Npg.Entities;
using Infrastructure.RabbitMQ;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class CreateClientRequestedHandler(AccountsDbContext context, RabbitMqPublisher publisher) : IEventHandler<CreateClientRequested>
{
    private readonly AccountsDbContext context = context;
    private readonly RabbitMqPublisher publisher = publisher;
    public async Task HandleAsync(CreateClientRequested @event)
    {
        var account = await context.Accounts
            .FirstOrDefaultAsync(a => a.ClientId == @event.Client.Id);

        if (account is null)
            account = await CreateAccount(@event);

        var accountCreatedEvent = new AccountCreated(account.Id, account.ClientId,
            account.ClientName, account.ClientEmail);

        publisher.Publish(accountCreatedEvent, "accounts.events");
    }

    private async Task<Account> CreateAccount(CreateClientRequested @event)
    {
        var account = new Account
        {
            Id = Guid.NewGuid(),
            ClientId = @event.Client.Id,
            ClientName = @event.Client.Name,
            ClientEmail = @event.Client.Email
        };

        context.Accounts.Add(account);
        await context.SaveChangesAsync();
        return account;
    }
}