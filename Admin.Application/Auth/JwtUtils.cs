namespace Admin.Application.Auth;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Admin.Application.Dto;
using Admin.Application.Helpers;
using Admin.Domain.Entities.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using Admin.Domain.Interfaces.Repositories.Authorization;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


public interface IJwtUtils
{
    public string GenerateJwtToken(AuthInputModel user);
    public string? ValidateJwtToken(string token);
    public Task<RefreshToken> GenerateRefreshToken(string ipAddress);
}

public class JwtUtils : IJwtUtils
{
    private IUnitOfWorkRepository _uoW;
    private IRefreshTokenRepository _context;
    private readonly AppSettings _appSettings;

    public JwtUtils(
        IUnitOfWorkRepository uoW,
        IRefreshTokenRepository context,
        IOptions<AppSettings> appSettings)
    {
        _uoW = uoW;
        _context = context;
        _appSettings = appSettings.Value;
    }

    public string GenerateJwtToken(AuthInputModel user)
    {
        // generate token that is valid for 15 minutes
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("cd_user", user.Login) }),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string? ValidateJwtToken(string token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var login = jwtToken.Claims.First(x => x.Type == "cd_user").Value;

            // return user id from JWT token if validation successful
            return login;
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }

    public async Task<RefreshToken> GenerateRefreshToken(string ipAddress)
    {
        var refreshToken = new RefreshToken
        {
            Token = await getUniqueToken(),
            // token is valid for 7 days
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

        return refreshToken;

        async Task<string> getUniqueToken()
        {            
            // token is a cryptographically strong random sequence of values
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            // ensure token is unique by checking against db
            var tokenIsUnique = !(await _context.GetAllAsync(new { Token = token })).Any();

            if (!tokenIsUnique)
                return await getUniqueToken();

            return token;            
        }
    }
}