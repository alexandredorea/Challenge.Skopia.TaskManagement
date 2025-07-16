namespace Challenge.Skopia.TaskManagement.Api.Controllers.Base;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController<TController> : ControllerBase
    where TController : BaseController<TController>
{
    private ILogger<TController>? logger;
    private IMediator? mediator;

    internal ILogger<TController> Logger
        => logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<TController>>();

    internal IMediator Mediator
        => mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}