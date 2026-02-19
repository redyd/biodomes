using BioDomes.Domains.Core;

namespace BioDomes.Infrastructures.repository;

public interface IRepository
{
    List<Species> FindAll();
}