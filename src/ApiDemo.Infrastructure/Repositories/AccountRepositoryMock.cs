// -------------------------------------------------------------------------------------
//  <copyright file="AccountRepositoryMock.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Repositories;

using Application.Repositories;
using Domain.Accounts.Entity;
using Domain.Accounts.ValueObjects;

public class AccountRepositoryMock : IAccountRepository
{
    private static readonly List<Account> Accounts = [
        new(new AccountId("usera@mail.com")),
        new(new AccountId("userb@mail.com")),
    ];

    public Task<Account?> GetByIdAsync(AccountId id, CancellationToken cancellationToken = default)
    {
        var account =
            Accounts.FirstOrDefault(x => string.Equals(x.Id.Email, id.Email, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(account);
    }
}