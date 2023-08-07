namespace BillService.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
    }
}
