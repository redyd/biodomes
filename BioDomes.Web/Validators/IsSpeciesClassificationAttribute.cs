using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Repository;

namespace BioDomes.Web.Validators;

public class IsSpeciesClassificationAttribute :  ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult("La classification est requise.");

        var repository = validationContext.GetService(typeof(IClassificationRepository)) as IClassificationRepository;
        if (repository == null)
            return new ValidationResult("Impossible de valider la classification.");

        var input = value.ToString();
        if (input != null && repository.FetchAll().Contains(input, StringComparer.OrdinalIgnoreCase))
            return ValidationResult.Success;

        return new ValidationResult(
            $"La classification '{value}' est invalide.\n " +
            $"Valeurs acceptées : {string.Join(", ", repository.FetchAll())}"
        );
    }
}
