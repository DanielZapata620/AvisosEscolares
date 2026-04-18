using AvisosEscolaresApi.Models.Entities;
using AvisosEscolaresApi.Repositories;
using AvisosEscolaresApi.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

var cs = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AvisosescolaresContext>(x =>
{
    x.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

builder.Services.AddScoped(typeof(Repository<>), typeof(Repository<>));
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AlumnosServices>();
builder.Services.AddScoped<AvisosService>();


builder.Services.AddAutoMapper(x => { }, typeof(Program).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

var app = builder.Build();

app.MapControllers();

app.Run();

