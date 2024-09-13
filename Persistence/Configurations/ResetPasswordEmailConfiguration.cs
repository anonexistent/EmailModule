using Microsoft.EntityFrameworkCore;
using Models;

namespace Persistence.Configurations;

public static class ResetPasswordEmailConfiguration
{
    public static ModelBuilder ConfigureResetPasswordEmailTable(this ModelBuilder builder)
    {
        builder.Entity<ResetPasswordMessage>(entity =>
        {
            entity.ToTable("reset_password_emails");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id).ValueGeneratedOnAdd();

        });

        return builder;
    }
}
