namespace EstablishmentRating.Models;

public class GalleryImage
{
    public int Id { get; set; }
    public string ImagePath { get; set; }
    
    public int EstablishmentId { get; set; }
    public Establishment? Establishment { get; set; }
}