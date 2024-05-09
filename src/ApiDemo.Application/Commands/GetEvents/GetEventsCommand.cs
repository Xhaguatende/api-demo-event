// -------------------------------------------------------------------------------------
//  <copyright file="GetEventsCommand.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.GetEvents;

using MediatR;

public record GetEventsCommand : IRequest<List<GetEventsCommandResponse>>;