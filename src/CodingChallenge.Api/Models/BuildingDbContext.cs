using Microsoft.EntityFrameworkCore;

namespace CodingChallenge.Api.Models
{
    public class BuildingDbContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }

        public BuildingDbContext(DbContextOptions<BuildingDbContext> options)
            : base(options)
        {
        }
    }
}
