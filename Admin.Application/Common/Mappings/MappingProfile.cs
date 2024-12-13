using System.Reflection;

using Admin.Application.Dto;
using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;
using Admin.Application.Dto.Authorization.User;
using Admin.Application.Dto.Authorization.Subscriber;
using Admin.Application.Dto.Authorization.Transaction;
using Admin.Application.Dto.Authorization.TransactionActions;
using Admin.Application.Dto.Authorization.Profile;
using Profile = Admin.Domain.Entities.Authorization.Profile;
using Admin.Application.Dto.Authorization.UserPreferences;

namespace Admin.Application.Common.Mappings;
public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

        AddGlobalMappings();

        AddAutorizacaoMappings();
    }

    private void AddGlobalMappings()
    {        
        CreateMap<ResponseBase, MaintenanceResultViewModel>()
            .ForMember(d => d.Code, f => f.MapFrom(s => s.return_code))
            .ForMember(d => d.Description, f => f.MapFrom(s => s.return_chav));

        CreateMap<UserDetailViewModel, UserIdentity>();

        CreateMap<Transaction, TransactionViewModel>()
            .ForMember(d => d.Code, f => f.MapFrom(s => s.TransactionCode))
            .ForMember(d => d.Description, f => f.MapFrom(s => s.Description));

        CreateMap<Menu, MenuViewModel>()
            .ForMember(d => d.Label, f => f.MapFrom(s => s.Name))
            .ForMember(d => d.Icon, f => f.MapFrom(s => !string.IsNullOrWhiteSpace(s.Icon) ? s.Icon : s.Type == "M" ? "pi pi-fw pi-angle-right" : "pi pi-fw pi-circle"))
            .ForMember(d => d.RouterLink, f => f.Condition(s => !string.IsNullOrWhiteSpace(s.Path)))
            .ForMember(d => d.RouterLink, f => f.MapFrom(s => new string[] { s.Path }))            
            .ForMember(d => d.CdAgrpMenu, f => f.MapFrom(s => s.GroupingCode));

        CreateMap<FilterInputModel, Filter>();
    }

    private void AddAutorizacaoMappings()
    {
        #region Subscriber Mappings
        CreateMap<Subscriber, SubscriberComboViewModel>();
        #endregion Subscriber Mappings

        #region Transaction Mappings
        CreateMap<Transaction, TransactionGridViewModel>()
            .ForMember(d => d.FlagPermission, f => f.MapFrom(s => s.FlagPermission == "Y"))
            .ForMember(d => d.FlagOriginalPermission, f => f.MapFrom(s => s.FlagOriginalPermission == "Y"));
        CreateMap<TransactionChangeInputModel, Transaction>()
            .ForMember(d => d.FlagPermission, f => f.MapFrom(s => s.FlagPermission == true ? "Y" : "N"));
        CreateMap<TransactionActions, TransactionActionGridViewModel>()
            .ForMember(d => d.FlagPermission, f => f.MapFrom(s => s.FlagPermission == "Y"))
            .ForMember(d => d.FlagOriginalPermission, f => f.MapFrom(s => s.FlagOriginalPermission == "Y"));
        CreateMap<TransactionActionChangeInputModel, TransactionActions>()
            .ForMember(d => d.FlagPermission, f => f.MapFrom(s => s.FlagPermission == true ? "Y" : "N"));

        CreateMap<TransactionChangeInputModel, ProfileTransactions>();
        CreateMap<TransactionChangeInputModel, ProfileActions>();
        CreateMap<TransactionActionChangeInputModel, ProfileActions>();
        #endregion Transaction Mappings

        #region Profile Mappings
        CreateMap<Profile, ProfileComboViewModel>();

        CreateMap<ProfileCreateInputModel, Profile>();
        CreateMap<ProfileChangeInputModel, Profile>();

        CreateMap<Profile, ProfileDetailViewModel>();
        CreateMap<Profile, ProfileGridViewModel>();
        #endregion Profile Mappings

        #region User Mappings
        CreateMap<UserCreateInputModel, User>();
        CreateMap<UserChangeInputModel, User>();

        CreateMap<User, UserDetailViewModel>();
        CreateMap<User, UserGridViewModel>();

        CreateMap<UserPreferenceCreateInputModel, UserPreferences>();
        CreateMap<UserPreferenceChangeInputModel, UserPreferences>();

        CreateMap<UserPreferences, UserPreferenceDetailViewModel>();
        #endregion User Mappings
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var mapFromType = typeof(IMapFrom<>);

        var mappingMethodName = nameof(IMapFrom<object>.Mapping);

        bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;

        var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();

        var argumentTypes = new Type[] { typeof(AutoMapper.Profile) };

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod(mappingMethodName);

            if (methodInfo != null)
            {
                methodInfo.Invoke(instance, new object[] { this });
            }
            else
            {
                var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

                if (interfaces.Count > 0)
                {
                    foreach (var @interface in interfaces)
                    {
                        var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

                        interfaceMethodInfo?.Invoke(instance, new object[] { this });
                    }
                }
            }
        }
    }
}
