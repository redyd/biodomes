using BioDomes.Domains.Core;
using BioDomes.Infrastructures.repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.SpeciesPages;

public class EditModel : PageModel
{
    private readonly IRepository _repository;

    public EditModel(IRepository repository)
    {
        _repository = repository;
    }

    public Species? Species { get; set; }

    public IActionResult OnGet(string slug)
    {
        Species = _repository.FindBySlug(slug);

        if (Species == null)
            return NotFound();

        return Page();
    }
}