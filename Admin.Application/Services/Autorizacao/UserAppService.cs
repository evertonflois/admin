using AutoMapper;

using Admin.Application.Helpers;
using Admin.Application.Dto;
using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;
using Admin.Domain.Interfaces.Repositories.Authorization;
using Admin.Application.Interfaces.Services.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using Microsoft.Extensions.Options;
using Admin.Application.Dto.Authorization.User;

namespace Admin.Application.Services.Authorization
{
    public class UserAppService : CrudBaseAppService<User, ResponseBase, UserDetailViewModel, UserGridViewModel, UserCreateInputModel, UserChangeInputModel, MaintenanceResultViewModel>, IUserAppService
    {
        private IUnitOfWorkRepository _uoW;
        private IMapper _mapper;
        private IUserRepository _repository;
        private readonly AppSettings _appSettings;
        private static Random random = new Random();

        public UserAppService(IUnitOfWorkRepository uoW, IUserRepository repository, IMapper mapper, IOptions<AppSettings> appSettings) : base(uoW, repository, mapper)
        {
            _uoW = uoW;
            _mapper = mapper;
            _repository = repository;
            _appSettings = appSettings.Value;
        }

        public async Task SetUserContext(Microsoft.AspNetCore.Http.HttpContext context, string? userId)
        {
            using (await _uoW.OpenConnectionAsync())
            {
                // attach user to context on successful jwt validation
                if (userId.Equals(_appSettings.Admin.Login))
                    context.Items["User"] = new UserIdentity() { SubscriberId = Guid.NewGuid().ToString(), ProfileCode = "ADM", Login = userId, Name = "Admin" };
                else
                    context.Items["User"] = _mapper.Map<UserIdentity>(await base.GetDetails(new { cd_user = userId }));
            }
        }

        public override async Task<MaintenanceResultViewModel> Create(UserCreateInputModel model)
        {
            using (await _uoW.OpenConnectionAsync())
            {
                var userExists = (await _repository.GetAllAsync(new { model.Login })).Any();
                if (userExists)
                    throw new AppException(model.Login + "' user is already being used.");

                if (model.Password == null)
                    model.Password = RandomString(6);

                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

                var result = _mapper.Map<MaintenanceResultViewModel>(await base.Create(model));

                return result;
            }
        }
        
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
