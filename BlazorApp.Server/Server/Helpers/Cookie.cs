using BlazorApp.DbModel;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using BlazorApp.Helpers;
using BlazorApp.Models;
using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _applicationDbContext;

    // Constructor initializing IHttpContextAccessor and ApplicationDbContext
    public UserService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _applicationDbContext = dbContext;
    }

    // Method to set a user cookie and manage session data
    public void SetUserCookie(string loginCredential, bool rememberMe)
    {
        // Generate the value for the cookie
        var cookieValue = CookieGenerator.Create(loginCredential);

        // Define options for the cookie
        var cookieOptions = new CookieOptions
        {
            Expires = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddMinutes(60), // Set expiration based on rememberMe flag
            HttpOnly = true, // Cookie is accessible only through HTTP requests
            Secure = true, // Cookie is only sent over HTTPS
            IsEssential = true // Cookie is essential for the app functionality
        };

        // Append the cookie to the HTTP response
        _httpContextAccessor.HttpContext.Response.Cookies.Append("X-KEY", cookieValue, cookieOptions);

        // Initialize email validator
        var validate = new EmailAddressAttribute();
        Session currentSession = null;

        // Check if loginCredential is a valid email or username
        if (validate.IsValid(loginCredential))
        {
            // Retrieve the session based on email
            currentSession = _applicationDbContext.Sessions.FirstOrDefault(e => e.Credential == loginCredential);
        }
      

        if (currentSession != null)
        {
            // Update the existing session with the new cookie value and expiration time
            currentSession.CookieString = cookieValue;
            currentSession.ExpireTime = cookieOptions.Expires.Value;

            _applicationDbContext.Entry(currentSession).State = EntityState.Modified;
        }
        else
        {
            // Create a new session if no existing session is found
            var newSession = new Session
            {
                Credential = loginCredential,
                CookieString = cookieValue,
                ExpireTime = cookieOptions.Expires.Value
            };

            _applicationDbContext.Sessions.Add(newSession);
        }

        // Save changes to the database
        _applicationDbContext.SaveChanges();
    }
}
