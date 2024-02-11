using InternetBank.Models;
using Microsoft.EntityFrameworkCore;

namespace InternetBank.Services
{
    public class CardsService
    {
        private readonly AppDbContext _dbContext = new();
        public CardsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Card>> GetCards(int id)
        {
            List<Card> cards = await _dbContext.Cards.Where(card => card.OwnerId == id).ToListAsync();
            return cards;
        }

		public async Task<Card> GetCard(int id)
		{
			Card card = await _dbContext.Cards.SingleOrDefaultAsync(card => card.Id == id);
			return card;
		}

		public async Task<bool> AddCard(Card card)
        {
            try
            {
                _dbContext.Cards.Add(card);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
