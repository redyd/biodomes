using BioDomes.Domains.Repository;
using BioDomes.Web.PageModels;
using BioDomes.Web.ViewModels;

namespace BioDomes.Web.Pages.SpeciesPages;

public class SpeciesModel : AppPageModel
{
    private readonly ISpeciesRepository _speciesRepository;

    public SpeciesModel(ISpeciesRepository speciesRepository, IWebHostEnvironment environment): base(environment)
    {
        _speciesRepository = speciesRepository;
    }

    public List<SpecieView> SpeciesList { get; set; } = [];

    public async Task OnGetAsync()
    {
        var species = await _speciesRepository.FindAllAsync();
        SpeciesList = species
            .Select(specie =>
            {
                var path = specie.ImageUrl != null ? UseFileFromPath(specie.ImageUrl) : null;
                
                return new SpecieView
                {
                    Name = specie.Name,
                    Type = specie.Type,
                    Diet = specie.Diet,
                    Height = FormatCentimeter(specie.Height),
                    Weight = FormatGram(specie.Weight),
                    ImageUrl = path
                };
            })
            .ToList();
    }
}
