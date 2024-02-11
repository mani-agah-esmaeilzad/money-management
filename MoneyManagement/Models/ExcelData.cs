namespace MoneyManagement.Models
{
    public class ExcelData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string CardNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateTimeSubmited { get; set; }
    }
}
