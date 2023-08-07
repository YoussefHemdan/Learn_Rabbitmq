
using MassTransit;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using System;

namespace OrderService
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

            //connect to DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            //rabbitmq - masstransit configuration
            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((ctx , cfg) =>
                {
                    var uri = new Uri(builder.Configuration["ServiceBus:Uri"]);
                    cfg.Host(uri, host =>
                    {
                        host.Username( builder.Configuration["ServiceBus:Username"]);
                        host.Password( builder.Configuration["ServiceBus:Password"]);
                    });
                });
            });


            //builder.Services.AddMassTransitHostedService();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}