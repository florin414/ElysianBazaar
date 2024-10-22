using Infrastructure.Npg.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Npg.Contexts;

public partial class AccountsDbContext : DbContext
{
    private static readonly Action<ModelBuilder> ConfigureModels = modelBuilder =>
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountsDbContext).Assembly);
    };

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureModels(modelBuilder);
    }
}