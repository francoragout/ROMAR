using Infraestructure.Database;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infraestructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Register Dapper context
builder.Services.AddSingleton<DapperContext>();

// Register repositories and services
builder.Services.AddScoped<IClienteRepository, ClientRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// Add services to the container.
builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
