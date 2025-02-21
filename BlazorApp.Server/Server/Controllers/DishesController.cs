using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazorApp.Interfaces;
using BlazorApp.Models;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using ISession = BlazorApp.Interfaces.ISession;
using BlazorApp.Models.Response;
namespace BlazorApp.Server.Controllers
{
    [Route("api/Dishes")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IDishes _dishes;
        private readonly ISession _session;

        private static List<Dish> dishes = new List<Dish>
    {
        new Dish { Id = Guid.NewGuid(), Name = "Pasta", Description = "Italian pasta", Price = 12.99M, Category = "Main" },
        new Dish { Id = Guid.NewGuid(), Name = "Pizza", Description = "Cheese pizza", Price = 15.99M, Category = "Main" }
    };
        public DishesController(IDishes dishes, ISession session)
        {
            _dishes = dishes;
            _session = session;

        }

        // GET: api/Dishes
        [HttpGet]
        public async Task<IActionResult> Dishes()
        {
            try
            {
                var dishes = _dishes.GetAllDishes();

                if (dishes == null || !dishes.Any())
                {
                    return NotFound("No dishes available.");
                }

                return Ok(dishes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DishesController: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        // GET: api/Dishes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid dish ID.");
            }

            var dishResponse = _dishes.GetDishById(id);
            if (dishResponse == null || dishResponse.Dish == null)
            {
                return NotFound(new { Message = "Dish not found" });
            }

            return Ok(dishResponse.Dish);
        }


        [HttpPost("AddDish")]
        public async Task<IActionResult> AddDish([FromForm] DishDto dishDto)
        {
            Console.WriteLine("AddDish method called."); // Проверка вызова метода
            try
            {
                Dish dish = new Dish
                {
                    Category = dishDto.Category,
                    Name = dishDto.Name,
                    Price = dishDto.Price,
                    Description = dishDto.Description,
                };
                if (dishDto.imageFile != null && dishDto.imageFile.Length > 0)
                {
                    var projectDirectory = @"C:\Users\wonde\source\repos\BlazorApp\BlazorApp";
                    var ImagesDirectory = Path.Combine(projectDirectory, "wwwroot", "Images");


                    if (!Directory.Exists(ImagesDirectory))
                    {
                        Directory.CreateDirectory(ImagesDirectory);
                    }

                    var fileName = Path.GetFileName(dishDto.imageFile.FileName);
                    var filePath = Path.Combine(ImagesDirectory, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dishDto.imageFile.CopyToAsync(stream);
                    }

                    dish.ImageUrl = $"/Images/{fileName}";
                }

                var dishResponse = _dishes.AddDish(dish);
                if (dishResponse.Status)
                {
                    return RedirectToAction("Dishes");
                }
                else
                {
                    return BadRequest(new { Message = "Failed to add dish, please try again" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error message

                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }

        }


        // POST: api/Dishes/Edit/{id}
        [HttpPut("EditDish")]
        public async Task<IActionResult> EditDish([FromForm] DishDto dishDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Dish dish = new Dish();
            if (Guid.TryParse(dishDto.Id, out Guid dishId))
            {

                dish.Category = dishDto.Category;
                dish.Name = dishDto.Name;
                dish.Price = dishDto.Price;
                dish.Description = dishDto.Description;
                dish.Id = dishId;


            }
            else
            {
                return BadRequest(new { Message = "Id is wrong" });
            }
            if (dishDto.imageFile != null && dishDto.imageFile.Length > 0)
            {
                var projectDirectory = @"C:\Users\wonde\source\repos\BlazorApp\BlazorApp";
                var ImagesDirectory = Path.Combine(projectDirectory, "wwwroot", "Images");


                if (!Directory.Exists(ImagesDirectory))
                {
                    Directory.CreateDirectory(ImagesDirectory);
                }

                var fileName = Path.GetFileName(dishDto.imageFile.FileName);
                var filePath = Path.Combine(ImagesDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dishDto.imageFile.CopyToAsync(stream);
                }

                dish.ImageUrl = $"/Images/{fileName}";
            }

            var dishResponse = _dishes.UpdateDish(dish.Id, dish);
            if (dishResponse.Status)
            {
                return Ok(new { Message = "Dish updated successfully" });
            }
            else
            {
                return BadRequest(new { Message = "Failed to update the dish" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(Guid id)
        {
            var dishResponse = await _dishes.DeleteDishAsync(id);
            if (dishResponse.Status)
            {
                return Ok(new { Status = true, Message = "Dish deleted successfully" });
            }
            else
            {
                return BadRequest(new { Status = false, Message = "Failed to delete the dish" });
            }
        }
    }
}
