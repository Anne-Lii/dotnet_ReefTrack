using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ReefTrack.Data;

public class ReefTrackIdentityDbContext : IdentityDbContext<IdentityUser>
{
    public ReefTrackIdentityDbContext(DbContextOptions<ReefTrackIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
       
    }
}
