using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Domain.Repository;

public interface IAuthorRepository : IRepository<Author>
{
    Task<Author?> FindByEmail(string email);
}
