using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reeftrack.Models;


namespace ReefTrack.Data;

public class ApplicationDbContext : IdentityDbContext//Ärver IdentityDbContext
{
    //konstruktor
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    //DbSet för tabellerna
    public DbSet<Aquarium> Aquariums { get; set; }
    public DbSet<Models.Fish> Fishes { get; set; }
    public DbSet<Models.Coral> Corals { get; set; }

}
