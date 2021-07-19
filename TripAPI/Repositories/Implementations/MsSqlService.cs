using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripAPI.DTO.Request;
using TripAPI.DTO.Response;
using TripAPI.Models;
using TripAPI.Repositories.Interfaces;

namespace TripAPI.Repositories.Implementations
{
    public class MsSqlService : IDbService
    {
        private readonly s19346Context _context;

        public MsSqlService(s19346Context context)
        {
            _context = context;
        }

        public async Task<bool> AddClientTripAsync(int idTrip, ClientTripRequestDTO newClient)
        {
            var trip = await _context.Trips.SingleOrDefaultAsync(x => x.IdTrip == idTrip);
            if (trip == null) return false;

            var client = await _context.Clients.SingleOrDefaultAsync(x => x.Pesel == newClient.Pesel);
            if (client == null)
            {
                var maxIdClient = await _context.Clients.MaxAsync(x => x.IdClient);

                await _context.Clients.AddAsync(new Client
                {
                    IdClient = maxIdClient + 1,
                    FirstName = newClient.FirstName,
                    LastName = newClient.LastName,
                    Email = newClient.Email,
                    Telephone = newClient.Telephone,
                    Pesel = newClient.Pesel
                });
                await _context.ClientTrips.AddAsync(new ClientTrip
                {
                    IdClient = maxIdClient + 1,
                    IdTrip = idTrip,
                    RegisteredAt = DateTime.Now
                });

                return await _context.SaveChangesAsync() > 0;
            }

            var clientTrip = await _context.ClientTrips
                .Include(x => x.IdClientNavigation)
                .Include(x => x.IdTripNavigation)
                .AnyAsync(x => x.IdClientNavigation.IdClient == client.IdClient 
                                && x.IdTripNavigation.IdTrip == idTrip);

            if (clientTrip) return false;

            await _context.ClientTrips.AddAsync(new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteClientByIdAsync(int idClient)
        {
            var client = await _context.Clients
                .Include(x => x.ClientTrips)
                .SingleOrDefaultAsync(x => x.IdClient == idClient);

            if (client == null) return false;
            if (client.ClientTrips.Count > 0) return false;

            _context.Clients.Remove(client);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ICollection<TripResponseDTO>> GetTripsAsync()
        {
            return await _context.Trips
                .Include(x => x.CountryTrips)
                .Include(x => x.ClientTrips)
                .Select(x => new TripResponseDTO
                {
                    Name = x.Name,
                    Description = x.Description,
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo,
                    MaxPeople = x.MaxPeople,
                    Countries = x.CountryTrips.Select(y => new CountryResponseDTO 
                    {
                        Name = y.IdCountryNavigation.Name
                    }).ToList(),
                    Clients = x.ClientTrips.Select(y => new ClientResponseDTO 
                    {
                        FirstName = y.IdClientNavigation.FirstName,
                        LastName = y.IdClientNavigation.LastName
                    }).ToList()
                }).OrderByDescending(x => x.DateFrom).ToListAsync();
        }
    }
}