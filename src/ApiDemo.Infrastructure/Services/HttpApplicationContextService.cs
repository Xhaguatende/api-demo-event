// -------------------------------------------------------------------------------------
//  <copyright file="HttpApplicationContextService.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Services;

using System.Security.Claims;
using ApiDemo.Application.Services;
using Microsoft.AspNetCore.Http;

public class HttpApplicationContextService : IApplicationContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpApplicationContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserEmail()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null)
        {
            return string.Empty;
        }

        var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

        return claim is null ? string.Empty : claim.Value;
    }
}