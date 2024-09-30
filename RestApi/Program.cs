using RestApi.Services.Concretes;
using Microsoft.EntityFrameworkCore;
using RestApi.Persistence.DataBase;
using RestApi.Domain;
using FluentValidation;
using RestApi.Domain.Validators;
using RestApi.JWT;
using RestApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(JwtConfiguration.ConfigAuthentication)
    .AddJwtBearer(JwtConfiguration.ConfigBearer);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerConfiguration.ConfigSwaggerGen);

builder.Services.AddScoped<AuthorizationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ExpenseService>();
builder.Services.AddScoped<IncomeService>();
builder.Services.AddScoped<TokenHandler>();
builder.Services.AddScoped<ExpenseCategoryService>();
builder.Services.AddScoped<IValidator<ExpenseCategory>, ExpenseCategoryValidator>();
builder.Services.AddScoped<IValidator<Expense>, ExpenseValidator>();
builder.Services.AddScoped<IValidator<Income>, IncomeValidator>();
builder.Services.AddScoped<IValidator<User>, UserValidator>();

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();