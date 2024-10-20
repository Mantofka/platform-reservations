using Application.Abstractions.Colivings;
using Application.Abstractions.Tenant;
using Application.Contracts;
using Application.Contracts.Maintenance;
using Application.Contracts.Tenant;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ColivingReservationsPlatform.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MaintenanceController : ControllerBase
{
    private readonly IMaintenanceService _service;

    public MaintenanceController(IMaintenanceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<MaintenanceResponseDto[]>> Get()
    {
        var maintenances = await _service.GetPagedList();
        var result = Ok(maintenances);

        return result;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<MaintenanceResponseDto>> Get(Guid id)
    {
        var maintenance = await _service.FindById(id);
        if (maintenance == null)
        {
            return NotFound();
        }
        return Ok(maintenance);
    }
    
    [HttpGet("{colivingId}/room/{roomId}/tenant/{tenantId}")]
    public async Task<ActionResult<MaintenanceResponseDto[]>> GetMaintenances(Guid colivingId, Guid roomId, Guid tenantId)
    {
        var maintenance = await _service.GetMaintenances(colivingId, roomId, tenantId);
        if (maintenance == null)
        {
            return NotFound();
        }
        return Ok(maintenance);
    }

    [HttpPost]
    public async Task<ActionResult<MaintenanceResponseDto>> Post([FromBody] MaintenanceCreateDto input)
    {
        try
        {
            var result = await _service.Create(input);
            return Created("created", result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { Errors = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MaintenanceResponseDto>> Put(Guid id, [FromBody] MaintenanceCreateDto input)
    {
        var maintenance = await _service.Edit(id, input);

        var result = Accepted(maintenance);

        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _service.Remove(id);

            var result = NoContent();

            return result;
        }
        catch (Exception e)
        {
            return NotFound("Maintenance with provided id not found to delete.");
        }
    }
}
