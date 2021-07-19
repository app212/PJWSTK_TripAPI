using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TripAPI.DTO.Request;
using TripAPI.DTO.Response;
using TripAPI.Repositories.Interfaces;

namespace TripAPI.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public TripsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            ICollection<TripResponseDTO> trips = await _dbService.GetTripsAsync();
            if (trips.Count == 0) return NotFound("There are no trips");
            return Ok(trips);
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] ClientTripRequestDTO newClient)
        {
            var test = await _dbService.AddClientTripAsync(idTrip, newClient);
            if (!test)
            {
                return NotFound("There is no trip with this id or client already booked this trip");
            }
            return Ok("Client added to the trip");
        }
    }
}
