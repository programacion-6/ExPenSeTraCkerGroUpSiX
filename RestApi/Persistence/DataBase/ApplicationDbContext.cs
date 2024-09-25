using Microsoft.EntityFrameworkCore;
using RestApi.Domain;

namespace RestApi.Persistence.DataBase;

public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }    
        public DbSet<Income> ExpenseCategory { get; set; }    
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }    
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    
    }

