using Infrastructure.Npg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Npg.Configurations;

public class AccountConfiguration: IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable(nameof(Account));
        
        builder.HasKey(o => o.Id);
        builder.Property(a => a.ClientId).
            IsRequired();
        builder.Property(a => a.ClientEmail).
            IsRequired();
        builder.Property(a => a.ClientName).
            IsRequired();
    }
}