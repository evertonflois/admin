using Microsoft.AspNetCore.Mvc;
using Admin.Application.Auth;
using Admin.Application.Interfaces.Services.Authorization;
using Admin.Application.Dto.Authorization.Subscriber;

namespace Admin.UI.Controllers.Areas.Authorization;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SubscriberController : ApiControllerBase
{
    private ISubscriberAppService _subscriberService;

    public SubscriberController(ISubscriberAppService subscriberService)
    {
        _subscriberService = subscriberService;
    }   

    [HttpGet("GetCombo")]
    public async Task<ActionResult<IEnumerable<SubscriberComboViewModel>>> GetCombo()
    {
        var result = await _subscriberService.GetCombo();

        return Ok(result);
    }
}
