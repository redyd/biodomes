using BioDomes.Domains.Core;
using BioDomes.Infrastructures.repository;
using BioDomes.Web.InputModel;
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
    
    public async Task<IActionResult> OnPost(string slug, string action)
    {
        Species = _repository.FindBySlug(slug);

        if (Input == null || !ModelState.IsValid)
        {
            return Page();
        }
        
        if (action == "delete")
        {
            _repository.Delete(slug);
            TempData["Message"] = "L'espece " + slug + " a bien été supprimé";
            return RedirectToPage("/SpeciesPages/Species");
        }

        if (Input.Image != null)
        {
            var fileName = $"{slug}{Path.GetExtension(Input.Image.FileName)}";
            var path = Path.Combine("wwwroot/images", fileName);

            using var stream = System.IO.File.Create(path);
            await Input.Image.CopyToAsync(stream);

            _repository.UpdateImage(slug, $"/images/{fileName}");
        }

        try
        {
            _repository.Update(slug, new SpeciesData
            {
                Name = Input.Name,
                Type = Input.Type,
                Diet = Input.Diet,
                Height = Input.Height
            });

            TempData["SuccessMessage"] = Input.Name;
            return RedirectToPage("/SpeciesPages/Species");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Input.Name", ex.Message);
            Species = _repository.FindBySlug(slug);
            return Page();
        }
    }
}