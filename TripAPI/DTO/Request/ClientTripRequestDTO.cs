using DataAnnotationsExtensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace TripAPI.DTO.Request
{
    public class ClientTripRequestDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, Email]
        public string Email { get; set; }
        [Required, Phone]
        public string Telephone { get; set; }
        [Required, RegularExpression("[0-9]{11}", ErrorMessage = "PESEL must use 11 digits")]
        public string Pesel { get; set; }
        [Required]
        public int IdTrip { get; set; }
        [Required]
        public string TripName { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
