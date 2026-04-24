using AvisosEscolaresApi.Models.Entities;
using AvisosEscolaresApi.Repositories;
using AvisosEscolaresApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        //x.Audience = builder.Configuration.GetValue<string>("Jwt:Audience");
        //x.Configuration.Issuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
        x.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:SecretKey") ?? ""));
        x.TokenValidationParameters.ValidateIssuer = true;
        x.TokenValidationParameters.ValidateAudience = true;
        x.TokenValidationParameters.ValidateLifetime = true;
        x.TokenValidationParameters.ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience");
        x.TokenValidationParameters.ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
        //x.TokenValidationParameters.ClockSkew = TimeSpan(0);
    }
  );


builder.Services.AddControllers();

var cs = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AvisosescolaresContext>(x =>
{
    x.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped(typeof(Repository<>), typeof(Repository<>));
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AlumnosServices>();
builder.Services.AddScoped<AvisosService>();
builder.Services.AddScoped<MaestrosService>();


builder.Services.AddAutoMapper(x => { }, typeof(Program).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

var app = builder.Build();

app.MapControllers();

app.Run();

