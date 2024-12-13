using System.Security.Claims;
using Application.Abstractions.Colivings;
using Application.Abstractions.User;
using Application.Contracts;
using Application.Contracts.Tenant;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ColivingReservationsPlatform.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColivingController : ControllerBase
{
    private readonly IColivingService _service;
    private readonly IUserContextService _userContextService;

    public ColivingController(IColivingService service, IUserContextService userContextService)
    {
        _service = service;
        _userContextService = userContextService;
    }

    [HttpGet]
    public async Task<ActionResult<ColivingResponseDto[]>> Get()
    {
        var user = _userContextService.GetUser();

        var colivings = await _service.GetPagedList();
        var result = Ok(colivings);

        return result;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ColivingResponseDto>> Get(Guid id)
    {
        var coliving = await _service.FindById(id);
        if (coliving == null)
        {
            return NotFound();
        }
        return Ok(coliving);
    }
    
    [HttpGet("owner")]
    [Authorize(Roles = "ColivingOwner")]
    public async Task<ActionResult<ColivingResponseDto[]>> GetOwnerColivings()
    {
        var userId = _userContextService.GetUserId();

        var colivings = await _service.GetPagedOwnerColivings(userId);
        var result = Ok(colivings);

        return result;
    }
    
    [HttpGet("{id}/room/{roomId}/tenants")]
    [Authorize(Roles = "ColivingOwner, Administrator")]
    public async Task<ActionResult<ColivingResponseDto>> GetTenants(Guid id, Guid roomId)
    {
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();
        try
        {
            if (userRole == "ColivingOwner")
            {
                var colivingOwnerId = await _service.GetOwnerIdByColivingId(id);
                if (colivingOwnerId != userId)
                {
                    return StatusCode(400, new { Error = "You do not have permission check this coliving tenants." });
                }
            }
            var tenants = await _service.GetTenants(id, roomId);
            return Ok(tenants);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }

    }

    [HttpPost]
    [Authorize(Roles = "ColivingOwner, Administrator")]
    public async Task<ActionResult<ColivingResponseDto>> Post([FromBody] ColivingCreateDto input)
    {
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();
        try
        {
            if (userRole == "Administrator")
            {
                userId = input.UserId!.Value;
            }
            var result = await _service.Create(input, userId);
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
    [Authorize(Roles = "ColivingOwner, Administrator")]
    public async Task<ActionResult<ColivingResponseDto>> Put(Guid id, [FromBody] ColivingCreateDto input)
    {
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();
        try
        {
            if (userRole == "ColivingOwner")
            {
                var colivingOwnerId = await _service.GetOwnerIdByColivingId(id);

                if (colivingOwnerId != userId)
                {
                    return StatusCode(400, new { Error = "You do not have permission to edit this coliving." });
                }
            }
            
            var coliving = await _service.Edit(id, input);

            var result = Accepted(coliving);
            
            return result;
        }
        catch (Exception e)
        {
            return NoContent();
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ColivingOwner, Administrator")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var userId = _userContextService.GetUserId();
        var userRole = _userContextService.GetUserRole();
        try
        {
            if (userRole == "ColivingOwner")
            {
                var colivingOwnerId = await _service.GetOwnerIdByColivingId(id);

                if (colivingOwnerId != userId)
                {
                    return StatusCode(401, new { Error = "You do not have permission to delete this coliving." });
                }
            }
            await _service.Remove(id);

            var result = NoContent();

            return result;
        }
        catch (Exception e)
        {
            return NotFound("Coliving with provided id not found to delete.");
        }
    }
}
