using Photography.Models;

namespace Photography.Data
{
    using System.Data.Entity;

    public class PhotographyContext : DbContext
    {
        public PhotographyContext()
            : base("name=PhotographyContext")
        {
        }

        public DbSet<Accessory> Accessories { get; set; }
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Lens> Lenses { get; set; }
        public DbSet<Photographer> Photographers { get; set; }
        public DbSet<Workshop> Workshops { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workshop>()
                .HasMany(w => w.Participants)
                .WithMany(p => p.WorkshopsParticipating)
                .Map(pw =>
                {
                    pw.MapLeftKey("WorkShopId");
                    pw.MapRightKey("PhotographerId");
                    pw.ToTable("WorkshopPhotographer");
                });
            modelBuilder.Entity<Camera>()
                .Map<DSLRCamera>(c => c.Requires("Type").HasValue("DSLR"))
                .Map<MirrorlessCamera>(c => c.Requires("Type").HasValue("Mirrorless"));
        }
    }
}