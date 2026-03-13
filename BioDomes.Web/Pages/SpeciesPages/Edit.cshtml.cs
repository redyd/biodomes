using BioDomes.Domains.Core;
using BioDomes.Domains.InputModel;
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
    
    [BindProperty]
    public SpeciesInputModel? Input  { get; set; }

    public IActionResult OnGet(string slug)
    {
        Species = _repository.FindBySlug(slug);

        if (Species == null)
            return NotFound();

        Input = new SpeciesInputModel
        {
            Name = Species.Name,
            Type = Species.Type,
            Diet = Species.Diet,
            Height = Species.Height
        };
        
        return Page();
    }
    
    public IActionResult OnPost(string slug)
    {
        if (Input == null || !ModelState.IsValid)
            return Page();

        _repository.Update(slug, Input);
        return RedirectToPage("/SpeciesPages/Species");
    }
    
}