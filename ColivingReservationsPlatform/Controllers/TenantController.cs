using Application.Abstractions.Tenant;
using Application.Contracts.Tenant;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColivingReservationsPlatform.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "administrator,coliving-owner")]
public class TenantController : ControllerBase
{
    private readonly ITenantService _service;

    public TenantController(ITenantService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<TenantResponseDto[]>> Get()
    {
        var tenants = await _service.GetPagedList();
        var result = Ok(tenants);

        return result;
    }

    [HttpGet("{id}")]
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
    public async Task<ActionResult<TenantResponseDto>> Put(Guid id, [FromBody] TenantCreateDto input)
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