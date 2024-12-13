using AutoMapper;

using Admin.Domain.Interfaces.Repositories;
using Admin.Application.Interfaces.Services;
using Admin.Domain.Interfaces.Context.Connection;
using Admin.Application.Dto;
using Admin.Domain.Entities;

namespace Admin.Application.Services
{
    public abstract class CrudBaseAppService<TEntity, TMaintenanceResult, TDetailViewModel, TGridViewModel, TCreateInputModel, TChangeInputModel, TMaintenanceResultViewModel> : ICrudBaseAppService<TDetailViewModel, TGridViewModel, TCreateInputModel, TChangeInputModel, TMaintenanceResultViewModel>
    {
        private IUnitOfWorkRepository _uoW;
        private readonly IMapper _mapper;
        private IRepositoryBase<TEntity, TMaintenanceResult> _repository;        

        public CrudBaseAppService(IUnitOfWorkRepository uoW, IRepositoryBase<TEntity, TMaintenanceResult> repository, IMapper mapper)
        {
            _uoW = uoW;
            _mapper = mapper;
            _repository = repository;                
        }

        public virtual async Task<IEnumerable<TGridViewModel>> GetAll(IEnumerable<FilterInputModel> filter, string sortField, string sortOrder, int pageNumber, int pageSize)
        {
            using (await _uoW.OpenConnectionAsync())
            {
                var data = await _repository.GetAllFilterAsync(_mapper.Map<IEnumerable<Filter>>(filter), sortField, sortOrder, pageNumber, pageSize);

                var result = _mapper.Map<IEnumerable<TGridViewModel>>(data);

                return result;
            }
        }

        public virtual async Task<TDetailViewModel> GetDetails(object key)
        {
            using (await _uoW.OpenConnectionAsync())
            {
                var getByIdResult = await _repository.GetByIdAsync(key);

                var result = _mapper.Map<TDetailViewModel>(getByIdResult);

                return result;
            }
        }

        public virtual async Task<TMaintenanceResultViewModel> Create(TCreateInputModel model)
        {
            using (await _uoW.BeginTransactionAsync())
            {
                var entity = _mapper.Map<TEntity>(model);

                var createResult = await _repository.CreateAsync(entity);

                await _uoW.SaveChangesAsync();

                var result = _mapper.Map<TMaintenanceResultViewModel>(createResult);

                return result;
            }
        }

        public virtual async Task<TMaintenanceResultViewModel> Change(TChangeInputModel model)
        {
            using (await _uoW.BeginTransactionAsync())
            {
                var entity = _mapper.Map<TEntity>(model);

                var changeResult = await _repository.ChangeAsync(entity);

                await _uoW.SaveChangesAsync();

                var result = _mapper.Map<TMaintenanceResultViewModel>(changeResult);

                return result;
            }
        }        

        public virtual async Task<TMaintenanceResultViewModel> Remove(object key)
        {
            using (await _uoW.BeginTransactionAsync())
            {
                var entity = _mapper.Map<TEntity>(key);

                var changeResult = await _repository.RemoveAsync(entity);

                await _uoW.SaveChangesAsync();

                var result = _mapper.Map<TMaintenanceResultViewModel>(changeResult);

                return result;
            }
        }
    }
}
