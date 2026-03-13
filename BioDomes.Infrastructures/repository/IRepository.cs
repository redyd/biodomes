using BioDomes.Domains.Core;
using BioDomes.Domains.InputModel;

namespace BioDomes.Infrastructures.repository;

public interface IRepository
{
    List<Species> FindAll();

    Species? FindBySlug(string slug);

    Species? Update(string slug, SpeciesInputModel input);
}