using BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models
{
    public class WebSocketMessage
    {
        public string Action { get; set; }
        public Dish Dish { get; set; }
    }
}
