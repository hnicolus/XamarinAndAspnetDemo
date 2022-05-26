using System;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=appdb.db");
        }
    }
}