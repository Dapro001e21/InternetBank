using InternetBank.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace InternetBank.Services
{
	public class TransactionsService
	{
		private readonly AppDbContext _dbContext = new();
		public TransactionsService(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<(bool Success, string Message)> AddMoneyTransaction(Transaction transaction)
		{
			if (transaction.SenderIdCard == transaction.RecieverIdCard)
				return (false, "Счет отправителя и получателя совпадают. Укажите другой счет!!!");
			using (var transation = await _dbContext.Database.BeginTransactionAsync())
			{
				try
				{
					Card senderCard = await _dbContext.Cards.SingleOrDefaultAsync(c => c.Id == transaction.SenderIdCard);
					Card recieverCard = await _dbContext.Cards.SingleOrDefaultAsync(c => c.Id == transaction.RecieverIdCard);
					senderCard.Money -= transaction.Money;
					recieverCard.Money += transaction.Money;
					_dbContext.Transactions.Add(transaction);
					await _dbContext.SaveChangesAsync();
					await transation.CommitAsync();
					return (true, "");
				}
				catch (Exception)
				{
					transation.Rollback();
					return (false, "");
				}
			}			
		}
		public async Task<List<Transaction>> GetTransactionHistory(int cardId)
		{
			List<Transaction> transList = await _dbContext.Transactions.Where(trans => trans.SenderIdCard == cardId || trans.RecieverIdCard == cardId).ToListAsync();
			return transList;
		}
	}
}
