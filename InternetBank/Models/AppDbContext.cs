using InternetBank.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InternetBank.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
		public DbSet<Transaction> Transactions { get; set; }
		public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}