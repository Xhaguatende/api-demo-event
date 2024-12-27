// -------------------------------------------------------------------------------------
//  <copyright file="Account.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Accounts.Entity;

using Primitives;
using ValueObjects;

public class Account : Entity<AccountId>
{
    public Account(AccountId id)
    : base(id)
    {
    }
}