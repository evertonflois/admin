using Admin.Application.Dto;

namespace Admin.Application.Interfaces.Services;

public interface IAuthAppService
{
    Task<AuthViewModel> Authenticate(AuthInputModel model, string ipAddress);
    Task<AuthViewModel> RefreshToken(string token, string ipAddress);
    Task RevokeToken(string token, string ipAddress);
}
