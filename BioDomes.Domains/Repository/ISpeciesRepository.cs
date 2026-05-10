using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface ISpeciesRepository
{
    Task<ResultT<List<Specie>>> FindAllAsync(string? userId, bool isAdmin);
    Task<ResultT<Dictionary<string, Specie>>> FindAllAsDictAsync(string? userId, bool isAdmin);

    Task<ResultT<Specie>> FindBySlugAsync(string slug, string? userId, bool isAdmin);

    Task<ResultT<Specie>> UpdateAsync(UpdateSpecieRequest toUpdate, string? userId, bool isAdmin);

    Task<ResultT<Specie>> AddAsync(CreateSpecieRequest toAdd, string? userId);

    Task<ResultT<Specie>> DeleteAsync(string specieSlug, string? userId, bool isAdmin);
}