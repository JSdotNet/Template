using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Domain.Models;
using SolutionTemplate.Domain.Repository;
using SolutionTemplate.Infrastructure.EF._;
using SolutionTemplate.Infrastructure.EF.Data;

namespace SolutionTemplate.Infrastructure.EF.Repository;


internal sealed class AuthorRepository : RepositoryBase<Author, AuthorId>, IAuthorRepository
{
    public AuthorRepository(DataContext dataContext) : base(dataContext) {}
    public Task<Author?> FindByEmail(string email)
    {
        return DataContext.Authors.FirstOrDefaultAsync(a => a.Email == email);
    }
}