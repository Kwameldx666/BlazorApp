using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class DishDto
    {
        public string? Id { get; set; } 
        [Required]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; }

        public string Description { get; set; }
        public IFormFile imageFile { get; set; }
    }


}
