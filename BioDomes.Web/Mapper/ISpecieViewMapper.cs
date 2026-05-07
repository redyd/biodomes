using BioDomes.Domains.Core;
using BioDomes.Web.ViewModels;

namespace BioDomes.Web.Mapper;

public interface ISpecieViewMapper
{
    SpecieView MapSingle(Specie specie, bool isAdmin, string? userId, Func<string?, string> resolveImage);

    List<SpecieView> MapAll(List<Specie> species, bool isAdmin, string? userId,
        Func<string?, string> resolveImage);
}