using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TripAPI.Repositories.Interfaces;

namespace TripAPI.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public ClientsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClientById(int idClient)
        {
            var isClientRemoved = await _dbService.DeleteClientByIdAsync(idClient);
            if (!isClientRemoved)
            {
                return NotFound("There is no client with this id or they have booked trip");
            }
            return NoContent();
        }
    }
}
