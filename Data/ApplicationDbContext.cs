using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reeftrack.Models;
using ReefTrack.Models;


namespace ReefTrack.Data;

public class ApplicationDbContext : IdentityDbContext//Ärver IdentityDbContext
{
    //konstruktor
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Behåll Identity-konfigurationen

        // Aquarium -> Fish (1:M)
        modelBuilder.Entity<Fish>()
            .HasOne(f => f.Aquarium)
            .WithMany(a => a.Fishes)
            .HasForeignKey(f => f.AquariumId)
            .OnDelete(DeleteBehavior.Cascade);

        // Aquarium -> Coral (1:M)
        modelBuilder.Entity<Coral>()
            .HasOne(c => c.Aquarium)
            .WithMany(a => a.Corals)
            .HasForeignKey(c => c.AquariumId)
            .OnDelete(DeleteBehavior.Cascade);
    }


    //DbSet för tabellerna
    public DbSet<Aquarium> Aquariums { get; set; }
    public DbSet<Models.Fish> Fishes { get; set; }
    public DbSet<Models.Coral> Corals { get; set; }

}
