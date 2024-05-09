// -------------------------------------------------------------------------------------
//  <copyright file="UpsertEventCommand.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.UpsertEvent;

using MediatR;

public record UpsertEventCommand : IRequest<UpsertEventCommandResponse>
{
    public Guid? Id { get; set; }
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public string Title { get; set; } = default!;
}