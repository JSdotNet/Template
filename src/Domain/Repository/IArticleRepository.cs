using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Domain.Repository;

public interface IArticleRepository : IRepository<Article>
{
    // Repository can have methods that are specific to the entity ...
}