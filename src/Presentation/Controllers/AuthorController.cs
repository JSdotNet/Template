using MediatR;

using Microsoft.AspNetCore.Mvc;

using SolutionTemplate.Application.Authors.Commands;
using SolutionTemplate.Application.Authors.Queries;
using SolutionTemplate.Presentation.Api._;

namespace SolutionTemplate.Presentation.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ApiController
{
    public AuthorController(ISender sender) : base(sender) { }


    [HttpPost(Name = "CreateAuthor")]
    public async Task<ActionResult<Guid>> Create(CreateAuthor.Command command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return CreatedAtRoute("GetAuthorById", new {id = (Guid) result}, (Guid) result);
    }

    [HttpGet(Name = "GetAllAuthors")]
    public async Task<ActionResult<IList<GetAuthors.Response>>> GetAll(CancellationToken cancellationToken)
    {
        var authors = await Sender.Send(new GetAuthors.Query(), cancellationToken);

        return Ok((IList<GetAuthors.Response>) authors.Value);
    }

    [HttpGet("{id}", Name = "GetAuthorById")]
    public async Task<ActionResult<GetAuthor.Response>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var author = await Sender.Send(new GetAuthor.Query(id), cancellationToken);

        return Ok((GetAuthor.Response) author);
    }

    [HttpPut( Name = "UpdateAuthor")]
    public async Task<IActionResult> UpdateAsync(UpdateAuthor.Command command, CancellationToken cancellationToken)
    {
        await Sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteAuthor")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await Sender.Send(new DeleteAuthor.Command(id), cancellationToken);

        return NoContent();
    }
}