using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Application.Behaviors;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Authentication;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Persistance.Context;
using CleanArchitecture.Persistance.Services;
using CleanArchitecture.WebApi.Middleware;
using CleanArchitecture.WebApi.OptionsSetup;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<ExceptionMiddleware>();//Middlewarei çalýþtýrmak için gerekli
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddAuthentication().AddJwtBearer();


builder.Services.AddAutoMapper(typeof(CleanArchitecture.Persistance.AssemblyReference).Assembly);//Automapper persistance katmanýndaki
                                                                                                 //otomatik kendi yapýsýný bulacak

string connectionString = builder.Configuration.GetConnectionString("SqlServer");//appsettings.json'dan veri tabaný baðlantý bilgilerini aldýk.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));/*Persistance katmanýndaki veri tabaný baðlantý
                                                    classý olan AppDbContext'e ulaþtýk ve yine referans olarak Persistance katmanýný verdik.*/

builder.Services.AddIdentity<User, IdentityRole>(/*options=>
{
    options.Password.RequireNonAlphanumeric = false; // 
    options.Password.RequiredLength = 1; //kaç karakter ister en az
    options.Password.RequireUppercase = false; //büyük karakter zorunluluðunu iptal eder . istersek böyle kurallarý kaldýrabiliriz.
}*/).AddEntityFrameworkStores<AppDbContext>();// User kýsmýný contexte baðlýyoruz

builder.Services.AddControllers()
    .AddApplicationPart(typeof(CleanArchitecture.Presentation.AssemblyReference).Assembly);/*WebApi solutiona sað týklayýp add/project reference 
dedik ve CleanArchitecture.Presentation katmanýný referasn olarak verip AddControllers() tan sonra .AddApplicationPart diyerek controllerlarýn
bulunduðu katmaný referans verdik. Bu sayede controllerda yazdýðýmýz kodlarý proje algýlayabilecek(AssemblyReference)*/


builder.Services.AddMediatR(cfr => cfr.RegisterServicesFromAssembly(typeof(CleanArchitecture.Application.AssemblyReference).Assembly));//CQRS pattern için gerekli

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));//validation behavior için gerekli yani yazýlan validationlarý çalýþtýrmak için
builder.Services.AddValidatorsFromAssembly(typeof
    (CleanArchitecture.Application.AssemblyReference).Assembly);//validator için application katmanýnýn assembly referansýný verdik

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddlewareExtensions();//Middleware çalýþtýrmak için gerekli

app.UseHttpsRedirection();


app.MapControllers();

app.Run();
