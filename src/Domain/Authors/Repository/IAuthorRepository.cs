using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Authors;

namespace SolutionTemplate.Domain.Authors.Repository;

public interface IAuthorRepository : IRepository<Author>
{
    Task<Author?> FindByEmail(string email);
}
