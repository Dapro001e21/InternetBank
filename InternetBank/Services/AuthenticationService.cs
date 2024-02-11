using InternetBank.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace InternetBank.Services
{
    public class AuthenticationService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _accessor;
        public AuthenticationService(AppDbContext dbContext, IHttpContextAccessor accessor)
        {
            _dbContext = dbContext;
            _accessor = accessor;
        }

        public async Task<(bool Success, string Message)> UserLoginAsync(string email, string password)
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(user => user.Email == email && user.Password == password);
            if (user != null)
            {
                CookieAuthentication(user.Name, user.Id);
                return (true, "");
            }
            return (false, "Ошибка авторизации!!!");
        }

        public async Task<(bool Success, string Message)> UserRegistrationAsync(string name, string email, string password, string repeatPassword)
        {
            if(password != repeatPassword)
            {
                return (false, "Пароли не совпадают!!!");
            }
            User user = await _dbContext.Users.SingleOrDefaultAsync(user => user.Email == email);
            if (user != null)
            {
                return (false, "Пользователь с такой почтой уже зарегестрирован!!!");
            }
            _dbContext.Users.Add(new User() { Name = name, Email = email, Password = password });
            await _dbContext.SaveChangesAsync();
            await UserLoginAsync(email, password);
            return (true, "");
        }

        private async void CookieAuthentication(string name, int id)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim("Id", id.ToString())
                };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { };

            await _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}