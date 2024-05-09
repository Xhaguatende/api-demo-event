// -------------------------------------------------------------------------------------
//  <copyright file="DeleteEventCommandResponse.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.DeleteEvent;

public record DeleteEventCommandResponse(Guid Id, bool Success);