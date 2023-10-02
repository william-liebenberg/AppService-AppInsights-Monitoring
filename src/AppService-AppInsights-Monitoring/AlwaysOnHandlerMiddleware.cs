﻿using System.Net;
using Microsoft.Net.Http.Headers;

namespace AppService.AppInsights.Monitoring;

public class AlwaysOnHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public AlwaysOnHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Headers[HeaderNames.UserAgent].Contains("AlwaysOn"))
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
            return;
        }

        await _next(httpContext);
    }
}

public static class AlwaysOnHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseAlwaysOnHandlerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AlwaysOnHandlerMiddleware>();
    }
}