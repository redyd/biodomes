using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Core;

namespace BioDomes.Web.Validators;

public class IsSpeciesClassificationAttribute :  ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult("La classification est requise.");

        if (Enum.TryParse<ESpeciesClassification>(value.ToString(), out _))
            return ValidationResult.Success;

        return new ValidationResult(
            $"La classification '{value}' est invalide.\n " +
            $"Valeurs acceptées : {string.Join(", ", Enum.GetNames<ESpeciesClassification>())}"
        );
    }
}