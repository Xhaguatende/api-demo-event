// -------------------------------------------------------------------------------------
//  <copyright file="EventRepositoryMock.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Repositories;

using ApiDemo.Application.Repositories;
using Application.Services;
using Domain.Events;

public class EventRepositoryMock : IEventRepository
{
    private static readonly List<Event> Events = [];

    private readonly IApplicationContextService _applicationContextService;

    public EventRepositoryMock(IApplicationContextService applicationContextService)
    {
        _applicationContextService = applicationContextService;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existingEvent = Events.FirstOrDefault(x => x.Id == id);

        if (existingEvent == null)
        {
            return Task.CompletedTask;
        }

        Events.Remove(existingEvent);

        return Task.CompletedTask;
    }

    public Task<List<Event>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Events);
    }

    public Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Events.FirstOrDefault(x => x.Id == id));
    }

    public Task<Event> UpsertAsync(Event @event, CancellationToken cancellationToken = default)
    {
        var existingEvent = Events.FirstOrDefault(x => x.Id == @event.Id);

        var email = _applicationContextService.GetUserEmail();

        var date = DateTime.UtcNow;
        @event.UpdatedAt = date;
        @event.UpdatedBy = email;

        if (existingEvent is null)
        {
            @event.CreatedAt = date;
            @event.CreatedBy = email;

            Events.Add(@event);
            return Task.FromResult(@event);
        }

        existingEvent.Update( @event.Title, @event.Description, @event.StartDate);

        return Task.FromResult(existingEvent);
    }
}