// -------------------------------------------------------------------------------------
//  <copyright file="GetEventByIdCommandHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.GetEventById;

using Domain.Events.Exceptions;
using MediatR;
using Repositories;

public class GetEventByIdCommandHandler : IRequestHandler<GetEventByIdCommand, GetEventByIdCommandResponse>
{
    private readonly IEventRepository _eventRepository;

    public GetEventByIdCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<GetEventByIdCommandResponse> Handle(GetEventByIdCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

        if (@event is null)
        {
            throw new EventNotFoundException(request.Id);
        }

        return new GetEventByIdCommandResponse
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            StartDate = @event.StartDate
        };
    }
}