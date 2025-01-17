using RestApi.Services.Concretes;
using Microsoft.EntityFrameworkCore;
using RestApi.Persistence.DataBase;
using RestApi.Domain;
using FluentValidation;
using RestApi.Domain.Validators;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ExpenseService>(); 
builder.Services.AddScoped<IncomeService>(); 
builder.Services.AddScoped<ExpenseCategoryService>();
builder.Services.AddScoped<IValidator<ExpenseCategory>, ExpenseCategoryValidator>();
builder.Services.AddScoped<IValidator<Expense>, ExpenseValidator>();
builder.Services.AddScoped<IValidator<Income>, IncomeValidator>();
builder.Services.AddScoped<IValidator<User>, UserValidator>();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpenseTracker API v1");
        options.RoutePrefix = string.Empty; 
    });
}
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
