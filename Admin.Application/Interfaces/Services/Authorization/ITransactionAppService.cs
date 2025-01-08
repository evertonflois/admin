using Admin.Application.Dto;
using Admin.Application.Dto.Authorization.Transaction;

namespace Admin.Application.Interfaces.Services.Authorization
{
    public interface ITransactionAppService : ICrudBaseAppService<object, TransactionGridViewModel, object, TransactionChangeInputModel, MaintenanceResultViewModel>
    {
        Task<IEnumerable<TransactionGridViewModel>> GetAll(IEnumerable<FilterInputModel> filter, string sortField, string sortOrder, int pageNumber, int pageSize);
        Task<MaintenanceResultViewModel> Change(TransactionChangeInputModel[] model, string cd_user);
    }
}
