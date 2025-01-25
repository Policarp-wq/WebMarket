
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using WebMarket.Authorization;
using WebMarket.Authorization.JWT;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories;
using WebMarket.DataAccess.Repositories.Abstractions;
using WebMarket.Middlewares;
using WebMarket.ServerExceptions;
using WebMarket.Services;
using WebMarket.SupportTools;

namespace WebMarket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
            builder.Configuration.AddJsonFile("appsettings.json");
            builder.Configuration.AddEnvironmentVariables();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddScoped<ApiAuthFilter>();

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
            builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            builder.Services.AddScoped<IShoppingCartElementsRepository, ShoppingCartElementsRepository>();
            builder.Services.AddScoped<IReviewsRepository, ReviewsRepository>();

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<ShoppingCartService>();
            builder.Services.AddScoped<ReviewService>();

            builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection(nameof(JWTOptions)));

            builder.Services.AddScoped<JWTProvider>();
            // Configure Kestrel to use the configuration from appsettings.json
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(int.Parse(Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORTS")));
            });

            builder.Logging.AddConsole();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            string dbConnection = ConnectionString
                .GetReplacedEnvVariables(builder.Configuration.GetConnectionString("Database"),
                ["DB_SERVER", "DB_USER", "DB_PASSWORD"]);
            builder.Services.AddDbContext<MarketContext>(opt =>
                opt.UseNpgsql(dbConnection).LogTo(Console.WriteLine, new[] {
                        RelationalEventId.CommandError,
                    }, LogLevel.Debug)
            );

            string redisConnection = ConnectionString.GetReplacedEnvVariables(builder.Configuration.GetConnectionString("Redis"),
                ["REDIS_SERVER", "REDIS_PASSWORD"]);

            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));

            builder.Services
                .AddHealthChecks()
                .AddDbContextCheck<MarketContext>("Database")
                .AddRedis(redisConnection, name: "Redis");
            string secretKey = builder.Configuration.GetSection(nameof(JWTOptions))["SecretKey"];
            builder.Services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme
                ).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
                {
                    opt.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(secretKey)
                            )
                    };

                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (context) =>
                        {
                            context.Token = context.Request.Cookies[JWTOptions.CookiesName];

                            return Task.CompletedTask;
                        }
                    };
                });
            builder.Services.AddAuthorization();


            builder.Services.AddControllers().AddNewtonsoftJson(opt =>
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseExceptionHandler();
            app.UseMiddleware<LoggingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors();
            app.Logger.LogInformation("Starting app ;)");
            app.MapHealthChecks("/healtz");
            app.MapControllers();

            app.Run();
        }
    }
}
