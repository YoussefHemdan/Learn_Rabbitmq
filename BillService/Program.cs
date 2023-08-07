
using BillService.Data;
using BillService.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace BillService
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
                x.AddConsumer<OrderConsumer>();
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    var uri = new Uri(builder.Configuration["ServiceBus:Uri"]);
                    cfg.Host(uri, host =>
                    {
                        host.Username(builder.Configuration["ServiceBus:Username"]);
                        host.Password(builder.Configuration["ServiceBus:Password"]);
                    });
                    cfg.ReceiveEndpoint(builder.Configuration["ServiceBus:Queue"], c =>
                    {
                        c.ConfigureConsumer<OrderConsumer>(ctx);
                    });
                 
                });
            });
            //builder.Services.AddMassTransitHostedService<>();
            //builder.Services.AddSingleton<IConsumer, OrderConsumer>();

            //var factory = new ConnectionFactory { HostName = "localhost" };
            //using var connection = factory.CreateConnection();
            //using var channel = connection.CreateModel();

            //channel.QueueDeclare(queue: "hello",
            //         durable: false,
            //         exclusive: false,
            //         autoDelete: false,
            //         arguments: null);

            //var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            //{
            //    cfg.ReceiveEndpoint("hello", e =>
            //    {
            //        e.Consumer<OrderConsumer>();
            //    });
            //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            //app.MapControllers();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}