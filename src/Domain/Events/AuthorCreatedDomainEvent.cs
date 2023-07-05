
using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Domain.Events;

public sealed record AuthorCreatedDomainEvent(AuthorId AuthorId) : IDomainEvent;