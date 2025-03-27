using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AlphaDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<MemberUserEntity, IdentityRole>(x =>
    {
        x.Password.RequiredLength = 8;
        x.User.RequireUniqueEmail = true;
        x.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<AlphaDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/auth/signin";
    x.LogoutPath = "/auth/signout";
    x.AccessDeniedPath = "/auth/accessDenied";
    x.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    x.SlidingExpiration = true;
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IBaseRepository<MemberUserEntity>, BaseRepository<MemberUserEntity>>();
builder.Services.AddScoped<IBaseRepository<MemberModel>, BaseRepository<MemberModel>>();
builder.Services.AddScoped<IBaseRepository<AddressEntity>, BaseRepository<AddressEntity>>();
builder.Services.AddScoped<IBaseRepository<PictureEntity>, BaseRepository<PictureEntity>>();
builder.Services.AddScoped<IMemberService, MemberService>();

var app = builder.Build();

app.UseHsts();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();