namespace InternetBank.Models
{
    public class Card
    {
        public int Id { get; set; } 
        public string OwnerEmail { get; set; }
        public decimal Money {  get; set; }
    }
}
