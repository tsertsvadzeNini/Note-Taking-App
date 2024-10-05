using Microsoft.EntityFrameworkCore;
using Note_Taking_App.Models;

namespace Note_Taking_App.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }
    }
}
