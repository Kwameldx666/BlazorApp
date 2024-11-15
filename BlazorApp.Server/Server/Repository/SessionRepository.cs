﻿using Azure.Core;
using BlazorApp.DbModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using BlazorApp.Helpers;
using BlazorApp.Models;
using ISession = BlazorApp.Interfaces.ISession;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Repository
{
    public class SessionRepository : ISession
    {
 
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _applicationDbContext;


        public SessionRepository(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _applicationDbContext = dbContext;
        }

        // Sets a user cookie and updates or creates a session
        public void SetUserCookie(string loginCredential, bool rememberMe)
        {
            // Генерация значения куки
            var cookieValue = CookieGenerator.Create(loginCredential);

            // Установка куки
            var cookieOptions = new CookieOptions
            {
                Expires = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddMinutes(60),
                HttpOnly = true,
                Secure = true,
                IsEssential = true
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-KEY", cookieValue, cookieOptions);

            // Работа с базой данных
            var validate = new EmailAddressAttribute();
            Session currentSession = null;

            if (validate.IsValid(loginCredential))
            {
                currentSession = _applicationDbContext.Sessions.FirstOrDefault(e => e.Credential == loginCredential);
            }
          

            if (currentSession != null)
            {
                // Обновление существующей сессии
                currentSession.CookieString = cookieValue;
                currentSession.ExpireTime = cookieOptions.Expires.Value;

                _applicationDbContext.Entry(currentSession).State = EntityState.Modified;
            }
            else
            {
                // Создание новой сессии
                var newSession = new Session
                {
                    Credential = loginCredential,
                    CookieString = cookieValue,
                    ExpireTime = cookieOptions.Expires.Value
                };

                _applicationDbContext.Sessions.Add(newSession);
            }

            _applicationDbContext.SaveChanges();
        }

    

    // Retrieves user data based on the cookie value asynchronously
    public UserData GetUserByCookie(string cookieValue)
        {
            var decryptedValue = CookieGenerator.Validate(cookieValue);
            var session =  _applicationDbContext.Sessions
                .FirstOrDefault(s => s.CookieString == cookieValue);

            if (session != null)
            {
                // Retrieve user by session.Username
                var user = _applicationDbContext.Users
                    .FirstOrDefault(u => u.Username == session.Credential);
                if (user == null)
                {
                    user = _applicationDbContext.Users
                   .FirstOrDefault(u => u.Email == session.Credential);
                }
                return user;
            }

            return null;
        }

        // Logs out the user by deleting the cookie and clearing the session
        public void UserLogout()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                // Delete user cookie
                httpContext.Response.Cookies.Delete("X-KEY");

                // Clear session data
                httpContext.Session.Clear();
            }
        }
        private Guid GetUserIdFromDatabaseAsync()
        {
            var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["X-KEY"];

            if (string.IsNullOrEmpty(cookieValue))
            {
                // Логируем отсутствие cookie
                return Guid.Empty;
            }

            try
            {
                var user = GetUserByCookie(cookieValue);

                if (user == null)
                {
                    // Логируем случай, когда пользователь не найден
                    return Guid.Empty;
                }

                return user.Id;
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
        }
        public Guid GetUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            Guid userId;

            if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out userId))
            {
                if (userId == Guid.Empty)
                {
                    userId = GetUserIdFromDatabaseAsync();
                    if (userId == Guid.Empty)
                    {
                        return Guid.Empty;
                    }
                }
            }
            else
            {
                userId = GetUserIdFromDatabaseAsync();
                if (userId == Guid.Empty)
                {
                    return Guid.Empty;
                }
            }
            SetSession("UserId", userId.ToString());
            return userId; // Замените на нужный результат
        }

        public async Task SessionStatus()
        {
            {
                var apiCookie = _httpContextAccessor.HttpContext.Request.Cookies["X-KEY"];
                if (apiCookie != null)
                {
                    var profile = GetUserByCookie(apiCookie);
                    if (profile != null)
                    {
                        // Устанавливаем объекты сессии
                        _httpContextAccessor.HttpContext.Session.SetString("LoginStatus", "login");
                        _httpContextAccessor.HttpContext.Session.SetString("Permission", profile.Roles.ToString());
                    }
                    else
                    {
                        // Очищаем сессию и удаляем cookie
                        _httpContextAccessor.HttpContext.Session.Clear();

                        if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("X-KEY"))
                        {
                            var cookie = new CookieOptions
                            {
                                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                                HttpOnly = true,
                                Secure = _httpContextAccessor.HttpContext.Request.IsHttps
                            };

                            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-KEY", string.Empty, cookie);
                        }

                        _httpContextAccessor.HttpContext.Session.SetString("LoginStatus", "logout");
                    }
                }
                else
                {
                    _httpContextAccessor.HttpContext.Session.SetString("LoginStatus", "logout");
                }
            }
        }

        public void SetSession(string name, string v)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Session name cannot be null or empty.", nameof(name));
            }

            // Проверка, что значение не null
            if (v == null)
            {
                throw new ArgumentNullException(nameof(v), "Session value cannot be null.");
            }

            _httpContextAccessor.HttpContext.Session.SetString(name, v);
        }
    }
}
