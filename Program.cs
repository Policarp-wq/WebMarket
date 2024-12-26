
using Microsoft.EntityFrameworkCore;
using WebMarket.Authorization;
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

            builder.Services.AddScoped<ApiAuthFilter>();

            // Configure Kestrel to use the configuration from appsettings.json
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Configure(builder.Configuration.GetSection("Kestrel"));
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

            ConnectionString db = new ConnectionString(builder.Configuration.GetConnectionString("Database"));
            builder.Services.AddDbContext<MarketContext>(opt =>
                opt.UseNpgsql(db.GetReplacedEnvVariables()).LogTo(Console.WriteLine, LogLevel.Debug)
            );

            builder.Services.AddControllers().AddNewtonsoftJson(opt => 
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            //app.UseAuthorization();
            app.UseCors();

            app.Logger.LogInformation("Starting app ;)");

            app.MapControllers();

            app.Run();
        }
    }
}
