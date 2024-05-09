// -------------------------------------------------------------------------------------
//  <copyright file="UpsertEventCommandValidator.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.UpsertEvent;

using FluentValidation;

public class UpsertEventCommandValidator : AbstractValidator<UpsertEventCommand>
{
    public UpsertEventCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.StartDate).NotEmpty();
    }
}