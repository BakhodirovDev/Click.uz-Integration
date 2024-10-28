using Application.Interface.Repository.Transaction;
using Application.Interface.User;
using Application.Interfaces.AutoPay.Click;
using Application.Repository;
using Application.Service.User;
using Application.Services.AutoPay.Click;
using Domain.Settings;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<ClickSettings>(builder.Configuration.GetSection("ClickSettings"));

builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IClickService, ClickService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

    options.AddPolicy("Click", builder =>
    {
        builder.WithOrigins(
            "http://213.230.65.140",
            "http://217.29.119.130",
            "http://217.29.119.131",
            "http://217.29.119.132",
            "http://217.29.119.133"
        )
        .WithMethods("POST")
        .AllowAnyHeader();
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
