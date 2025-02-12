using Clean_Life_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Clean_Life_API.ApplicationDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // تعريف جدول المستخدمين
        public DbSet<user> Users { get; set; }
    }
}
