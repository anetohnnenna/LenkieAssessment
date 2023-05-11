using LibraryApi.Controllers;
using LibraryApi.Data;
using LibraryApi.Interface;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer").AddIdentityServerAuthentication("Bearer", options =>
{
    options.ApiName = "myApi";
    options.Authority = "https://localhost:7074";
});
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryData")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepository<RegistrationModel>, EFRepository<RegistrationModel>>();
builder.Services.AddScoped<IRepository<LibraryBook>, EFRepository<LibraryBook>>();
builder.Services.AddScoped<IRepository<BookOrder>, EFRepository<BookOrder>>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
