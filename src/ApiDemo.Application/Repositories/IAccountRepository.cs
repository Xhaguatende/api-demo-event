// -------------------------------------------------------------------------------------
//  <copyright file="IAccountRepository.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Repositories;

using Domain.Accounts.Entity;
using Domain.Accounts.ValueObjects;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(AccountId id, CancellationToken cancellationToken = default);
}