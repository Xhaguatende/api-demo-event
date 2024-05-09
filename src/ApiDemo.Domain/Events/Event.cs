// -------------------------------------------------------------------------------------
//  <copyright file="Event.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Events;

using Primitives;

public class Event : Entity<Guid>
{
    public Event(Guid id, string title, string description, DateTime startDate)
        : base(id)
    {
        Title = title;
        Description = description;
        StartDate = startDate;
    }

    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public string Title { get; private set; }

    public void Update(string title, string description, DateTime startDate)
    {
        Title = title;
        Description = description;
        StartDate = startDate;
    }
}