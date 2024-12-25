using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EstablishmentRating.Models;

public class EstablishmentRatingContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Establishment> Establishments { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<GalleryImage> GalleryImages { get; set; }
    
    public EstablishmentRatingContext(DbContextOptions<EstablishmentRatingContext> options) : base(options) {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}