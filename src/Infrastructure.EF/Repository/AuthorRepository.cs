using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Domain.Authors;
using SolutionTemplate.Domain.Authors.Repository;
using SolutionTemplate.Infrastructure.EF._;
using SolutionTemplate.Infrastructure.EF.Data;

namespace SolutionTemplate.Infrastructure.EF.Repository;

internal sealed class AuthorRepository(DataContext dataContext) : RepositoryBase<Author>(dataContext), IAuthorRepository
{
    public Task<Author?> FindByEmail(string email)
    {
        return DataContext.Authors.FirstOrDefaultAsync(a => a.Email == email);
    }
}