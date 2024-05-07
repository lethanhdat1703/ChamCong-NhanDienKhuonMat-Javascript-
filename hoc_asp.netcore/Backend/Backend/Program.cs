using Backend.Data;
using Backend.Models;
using Backend.Repository;
using Backend.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   
builder.Services.AddDbContext<NhandienDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DemoDB"));
});
//
builder.Services.AddAutoMapper(typeof(Program));
//
builder.Services.AddScoped<ITimeSheetRepository,TimeSheetRepository>();
//
builder.Services.AddScoped<IFaceModelsRepository, FaceModelsRepository>();
//
builder.Services.AddScoped<IGenericRepository<TimeSheet>, GenericRepository<TimeSheet>>();
//
builder.Services.AddScoped<IGenericRepository<Employee>, GenericRepository<Employee>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
