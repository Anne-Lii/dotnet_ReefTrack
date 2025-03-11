using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reeftrack.Models;
using ReefTrack.Models;


namespace ReefTrack.Data;

public class ApplicationDbContext : IdentityDbContext//Ärver från IdentityDbContext för autentisering
{
    //konstruktor som tar emot konfigurationsalternativ för databasen
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    //konfigurerar relationer och beteenden i databasen
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Behåller Identity-konfigurationen

        //definierar relation mellan Aquarium och Fish (1:M)
        modelBuilder.Entity<Fish>()
            .HasOne(f => f.Aquarium)//en fisk hör till ett akvarium
            .WithMany(a => a.Fishes)//ett akvarium kan ha många fiskar
            .HasForeignKey(f => f.AquariumId)//FK till akvarium
            .OnDelete(DeleteBehavior.Cascade);//om ett akvarium raderas tas akvariets fiskar bort

        //definerar realtionen mellan Aquarium och Coral (1:M)
        modelBuilder.Entity<Coral>()
            .HasOne(c => c.Aquarium)
            .WithMany(a => a.Corals)
            .HasForeignKey(c => c.AquariumId)
            .OnDelete(DeleteBehavior.Cascade);
    }


    //DbSet respresenterar tabellerna i databasen
    public DbSet<Aquarium> Aquariums { get; set; }
    public DbSet<Models.Fish> Fishes { get; set; }
    public DbSet<Models.Coral> Corals { get; set; }

}
