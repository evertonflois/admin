using Admin.Application.Dto.Authorization.Subscriber;

namespace Admin.Application.Interfaces.Services.Authorization
{
    public interface ISubscriberAppService 
    {
        Task<IEnumerable<SubscriberComboViewModel>> GetCombo();
    }
}
