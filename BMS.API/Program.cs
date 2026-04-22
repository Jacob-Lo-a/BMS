using BMS.API.Middlewares;
using BMS.API.Repositories;
using BMS.API.Services;
using BMS.Core.Interfaces;
using BMS.Core.Models;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")),
            NameClaimType = ClaimTypes.NameIdentifier
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("SecurityLogger");

                logger.LogWarning("401 Unauthorized: {Path}",
                    context.HttpContext.Request.Path);

                return Task.CompletedTask;
            },

            OnForbidden = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("SecurityLogger");

                logger.LogWarning("403 Forbidden: {Path}",
                    context.HttpContext.Request.Path);

                return Task.CompletedTask;
            }
        };
    });

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddDbContext<CloudBookStoreDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CloudBookstoreDb"))
    );
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGuidGenerator, GuidGenerator>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISftpService, SftpService>();
builder.Services.Configure<Sftpsettings>(
    builder.Configuration.GetSection("SftpSettings"));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // JWT 設定
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "請輸入 JWT Token（格式：{token}）"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("CloudBookStoreDB")));

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ApiBehaviorMiddleware>();

app.UseHangfireDashboard();

app.MapControllers();  

RecurringJob.AddOrUpdate<IOrderService>(
    "Daily-Sales-Report",
    service => service.ExportOrdersAsync(),
    Cron.Daily(3) 
);
// make you feel my love should I give up or should I just keep chasing pavement 
app.Run();
