using Microsoft.EntityFrameworkCore;
using RestApi.Domain;

namespace RestApi.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserGoals> UsersGoals { get; set; }
    public DbSet<UserItems> UsersItems { get; set; }
}