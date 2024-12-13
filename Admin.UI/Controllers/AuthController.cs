
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Admin.Application.Dto;
using Admin.Application.Interfaces.Services;

namespace Admin.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        protected IAuthAppService _authService;

        public AuthController(IAuthAppService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<ActionResult<AuthViewModel>> Authenticate(AuthInputModel model)
        {
            var result = await _authService.Authenticate(model, ipAddress());
            if (result != null && result.RefreshToken != null)
                setTokenCookie(result.RefreshToken);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<AuthViewModel>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.RefreshToken(refreshToken, ipAddress());
            if (result != null && result.RefreshToken != null)
                setTokenCookie(result.RefreshToken);
            return Ok(result);
        }
                
        [HttpPost("RevokeToken")]
        public async Task<ActionResult<AuthViewModel>> RevokeToken(RevokeTokenInputModel? model)
        {            
            var token = model?.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });
                        
            await _authService.RevokeToken(token, ipAddress());
            
            return Ok(new { message = "Token revoked" });
        }

        #region private methods
        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        #endregion private methods
    }
}
