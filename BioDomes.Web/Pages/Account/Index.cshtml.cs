using BioDomes.Infrastructures.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using BioDomes.Web.PageModels;

namespace BioDomes.Web.Pages.Account;

[Authorize]
public class Index(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, IWebHostEnvironment environment) : AppPageModel(environment)
{
    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public DateOnly? Birthdate { get; set; }
        public string? SearchOrganism { get; set; }
        public string? ImageUrl { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string? ConfirmPassword { get; set; }
    }

    public async Task OnGetAsync()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return;

        Input = new InputModel
        {
            Email = user.Email,
            Birthdate = user.Birthdate,
            SearchOrganism = user.SearchOrganism,
            ImageUrl = user.ImageUrl
        };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        user.Email = Input.Email;
        user.UserName = Input.Email;
        user.Birthdate = Input.Birthdate;
        user.SearchOrganism = Input.SearchOrganism;
        user.ImageUrl = Input.ImageUrl;

        await userManager.UpdateAsync(user);

        if (!string.IsNullOrEmpty(Input.NewPassword) && !string.IsNullOrEmpty(Input.CurrentPassword))
        {
            await userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword);
        }

        await signInManager.RefreshSignInAsync(user);

        SuccessNotification("Profil mis à jour avec succès !");
        return RedirectToPage();
    }
}