using Microsoft.AspNetCore.Mvc;
using Admin.Application.Auth;
using Admin.Application.Dto;
using Admin.Application.Interfaces.Services.Authorization;
using Admin.Application.Dto.Authorization.Transaction;

namespace Admin.UI.Controllers.Areas.Authorization;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TransactionController : ApiControllerBase
{
    private ITransactionAppService _transactionService;

    public TransactionController(ITransactionAppService transactionService)
    {
        _transactionService = transactionService;
    }   
        
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionGridViewModel>>> Get(string filter, string sortField, string sortOrder, int pageNumber, int pageSize)
    {
        var result = await _transactionService.GetAll(ParseFilter(filter), sortField, sortOrder, pageNumber, pageSize);

        return Ok(result);
    }

    [HttpPost("Change")]
    public async Task<ActionResult<MaintenanceResultViewModel>> Change([FromBody] TransactionChangeInputModel[] model)
    {        
        var result = await _transactionService.Change(model, GetUserLogin());

        return MaintenanceResult(result);
    }
}
