using BlazorApp.Models.Enums;
using BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models
{
    public class UserDto
    {
        public Guid Id { get; set; } 

        public string Username { get; set; }

        public string Email { get; set; } 

        public Role Roles { get; set; }

        public string? IP { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

    }
}
