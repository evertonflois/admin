namespace Admin.Application.Auth;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using Admin.Application.Helpers;
using Admin.Application.Interfaces.Services.Authorization;

public class JwtMiddleware
{    
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserAppService userAppService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        if (userId != null)        
            await userAppService.SetUserContext(context, userId);
                
        await _next(context);
    }
}