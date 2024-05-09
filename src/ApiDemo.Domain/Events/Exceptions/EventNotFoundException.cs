// -------------------------------------------------------------------------------------
//  <copyright file="EventNotFoundException.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Events.Exceptions;

using Domain.Exceptions;

public class EventNotFoundException : NotFoundException
{
    public EventNotFoundException(Guid id) : base($"Event with id: '{id}' not found.")
    {
    }

    public EventNotFoundException(Guid id, Exception innerException) :
        base($"Event with id: '{id}' not found.", innerException)
    {
    }
}