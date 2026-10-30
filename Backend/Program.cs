using System.Reflection;
using System.Text;
using Backend.Database;
using Backend.Extensions;
using Backend.Grpc.Interceptors;
using Backend.Grpc.UserLogic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Before Build

IWebHostEnvironment env = builder.Environment;

string jwtSecret;
if (env.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext, DevelopmentDbContext>();
    jwtSecret = "SECRETSECRETSECRETSECRETSECRETSECRET";
}
else
{
    builder.Services.AddDbContext<AppDbContext>();
    jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ??
                throw new NullReferenceException("JWT secret not present in environment");
}

builder.Configuration["JwtSettings:Secret"] = jwtSecret;
builder.Configuration["JwtSettings:Issuer"] = "VLB-server";
builder.Configuration["JwtSettings:Audience"] = "VLB-client";

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddGrpc(options => { options.Interceptors.Add<GrpcExceptionInterceptor>(); });
builder.Services.AddApplicationServices(typeof(UserRepository).Assembly);

#endregion

WebApplication app = builder.Build();

#region After build

using (IServiceScope scope = app.Services.CreateScope())
{
    AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("🚀 Database connection verification: SUCCESS!");
            dbContext.Database.EnsureCreated();
        }
        else
        {
            Console.WriteLine("❌ Database connection verification: FAILED (Database might not exist).");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"💥 Database connection verification: CRASHED! Error: {ex.Message}");
    }
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcControllers(Assembly.GetExecutingAssembly());

app.Run();

#endregion