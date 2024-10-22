using Infrastructure.Npg.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Npg.Contexts;

public partial class AccountsDbContext : DbContext
{
    public AccountsDbContext(DbContextOptions<AccountsDbContext> options) : base(options) { }
    
    public DbSet<Account> Accounts { get; set; }
}