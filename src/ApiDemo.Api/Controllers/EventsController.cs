// -------------------------------------------------------------------------------------
//  <copyright file="EventsController.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Controllers;

using System.Net.Mime;
using Application.Commands.DeleteEvent;
using Application.Commands.GetEventById;
using Application.Commands.GetEvents;
using Application.Commands.UpsertEvent;
using Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Defines the <see cref="EventsController" />.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EventsController : ApiDemoControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventsController"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public EventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Deletes an event.
    /// </summary>
    /// <param name="id">The event id.</param>
    /// <returns>The delete event response.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "The event was deleted successfully.", typeof(DeleteEventCommandResponse))]
    [ProducesResponseType(typeof(DeleteEventCommandResponse), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEventAsync(Guid id)
    {
        var response = await _mediator.Send(new DeleteEventCommand(id));

        return Ok(response);
    }

    /// <summary>
    /// Gets an event.
    /// </summary>
    /// <param name="id">The event id.</param>
    /// <returns>The event response.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "The event was obtained successfully.", typeof(GetEventByIdCommandResponse))]
    [ProducesResponseType(typeof(GetEventByIdCommandResponse), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEventByIdAsync(Guid id)
    {
        var response = await _mediator.Send(new GetEventByIdCommand(id));

        return Ok(response);
    }

    /// <summary>
    /// Gets all the events.
    /// </summary>
    /// <returns>All the events.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "The events were obtained successfully.", typeof(GetEventsCommandResponse))]
    [ProducesResponseType(typeof(GetEventsCommandResponse), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    [HttpGet]
    public async Task<IActionResult> GetEventsAsync()
    {
        var response = await _mediator.Send(new GetEventsCommand());

        return Ok(response);
    }

    /// <summary>
    /// Upserts an event.
    /// </summary>
    /// <param name="command">The upsert event command.</param>
    /// <returns>The upserted event</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "The event was upserted successfully.", typeof(UpsertEventCommandResponse))]
    [ProducesResponseType(typeof(UpsertEventCommandResponse), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    [HttpPost]
    public async Task<IActionResult> UpsertEventAsync(UpsertEventCommand command)
    {
        var response = await _mediator.Send(command);

        return Ok(response);
    }
}