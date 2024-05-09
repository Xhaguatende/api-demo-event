// -------------------------------------------------------------------------------------
//  <copyright file="UpsertEventCommandHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.UpsertEvent;

using Domain.Events;
using Domain.Events.Exceptions;
using MediatR;
using Repositories;

public class UpsertEventCommandHandler : IRequestHandler<UpsertEventCommand, UpsertEventCommandResponse>
{
    private readonly IEventRepository _eventRepository;

    public UpsertEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<UpsertEventCommandResponse> Handle(
        UpsertEventCommand request,
        CancellationToken cancellationToken)
    {
        Event @event;

        if (request.Id is null)
        {
            @event = new Event(
                Guid.NewGuid(),
                request.Title,
                request.Description,
                request.StartDate);
        }
        else
        {
            var existingEvent = await _eventRepository.GetByIdAsync(request.Id.Value, cancellationToken);

            @event = existingEvent ?? throw new EventNotFoundException(request.Id.Value);

            @event.Update(
                request.Title,
                request.Description,
                request.StartDate);
        }

        await _eventRepository.UpsertAsync(@event, cancellationToken);

        return new UpsertEventCommandResponse
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            StartDate = @event.StartDate
        };
    }
}