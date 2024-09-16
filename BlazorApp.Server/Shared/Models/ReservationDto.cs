using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models
{
    public class ReservationDto
    {

        [Key]
        public Guid ReservationId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100, ErrorMessage = "Customer name can't be longer than 100 characters")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Number of people is required")]
        [Range(1, 20, ErrorMessage = "Number of people must be between 1 and 20")]
        public int NumberOfPeople { get; set; }

        [Required(ErrorMessage = "Reservation date is required")]
        [DataType(DataType.Date)]
        public DateTime ReservationDate { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? ReservationTime { get; set; }  // Optional field

        public string Status { get; set; } = "Pending"; // Default status

    }
}
