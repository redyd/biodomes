using BioDomes.Domains.Core;
using BioDomes.Web.ViewModels;

namespace BioDomes.Web.Mapper;

public class SpecieViewMapper : ISpecieViewMapper
{
    public SpecieView MapSingle(Specie specie, bool isAdmin, string? userId, Func<string?, string> resolveImage)
    {
        return new SpecieView
        {
            Slug = specie.Slug,
            Name = specie.Name,
            Type = specie.Classification.Label,
            Diet = specie.Diet.Label,
            Height = FormatCentimeter(specie.Height),
            Weight = FormatGram(specie.Weight),
            ImageUrl = resolveImage(specie.ImageUrl),
            CanEdit = userId is not null && (isAdmin || userId == specie.CreatedBy),
        };
    }

    public List<SpecieView> MapAll(List<Specie> species, bool isAdmin, string? userId,
        Func<string?, string> resolveImage)
    {
        return species.Select(specie => MapSingle(specie, isAdmin, userId, resolveImage)).ToList();
    }

    private static string FormatGram(long grams)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(grams);

        string[] units = { "gramme", "kilogramme", "tonne", "kilotonne", "mégatonne" };

        double value = grams;
        var unitIndex = 0;

        while (value >= 1000 && unitIndex < units.Length - 1)
        {
            value /= 1000;
            unitIndex++;
        }

        var unit = units[unitIndex];

        if (value > 1)
        {
            unit += "s";
        }

        return $"{value:0.##} {unit}";
    }

    private static string FormatCentimeter(long centimeters)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(centimeters);
        var tolerance = 0.1;
        double value;
        string unit;

        if (centimeters >= 100)
        {
            value = centimeters / 100.0;
            unit = Math.Abs(value - 1) < tolerance ? "mètre" : "mètres";
        }
        else
        {
            value = centimeters;
            unit = Math.Abs(value - 1) < tolerance ? "centimètre" : "centimètres";
        }

        return $"{value:0.##} {unit}";
    }
}