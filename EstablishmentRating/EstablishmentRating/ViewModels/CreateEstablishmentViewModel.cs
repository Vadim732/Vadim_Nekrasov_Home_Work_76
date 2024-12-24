using System.ComponentModel.DataAnnotations;

namespace EstablishmentRating.ViewModels;

public class CreateEstablishmentViewModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public IFormFile Image { get; set; }
    [Required]
    [MinLength(30, ErrorMessage = "Описание должно быть не короче 30 символов! Расскажите о своём заведении подробнее с:")]
    public string Description { get; set; }
    
}