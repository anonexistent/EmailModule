using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Persistence.Configurations;
using System.Security.Principal;

namespace Persistence;

public class EmailTestDbContext : DbContext
{
    public DbSet<ResetPasswordMessage> ResetPasswordMessages { get; set; }

    public EmailTestDbContext(DbContextOptions ops) : base(ops) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .ConfigureResetPasswordEmailTable();

        //RabbitDbContextConfiguration.ConfigureRabbitDbContext(modelBuilder);
    }
}
