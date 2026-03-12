using BioDomes.Domains.Core;
using BioDomes.Infrastructures.repository;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.SpeciesPages;

public class SpeciesModel : PageModel
{
    
    private readonly IRepository _repository;

    public SpeciesModel(IRepository repository)
    {
        _repository = repository;
    }
    
    public List<Species> SpeciesList { get; set; }

    public void OnGet()
    {
        SpeciesList = _repository.FindAll();
    }
}