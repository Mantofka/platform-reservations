using Application.Abstractions.Room;
using Application.Abstractions.User;
using Application.Contracts.Room;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColivingReservationsPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _service;
        private readonly IUserContextService _userContextService;
        public RoomController(IRoomService service, IUserContextService userContextService)
        {
            _service = service;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<ActionResult<RoomResponseDto[]>> Get()
        {
            var rooms = await _service.GetPagedList();
            var result = Ok(rooms);

            return result;
        }
        
        [HttpGet("coliving/{colivingId}")]
        public async Task<ActionResult<RoomResponseDto[]>> GetColivingRooms(Guid colivingId)
        {
            var rooms = await _service.GetPagedColivingRoomsList(colivingId);
            
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
        [Authorize(Roles = "ColivingOwner, Administrator")]
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
        [Authorize(Roles = "ColivingOwner, Tenant, Administrator")]
        public async Task<ActionResult<RoomResponseDto>> Post([FromBody] AssignTenantDto input)
        {
            try
            {
                var userId = _userContextService.GetUserId();
                var userRole = _userContextService.GetUserRole();

                if (userRole == "ColivingOwner")
                {
                    var colivingOwnerId = await _service.GetOwnerIdByRoomId(input.Roomid);

                    if (colivingOwnerId == null)
                    {
                        return StatusCode(400, new { Error = "Coliving does not exist." });
                    }

                    if (colivingOwnerId != userId)
                    {
                        return StatusCode(400,
                            new
                            {
                                Error = "You do not have permission to assign tenants to room that not belongs to you."
                            });
                    }
                }

                await _service.AssignTenant(input, userId);

                return NoContent();

            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Errors = ex.Errors });
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, new { Error = "The tenant is already assigned to this room." });
            }
            
            catch (Exception e)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
            }

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ColivingOwner, Administrator")]
        public async Task<ActionResult<RoomResponseDto>> Put(Guid id, [FromBody] RoomCreateDto input)
        {
            var userRole = _userContextService.GetUserRole();
            var userId = _userContextService.GetUserId();
            try
            {
                if (userRole == "ColivingOwner")
                {
                    var colivingOwnerId = await _service.GetOwnerIdByRoomId(id);

                    if (colivingOwnerId != userId)
                    {
                        return StatusCode(401, new { Error = "You do not have permission to edit this room." });
                    }
                }
                
                var room = await _service.Edit(id, input);

                var result = Accepted(room);

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
            var userRole = _userContextService.GetUserRole();
            var userId = _userContextService.GetUserId();
            try
            {
                if (userRole == "ColivingOwner")
                {
                    var colivingOwnerId = await _service.GetOwnerIdByRoomId(id);

                    if (colivingOwnerId != userId)
                    {
                        return StatusCode(401, new { Error = "You do not have permission to delete this room." });
                    }
                }
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
