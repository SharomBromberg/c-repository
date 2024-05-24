

using FlightAPP.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
public class FlightController : ControllerBase
{
    private IJourneyService _journeyService;

    public FlightController(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    [HttpGet("AllFlights")]
    public async Task<IActionResult> GetFlights()
    {
        var flights = await _journeyService.GetAllFlights();
        return Ok(flights);
    }

    [HttpGet("Flights")]
    public async Task<IActionResult> GetFlights(string origin, string destination, string currency, string type, bool allowStops)
    {
        if (type == "round")
        {
            var flights = await _journeyService.GetRoundTripFlights(origin, destination, currency, allowStops);
            return Ok(flights);
        }
        else
        {
            var flights = await _journeyService.GetOneWayFlights(origin, destination, currency, allowStops);
            return Ok(flights);
        }
    }
}