using Application.Abstractions.Coliving;
using Application.Contracts;
using Microsoft.AspNetCore.Mvc;
namespace ColivingReservationsPlatform.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AdsFollowUpsController : ControllerBase
{
    private readonly IColivingService _service;

    public AdsFollowUpsController(IColivingService service)
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
}
