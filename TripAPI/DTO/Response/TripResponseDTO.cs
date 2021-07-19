using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripAPI.Models;

namespace TripAPI.DTO.Response
{
    public class TripResponseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public ICollection<CountryResponseDTO> Countries { get; set; }
        public ICollection<ClientResponseDTO> Clients { get; set; }
    }
}
