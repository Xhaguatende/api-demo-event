// -------------------------------------------------------------------------------------
//  <copyright file="DeleteEventCommand.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.DeleteEvent;

using MediatR;

public record DeleteEventCommand(Guid Id) : IRequest<DeleteEventCommandResponse>;