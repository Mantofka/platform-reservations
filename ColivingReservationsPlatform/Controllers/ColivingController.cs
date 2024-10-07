using Application.Abstractions.Colivings;
using Application.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
namespace ColivingReservationsPlatform.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColivingController : ControllerBase
{
    private readonly IColivingService _service;

    public ColivingController(IColivingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ColivingResponseDto[]>> Get()
    {
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

    [HttpPost]
    public async Task<ActionResult<ColivingResponseDto>> Post([FromBody] ColivingCreateDto input)
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
    public async Task<ActionResult<ColivingResponseDto>> Put(Guid id, [FromBody] ColivingCreateDto input)
    {
        var coliving = await _service.Edit(id, input);

        var result = Accepted(coliving);

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
            return NotFound("Coliving with provided id not found to delete.");
        }
    }
}
