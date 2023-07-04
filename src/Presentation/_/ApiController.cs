using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace SolutionTemplate.Presentation.Api._;

public abstract class ApiController : ControllerBase
{
    protected ISender Sender { get; }

    protected ApiController(ISender sender)
    {
        Sender = sender;
    }
}