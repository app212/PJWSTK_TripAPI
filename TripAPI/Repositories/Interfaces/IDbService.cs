using System.Collections.Generic;
using System.Threading.Tasks;
using TripAPI.DTO.Request;
using TripAPI.DTO.Response;

namespace TripAPI.Repositories.Interfaces
{
    public interface IDbService
    {
        public Task<ICollection<TripResponseDTO>> GetTripsAsync();

        public Task<bool> DeleteClientByIdAsync(int idClient);

        public Task<bool> AddClientTripAsync(int idTrip, ClientTripRequestDTO newClient);
    }
}
