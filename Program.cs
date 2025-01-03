
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StackExchange.Redis;
using WebMarket.Authorization;
using WebMarket.Middlewares;
using WebMarket.Models;
using WebMarket.Services;

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

            builder.Services.AddScoped<ApiAuthFilter>();

            // Configure Kestrel to use the configuration from appsettings.json
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(int.Parse(Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORTS"))); // Слушать на всех сетевых интерфейсах
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
            Console.WriteLine("sas " + dbConnection);
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


            builder.Services.AddControllers().AddNewtonsoftJson(opt =>
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<LoggingMiddleware>();

            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();

            //app.UseAuthorization();
            app.UseCors();
            app.Logger.LogInformation("Starting app ;)");
            app.MapHealthChecks("/healtz");
            app.MapControllers();

            app.Run();
        }
    }
}
