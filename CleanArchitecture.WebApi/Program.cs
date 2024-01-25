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
builder.Services.AddTransient<ExceptionMiddleware>();//Middlewarei �al��t�rmak i�in gerekli
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddAuthentication().AddJwtBearer();


builder.Services.AddAutoMapper(typeof(CleanArchitecture.Persistance.AssemblyReference).Assembly);//Automapper persistance katman�ndaki
                                                                                                 //otomatik kendi yap�s�n� bulacak

string connectionString = builder.Configuration.GetConnectionString("SqlServer");//appsettings.json'dan veri taban� ba�lant� bilgilerini ald�k.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));/*Persistance katman�ndaki veri taban� ba�lant�
                                                    class� olan AppDbContext'e ula�t�k ve yine referans olarak Persistance katman�n� verdik.*/

builder.Services.AddIdentity<User, IdentityRole>(/*options=>
{
    options.Password.RequireNonAlphanumeric = false; // 
    options.Password.RequiredLength = 1; //ka� karakter ister en az
    options.Password.RequireUppercase = false; //b�y�k karakter zorunlulu�unu iptal eder . istersek b�yle kurallar� kald�rabiliriz.
}*/).AddEntityFrameworkStores<AppDbContext>();// User k�sm�n� contexte ba�l�yoruz

builder.Services.AddControllers()
    .AddApplicationPart(typeof(CleanArchitecture.Presentation.AssemblyReference).Assembly);/*WebApi solutiona sa� t�klay�p add/project reference 
dedik ve CleanArchitecture.Presentation katman�n� referasn olarak verip AddControllers() tan sonra .AddApplicationPart diyerek controllerlar�n
bulundu�u katman� referans verdik. Bu sayede controllerda yazd���m�z kodlar� proje alg�layabilecek(AssemblyReference)*/


builder.Services.AddMediatR(cfr => cfr.RegisterServicesFromAssembly(typeof(CleanArchitecture.Application.AssemblyReference).Assembly));//CQRS pattern i�in gerekli

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));//validation behavior i�in gerekli yani yaz�lan validationlar� �al��t�rmak i�in
builder.Services.AddValidatorsFromAssembly(typeof
    (CleanArchitecture.Application.AssemblyReference).Assembly);//validator i�in application katman�n�n assembly referans�n� verdik

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddlewareExtensions();//Middleware �al��t�rmak i�in gerekli

app.UseHttpsRedirection();


app.MapControllers();

app.Run();
