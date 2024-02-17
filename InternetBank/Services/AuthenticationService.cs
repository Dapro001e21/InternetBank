﻿using InternetBank.Models;
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
        private readonly AppDbContext _dbContext = new();
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
                CookieAuthentication(user.Name, user.Email);
                return (true, "");
            }
            return (false, "Ошибка авторизации!!!");
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(User user, string newPassword)
        {
            try
            {
                User temp = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == user.Id);
                if(temp != null)
                {
                    temp.Password = newPassword;
                    await _dbContext.SaveChangesAsync();
                }
                return (true, "");
            }
            catch (Exception)
            {
                return (false, "");
            }
        }
        public async Task<(bool Success, string Message, int Id)> EmailIsExist(string email)
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(user => user.Email == email);
            if (user != null)
            {
                return (true, "", user.Id);
            }
            return (false, "Такого Email не существует!", -1);
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
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
            CookieAuthentication(name, email);
            return (true, "");
        }

        private async void CookieAuthentication(string name, string email)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, email)
                };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { };

            await _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}