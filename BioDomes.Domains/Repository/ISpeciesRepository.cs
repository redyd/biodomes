using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface ISpeciesRepository
{
    Task<Result<List<Specie>>> FindAllAsync(string? userId, bool isAdmin);
    Task<Result<Dictionary<int, Specie>>> FindAllAsDictAsync(string? userId, bool isAdmin);

    Task<Result<int>> CountAsync();

    Task<Result<Specie>> FindBySlugAsync(string slug, string? userId, bool isAdmin);

    Task<Result<Specie>> UpdateAsync(UpdateSpecieRequest toUpdate, string? userId, bool isAdmin);

    Task<Result<Specie>> AddAsync(CreateSpecieRequest toAdd, string? userId);

    Task<Result<Specie>> DeleteAsync(int specieId, string? userId, bool isAdmin);
}