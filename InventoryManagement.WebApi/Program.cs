using InventoryManagement.WebApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();


app.Run();


/*
 *
 * 1. Anwender registriert sich mit seiner E-Mail-Adresse
 * 2. Anwender bekommt eine E-Mail mit einem persönlichen Anmeldelink. URL ist "https://domain.com/session?token=eyjgjg"
 * 3. Token ist JWT-Token mit einer Gültigkeit von 2 Wochen
 */ 