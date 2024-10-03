using Microsoft.EntityFrameworkCore;
using TaskForSYNEL.Entities;

namespace TaskForSYNEL.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }
}