using System.ComponentModel.DataAnnotations;

namespace EstablishmentRating.Models;

public class Establishment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    
    public int? UserId { get; set; }
    public User? User { get; set; }
    
    public List<GalleryImage>? GalleryImages { get; set; }
    
    public ICollection<Review>? Reviews { get; set; }
}