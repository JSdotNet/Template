using MediatR;

using Microsoft.AspNetCore.Http;
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Create(CreateAuthor.Command command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        //return Ok((Guid) result);

        return CreatedAtRoute("GetAuthorById", new {id = (Guid) result}, result);
    }

    [HttpGet(Name = "GetAllAuthors")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<GetAuthors.Response>>> GetAll(CancellationToken cancellationToken)
    {
        var authors = await Sender.Send(new GetAuthors.Query(), cancellationToken);

        return Ok((IList<GetAuthors.Response>) authors.Value);
    }

    [HttpGet("{id}", Name = "GetAuthorById")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetAuthor.Response>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var author = await Sender.Send(new GetAuthor.Query(id), cancellationToken);

        return Ok((GetAuthor.Response) author);
    }

    [HttpPut( Name = "UpdateAuthor")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(UpdateAuthor.Command command, CancellationToken cancellationToken)
    {
        await Sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteAuthor")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await Sender.Send(new DeleteAuthor.Command(id), cancellationToken);

        return NoContent();
    }


    // TODO Check swagger for possible responses
}