using Microsoft.EntityFrameworkCore;

namespace DateApp.EntityContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        //public DbSet<UserPreferences> Users_Preferences { get; set; }
        public DbSet<EducationDegrees> Educations { get; set; }

        //public DbSet<AllServices> ListAllServices { get; set; }
        //public DbSet<CompletedServices> CompletedServices { get; set; }
        public DbSet<Couples> User_Couples { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
           : base(options)
        {
        }

        public ApplicationContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                 .HasIndex(u => new { u.Email })
                 .IsUnique();
            //modelBuilder.Entity<UserPreferences>()
            //   .HasIndex(up => new { up.UserId })
            //   .IsUnique();
            //modelBuilder.Entity<CompletedServices>()
            //   .HasIndex(Cs => new { Cs.UserId })
            //.IsUnique();
            modelBuilder.Entity<Couples>()
              .HasKey(C => new { C.FirstUserId, C.SecondUserId });

            modelBuilder.Entity<EducationDegrees>()
            .HasKey(C => new { C.Id});

            //modelBuilder.Entity<CompletedServices>()
            //  .HasIndex(C => new { C.Id })
            //  .IsUnique();
            //modelBuilder.Entity<AllServices>().HasNoKey();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=dateapp;Username=postgres;Password=root");
        }
    }
}