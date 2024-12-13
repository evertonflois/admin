using Microsoft.AspNetCore.Mvc;
using Admin.Application.Auth;
using Admin.Application.Dto;
using Admin.Application.Interfaces.Services.Authorization;
using Admin.Application.Dto.Authorization.Profile;

namespace Admin.UI.Controllers.Areas.Authorization;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProfileController : ApiControllerBase
{
    private IProfileAppService _profileService;

    public ProfileController(IProfileAppService profileService)
    {
        _profileService = profileService;
    }   

    [HttpGet("GetCombo")]
    public async Task<ActionResult<IEnumerable<ProfileComboViewModel>>> GetCombo(string subscriberId)
    {
        var result = await _profileService.GetCombo(subscriberId);

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProfileGridViewModel>>> Get(string filter, string sortField, string sortOrder, int pageNumber, int pageSize)
    {
        var result = await _profileService.GetAll(ParseFilter(filter), sortField, sortOrder, pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("GetDetails")]
    public async Task<ActionResult<ProfileDetailViewModel>> GetDetails([FromQuery] ProfileDetailInputModel key)
    {
        var result = await _profileService.GetDetails(key);

        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<MaintenanceResultViewModel>> Create([FromBody] ProfileCreateInputModel model)
    {
        SetCreateDefaults(model);

        var result = await _profileService.Create(model);

        return MaintenanceResult(result);
    }

    [HttpPost("Change")]
    public async Task<ActionResult<MaintenanceResultViewModel>> Change([FromBody] ProfileChangeInputModel model)
    {
        SetChangeDefaults(model);

        var result = await _profileService.Change(model);

        return MaintenanceResult(result);
    }

    [HttpPost("Remove")]
    public async Task<ActionResult<MaintenanceResultViewModel>> Remove([FromBody] ProfileChangeInputModel key)
    {
        var result = await _profileService.Remove(key);

        return MaintenanceResult(result);
    }
}
