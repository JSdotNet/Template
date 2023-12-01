using SolutionTemplate.Domain._;

namespace SolutionTemplate.Domain.Articles.Repository;

public interface IArticleRepository : IRepository<Article>
{
    // Repository can have methods that are specific to the entity ...
}