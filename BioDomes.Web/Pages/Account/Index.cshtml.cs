using BioDomes.Infrastructures.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using BioDomes.Web.PageModels;

namespace BioDomes.Web.Pages.Account;

[Authorize]
public class Index(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, IWebHostEnvironment env) : AppPageModel(env)
{
    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        public DateOnly? Birthdate { get; set; }
        public string? SearchOrganism { get; set; }
        public string? ImageUrl { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password), StringLength(100, MinimumLength = 6)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password), Compare("NewPassword")]
        public string? ConfirmPassword { get; set; }
    }

    private bool WantsToChangePassword =>
        !string.IsNullOrEmpty(Input.CurrentPassword) ||
        !string.IsNullOrEmpty(Input.NewPassword) ||
        !string.IsNullOrEmpty(Input.ConfirmPassword);

    public async Task OnGetAsync()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return;
        }

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
        if (!WantsToChangePassword)
        {
            ModelState.Remove(nameof(Input.CurrentPassword));
            ModelState.Remove(nameof(Input.NewPassword));
            ModelState.Remove(nameof(Input.ConfirmPassword));
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        if (WantsToChangePassword && !await ValidateAndChangePasswordAsync(user) || !await UpdateProfileAsync(user))
        {
            return Page();
        }

        await signInManager.RefreshSignInAsync(user);
        SuccessNotification("Profil mis à jour avec succès !");
        return RedirectToPage();
    }

    private async Task<bool> ValidateAndChangePasswordAsync(UserEntity user)
    {
        if (string.IsNullOrEmpty(Input.CurrentPassword))
        {
            ErrorNotification("Le mot de passe actuel est requis.");
            return false;
        }

        if (string.IsNullOrEmpty(Input.NewPassword))
        {
            ErrorNotification("Le nouveau mot de passe est requis.");
            return false;
        }

        if (Input.NewPassword != Input.ConfirmPassword)
        {
            ErrorNotification("Les nouveaux mots de passe ne correspondent pas.");
            return false;
        }

        if (!await userManager.CheckPasswordAsync(user, Input.CurrentPassword))
        {
            ErrorNotification("Le mot de passe actuel est incorrect.");
            return false;
        }

        var result = await userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword);

        return result.Succeeded;
    }

    private async Task<bool> UpdateProfileAsync(UserEntity user)
    {
        var hasError = RestoreIfEmpty(Input.SearchOrganism, user.SearchOrganism, "L'organisme de recherche ne peut pas être vide.",
                           v => { Input.SearchOrganism = v; ModelState.Remove($"{nameof(Input)}.{nameof(Input.SearchOrganism)}"); })
                     | RestoreIfEmpty(Input.ImageUrl, user.ImageUrl, "L'avatar ne peut pas être vide.",
                           v => { Input.ImageUrl = v; ModelState.Remove($"{nameof(Input)}.{nameof(Input.ImageUrl)}"); })
                     | RestoreIfEmpty(Input.Birthdate?.ToString(), user.Birthdate?.ToString(), "La date de naissance ne peut pas être vide.",
                           _ => { Input.Birthdate = user.Birthdate; ModelState.Remove($"{nameof(Input)}.{nameof(Input.Birthdate)}"); });

        if (hasError)
        {
            return false;
        }

        user.Email = Input.Email;
        user.UserName = Input.Email;
        user.Birthdate = Input.Birthdate;
        user.SearchOrganism = Input.SearchOrganism;
        user.ImageUrl = Input.ImageUrl;

        var result = await userManager.UpdateAsync(user);
        
        return result.Succeeded;
    }

    private bool RestoreIfEmpty(string? inputValue, string? savedValue, string errorMessage, Action<string> restore)
    {
        if (!string.IsNullOrEmpty(inputValue) || savedValue == null)
        {
            return false;
        }

        ErrorNotification(errorMessage);
        restore(savedValue);
        return true;
    }
}