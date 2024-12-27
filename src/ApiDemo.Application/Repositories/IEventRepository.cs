// -------------------------------------------------------------------------------------
//  <copyright file="IEventRepository.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Repositories;

using Domain.Events;

public interface IEventRepository
{
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Event>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Event> UpsertAsync(Event @event, CancellationToken cancellationToken = default);
}