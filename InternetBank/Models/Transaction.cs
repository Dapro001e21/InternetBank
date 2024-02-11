namespace InternetBank.Models
{
	public class Transaction
	{
		public int Id {  get; set; }
		public int SenderIdCard { get; set; }
		public int RecieverIdCard { get; set; }
		public DateTime TransactionTime { get; set; }
		public decimal Money { get; set; }
	}
}
