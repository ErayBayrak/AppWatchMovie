
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string tokenKey = builder.Configuration.GetSection("AppSettings:Token").Value;
SecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
options.TokenValidationParameters = new TokenValidationParameters
{
	ValidateIssuer = true,
	ValidateAudience = true,
	ValidateLifetime = true,
	ValidIssuer = "https://localhost:44394",
	ValidAudience = "https://localhost:44394",
	ValidateIssuerSigningKey = true,
	IssuerSigningKey=key
}
);
builder.Services.AddScoped<IUserDal, EfUserDal>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IAuthService, AuthManager>();

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
