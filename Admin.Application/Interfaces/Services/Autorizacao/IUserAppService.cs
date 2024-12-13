using Admin.Application.Dto;
using Admin.Application.Dto.Authorization.User;

namespace Admin.Application.Interfaces.Services.Authorization
{
    public interface IUserAppService : ICrudBaseAppService<UserDetailViewModel, UserGridViewModel, UserCreateInputModel, UserChangeInputModel, MaintenanceResultViewModel>
    {
        Task SetUserContext(Microsoft.AspNetCore.Http.HttpContext context, string? userId);
    }
}
