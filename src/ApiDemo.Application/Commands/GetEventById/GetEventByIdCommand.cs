// -------------------------------------------------------------------------------------
//  <copyright file="GetEventByIdCommand.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.GetEventById;

using MediatR;

public record GetEventByIdCommand(Guid Id) : IRequest<GetEventByIdCommandResponse>;