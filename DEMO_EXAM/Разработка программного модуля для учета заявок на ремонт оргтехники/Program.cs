using Разработка_программного_модуля_для_учета_заявок_на_ремонт_оргтехники.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Разрешить любые источники
              .AllowAnyMethod()  // Разрешить любые методы
              .AllowAnyHeader(); // Разрешить любые заголовки
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

List<Order> repo = new List<Order>()
{
    new Order(1, "Компьютер", "Lenovo ThinkPad", "Заменить жесткий диск", "Иван Иванов", "В процессе", "Петр Петров", "+7 900 123-45-67", new DateTime(2024, 12, 1), null),
    new Order(2, "Принтер", "HP LaserJet", "Заменить картридж", "Мария Петрова", "Выполнена", "Алексей Смирнов", "+7 900 987-65-43", new DateTime(2024, 11, 10), new DateTime(2024, 11, 12)),
    new Order(3, "Ноутбук", "MacBook Pro", "Проблемы с зарядкой", "Сергей Ковалев", "В процессе", "Иван Дьяков", "+7 900 654-32-10", new DateTime(2024, 12, 5), null)
};

app.MapGet("/", () =>
{
    return Results.Json(repo);
});

app.MapPost("/", (Order o) => repo.Add(o));

app.MapPut("/{num}", (int num, OrderUpdateDTO dto) =>
{
    var existingOrder = repo.FirstOrDefault(o => o.Num == num);
    if (existingOrder is null)
    {
        return Results.NotFound();
    }
    if (dto.Status != null)
        existingOrder.Status = dto.Status;

    if (dto.Description != null)
        existingOrder.Description = dto.Description;

    if (!string.IsNullOrEmpty(dto.Master))
        existingOrder.Master = dto.Master;

    if (!string.IsNullOrEmpty(dto.MasterComment))
        existingOrder.MasterComment = dto.MasterComment;

    return Results.Ok(existingOrder);
});

app.MapGet("/{num}", (int num) => repo.Find(o => o.Num == num));
app.MapGet("/filter/{param}", (string param) => repo.FindAll(o =>
    o.Tech_type == param ||
    o.Model == param ||
    o.Description == param ||
    o.Client_name == param ||
    o.Status == param ||
    o.Master == param ||
    o.Num_tel == param));

app.MapGet("/stats/completed", () =>
{
    var completedOrders = repo.Count(o => o.Status == "Выполнена");
    return Results.Json(completedOrders);
});

app.MapGet("/stats/average-time", () =>
{
    var completedOrders = repo.Where(o => o.Status == "Выполнена" && o.CheckInDate.HasValue && o.CheckOutDate.HasValue).ToList();

    double averageTime = 0;
    if (completedOrders.Any())
    {
        averageTime = completedOrders.Average(o => (o.CheckOutDate.Value - o.CheckInDate.Value).TotalDays);
    }

    return Results.Json(averageTime);
});

app.MapGet("/stats/tech-types", () =>
{
    var techTypeStats = repo
        .Where(o => !string.IsNullOrEmpty(o.Tech_type)) 
        .GroupBy(o => o.Tech_type)
        .Select(g => new { TechType = g.Key, Count = g.Count() })
        .ToList();

    return Results.Json(techTypeStats);
});

app.Run();
