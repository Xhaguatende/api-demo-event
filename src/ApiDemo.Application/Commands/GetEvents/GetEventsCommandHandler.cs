// -------------------------------------------------------------------------------------
//  <copyright file="GetEventsCommandHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.GetEvents;

using MediatR;
using Repositories;

public class GetEventsCommandHandler : IRequestHandler<GetEventsCommand, List<GetEventsCommandResponse>>
{
    private readonly IEventRepository _eventRepository;

    public GetEventsCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<List<GetEventsCommandResponse>> Handle(
        GetEventsCommand request,
        CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllAsync(cancellationToken);

        return events.Select(
            e => new GetEventsCommandResponse
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                StartDate = e.StartDate,
                CreatedBy = e.CreatedBy,
                UpdatedBy = e.UpdatedBy
            }).ToList();
    }
}