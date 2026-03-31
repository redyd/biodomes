using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.SpeciesPages;

public class SpeciesModel : AppPageModel
{
    private readonly IRepository _repository;

    public SpeciesModel(IRepository repository, IWebHostEnvironment environment): base(environment)
    {
        _repository = repository;
    }
    
    public List<Species> SpeciesList { get; set; }

    public void OnGet()
    {
        SpeciesList = _repository.FindAll();
    }
}