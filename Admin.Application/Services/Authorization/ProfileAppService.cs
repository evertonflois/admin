using AutoMapper;
using Admin.Domain.Interfaces.Repositories.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using Admin.Domain.Entities;
using Admin.Application.Dto;
using Admin.Application.Interfaces.Services.Authorization;
using Admin.Application.Dto.Authorization.Profile;

namespace Admin.Application.Services.Authorization
{
    public class ProfileAppService : CrudBaseAppService<Domain.Entities.Authorization.Profile, ResponseBase, ProfileDetailViewModel, ProfileGridViewModel, ProfileCreateInputModel, ProfileChangeInputModel, MaintenanceResultViewModel>, IProfileAppService
    {
        private IUnitOfWorkRepository _uoW;
        private IMapper _mapper;
        private IProfileRepository _repository;
        
        public ProfileAppService(IUnitOfWorkRepository uoW, IProfileRepository repository, IMapper mapper) : base(uoW, repository, mapper)
        {
            _uoW = uoW;
            _mapper = mapper;
            _repository = repository;            
        }        

        public async Task<IEnumerable<ProfileComboViewModel>> GetCombo(string subscriberId)
        {
            using (await _uoW.OpenConnectionAsync())
            {
                var data = await _repository.GetComboAsync(subscriberId);

                var result = _mapper.Map<IEnumerable<ProfileComboViewModel>>(data);

                return result;
            }
        }
    }
}
