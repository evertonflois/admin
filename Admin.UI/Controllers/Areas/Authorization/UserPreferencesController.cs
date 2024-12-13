using Microsoft.AspNetCore.Mvc;

using Admin.Application.Auth;
using Admin.Application.Dto;
using Admin.Application.Interfaces.Services.Authorization;
using Admin.Application.Dto.Authorization.UserPreferences;

namespace Admin.UI.Controllers.Areas.Authorization;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserPreferencesController : ApiControllerBase
{
    private IUserPreferencesAppService _userPreferencesService;

    public UserPreferencesController(IUserPreferencesAppService userPreferencesService)
    {
        _userPreferencesService = userPreferencesService;
    }    
    
    [HttpGet("GetDetails")]
    public async Task<ActionResult<UserPreferenceDetailViewModel>> GetDetails([FromQuery]UserPreferenceDetailInputModel key)
    {
        var result = await _userPreferencesService.GetDetails(key);

        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<MaintenanceResultViewModel>> Create([FromBody]UserPreferenceCreateInputModel model)
    {
        SetCreateDefaults(model);

        var result = await _userPreferencesService.Create(model);

        return MaintenanceResult(result);
    }

    [HttpPost("Change")]
    public async Task<ActionResult<MaintenanceResultViewModel>> Change([FromBody]UserPreferenceChangeInputModel model)
    {
        SetChangeDefaults(model);

        var result = await _userPreferencesService.Change(model);

        return MaintenanceResult(result);
    }
}
