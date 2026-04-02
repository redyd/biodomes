using BioDomes.Domains.Repository;
using BioDomes.Web.PageModels;
using BioDomes.Web.ViewModels;

namespace BioDomes.Web.Pages.SpeciesPages;

public class SpeciesModel : AppPageModel
{
    private readonly IRepository _repository;

    public SpeciesModel(IRepository repository, IWebHostEnvironment environment): base(environment)
    {
        _repository = repository;
    }

    public List<SpecieView> SpeciesList { get; set; } = [];

    public void OnGet()
    {
        SpeciesList = _repository
            .FindAll()
            .Select(specie =>
            {
                var path = specie.ImageUrl != null ? UseFileFromPath(specie.ImageUrl) : null;
                
                return new SpecieView
                {
                    Name = specie.Name,
                    Type = specie.Type,
                    Diet = specie.Diet,
                    Height = specie.Height,
                    ImageUrl = path
                };
            })
            .ToList();
    }
}