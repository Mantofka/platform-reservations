using Application.Abstractions.Room;
using Application.Contracts.Room;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ColivingReservationsPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _service;
        public RoomController(IRoomService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<RoomResponseDto[]>> Get()
        {
            var rooms = await _service.GetPagedList();
            var result = Ok(rooms);

            return result;
        }
    
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomResponseDto>> Get(Guid id)
        {
            var room = await _service.FindById(id);
            if (room == null)
            {
                return NotFound();
            }
            
            return Ok(room);
        }

        [HttpPost]
        public async Task<ActionResult<RoomResponseDto>> Post([FromBody] RoomCreateDto input)
        {
            try
            {
                var room = await _service.Create(input);

                var result = Created("created", room);

                return result;
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
        
        [HttpPost("assignTenant")]
        public async Task<ActionResult<RoomResponseDto>> Post([FromBody] AssignTenantDto input)
        {
            try
            {
                var room = await _service.AssignTenant(input);

                var result = Accepted(room);

                return result;
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Errors = ex.Errors });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoomResponseDto>> Put(Guid id, [FromBody] RoomCreateDto input)
        {
            var room = await _service.Edit(id, input);

            var result = Accepted(room);

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
                return NotFound("Room with provided id not found to delete.");
            }

        }
    }
}
