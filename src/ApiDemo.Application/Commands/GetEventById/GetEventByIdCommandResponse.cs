// -------------------------------------------------------------------------------------
//  <copyright file="GetEventByIdCommandResponse.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.GetEventById;

public class GetEventByIdCommandResponse
{
    public string Description { get; set; } = default!;
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public string Title { get; set; } = default!;
}