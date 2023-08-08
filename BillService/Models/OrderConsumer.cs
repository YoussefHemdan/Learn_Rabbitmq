using BillService.Data;
using MassTransit;
using Newtonsoft.Json;
using SharedModels;

namespace BillService.Models
{
    public class OrderConsumer : IConsumer<OrderMsg>
    {
        private readonly AppDbContext db;

        public OrderConsumer(AppDbContext db)
        {
            this.db = db;
        }
     
       
        public Task Consume(ConsumeContext<OrderMsg> context )
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);

            var order = JsonConvert.DeserializeObject<OrderMsg>(jsonMessage);


            db.Bills.Add(new()
            {
                Name = order.Name,
                OrderId = order.Id
            });
            db.SaveChanges();
            return Task.CompletedTask;
        }


        //public async Task<List<OrderDto>>? ShowList()
        //{
        //    if (orderDtos.Count != 0)
        //    {
        //        return orderDtos;
        //    }
        //    return null;
        //}
      
        
    }
}
