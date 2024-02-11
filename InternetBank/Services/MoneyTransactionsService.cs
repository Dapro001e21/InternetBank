using InternetBank.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace InternetBank.Services
{
	public class MoneyTransactionsService
	{
		private readonly AppDbContext _dbContext = new();
		public MoneyTransactionsService(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<bool> AddMoneyTransaction(MoneyTransaction tran)
		{
			using (var transation = await _dbContext.Database.BeginTransactionAsync())
			{
				try
				{
					Card senderCard = await _dbContext.Cards.SingleOrDefaultAsync(c => c.Id == tran.SenderIdCard);
					Card recieverCard = await _dbContext.Cards.SingleOrDefaultAsync(c => c.Id == tran.RecieverIdCard);
					senderCard.Money -= tran.Money;
					recieverCard.Money += tran.Money;
					_dbContext.MoneyTransactions.Add(tran);
					await _dbContext.SaveChangesAsync();
					await transation.CommitAsync();
					return true;
				}
				catch (Exception)
				{
					transation.Rollback();
					return false;
				}
			}			
		}
		public async Task<List<MoneyTransaction>> GetTransactionHistory(int cardId)
		{
			List<MoneyTransaction> transList = await _dbContext.MoneyTransactions.Where(t => t.SenderIdCard == cardId || t.RecieverIdCard == cardId).ToListAsync();
			return transList;
		}
	}
}
