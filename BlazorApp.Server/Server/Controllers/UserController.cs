using Microsoft.AspNetCore.Mvc;
using BlazorApp.Interfaces;
using BlazorApp.Models;
using BlazorApp.Models.Response;
using BlazorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISession = BlazorApp.Interfaces.ISession;
using BlazorApp.Shared.Models;
using AutoMapper;
namespace BlazorApp.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userRepository;
        private readonly ISession _sessionService;
        private readonly IMapper _mapper;
        public UserController(IUser userRepository, ISession session, IMapper mapper)
        {
            _userRepository = userRepository;
            _sessionService = session;
            _mapper = mapper;
        }

        // GET: /User/Index
        [HttpGet]
        public async Task<IActionResult> Users(Guid? editId)
        {
            // Асинхронно получаем всех пользователей
            var users = _userRepository.GetAllUsers();

            UserResponse userToEdit = null;
            if (editId.HasValue)
            {
                // Асинхронно получаем одного пользователя по ID
                userToEdit = await _userRepository.GetOneUserByIdAsync(editId.Value);
            }

            // Создаем список для маппинга пользователей в DTO
            var usersDto = new List<UserDto>();

            if (users != null)
            {
                foreach (var user in users)
                {
                    var userDto = _mapper.Map<UserDto>(user);
                    usersDto.Add(userDto); // Добавляем преобразованного пользователя в список DTO
                }
            }
            var userToEditDto = _mapper.Map<UserDto>(userToEdit);
            // Формируем ViewModel с преобразованными пользователями
            var viewModel = new UserIndexViewModel
            {
                Users = usersDto, // Используем DTO пользователей
                UserToEdit = userToEditDto
            };

            return Ok(viewModel);
        }



        // POST: /User/DeleteConfirmed
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var deleteResponse = _userRepository.DeleteUser(id);

            if (deleteResponse.Status)
            {
                return Ok(new { message = "User deleted successfully" });
            }

            return BadRequest("An error occurred while deleting the user.");
        }


        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto modelDto)
        {
            // Маппинг DTO в модель данных
            var model = _mapper.Map<UserData>(modelDto);

            // Обновляем пользователя через репозиторий
            var updateUserResponse = await _userRepository.UpdateUser(model);

            if (updateUserResponse.Status)
            {
                // Если обновление успешно, перенаправляем на метод Index
                return Ok();
            }

            // Если возникла ошибка, добавляем сообщение в ModelState
            ModelState.AddModelError(string.Empty, "An error occurred while updating the user.");
            // Возвращаем результат в виде списка пользователей
            return BadRequest();
        }


        [HttpGet("role")]
        public IActionResult GetUserRole()
        {
            // Получаем идентификатор пользователя из сессии
            Guid userId = _sessionService.GetUserId();

            // Проверка, что идентификатор пользователя не пустой
            if (userId == Guid.Empty)
            {
                // Если пользователь не авторизован, возвращаем 401 Unauthorized
                return Unauthorized(new { Message = "User is not authorized or not found" });
            }

            // Получаем роль пользователя по его идентификатору
            var userRole = _userRepository.GetUserRole(userId);

            // Преобразуем роль в строку
            var userRoleString = userRole.ToString();

            // Возвращаем результат с кодом 200 OK
            return Ok(new { Role = userRoleString });
        }

    }
}
