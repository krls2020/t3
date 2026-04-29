using Microsoft.EntityFrameworkCore;
using Reminders.Models;

namespace Reminders.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Reminder> Reminders => Set<Reminder>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Reminder>().Property(r => r.Title).IsRequired().HasMaxLength(200);
        b.Entity<Reminder>().Property(r => r.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
