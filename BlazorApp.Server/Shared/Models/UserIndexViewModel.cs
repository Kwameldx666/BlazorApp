using System;
using System.Collections.Generic;
using BlazorApp.Models;
using BlazorApp.Shared.Models;

namespace BlazorApp.ViewModels
{
    public class UserIndexViewModel
    {
        public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();
        public UserDto UserToEdit { get; set; }
        public string ErrorMessage { get; set; }
    }
}
