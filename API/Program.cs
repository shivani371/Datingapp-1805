using System.Text;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors();
builder.Services.AddScoped<ITokenService ,TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
{
    OnAuthenticationFailed = context =>
    {
        Console.WriteLine("AUTH FAILED: " + context.Exception.Message);
        return Task.CompletedTask;
    },
    OnTokenValidated = context =>
    {
        Console.WriteLine("TOKEN VALIDATED");
        return Task.CompletedTask;
    }
};

    var tokenKey=builder.Configuration["TokenKey"] ?? throw new Exception("Token key is not found=pragoram.cs");
    options.TokenValidationParameters= new TokenValidationParameters
    {
        ValidateIssuerSigningKey=true,
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        ValidateIssuer=false,
        ValidateAudience=false,
        ClockSkew = TimeSpan.Zero
    };

}
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().
WithOrigins("http://localhost:4200","https://localhost:4200"));
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
