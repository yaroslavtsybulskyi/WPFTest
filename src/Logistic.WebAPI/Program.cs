using Logistic.Core.Services;
using Logistic.DAL;
using Logistic.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IRepository<Vehicle>, InMemoryRepository<Vehicle>>();
builder.Services.AddSingleton<IService<Vehicle>, VehicleService>();
builder.Services.AddSingleton<IRepository<Warehouse>, InMemoryRepository<Warehouse>>();
builder.Services.AddSingleton<IService<Warehouse>, WarehouseService>();

builder.Services.AddSingleton<IReportRepository<Vehicle>, JsonRepository<Vehicle>>();
builder.Services.AddSingleton<IReportRepository<Vehicle>, XmlRepository<Vehicle>>();
builder.Services.AddSingleton<IReportRepository<Warehouse>, JsonRepository<Warehouse>>();
builder.Services.AddSingleton<IReportRepository<Warehouse>, XmlRepository<Warehouse>>();

builder.Services.AddSingleton<IReportService<Vehicle>, ReportService<Vehicle>>();
builder.Services.AddSingleton<IReportService<Warehouse>, ReportService<Warehouse>>();

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
