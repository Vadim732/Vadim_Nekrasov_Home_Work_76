using EstablishmentRating.Models;

namespace EstablishmentRating.ViewModels;

public class EstablishmentIndexViewModel
{
    public List<Establishment> Establishments { get; set; }
    public PageViewModel PageViewModel { get; set; }
}