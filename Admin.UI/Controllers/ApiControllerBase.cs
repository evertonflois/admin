using MediatR;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using Admin.Application.Dto;
using Admin.Application.Interfaces.Dto;


namespace Admin.UI.Controllers;
[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected ActionResult MaintenanceResult(MaintenanceResultViewModel result)
    {
        if (result.Code == 0)
            return Ok(result);
        else
            return BadRequest(result);        
    }

    protected string? GetUserLogin()
    {
        var userIdentity = (UserIdentity?)HttpContext.Items["User"];
        if (userIdentity != null)
            return userIdentity.Login;

        return null;
    }

    protected UserIdentity? GetUserIdentity()
    {
        var userIdentity = (UserIdentity?)HttpContext.Items["User"];
        if (userIdentity != null)
            return userIdentity;

        return null;
    }

    protected void SetCreateDefaults(ICrudInputModel model)
    {
        model.CreationUser = model.ChangeUser = GetUserLogin();
        model.CreationDate = model.ChangeDate = DateTime.Now;
    }

    protected void SetChangeDefaults(ICrudInputModel model)
    {
        model.ChangeUser = GetUserLogin();
        model.ChangeDate = DateTime.Now;
    }

    protected IEnumerable<FilterInputModel> ParseFilter(string filter)
    {
        return JsonConvert.DeserializeObject<IEnumerable<FilterInputModel>>(filter);
    }
}
