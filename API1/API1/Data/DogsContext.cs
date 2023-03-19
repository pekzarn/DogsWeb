using API1.Entities;
using Microsoft.EntityFrameworkCore;

namespace API1.Data
{
    public class DogsContext : DbContext
    {
        public DbSet<Dog> Dogs {get; set;}

        public DogsContext(DbContextOptions options) : base(options) { }
    }
}