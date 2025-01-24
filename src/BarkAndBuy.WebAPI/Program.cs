
using Application;
using BarkAndBuy.WebAPI.Middlewares;
using Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace BarkAndBuy.WebAPI
{
    /// <summary>
    /// The main entry point of the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point of the application.
        /// </summary>
        /// <param name="args">The command-line arguments passed to the application.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           // builder.WebHost.UseKestrel().UseUrls("http://+:8080"); 

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddApplication();
            string connectionString = builder.Configuration["MySecrets__PostgreConnection"] 
                                      ?? builder.Configuration.GetConnectionString("PostgreConnection");
            
            var connectionStringForEFC = builder.Configuration.GetConnectionString("Postgres");
            
            builder.Services.AddInfrastructure(connectionString, connectionStringForEFC);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BarkAndBuy API", Version = "v1" });

                // Include the XML comments file
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();
            
            using (var scope = app.Services.CreateScope()) // butu graziau atskira metoda ApplyMigrations. Gal cia butu graziau i AddInfrastructure? 
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<MyDbContext>();
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandlingMiddleware();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseExceptionHandlingMiddleware();

            app.MapControllers();

            app.Run();
        }
    }
}
