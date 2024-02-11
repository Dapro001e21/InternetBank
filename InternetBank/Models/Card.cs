namespace InternetBank.Models
{
    public class Card
    {
        public int Id { get; set; } 
        public int OwnerId { get; set; }
        public decimal Money {  get; set; }
    }
}
