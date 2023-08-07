using BillService.Data;
using MassTransit;
using Newtonsoft.Json;

namespace BillService.Models
{
    public class OrderConsumer : IConsumer<OrderDto>
    {
        private readonly AppDbContext db;

        public OrderConsumer(AppDbContext db)
        {
            this.db = db;
        }
        public List<OrderDto> orderDtos { get; set; } 
       
        public async Task Consume(ConsumeContext<OrderDto> context )
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);

            var order = JsonConvert.DeserializeObject<OrderDto>(jsonMessage);


            db.Bills.Add(new()
            {
                Name = order.Name,
                OrderId = order.Id
            });
            db.SaveChanges();
            
        }


        public async Task<List<OrderDto>>? ShowList()
        {
            if (orderDtos.Count != 0)
            {
                return orderDtos;
            }
            return null;
        }
      
        
    }
}
