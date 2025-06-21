using ASPNetCoreJwtAuth.Data;
using ASPNetCoreJwtAuth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
builder.Services.AddControllers();
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

services.AddScoped<JWTAuthService>();
services.AddScoped<SignInManager>();

var jwtTokenConfig = builder.Configuration.GetSection("jwt").Get<JwtTokenConfig>();
services.AddSingleton(jwtTokenConfig);

services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtTokenConfig.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtTokenConfig.Audience,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret))
    };
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My First Swagger");
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name:"default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
//app.MapDefaultControllerRoute();
//app.MapControllers();

app.Run();
