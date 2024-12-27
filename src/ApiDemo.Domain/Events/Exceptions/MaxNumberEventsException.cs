// -------------------------------------------------------------------------------------
//  <copyright file="MaxNumberEventsException.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Events.Exceptions;

using Domain.Exceptions;

public class MaxNumberEventsException : DomainException
{
    private const string DefaultMessage = "Maximum number of events reached.";

    public MaxNumberEventsException() : base(DefaultMessage)
    {
    }

    public MaxNumberEventsException(Exception innerException) : base(DefaultMessage, innerException)
    {
    }
}