using AutoMapper;
using Admin.Domain.Interfaces.Repositories.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;
using Admin.Application.Dto;
using Admin.Application.Interfaces.Services.Authorization;
using Admin.Application.Dto.Authorization.Transaction;
using Admin.Application.Dto.Authorization.TransactionActions;


namespace Admin.Application.Services.Authorization
{
    public class TransactionAppService : CrudBaseAppService<Transaction, ResponseBase, object, TransactionGridViewModel, object, TransactionChangeInputModel, MaintenanceResultViewModel>, ITransactionAppService
    {
        private IUnitOfWorkRepository _uoW;
        private IMapper _mapper;        
        private ITransactionActionsRepository _repositoryTransactionActions;
        private IProfileTransactionsRepository _repositoryProfileTransactions;
        private IProfileActionsRepository _repositoryProfileActions;

        public TransactionAppService(IUnitOfWorkRepository uoW, ITransactionRepository repository, ITransactionActionsRepository repositoryTransactionActions, IProfileTransactionsRepository repositoryProfileTransactions, IProfileActionsRepository repositoryProfileActions, IMapper mapper) : base(uoW, repository, mapper)
        {
            _uoW = uoW;
            _mapper = mapper;            
            _repositoryTransactionActions = repositoryTransactionActions;
            _repositoryProfileTransactions = repositoryProfileTransactions;
            _repositoryProfileActions = repositoryProfileActions;
        }
        public override async Task<IEnumerable<TransactionGridViewModel>> GetAll(IEnumerable<FilterInputModel> filter, string sortField, string sortOrder, int pageNumber, int pageSize)
        {
            var result = await base.GetAll(filter, sortField, sortOrder, pageNumber, pageSize);
                      
            if (result != null)
            {
                using (await _uoW.OpenConnectionAsync())
                {
                    foreach (var item in result)
                    {

                        var filterSearch = new List<Filter>();
                        filterSearch.Add(new Filter("SubscriberId", item.SubscriberId));
                        filterSearch.Add(new Filter("TransactionCode", item.TransactionCode));
                        filterSearch.Add(new Filter("ProfileCode", item.ProfileCode));
                        item.Actions = _mapper.Map<IEnumerable<TransactionActionGridViewModel>>(await _repositoryTransactionActions.GetAllFilterAsync(filterSearch, "ActionCode", "asc", 1, 9999));
                    }                    
                }
            }

            return result;
        }

        public async Task<MaintenanceResultViewModel> Change(TransactionChangeInputModel[] model, string login)
        {
            model = VerifyFlagPermission(model);

            MaintenanceResultViewModel? result = null;
            var responseBase = new ResponseBase(0, "Permissions have been changed successfully.");
            using (await _uoW.BeginTransactionAsync())
            {
                foreach (var item in model)
                {
                    var entityPerfTrsc = _mapper.Map<ProfileTransactions>(item);
                    var entityPerfAcao = _mapper.Map<ProfileActions>(item);

                    var incluirAcoes = true;

                    // There was permitted, but the permission has been removed.
                    if (item.FlagOriginalPermission == true && item.FlagPermission == false)
                    {
                        responseBase = await _repositoryProfileTransactions.RemoveAsync(entityPerfTrsc);
                        if (responseBase.return_code != 0) goto sair;

                        responseBase = await _repositoryProfileActions.RemoveByTransactionAsync(entityPerfAcao);
                        if (responseBase.return_code != 0) goto sair;

                        incluirAcoes = false;
                    }
                    else if (item.FlagPermission == true && item.FlagOriginalPermission == false) // New permission
                    {                        
                        entityPerfTrsc.CreationUser =  entityPerfTrsc.ChangeUser = login;
                        entityPerfTrsc.CreationDate = entityPerfTrsc.ChangeDate = DateTime.Now;

                        responseBase = await _repositoryProfileTransactions.CreateAsync(entityPerfTrsc);
                        if (responseBase.return_code != 0) goto sair;
                    }
                    else if (item.FlagPermission == true && item.FlagOriginalPermission == true) // Already permitted
                    {
                        responseBase = await _repositoryProfileActions.RemoveByTransactionAsync(entityPerfAcao);
                        if (responseBase.return_code != 0) goto sair;
                    }
                    else
                        continue;


                    if (incluirAcoes == true && item.Actions?.Count() > 0)
                    {
                        foreach (var acao in item.Actions)
                        {
                            if (acao.FlagPermission == true)
                            {
                                entityPerfAcao = _mapper.Map<ProfileActions>(acao);
                                entityPerfAcao.SubscriberId = item.SubscriberId;
                                entityPerfAcao.ProfileCode = item.ProfileCode;
                                entityPerfAcao.TransactionCode = item.TransactionCode;
                                entityPerfAcao.CreationUser = entityPerfAcao.ChangeUser = login;
                                entityPerfAcao.CreationDate = entityPerfAcao.ChangeDate = DateTime.Now;

                                responseBase = await _repositoryProfileActions.CreateAsync(entityPerfAcao);
                                if (responseBase.return_code != 0) goto sair;
                            }
                        }
                    }                    
                }
                
                await _uoW.SaveChangesAsync();

            sair:
                if (responseBase.return_code == 0)
                    responseBase.return_chav = "Permissions have been changed successfully.";
                result = _mapper.Map<MaintenanceResultViewModel>(responseBase);

                return result;
            }
        }

        private TransactionChangeInputModel[] VerifyFlagPermission(TransactionChangeInputModel[] model)
        {
            foreach (var item in model.Where(m => m.FlagPermission == false && m.FlagOriginalPermission == false))
            {
                if (item.Actions != null && item.Actions.Any(a => a.FlagPermission == true))
                {
                    item.FlagPermission = true;
                    continue;
                }
            }

            return model;
        }
    }
}
