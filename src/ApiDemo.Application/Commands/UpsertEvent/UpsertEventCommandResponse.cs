// -------------------------------------------------------------------------------------
//  <copyright file="UpsertEventCommandResponse.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.UpsertEvent;

public class UpsertEventCommandResponse
{
    public string Description { get; set; } = default!;
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public string Title { get; set; } = default!;
}