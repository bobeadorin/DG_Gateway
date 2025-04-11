
using ApiGatewayService.Utilities;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGatewayService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            builder.Services.AddTransient<IConfigUpdates, ConfigUpdates>();

            builder.Services.AddOcelot(builder.Configuration);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowExpo", policy =>
                {
                    policy.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader();
                });
            });

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            var IPV4 = LocalInfo.GetLocalIP();

            Console.WriteLine(IPV4);

            var url = $"http://{IPV4}:5258";

            builder.WebHost.UseUrls(url);
            var app = builder.Build();

            app.Logger.LogInformation($"Application will run on: {url}");

            var configUpdates = app.Services.GetRequiredService<IConfigUpdates>();

            configUpdates.OcelotIpUpdate(IPV4);


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors("AllowExpo");
            }

            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.UseOcelot().Wait();

            app.Run();
        }
    }
}
