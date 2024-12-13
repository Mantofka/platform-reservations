using Application.Abstractions.Tenant;
using Application.Abstractions.User;
using Application.Contracts.Tenant;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColivingReservationsPlatform.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TenantController : ControllerBase
{
    private readonly ITenantService _service;
    private readonly IUserContextService _userContextService;

    public TenantController(ITenantService service, IUserContextService userContextService)
    {
        _service = service;
        _userContextService = userContextService;
    }

    [HttpGet]
    [Authorize(Roles = "ColivingOwner, Administrator")]
    public async Task<ActionResult<TenantResponseDto[]>> Get()
    {
        var tenants = await _service.GetPagedList();
        var result = Ok(tenants);

        return result;
    }
    
    [HttpGet("current")]
    [Authorize(Roles = "Tenant")]
    public async Task<ActionResult<TenantResponseDto>> GetTenant()
    {
        var userId = _userContextService.GetUserId();
        var tenant = await _service.GetCurrentTenant(userId);
        var result = Ok(tenant);

        return result;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "ColivingOwner, Administrator")]
    public async Task<ActionResult<TenantResponseDto>> Get(Guid id)
    {
        var tenant = await _service.FindById(id);
        
        if (tenant == null)
        {
            return NotFound();
        }

        return Ok(tenant);
    }

    [HttpPost]
    [Authorize(Roles = "ColivingOwner, Administrator")]
    public async Task<ActionResult<TenantResponseDto>> Post([FromBody] TenantCreateDto input)
    {
        try
        {
            var tenant = await _service.Create(input);
            return Created("created", tenant);
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
    [Authorize(Roles = "ColivingOwner, Administrator, Tenant")]
    public async Task<ActionResult<TenantResponseDto>> Put(Guid id, [FromBody] TenantUpdateDto input)
    {
        var tenant = await _service.Edit(id, input);

        var result = Accepted(tenant);

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
            return NotFound("Tenant with provided id not found to delete.");
        }
    }
}