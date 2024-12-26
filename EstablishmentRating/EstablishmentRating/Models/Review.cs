namespace EstablishmentRating.Models;

public class Review
{
    public int Id { get; set; }
    public int Stars { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public int? UserId { get; set; }
    public User? User { get; set; }
    
    public int? EstablishmentId { get; set; }
    public Establishment? Establishment { get; set; }
}