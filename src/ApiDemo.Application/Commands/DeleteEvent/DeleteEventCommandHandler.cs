// -------------------------------------------------------------------------------------
//  <copyright file="DeleteEventCommandHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.DeleteEvent;

using MediatR;
using Repositories;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, DeleteEventCommandResponse>
{
    private readonly IEventRepository _eventRepository;

    public DeleteEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<DeleteEventCommandResponse> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        await _eventRepository.DeleteAsync(request.Id, cancellationToken);

        return new DeleteEventCommandResponse(request.Id, true);
    }
}