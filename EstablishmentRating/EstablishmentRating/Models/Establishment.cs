using System.ComponentModel.DataAnnotations;

namespace EstablishmentRating.Models;

public class Establishment
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    [Url]
    public string Image { get; set; }
    [Required]
    [MinLength(30, ErrorMessage = "Описание должно быть не короче 30 символов! Расскажите о своём заведении подробнее с:")]
    public string Description { get; set; }
    
    public int? UserId { get; set; }
    public User? User { get; set; }
    
    public ICollection<Review>? Reviews { get; set; }
}