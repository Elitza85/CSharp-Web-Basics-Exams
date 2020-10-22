namespace SharedTrip.Data
{
    using Microsoft.Data.SqlClient.Server;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Cryptography.X509Certificates;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions db)
            : base(db)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<UserTrip> UserTrips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=SharedTrip;Integrated Security=true;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserTrip>().HasKey(x => new
            {
                x.TripId,
                x.UserId
            });
        }
    }
}
