using Microsoft.EntityFrameworkCore;
using RestApi.Domain;

namespace RestApi.Persistence.DataBase;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<ExpenseCategory> ExpenseCategory { get; set; }
    public DbSet<Expense> Expense { get; set; }
    public DbSet<Income> Income { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ExpenseCategory>().HasData(
            new ExpenseCategory { Name = "Electronics" },
            new ExpenseCategory { Name = "Transport" },
            new ExpenseCategory { Name = "Products" }
        );
    }
}