using InternetBank.Models;
using Microsoft.EntityFrameworkCore;

namespace InternetBank.Services
{
    public class UserService
    {
        private readonly AppDbContext _dbContext;
        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(string email, string newPassword)
        {
            try
            {
                User temp = await _dbContext.Users.SingleOrDefaultAsync(user => user.Email == email);
                if (temp != null)
                {
                    temp.Password = newPassword;
                    await _dbContext.SaveChangesAsync();
                }
                return (true, "");
            }
            catch (Exception)
            {
                return (false, "Ошибка смены пароля!");
            }
        }
        public async Task<(bool Success, string Message)> EmailIsExistAsync(string email)
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(user => user.Email == email);
            if (user != null)
            {
                return (true, "");
            }
            return (false, "Пользователя с такой почтой не существует!");
        }

        public async Task<User> GetUserAsync(int cardId)
        {
            Card card = await _dbContext.Cards.SingleOrDefaultAsync(card => card.Id == cardId);
            User user = await _dbContext.Users.SingleOrDefaultAsync(user => user.Id == card.OwnerId);
			return user;
        }
    }
}
