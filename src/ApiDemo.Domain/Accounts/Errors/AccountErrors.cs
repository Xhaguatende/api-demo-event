// -------------------------------------------------------------------------------------
//  <copyright file="AccountErrors.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Accounts.Errors;

using Shared;

public static class AccountErrors
{
    public static Error InvalidAccount(string property = "")
    {
        const string message = "The account is invalid";

        const string code = $"{nameof(AccountErrors)}.{nameof(InvalidAccount)}";

        return new Error(code, message, property);
    }
}