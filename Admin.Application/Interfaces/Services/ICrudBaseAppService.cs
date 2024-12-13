using Admin.Application.Dto;

namespace Admin.Application.Interfaces.Services
{
    public interface ICrudBaseAppService<TDetailViewModel, TGridViewModel, TCreateInputModel, TChangeInputModel, TMaintenanceResultViewModel>
    {
        Task<IEnumerable<TGridViewModel>> GetAll(IEnumerable<FilterInputModel> filter, string sortField, string sortOrder, int pageNumber, int pageSize);
        Task<TDetailViewModel> GetDetails(object key);
        Task<TMaintenanceResultViewModel> Create(TCreateInputModel model);
        Task<TMaintenanceResultViewModel> Change(TChangeInputModel model);
        Task<TMaintenanceResultViewModel> Remove(object key);
    }
}
