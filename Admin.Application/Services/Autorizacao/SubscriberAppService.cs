using AutoMapper;
using Admin.Domain.Interfaces.Repositories.Authorization;
using Admin.Application.Interfaces.Services.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using Admin.Application.Dto.Authorization.Subscriber;


namespace Admin.Application.Services.Authorization
{
    public class SubscriberAppService : ISubscriberAppService
    {
        private IUnitOfWorkRepository _uoW;
        private IMapper _mapper;
        private ISubscriberRepository _repository;
        
        public SubscriberAppService(IUnitOfWorkRepository uoW, ISubscriberRepository repository, IMapper mapper)
        {
            _uoW = uoW;
            _mapper = mapper;
            _repository = repository;            
        }

        public async Task<IEnumerable<SubscriberComboViewModel>> GetCombo()
        {
            using (await _uoW.OpenConnectionAsync())
            {
                var data = await _repository.GetComboAsync();

                var result = _mapper.Map<IEnumerable<SubscriberComboViewModel>>(data);

                return result;
            }
        }
    }
}
