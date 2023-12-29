
using Infrastucture.Configuation;
using Microsoft.Data.SqlClient;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
           policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()
           ));

            builder.Services.RegisterDbContext(builder.Configuration);
            builder.Services.RegisterIdentity();
            builder.Services.RegisterDI();
            builder.Services.RegisterToken(builder.Configuration);

           

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseCors();

            app.MapControllers();

            app.Run();
        }
    }
}
