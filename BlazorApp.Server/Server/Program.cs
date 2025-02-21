using BlazorApp.DbModel;
using BlazorApp.Interfaces;
using BlazorApp.Models;
using BlazorApp.Repository;
using BlazorApp.Server.Helpers;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using ISession = BlazorApp.Interfaces.ISession;

var builder = WebApplication.CreateBuilder(args);

// Configure database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        // Add custom JSON converters
        options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
    });
builder.Services.AddRazorPages();
builder.Services.AddLogging();

// Configure session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Пример конфигурации времени ожидания
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register application services

builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<ISession, SessionRepository>();
builder.Services.AddScoped<IDishes, DishRepository>();
builder.Services.AddScoped<IPaymentGateway, PaymentProcessor>();
builder.Services.AddScoped<IReservation, ReservationRepository>();
builder.Services.AddScoped<ICartService, RegularUserCartService>();
builder.Services.AddScoped<ICartService, VIPUserCartService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:8080/") });

// Register a singleton service
builder.Services.AddSingleton<INotification>(sp => NotificationService.Instance);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseSession(); // Ensure session middleware is used before routing
app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();