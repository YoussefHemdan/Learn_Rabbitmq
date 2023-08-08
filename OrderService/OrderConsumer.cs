using MassTransit;
using Newtonsoft.Json;
using OrderService.Data;
using OrderService.Models;
using SharedModels;

namespace OrderService
{
    public class OrderConsumer : IConsumer<OrderMsg>
    {
        private readonly AppDbContext db;

        public OrderConsumer(AppDbContext db)
        {
            this.db = db;
        }
        public Task Consume(ConsumeContext<OrderMsg> context)
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);

            var order = JsonConvert.DeserializeObject<Order>(jsonMessage);


            db.Orders.Add(new()
            {
                Name = order.Name,
                Id = order.Id
            });
            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
