using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Domain.Repository;

public interface IAuthorRepository : IRepository<Author, AuthorId>
{
    Task<Author?> FindByEmail(string email);
}
