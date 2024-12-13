using Microsoft.AspNetCore.Mvc;

using Admin.Application.Auth;
using Admin.Application.Dto;
using Admin.Application.Interfaces.Services.Authorization;
using Admin.Application.Dto.Authorization.User;

namespace Admin.UI.Controllers.Areas.Authorization;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ApiControllerBase
{
    private IUserAppService _userService;

    public UserController(IUserAppService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserGridViewModel>>> Get(string filter, string sortField, string sortOrder, int pageNumber, int pageSize)
    {
        var result = await _userService.GetAll(ParseFilter(filter), sortField, sortOrder, pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("GetDetails")]
    public async Task<ActionResult<UserDetailViewModel>> GetDetails([FromQuery]UserDetailInputModel key)
    {
        var result = await _userService.GetDetails(key);

        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<MaintenanceResultViewModel>> Create([FromBody]UserCreateInputModel model)
    {
        SetCreateDefaults(model);

        var result = await _userService.Create(model);

        return MaintenanceResult(result);
    }

    [HttpPost("Change")]
    public async Task<ActionResult<MaintenanceResultViewModel>> Change([FromBody]UserChangeInputModel model)
    {
        SetChangeDefaults(model);

        var result = await _userService.Change(model);

        return MaintenanceResult(result);
    }

    [HttpPost("Remove")]
    public async Task<ActionResult<MaintenanceResultViewModel>> Remove([FromBody]UserChangeInputModel key)
    {
        var result = await _userService.Remove(key);

        return MaintenanceResult(result);
    }

}
