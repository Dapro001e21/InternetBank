using InternetBank.Models;
using InternetBank.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<CardsService>();
builder.Services.AddTransient<TransactionsService>();
builder.Services.AddOptions<SmtpConfig>().BindConfiguration("SmtpConfig");
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<CodeGeneratingService>();
builder.Services.AddScoped<TransactionsService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(3);
});
builder.Services.AddScoped<HttpClient>();
builder.Services.AddSession(s => s.IdleTimeout = TimeSpan.FromMinutes(15));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.UseSession();
app.Run();
